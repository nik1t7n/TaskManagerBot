using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using DotNetEnv;
using TaskHelperBot.Services.Contracts;
using TaskHelperBot.Services.Implementations;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramBot.Data;
using TelegramBot.Entities;
using Microsoft.Extensions.Configuration;
using TelegramBot.Services;
using TelegramBot.Services.Contracts;
using TelegramBot.Services.Implementations;
using FluentMigrator.Runner;


namespace TelegramBot // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        private static ITelegramBotClient _botClient;
        private static ReceiverOptions _receiverOptions;

        private static IConfiguration configuration = new ConfigurationBuilder()
            .AddJsonFile("C:\\Users\\User\\Desktop\\VS_Projects\\TelegramBot\\TelegramBot\\appsettings.json")
            .Build();


        static async Task Main()
        {
            Env.Load("C:\\Users\\User\\Desktop\\VS_Projects\\TelegramBot\\TelegramBot\\sensitive.env");

            string botToken = Environment.GetEnvironmentVariable("API_KEY");

            _botClient = new TelegramBotClient(botToken);
            _receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = new[]
                {
                    UpdateType.Message,
                },
                ThrowPendingUpdates = true,
            };

            using var cts = new CancellationTokenSource();

            TelegramBot.Services.Contracts.IUpdateHandler updateHandler = new UpdateHandler(_botClient, configuration);

            IErrorHandler errorHandler = new ErrorHandler();

            _botClient.StartReceiving(updateHandler.HandleUpdateAsync, errorHandler.HandleErrorAsync, _receiverOptions, cts.Token);

            var me = await _botClient.GetMeAsync();
            Console.WriteLine($"{me.FirstName} has been started!");

            await Task.Delay(-1);


        }

    }
}