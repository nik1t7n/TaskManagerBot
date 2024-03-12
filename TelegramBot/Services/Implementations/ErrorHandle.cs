using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using TelegramBot.Services.Contracts;

namespace TelegramBot.Services.Implementations
{
    public class ErrorHandler : IErrorHandler
    {
        public Task HandleErrorAsync(ITelegramBotClient botClient, Exception error, CancellationToken cancellationToken)
        {
            var errorMessage = error switch
            {
                ApiRequestException apiRequestException => 
                    $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => error.ToString()
            };

            Console.WriteLine(errorMessage);
            return Task.CompletedTask;
        }
    }
}