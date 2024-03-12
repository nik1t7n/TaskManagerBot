using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TaskHelperBot.Services.Contracts;
using TaskHelperBot.Services.Implementations;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramBot.Data;
using TelegramBot.Entities;
using TelegramBot.Services.Contracts;

namespace TelegramBot.Services.Implementations
{
    public class UpdateHandler : IUpdateHandler
    {
        private readonly ITelegramBotClient _botClient;
        private readonly IConfiguration _configuration;

        public UpdateHandler(ITelegramBotClient botClient, IConfiguration configuration)
        {
            _botClient = botClient;
            _configuration = configuration;
        }

        public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            try
            {
                switch (update.Type)
                {
                    case UpdateType.Message:
                        await HandleMessageAsync(update.Message, cancellationToken);
                        break;
                    // Add handling for other update types if needed
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private async Task HandleMessageAsync(Message message, CancellationToken cancellationToken)
        {
            var connectionString = _configuration.GetConnectionString("RemoteConnection");
            DataContext _context = new DataContext(connectionString);
            IChallengeService challangeService = new ChallengeService(_context);

            if (message.Text == "/alltasks")
            {
                await HandleAllTasksAsync(message, challangeService);
            }
            else if (message.Text.StartsWith("/taskbyid"))
            {
                await HandleTaskByIdAsync(message, challangeService);
            }
            else if (message.Text.StartsWith("/updatetask"))
            {
                await HandleUpdateTaskAsync(message, challangeService);
            }
            else if (message.Text.StartsWith("/deletetask"))
            {
                await HandleDeleteTaskAsync(message, challangeService);
            }
            else if (message.Text.StartsWith("/info"))
            {
                await HandleInfoAsync(message);
            }
            else
            {
                await HandleAddTaskAsync(message, challangeService);
            }
        }

        private async Task HandleAllTasksAsync(Message message, IChallengeService challangeService)
        {
            List<Challenge> challenges = challangeService.GetAllChallenges();

            List<string> challengeStrings = challenges.Select(challenge =>
            {
                string formattedString = $"🆔*Id:* {challenge.Id}\n🚩*Task:* {challenge.Description}\n📅*CreatedAt:* {challenge.CreatedAt}\n";
                return formattedString;
            }).ToList();

            string result = string.Join(Environment.NewLine, challengeStrings);

            await _botClient.SendTextMessageAsync(
                message.Chat.Id,
                result,
                replyToMessageId: message.MessageId,
                parseMode: ParseMode.Markdown
            );
        }

        private async Task HandleTaskByIdAsync(Message message, IChallengeService challangeService)
        {
            string idString = message.Text.Substring("/taskbyid".Length).Trim();
            if (int.TryParse(idString, out int id))
            {
                Challenge challengeById = challangeService.GetChallengeById(id);
                if (challengeById != null)
                {
                    string formattedString = $"🆔*Id:* {challengeById.Id}\n🚩*Task:* {challengeById.Description}\n📅*CreatedAt:* {challengeById.CreatedAt}\n";
                    await _botClient.SendTextMessageAsync(
                        message.Chat.Id,
                        formattedString,
                        replyToMessageId: message.MessageId,
                        parseMode: ParseMode.Markdown
                    );
                }
                else
                {
                    await _botClient.SendTextMessageAsync(
                        message.Chat.Id,
                        "Challenge with the specified ID was not found.",
                        replyToMessageId: message.MessageId
                    );
                }
            }
            else
            {
                await _botClient.SendTextMessageAsync(
                    message.Chat.Id,
                    "Invalid challenge ID.",
                    replyToMessageId: message.MessageId
                );
            }
        }

        private async Task HandleUpdateTaskAsync(Message message, IChallengeService challangeService)
        {
            string command = message.Text.Substring("/updatetask".Length).Trim();
            string[] parts = command.Split(' ');

            if (parts.Length >= 2 && int.TryParse(parts[0], out int id))
            {
                string newDescription = string.Join(" ", parts.Skip(1));
                challangeService.UpdateChallenge(new Challenge { Id = id, Description = newDescription, CreatedAt = DateTime.Now, Deadline = DateTime.Now });
                await _botClient.SendTextMessageAsync(
                    message.Chat.Id,
                    $"Task with ID {id} has been successfully updated.",
                    replyToMessageId: message.MessageId
                );
            }
            else
            {
                await _botClient.SendTextMessageAsync(
                    message.Chat.Id,
                    "Invalid format. Please provide the task ID and the new description.",
                    replyToMessageId: message.MessageId
                );
            }
        }

        private async Task HandleDeleteTaskAsync(Message message, IChallengeService challangeService)
        {
            string idString = message.Text.Substring("/deletetask".Length).Trim();
            if (int.TryParse(idString, out int id))
            {
                challangeService.DeleteChallenge(id);
                await _botClient.SendTextMessageAsync(
                    message.Chat.Id,
                    $"Task with ID {id} has been successfully deleted.",
                    replyToMessageId: message.MessageId
                );
            }
            else
            {
                await _botClient.SendTextMessageAsync(
                    message.Chat.Id,
                    "Invalid task ID.",
                    replyToMessageId: message.MessageId
                );
            }
        }

        private async Task HandleInfoAsync(Message message)
        {
            string infoMessage = @"
🤖 *Task Manager Bot*
This bot helps you manage your tasks efficiently!

📝 *Available Commands:*
- `/alltasks`: View all tasks.
- `/taskbyid {id}`: View a specific task by its ID.
- `/addtask {description}`: Add a new task.
- `/updatetask {id} {changed_description}`: Update a task by its ID.
- `/deletetask {id}`: Delete a task by its ID.
- `/info`: Get information about available commands and usage.

🔹 *Usage Examples:*
- To view all tasks: `/alltasks`
- To view task with ID 5: `/taskbyid 5`
- To add a task: `/addtask Buy groceries`
- To update task with ID 4: `/updatetask 4 Buy bread`
- To delete task with ID 3: `/deletetask 3`

🔍 *Note:*
- Ensure you provide correct inputs when using commands that require IDs.
- Use commands without braces (e.g., `/taskbyid 5`) for better results.

🤖 Enjoy managing your tasks with Task Manager Bot!
";
            await _botClient.SendTextMessageAsync(
                message.Chat.Id,
                infoMessage,
                replyToMessageId: message.MessageId,
                parseMode: ParseMode.Markdown
            );
        }

        private async Task HandleAddTaskAsync(Message message, IChallengeService challangeService)
        {
            Challenge challengeToAdd = new()
            {
                Description = message.Text,
                CreatedAt = DateTime.UtcNow,
                Deadline = DateTime.UtcNow
            };

            challangeService.AddChallenge(challengeToAdd);

            await _botClient.SendTextMessageAsync(
                message.Chat.Id,
                "Task has been successfully added!",
                replyToMessageId: message.MessageId
            );
        }
    }
}
