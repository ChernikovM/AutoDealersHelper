using AutoDealersHelper.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace AutoDealersHelper.TelegramBot.Commands
{
    public class CityCommand : AbstractCommand, ICommandWithKeyboard, ICommandValidatable
    {
        public override string Name => CommandName(CommandNameId.C_CITY);

        public override ChatStates RequiredStateForRun => ChatStates.S_FILTER_SETTING_MENU;

        public override AbstractCommand PreviousCommand => new FilterSettingCommand();

        public override ChatStates CurrentState => ChatStates.S_SET_CITY;

        public override Dictionary<string, AbstractCommand> AvailableCommands => null;

        public ReplyKeyboardMarkup Keyboard => (this as ICommandWithKeyboard).GetKeyboard(AvailableCommands, PreviousCommand);

        public bool Validate(Database.Objects.User user)
        {
            var states = user.Filter.States;

            if (states.Count == 0 || (states.Count == 1 && states[0].Number == 0))
                throw new UndecidedParameterException("Область");

            return true;
        }

        protected async override Task<Message> Action(Database.Objects.User user, TelegramBotClient client)
        {
            await client.SendTextMessageAsync(user.ChatId, Name, replyMarkup: Keyboard);
            //тут получаем список областей, генерируем список городов и отправляем ответ
            return await this.SendExplanationString(user.ChatId, client);
        }
    }
}
