using AutoDealersHelper.Database.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace AutoDealersHelper.TelegramBot.Commands
{
    class SetModelCommand : ICommand
    {
        public string Name { get; } = "Модель";

        public string Description { get; } = "Выбрать модель авто";

        public ChatStates RequiredStateForRun { get; } = ChatStates.S_SETTING;

        public async Task Execute(Message mes, Bot bot)
        {
            await this.ChangeChatState(mes.Chat.Id, ChatStates.S_SET_MODEL);
            await this.SendBackButton(bot, mes.Chat.Id);
            try //TODO: 1) сделать общий метод public Execute безопасным (try/catch), а в нем вызывать private Run
            {
                using (bot.db = new Database.BotDbContext())
                {
                    var userBrand = bot.db.Users.First(x => x.ChatId == mes.Chat.Id).Brand; //сделать десериализацию бренда в список при обращении
                    if (userBrand == null)
                    {
                        await this.SendErrorMessage(bot, mes.Chat.Id, "Не выбрана марка автомобиля");
                        return;
                    }
                }

                await this.SendExplanationString(bot, mes.Chat.Id);
            }
            catch (Exception ex) //TODO: доработать текст респонса
            {
                //TODO: обработать все екзекьюты подобной констуркией
                bot.logger.Error(ex);
                await this.SendErrorMessage(bot, mes.Chat.Id, "Произошла непредвиденная ошибка, пожалуйста обратитесь в службу поддержки.");
            }
        }
    }
}
