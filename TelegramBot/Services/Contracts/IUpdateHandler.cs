using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramBot.Services.Contracts
{
    
    public interface IUpdateHandler
    {
        Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken);
    }
}