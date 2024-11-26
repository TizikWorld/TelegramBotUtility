﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramBotUtility.Models;

namespace TelegramBotUtility.Services
{
    public class MemoryStorage : IStorage
    {
        /// <summary>
        /// Хранилище сессий
        /// </summary>
        private readonly ConcurrentDictionary<long, Session> _sessions;
        //Коллекция-хранилище _sessions имеет тип данных ConcurrentDictionary.
        //тип ConcurrentDictionary аналогичен обычному Dictionary,
        //но позволяет одновременный безопасный доступ из разных потоков
        //(параллельный доступ и выполнение нескольких операций одновременно).


        public MemoryStorage()
        {
            _sessions = new ConcurrentDictionary<long, Session>();
        }

        public Session GetSession(long chatId)//работает с хранилищем сессий, позволяя нам при подключении клиента создать новую сессию или обновить существующую.
        {
            // Возвращаем сессию по ключу, если она существует
            if (_sessions.ContainsKey(chatId))
                return _sessions[chatId];

            // Создаем и возвращаем новую, если такой не было
            var newSession = new Session() { Mode = "0" };
            _sessions.TryAdd(chatId, newSession);
            return newSession;
        }
    }
}
