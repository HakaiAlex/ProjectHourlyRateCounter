﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace HourlyRateCounter
{
    internal class Connect
    {
        private static ITelegramBotClient botClient;
        static Stopwatch stopwatch = new Stopwatch();
        static TimeSpan interval;

        

        public static async Task MakingConnection(string token)
        {
            botClient = new TelegramBotClient(token); //!!!!!!!!!!Только эту строчку переделал.

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

            // Отправляет запрос на отмену, чтобы остановить бота
            cts.Cancel();
        }

        static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            
            //// Обработка только обновления сообщений: https://core.telegram.org/bots/api#message // Обработка только текстовых сообщений
            if (update.Message is { } message && message.Text is { } messageText)
            {
                var chatId = message.Chat.Id;
                Message sentMessage;
                if (message.Text == "/start")
                {

                    await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: "/start",
                        cancellationToken: cancellationToken,
                        replyMarkup: GetButtons());

                    

                    await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        replyMarkup: GetButtons(),
                        text: "Вы начали работать",
                        cancellationToken: cancellationToken);


                    switch (messageText)
                    {
                        case "Начало смены":
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
                }
                else
                {
                    await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: message.Text = "Напишите /start для старта");
                }
                
            }
            else
                return;
        }

        public static IReplyMarkup GetButtons()
        {
            var keyBoard = new ReplyKeyboardMarkup
                (
                    new[]
                    {
                        new[]
                        {
                            new KeyboardButton("Начало смены"),
                            new KeyboardButton("Конец смены")
                        }

                    }
                );
            return keyBoard;
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
