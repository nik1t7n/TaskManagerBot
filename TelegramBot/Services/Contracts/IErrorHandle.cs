using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;

namespace TelegramBot.Services.Contracts
{
    public interface IErrorHandler
    {
        Task HandleErrorAsync(ITelegramBotClient botClient, Exception error, CancellationToken cancellationToken);
    }
}