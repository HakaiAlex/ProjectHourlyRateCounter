using System.Threading.Tasks;

namespace HourlyRateCounter
{
    //Проверяю пуши p.s. Sasha
    internal class Program
    {
        static async Task Main(string[] args)
        {
            //Тут просто вводим токен, вызывая метод MakingConnection
            //класса Connect, который принимает аргумент string ТОКЕН
            Connect connect = new Connect();
            await Connect.MakingConnection("6121060658:AAGgih7ocxZAg8LtQZMWPl03g9-KlVLRNVM");
        }
    }
}
