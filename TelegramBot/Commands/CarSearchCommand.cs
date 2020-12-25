using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace AutoDealersHelper.TelegramBot.Commands
{
    class CarSearchCommand : ICommand
    {
        public string Name { get; } = "Поиск авто";

        public string Description { get; } = "Начать поиск автомобиля";

        public ChatStates RequiredStateForRun { get; } = ChatStates.S_MENU;

        public async Task Execute(Message mes, Bot bot)
        {
            await this.ChangeChatState(mes.Chat.Id, ChatStates.S_CAR_SEARCH);

            List<List<KeyboardButton>> rows = new List<List<KeyboardButton>>();
            rows.Add(new List<KeyboardButton>()
            {
                new KeyboardButton("Настройки"),
            });
            rows.Add(new List<KeyboardButton>()
            {
                new KeyboardButton("Назад в главное меню")
            });

            var keyboard = new ReplyKeyboardMarkup(rows, resizeKeyboard: true);

            await bot.Client.SendTextMessageAsync(mes.Chat.Id, Name, replyMarkup: keyboard);
        }
    }
}
