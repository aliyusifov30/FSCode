using AutoMapper;
using AutoMapper.Configuration.Annotations;
using FSCode.Application.DTOs.ReminderDTOs;
using FSCode.Application.Exceptions;
using FSCode.Application.HelperManager;
using FSCode.Application.Repositories;
using FSCode.Application.Services.EmailServices;
using FSCode.Application.Services.TelegramServices;
using FSCode.Domain.Entities;
using FSCode.Domain.Enums;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using System.IO;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using Telegram.Bot.Types;

namespace FSCode.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RemindersController : ControllerBase
    {
        IReminderRepository _reminderRepository;
        IMapper _mapper;
        IWebHostEnvironment _env;
        IEmailService _emailService;
        ITelegramService _telegramService;
        UserManager<AppUser> _userManager;
        public RemindersController(IReminderRepository reminderRepository, IMapper mapper, IEmailService emailService, IWebHostEnvironment env, ITelegramService telegramService, UserManager<AppUser> userManager)
        {
            _reminderRepository = reminderRepository;
            _mapper = mapper;
            _emailService = emailService;
            _env = env;
            _telegramService = telegramService;
            _userManager = userManager;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var reminders = await _reminderRepository.GetAllAsync(x => true, false);
            return Ok(reminders);
        }

        [HttpPost("Create")]
        [Authorize(Roles = "Member")]
        public async Task<IActionResult> Create(ReminderCreateDto reminderCreateDto)
        {
            var methodIinfo = MethodCheck(reminderCreateDto.Method);
            if (!methodIinfo.check) return BadRequest(methodIinfo.message.ToString());

            if (!ModelState.IsValid) return BadRequest(ModelState);

            var userName = User.Identity.Name;
            var user = await _userManager.FindByNameAsync(userName);

            if (user == null) return Unauthorized();

            var reminder = _mapper.Map<Reminder>(reminderCreateDto);

            await _reminderRepository.InsertAsync(reminder);
            await _reminderRepository.CommitAsync();

            var remainderTime = RemainderTime(DateTime.UtcNow.AddHours(4), reminderCreateDto.SendAt);

            if (reminderCreateDto.Method.Equals(ReminderEnum.Email.ToString(), StringComparison.InvariantCultureIgnoreCase)) 
            { 
                if (!RegexManager.CheckMailRegex(user.Email)) 
                { return BadRequest("Please fix email"); }
                
                string messageBody = _emailService.FormatHTML("Reminder", DateTime.UtcNow.AddHours(4).ToString(), reminderCreateDto.Content); 
                try { 
                    BackgroundJob.Schedule(() => _emailService.Send(user.Email, "Reminder Message", messageBody), remainderTime); 
                } 
                catch (Exception ex) 
                { 
                    throw new Exception(ex.Message); 
                }
            } 
            else if (reminderCreateDto.Method.Equals(ReminderEnum.Telegram.ToString(), StringComparison.InvariantCultureIgnoreCase)) 
            { 
                try 
                { 
                    BackgroundJob.Schedule(() => _telegramService.SendMessage(user.TGToken, user.TGId, reminderCreateDto.Content), remainderTime); 
                }
                catch (Exception ex) { throw new Exception(ex.Message); } 
            }

            return Ok(reminder);
        }

        [HttpPost("Edit")]
        [Authorize(Roles = "Member")]
        public async Task<IActionResult> Edit(ReminderEditDto editDto)
        {
            var methodIinfo = MethodCheck(editDto.Method);
            if (!methodIinfo.check) return BadRequest(methodIinfo.message);

            if (!ModelState.IsValid) return BadRequest(ModelState);

            var userName = User.Identity.Name;
            var user = await _userManager.FindByNameAsync(userName);

            if (user == null) return Unauthorized();

            var remainderTime = RemainderTime(DateTime.UtcNow.AddHours(4), editDto.SendAt);

            var entity = await _reminderRepository.GetAsync(x => x.Id == editDto.Id);

            if (entity == null) throw new ReminderNotFoundException();

            entity.Method = editDto.Method;
            entity.Content = editDto.Content;
            entity.SendAt = editDto.SendAt;

            await _reminderRepository.CommitAsync();
            
            BackgroundJob.Delete(editDto.JobId.ToString());

            if (editDto.Method.Equals(ReminderEnum.Email.ToString(), StringComparison.InvariantCultureIgnoreCase))
            {
                if (!RegexManager.CheckMailRegex(user.Email))
                { return BadRequest("Please fix email"); }

                string messageBody = _emailService.FormatHTML("Reminder", DateTime.UtcNow.AddHours(4).ToString(), editDto.Content);
                try
                {
                    BackgroundJob.Schedule(() => _emailService.Send(user.Email, "Reminder Message", messageBody), remainderTime);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            else if (editDto.Method.Equals(ReminderEnum.Telegram.ToString(), StringComparison.InvariantCultureIgnoreCase))
            {
                try
                {
                    BackgroundJob.Schedule(() => _telegramService.SendMessage(user.TGToken, user.TGId, editDto.Content), remainderTime);
                }
                catch (Exception ex) 
                { 
                    throw new Exception(ex.Message); 
                }
            }

            return Ok();
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(int id, string jobId)
        {
            await _reminderRepository.Remove(id);
            var deleted = BackgroundJob.Delete(jobId);
            await _reminderRepository.CommitAsync();

            return Ok();
        }


        private TimeSpan RemainderTime(DateTime nowTime, DateTime sendAt)
        {
            DateTime time = new DateTime(nowTime.Year, nowTime.Month, nowTime.Day, nowTime.Hour, nowTime.Minute, nowTime.Second);
            DateTime sentTime = new DateTime(sendAt.Year, sendAt.Month, sendAt.Day, sendAt.Hour, sendAt.Minute, sendAt.Second);

            return (sentTime - time);
        }
        private (bool check, StringBuilder message) MethodCheck(string method)
        {
            bool check = false;
            StringBuilder message = new();
            var enumList = Enum.GetValues(typeof(ReminderEnum)).Cast<ReminderEnum>().ToList();

            foreach (var item in enumList)
            {
                if (method.Equals(item.ToString(), StringComparison.InvariantCultureIgnoreCase))
                {
                    check = true;
                    break;
                }
            }

            if (!check)
            {
                message.Append("Method must be ");
                enumList.ForEach(item => message.Append(item + " "));
            }
            return (check, message);
        }
    }
}
