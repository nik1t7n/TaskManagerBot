using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using TaskHelperBot.Services.Contracts;
using TaskHelperBot.Services.Implementations;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramBot.Data;
using TelegramBot.Entities;


namespace TelegramBot // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        private static ITelegramBotClient _botClient;
        private static ReceiverOptions _receiverOptions;

        // public DataContext _context = new DataContext("Server=(local);Database=ChallengeManagerBotDb;Trusted_Connection=true;TrustServerCertificate=true;");

        static async Task Main()
        {
            _botClient = new TelegramBotClient("6204846942:AAHgT0HVHueG5d29vSXmUrIyT1h09ggYAoY"); 
            _receiverOptions = new ReceiverOptions 
            {
                AllowedUpdates = new[] 
                {
                UpdateType.Message, 
            },
                
                ThrowPendingUpdates = true,
            };

            using var cts = new CancellationTokenSource();

 
            _botClient.StartReceiving(UpdateHandler, ErrorHandler, _receiverOptions, cts.Token); 

            var me = await _botClient.GetMeAsync(); 
            Console.WriteLine($"{me.FirstName} has been started!");

            await Task.Delay(-1); 
            
 
        }

        private static async Task UpdateHandler(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            
            try
            {
                
                switch (update.Type)
                {
                    case UpdateType.Message:
                        {
                            var message = update.Message;
                            string[] commands = new string[] { "/alltasks", "/taskbyid", "updatetask", "/deletetask" };

                            DataContext _context = new DataContext("Server=(local);Database=ChallengeManagerBotDb;Trusted_Connection=true;TrustServerCertificate=true;");
                            IChallengeService challangeService = new ChallengeService(_context);

                            if (!commands.Contains(message!.Text))
                            {
                                Challenge challengeToAdd = new();

                                challengeToAdd.Description = message.Text;
                                challengeToAdd.CreatedAt = DateTime.UtcNow;
                                challengeToAdd.Deadline = DateTime.UtcNow;

                                challangeService.AddChallenge(challengeToAdd);

                                await botClient.SendTextMessageAsync(
                                    message.Chat.Id,
                                    "Task has been successfully added!",
                                    replyToMessageId: message.MessageId
                                    );
                            } 
                            else
                            {
                                if (message.Text == "/alltasks")
                                {
                                    List<Challenge> challenges = challangeService.GetAllChallenges();

                                    List<string> challengeStrings = challenges.Select(challenge =>
                                    {
                                        // Форматируем строку с ChallengeId, задачей, смайликом и датой создания
                                        string formattedString = $"🆔*Id:* {challenge.Id}\n🚩*Task:* {challenge.Description}\n📅*CreatedAt:* {challenge.CreatedAt}\n";

                                        return formattedString;
                                    }).ToList();

                                    // Объединяем все строки в одну строку
                                    string result = string.Join(Environment.NewLine, challengeStrings);

                                    await botClient.SendTextMessageAsync(
                                    message.Chat.Id,
                                    result,
                                    replyToMessageId: message.MessageId,
                                    parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown
                                    );
                                }
                                
                                else if (message.Text == "/taskbyid")
                                {
                                    Challenge challengeById = challangeService.GetChallengeById();

                                    List<string> challengeStrings = challenges.Select(challenge =>
                                    {
                                        // Форматируем строку с ChallengeId, задачей, смайликом и датой создания
                                        string formattedString = $"🆔*Id:* {challenge.Id}\n🚩*Task:* {challenge.Description}\n📅*CreatedAt:* {challenge.CreatedAt}\n";

                                        return formattedString;
                                    }).ToList();

                                    // Объединяем все строки в одну строку
                                    string result = string.Join(Environment.NewLine, challengeStrings);

                                    await botClient.SendTextMessageAsync(
                                    message.Chat.Id,
                                    result,
                                    replyToMessageId: message.MessageId,
                                    parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown
                                    );
                                }
                            }
                            return;
                        }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private static Task ErrorHandler(ITelegramBotClient botClient, Exception error, CancellationToken cancellationToken)
        {
            // Тут создадим переменную, в которую поместим код ошибки и её сообщение 
            var ErrorMessage = error switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => error.ToString()
            };

            Console.WriteLine(ErrorMessage);
            return Task.CompletedTask;
        }


    }
}