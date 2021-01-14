using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace AutoDealersHelper.TelegramBot.Commands
{
    public abstract class AbstractCommand
    {
        abstract public string Name { get; }
        abstract public ChatStates RequiredStateForRun { get; }
        abstract public AbstractCommand PreviousCommand { get; }
        abstract public ChatStates CurrentState { get; }
        abstract public Dictionary<string, AbstractCommand> AvailableCommands { get; } //TODO: readonly

        abstract protected Task<Message> Action(Database.Objects.User user, TelegramBotClient client);


        public async Task<Message> Run(Database.Objects.User user, TelegramBotClient client)
        {
            user.ChatState = this.CurrentState;
            return await Action(user, client); //TODO: может лучше не ждать до конца экшона?
        }

        protected string CommandName(CommandNameId id)
        {
            return CommandHelper.commandNames[id];
        }

    }
}
