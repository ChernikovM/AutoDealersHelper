using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace AutoDealersHelper.TelegramBot.Commands
{
    class NotImplementedCommand : AbstractCommand, ICommandValidatable
    {
        public override string Name => this.CommandName(CommandNameId.C_EMPTY_BUTTON);
        public override ChatStates RequiredStateForRun => ChatStates.S_ANY;
        public override ChatStates CurrentState => ChatStates.S_ANY;
        public override AbstractCommand PreviousCommand => null;
        public override Dictionary<string, AbstractCommand> AvailableCommands => null;

        public bool Validate(Database.Objects.User user)
        {
            throw new ArgumentException();//TODO: NotImplementedCommandException
            //return await this.SendErrorMessage(user.ChatId, "Такой команды не существует или она находится в разработке.\n\nПопробуйте другой пункт меню.", client)            
        }

        protected override async Task<Message> Action(Database.Objects.User user, TelegramBotClient client)
        {
            throw new NotImplementedException();
        }
    }
}
