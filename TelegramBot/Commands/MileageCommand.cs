using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace AutoDealersHelper.TelegramBot.Commands
{
    public class MileageCommand : AbstractCommand, ICommandWithKeyboard, IExplanationString
    {
        public ReplyKeyboardMarkup Keyboard => (this as ICommandWithKeyboard).GetKeyboard(AvailableCommands, PreviousCommand);

        public override string Name => CommandName(CommandNameId.C_MILEAGE);

        public override ChatStates RequiredStateForRun => ChatStates.S_FILTER_SETTING_MENU;

        public override AbstractCommand PreviousCommand => new FilterSettingCommand();

        public override ChatStates CurrentState => ChatStates.S_SET_MILEAGE;
        public ExplanationStringsId ExpStringId => ExplanationStringsId.EX_S_MILEAGE;
        public override Dictionary<string, AbstractCommand> AvailableCommands => null;

        protected async override Task<Message> Action(Database.Objects.User user, TelegramBotClient client)
        {
            await client.SendTextMessageAsync(user.ChatId, Name, replyMarkup: Keyboard);

            return await this.SendExplanationString(user.ChatId, client);
        }
    }
}
