using AutoDealersHelper.Database;
using AutoDealersHelper.Database.Objects;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace AutoDealersHelper.TelegramBot.Commands
{
    class SetBrandCommand : AbstractCommand, ICommandWithKeyboard
    {
        public override string Name => this.CommandName(CommandNameId.C_BRAND);
        public override ChatStates RequiredStateForRun => ChatStates.S_SETTING_FILTER_MENU;
        public override ChatStates CurrentState => ChatStates.S_SET_BRAND;
        public override AbstractCommand PreviousCommand => new FilterSettingCommand();

        public override Dictionary<string, AbstractCommand> AvailableCommands => null;

        public ReplyKeyboardMarkup Keyboard => (this as ICommandWithKeyboard).GetKeyboard(AvailableCommands, PreviousCommand);

        protected override async Task<Message> Action(Database.Objects.User user, TelegramBotClient client)
        {
            await client.SendTextMessageAsync(user.ChatId, Name, replyMarkup: Keyboard);

            using (BotDbContext db = new BotDbContext())
            {
                await this.SendCollectionAsList<Brand>(user.ChatId, db.Brands, client);
            }
            return await this.SendExplanationString(user.ChatId, client);
        }
    }
}
