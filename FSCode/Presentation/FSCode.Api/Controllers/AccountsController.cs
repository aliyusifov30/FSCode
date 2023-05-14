using FSCode.Application.DTOs.AccountDTOs;
using FSCode.Application.HelperManager;
using FSCode.Application.Services.EmailServices;
using FSCode.Application.Services.TokenServices;
using FSCode.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FSCode.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        readonly UserManager<AppUser> _userManager;
        readonly SignInManager<AppUser> _signInManager;
        readonly RoleManager<IdentityRole> _roleManager;
        readonly ITokenHandler _tokenHandler;
        readonly IEmailService _emailService;

        public AccountsController(UserManager<AppUser> userManager, IEmailService emailService, ITokenHandler tokenHandler, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
            _emailService = emailService;
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenHandler = tokenHandler;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDTO login)
        {
            AppUser user = await _userManager.FindByNameAsync(login.UserName);
            if (user == null)
            {
                return StatusCode(401, "Password or Username wrong");
            }
            var result = await _signInManager.PasswordSignInAsync(user, login.Password, false, false);
            if (!result.Succeeded)
            {
                return StatusCode(401, "Password or Username wrong");
            }
            string token = _tokenHandler.CreateAccessToken(user, 60);
            return Ok(token);
        }

        [HttpGet("createrole")]
        public async Task<IActionResult> CreateRoles()
        {
            var role1 = new IdentityRole("Member");
            await _roleManager.CreateAsync(role1);

            return Ok();
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterDto register)
        {
            AppUser exist = await _userManager.FindByNameAsync(register.Email);

            if (exist != null)
            {
                return BadRequest(register);
            }
            if (register.Password != register.RepeatPassword)
            {
                return BadRequest();
            }
            if (!RegexManager.CheckMailRegex(register.Email))
            {
                return BadRequest(register);
            }

            AppUser user = new()
            {
                Email = register.Email,
                UserName = register.UserName,
                TGToken = register.TGToken,
            };
            
            var result = await _userManager.CreateAsync(user, register.Password);
            await _userManager.AddToRoleAsync(user, "Member");
            return Ok(result);
        }



    }
}
