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
        static async Task Main(string[] args)
        {
            Connect connect = new Connect();
            await Connect.MakingConnection("6121060658:AAGgih7ocxZAg8LtQZMWPl03g9-KlVLRNVM");
        }
    }
}
