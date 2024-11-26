using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBotUtility.Services;

namespace TelegramBotUtility.Controllers
{
    public class TextMessageController
    {
        private readonly ITelegramBotClient _telegramClient;
        private readonly IStorage _memoryStorage;

        int Summator(string[] strings)
        {
            int sum=0;
            for(short i=0; i < strings.Length; i++)
            {
                sum = sum + Int32.Parse(strings[i]);
            }
            return sum;
        }

        public TextMessageController(ITelegramBotClient telegramBotClient, IStorage memoryStorage )
        {
            _telegramClient = telegramBotClient;
            _memoryStorage = memoryStorage;

        }
        public async Task Handle(Message message, CancellationToken ct)
        {
            // Объект, представляющий кнопки
            var buttons = new List<InlineKeyboardButton[]>();
            buttons.Add(new[]
                    {
                        InlineKeyboardButton.WithCallbackData($" Первый" , $"1"),
                        InlineKeyboardButton.WithCallbackData($" Второй" , $"2")
                    });

            var back = new List<InlineKeyboardButton[]>();
            back.Add(new[]
                    {
                        InlineKeyboardButton.WithCallbackData($" Back" , $"0")
                    });

            int temp = 0;
            temp = Int32.Parse(_memoryStorage.GetSession(message.From.Id).Mode);

            if (message.Text == "/start" || temp==0)
            {
                // передаем кнопки вместе с сообщением (параметр ReplyMarkup)
                await _telegramClient.SendTextMessageAsync(message.Chat.Id, $"<b>  Бот предназначен для работы с текстом.</b> {Environment.NewLine}" +
                    $"{Environment.NewLine}Первый режим выполняет подсчёт количества символов в тексте{Environment.NewLine}" +
                    $"Второй режим выполняет вычисление суммы чисел, которые вы ему отправляете",
                    cancellationToken: ct, parseMode: ParseMode.Html, replyMarkup: new InlineKeyboardMarkup(buttons));
            }

            switch (temp)
            {
                    case 1:

                    await _telegramClient.SendTextMessageAsync(message.Chat.Id,
                    $"Длина сообщения: {message.Text.Length} знаков", cancellationToken: ct, replyMarkup: new InlineKeyboardMarkup(back));

                    break;

                    case 2:

                    char[] separtors = " \r\n\"'".ToCharArray(); // Строка сепараторов
                    string[] strarr = message.Text.Split(separtors);
                    string[] resultarr = strarr.Where(x => int.TryParse(x, out int _tmp)).ToArray();

                    await _telegramClient.SendTextMessageAsync(message.Chat.Id,
                    $"Сумма чисел: {Summator(resultarr)}", cancellationToken: ct, replyMarkup: new InlineKeyboardMarkup(back));
                    break;
            }
        }
    }
}
