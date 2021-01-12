using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace AutoDealersHelper.TelegramBot.Commands
{
    class MenuCommand : AbstractCommand, ICommandWithKeyboard
    {
        public override string Name => CommandName(CommandNameId.C_MAIN_MENU);
        public override ChatStates CurrentState => ChatStates.S_MAIN_MENU;
        public override ChatStates RequiredStateForRun => ChatStates.S_START;
        public override AbstractCommand PreviousCommand => null;
        public override Dictionary<string, AbstractCommand> AvailableCommands => new Dictionary<string, AbstractCommand>()
        {
            { CommandName(CommandNameId.C_CAR_SEARCH_MENU), new CarSearchMenuCommand() },
        };

        public ReplyKeyboardMarkup Keyboard => (this as ICommandWithKeyboard).GetKeyboard(AvailableCommands, PreviousCommand, 2);

        protected override async Task<Message> Action(Database.Objects.User user, TelegramBotClient client)
        {
            return await client.SendTextMessageAsync(user.ChatId, Name, replyMarkup: Keyboard);
        }
    }
}
