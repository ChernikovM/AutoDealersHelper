using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace AutoDealersHelper.TelegramBot.Commands
{
    class MenuCommand : ICommand
    {
        public string Name { get; } = "Главное меню";

        public string Description { get; } = "Главное меню";

        public ChatStates RequiredStateForRun { get; } = ChatStates.S_START;

        public async Task Execute(Message mes, Bot bot)
        {
            await this.ChangeChatState(mes.Chat.Id, ChatStates.S_MENU);

            List<List<KeyboardButton>> rows = new List<List<KeyboardButton>>();
            rows.Add(new List<KeyboardButton>()
            {
                new KeyboardButton("Поиск авто"),
                new KeyboardButton("Пустая кнопка")
            });
            rows.Add(new List<KeyboardButton>()
            { 
                new KeyboardButton("Пустая кнопка")
            });

            var keyboard = new ReplyKeyboardMarkup(rows, resizeKeyboard: true);

            await bot.Client.SendTextMessageAsync(mes.Chat.Id, Name, replyMarkup: keyboard);
        }
    }
}
