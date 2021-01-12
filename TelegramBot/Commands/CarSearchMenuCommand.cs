using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace AutoDealersHelper.TelegramBot.Commands
{
    class CarSearchMenuCommand : AbstractCommand, ICommandWithKeyboard
    {
        public override string Name => this.CommandName(CommandNameId.C_CAR_SEARCH_MENU);
        public override ChatStates RequiredStateForRun => ChatStates.S_MAIN_MENU;
        public override ChatStates CurrentState => ChatStates.S_CAR_SEARCH_MENU;
        public ReplyKeyboardMarkup Keyboard => (this as ICommandWithKeyboard).GetKeyboard(AvailableCommands, PreviousCommand, 2);
        public override AbstractCommand PreviousCommand => new MenuCommand();

        public override Dictionary<string, AbstractCommand> AvailableCommands => new Dictionary<string, AbstractCommand>()
        {
            { CommandName(CommandNameId.C_FILTER_SETTING_MENU), new FilterSettingCommand() },
        };

        protected override async Task<Message> Action(Database.Objects.User user, TelegramBotClient client)
        {
            return await client.SendTextMessageAsync(user.ChatId, Name, replyMarkup: Keyboard);
        }
    }
}
