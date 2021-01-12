//TODO: изменить екзекют : написать приветствие и как пользоваться ботом

using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace AutoDealersHelper.TelegramBot.Commands
{
    public class StartCommand : AbstractCommand
    {
        public override string Name => CommandName(CommandNameId.C_START);

        public override ChatStates RequiredStateForRun => ChatStates.S_ANY;

        public override ChatStates CurrentState => ChatStates.S_START;

        public override AbstractCommand PreviousCommand => null;
        public override Dictionary<string, AbstractCommand> AvailableCommands => null;

        protected override async Task<Message> Action(Database.Objects.User user, TelegramBotClient client)
        {
            return await new MenuCommand().Run(user, client); //TODO: запихнуть в коммандХелпер словарь со всеми командами из Бот.сиэс
        }
    }
}
