using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace AutoDealersHelper.TelegramBot.Commands
{
    class SettingsCommand : ICommand
    {
        public string Name { get; } = "Настройки";

        public string Description { get; } = "Настройки фильтра поиска автомобилей";

        public ChatStates RequiredStateForRun { get; } = ChatStates.S_CAR_SEARCH;

        public async Task Execute(Message mes, Bot bot)
        {
            await this.ChangeChatState(mes.Chat.Id, ChatStates.S_SETTING);

            List<List<KeyboardButton>> rows = new List<List<KeyboardButton>>();
            rows.Add(new List<KeyboardButton>()
            {
                new KeyboardButton("Марка"),
                new KeyboardButton("Модель")
            });
            rows.Add(new List<KeyboardButton>()
            {
                new KeyboardButton("Цена"),
                new KeyboardButton("Год выпуска"),
            });
            rows.Add(new List<KeyboardButton>()
            {
                new KeyboardButton("Коробка передач"),
                new KeyboardButton("Топливо"),
            });
            rows.Add(new List<KeyboardButton>()
            {
                new KeyboardButton("Пробег"),
                new KeyboardButton("Объем двигателя"),
            });
            rows.Add(new List<KeyboardButton>()
            {
                new KeyboardButton("Область"),
                new KeyboardButton("Город"),
            });
            rows.Add(new List<KeyboardButton>()
            {
                new KeyboardButton("Сбросить"),
                new KeyboardButton("Готово"),
            });
            rows.Add(new List<KeyboardButton>()
            {
                new KeyboardButton("Вернуться в меню поиска"),
            });

            var keyboard = new ReplyKeyboardMarkup(rows, resizeKeyboard: true);

            await bot.Client.SendTextMessageAsync(mes.Chat.Id, Name, replyMarkup: keyboard);
        }
    }
}
