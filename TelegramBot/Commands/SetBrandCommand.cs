using AutoDealersHelper.Database.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace AutoDealersHelper.TelegramBot.Commands
{
    class SetBrandCommand : ICommand
    {
        public string Name { get; } = "Марка";

        public string Description { get; } = "Выбрать марку авто";

        public ChatStates RequiredStateForRun { get; } = ChatStates.S_SETTING; //TODO: сделать метод который вернет кнопку НАЗАД в зависимости от Стейта

        public async Task Execute(Message mes, Bot bot)
        {
            await this.ChangeChatState(mes.Chat.Id, ChatStates.S_SET_BRAND);

            await this.SendBackButton(bot, mes.Chat.Id);
            using (bot.db = new Database.BotDbContext())
            {
                await this.SendCollectionAsList<Brand>(bot, mes.Chat.Id, bot.db.Brands);
            }
            await this.SendExplanationString(bot, mes.Chat.Id);
        }
    }
}
