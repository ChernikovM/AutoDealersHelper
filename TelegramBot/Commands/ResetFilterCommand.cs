using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace AutoDealersHelper.TelegramBot.Commands
{
    public class ResetFilterCommand : AbstractCommand
    {
        public override string Name => CommandName(CommandNameId.C_RESET_FILTER);

        public override ChatStates RequiredStateForRun => ChatStates.S_FILTER_SETTING_MENU;

        public override AbstractCommand PreviousCommand => null;

        public override ChatStates CurrentState => ChatStates.S_FILTER_SETTING_MENU;

        public override Dictionary<string, AbstractCommand> AvailableCommands => null;

        protected async override Task<Message> Action(Database.Objects.User user, TelegramBotClient client)
        {
            user.Filter = new Database.Objects.Filter();
            return await new FilterSettingCommand().Run(user, client);
        }
    }
}
