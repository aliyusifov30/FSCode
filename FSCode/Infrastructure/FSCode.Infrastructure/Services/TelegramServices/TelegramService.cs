using FSCode.Application.Services.TelegramServices;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace FSCode.Infrastructure.Services.TelegramServices
{
    public class TelegramService : ITelegramService
    {
        //public async Task<User> GetUser(string token)
        //{
        //    var botClient = new TelegramBotClient(token);
        //    var me = await botClient.GetMeAsync();

        //    return me;
        //}

        public async Task SendMessage(string token, string chatId, string connect)
        {
            var botClient = new TelegramBotClient(token);

            Message message = await botClient.SendTextMessageAsync(
                chatId: chatId,
                        text: connect,
                cancellationToken: new CancellationToken());
        }
    }
}