using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace AutoDealersHelper.TelegramBot.Commands
{
    class BackCommand : AbstractCommand
    {
        public override string Name => CommandName(CommandNameId.C_BACK);

        public override ChatStates RequiredStateForRun => ChatStates.S_ANY;

        public override AbstractCommand PreviousCommand => null;

        public override ChatStates CurrentState => ChatStates.S_ANY;

        public override Dictionary<string, AbstractCommand> AvailableCommands => null;

        protected override Task<Message> Action(Database.Objects.User user, TelegramBotClient client)
        {
            throw new NotImplementedException();
        }
    }
}
