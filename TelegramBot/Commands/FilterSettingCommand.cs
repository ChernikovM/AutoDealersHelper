using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace AutoDealersHelper.TelegramBot.Commands
{
    class FilterSettingCommand : AbstractCommand, ICommandWithKeyboard
    {
        public override string Name => this.CommandName(CommandNameId.C_FILTER_SETTING_MENU);
        public override ChatStates RequiredStateForRun => ChatStates.S_CAR_SEARCH_MENU;
        public override ChatStates CurrentState => ChatStates.S_SETTING_FILTER_MENU;
        public override AbstractCommand PreviousCommand => new CarSearchMenuCommand();
        public override Dictionary<string, AbstractCommand> AvailableCommands => new Dictionary<string, AbstractCommand>()
        {
            { CommandName(CommandNameId.C_BRAND), new SetBrandCommand() },
            { CommandName(CommandNameId.C_MODEL), new SetModelCommand() },
            { CommandName(CommandNameId.C_PRICE), new NotImplementedCommand() },
            { CommandName(CommandNameId.C_YEAR), new NotImplementedCommand() },
            { CommandName(CommandNameId.C_GEARBOX), new NotImplementedCommand() },
            { CommandName(CommandNameId.C_FUEL), new NotImplementedCommand() },
            { CommandName(CommandNameId.C_MILEAGE), new NotImplementedCommand() },
            { CommandName(CommandNameId.C_VOLUME), new NotImplementedCommand() },
            { CommandName(CommandNameId.C_STATE), new NotImplementedCommand() },
            { CommandName(CommandNameId.C_CITY), new NotImplementedCommand() },
            { CommandName(CommandNameId.C_RESET_FILTER), new NotImplementedCommand() },
            { CommandName(CommandNameId.C_APPLY_FILTER), new NotImplementedCommand() },
        };

        public ReplyKeyboardMarkup Keyboard => (this as ICommandWithKeyboard).GetKeyboard(AvailableCommands, PreviousCommand);

        protected override async Task<Message> Action(Database.Objects.User user, TelegramBotClient client)
        {
            return await client.SendTextMessageAsync(user.ChatId, Name, replyMarkup: Keyboard);
        }
    }
}
