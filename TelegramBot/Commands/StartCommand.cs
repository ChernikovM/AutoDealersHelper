using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace AutoDealersHelper.TelegramBot.Commands
{
    class StartCommand : ICommand
    {
        public string Name { get; } = "/start";

        public string Description { get; } = "Начать работу с ботом";

        public ChatStates RequiredStateForRun { get; } = ChatStates.S_START;

        public async Task Execute(Message mes, Bot bot) //TODO: изменить екзекют : написать приветствие и как пользоваться ботом
        {
            using (bot.db = new Database.BotDbContext())
            {
                if (bot.db.Users.Any(x => x.ChatId == mes.Chat.Id) == false) //TODO: тут создаются карточки пользователей
                {
                    bot.db.Users.Add(new Database.Objects.User()
                    {
                        ChatId = mes.Chat.Id,
                        ChatStateId = ChatStates.S_START.ToString(),
                    });
                    await bot.db.SaveChangesAsync();
                }
            }

            MenuCommand com = new MenuCommand();
            await com.Execute(mes, bot);
        }
    }
}
