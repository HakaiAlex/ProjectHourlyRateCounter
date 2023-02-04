using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace HourlyRateCounter
{
    //Проверяю пуши p.s. Sasha
    internal class Program
    {
        private static ITelegramBotClient botClient;
        static Stopwatch stopwatch = new Stopwatch();
        static TimeSpan interval;
        static async Task Main(string[] args)
        {
            botClient = new TelegramBotClient("6088264173:AAHYkH900CQx1MQ5mLdyFYbTakwieMnq1Fo");
            using CancellationTokenSource cts = new CancellationTokenSource();

            // Начало приема не блокирует поток вызывающего абонента. Прием осуществляется в пуле потоков(ThreadPool).
            ReceiverOptions receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = Array.Empty<UpdateType>() // получать все типы обновлений
            };

            botClient.StartReceiving(
                updateHandler: HandleUpdateAsync,
                pollingErrorHandler: HandlePollingErrorAsync,
                receiverOptions: receiverOptions,
                cancellationToken: cts.Token
            );

            var me = await botClient.GetMeAsync();

            Console.WriteLine($"Start listening for @{me.Username}");
            Console.ReadLine();

            // Send cancellation request to stop bot
            cts.Cancel();


        }

        static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            // Only process Message updates: https://core.telegram.org/bots/api#message // Only process text messages
            if (update.Message is { } message && message.Text is { } messageText)
            {


                var chatId = message.Chat.Id;

                Console.WriteLine($"Received a '{messageText}' message in chat {chatId}.");

                Message sentMessage;
                 

                // Пишу обработчик команд/кнопок
                switch (messageText)
                {
                    case "/Go":
                        {
                            stopwatch.Start();
                            sentMessage = await botClient.SendTextMessageAsync(
                            chatId: chatId,
                            text: "Вы начали работать",
                            cancellationToken: cancellationToken); // логика обработки
                        }
                        break;

                    case "/Finish":
                        {
                            stopwatch.Stop();
                            interval = stopwatch.Elapsed;
                            stopwatch.Reset();
                            sentMessage = await botClient.SendTextMessageAsync(
                            chatId: chatId,
                            text: "Вы закончили работать, вы потратили " + interval.Minutes,
                            cancellationToken: cancellationToken); // логика обработки
                        }
                        break;

                }

                // Echo received message text
                //Message sentMessage = await botClient.SendTextMessageAsync(
                //    chatId: chatId,
                //    text: "You said:\n" + messageText,
                //    cancellationToken: cancellationToken);
            }
            else
                return;
        }

        static Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(ErrorMessage);
            return Task.CompletedTask;
        }
    }
}
