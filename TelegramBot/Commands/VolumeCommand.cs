using AutoDealersHelper.Database.Objects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace AutoDealersHelper.TelegramBot.Commands
{
    public class VolumeCommand : AbstractCommand, ICommandWithKeyboard
    {
        public override string Name => CommandName(CommandNameId.C_VOLUME);

        public override ChatStates RequiredStateForRun => ChatStates.S_FILTER_SETTING_MENU;

        public override AbstractCommand PreviousCommand => new FilterSettingCommand();

        public override ChatStates CurrentState => ChatStates.S_SET_VOLUME;

        public override Dictionary<string, AbstractCommand> AvailableCommands => null;

        public ReplyKeyboardMarkup Keyboard => (this as ICommandWithKeyboard).GetKeyboard(AvailableCommands, PreviousCommand);

        protected async override Task<Telegram.Bot.Types.Message> Action(User user, TelegramBotClient client)
        {
            await client.SendTextMessageAsync(user.ChatId, Name, replyMarkup: Keyboard);

            return await this.SendExplanationStringForDbSet(user.ChatId, client);
        }
    }
}
