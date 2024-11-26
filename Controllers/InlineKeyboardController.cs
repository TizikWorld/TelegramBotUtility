using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBotUtility.Services;

namespace TelegramBotUtility.Controllers
{
        public class InlineKeyboardController
        {
            private readonly ITelegramBotClient _telegramClient;
            private readonly IStorage _memoryStorage;

        public InlineKeyboardController(ITelegramBotClient telegramBotClient, IStorage memoryStorage)
        {
                _telegramClient = telegramBotClient;
                _memoryStorage = memoryStorage;
        }
        public async Task Handle(CallbackQuery? callbackQuery, CancellationToken ct)
        {
            if (callbackQuery?.Data == null)
                return;

            var buttons = new List<InlineKeyboardButton[]>();
            buttons.Add(new[]
                    {
                        InlineKeyboardButton.WithCallbackData($" Первый" , $"1"),
                        InlineKeyboardButton.WithCallbackData($" Второй" , $"2")
                    });

            // Обновление пользовательской сессии новыми данными
            _memoryStorage.GetSession(callbackQuery.From.Id).Mode = callbackQuery.Data;

            Console.WriteLine($"Контроллер {GetType().Name} обнаружил нажатие на кнопку");

            //await _telegramClient.SendTextMessageAsync(callbackQuery.From.Id, $"Обнаружено нажатие на кнопку{callbackQuery.Data}", cancellationToken: ct);

            switch (Int32.Parse(callbackQuery.Data))
            {
                case 0:
                    await _telegramClient.SendTextMessageAsync(callbackQuery.From.Id, $"<b>  Бот предназначен для работы с текстом.</b> {Environment.NewLine}" +
                    $"{Environment.NewLine}Первый режим выполняет подсчёт количества символов в тексте{Environment.NewLine}" +
                    $"Второй режим выполняет вычисление суммы чисел, которые вы ему отправляете",
                    cancellationToken: ct, parseMode: ParseMode.Html, replyMarkup: new InlineKeyboardMarkup(buttons));
                    break;

                case 1:
                    await _telegramClient.SendTextMessageAsync(callbackQuery.From.Id, $"Введите текст", cancellationToken: ct);

                    break;

                case 2:
                    await _telegramClient.SendTextMessageAsync(callbackQuery.From.Id, $"Введите числа", cancellationToken: ct);
                    break;

            }
        }
    }
}

