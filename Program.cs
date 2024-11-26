using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text;
using Telegram.Bot;
using TelegramBotUtility.Controllers;
using TelegramBotUtility.Services;

namespace TelegramBotUtility
{
        public class Program
        {
        static async Task Main(string[] args) // меняем тип void на асинхронный , а Task позволяет вызывать метод асинхронно
            {
                Console.OutputEncoding = Encoding.Unicode;

                // Объект, отвечающий за постоянный жизненный цикл приложения
                var host = new HostBuilder()
                    .ConfigureServices((hostContext, services) => ConfigureServices(services)) // Задаем конфигурацию
                    .UseConsoleLifetime() // Позволяет поддерживать приложение активным в консоли
                    .Build(); // Собираем

                Console.WriteLine("Сервис запущен");
                // Запускаем сервис
                await host.RunAsync();
                Console.WriteLine("Сервис остановлен");
            }

            static void ConfigureServices(IServiceCollection services)
            {

                services.AddSingleton<IStorage, MemoryStorage>(); // cs0311 означала что класс memorystorage не наследовался от интерфейса

                services.AddTransient<TextMessageController>();
                services.AddTransient<InlineKeyboardController>();

                // Регистрируем объект TelegramBotClient c токеном подключения
                services.AddSingleton<ITelegramBotClient>(provider => new TelegramBotClient("secrets "));
                // Регистрируем постоянно активный сервис бота
                services.AddHostedService<Bot>();          
            }
        }
    
}
