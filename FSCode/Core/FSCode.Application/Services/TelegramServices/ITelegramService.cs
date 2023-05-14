using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace FSCode.Application.Services.TelegramServices
{
    public interface ITelegramService
    {

        //Task<User> GetUser(string token);
        Task SendMessage(string token, string chatId, string connect);
    }
}
