using AutoDealersHelper.Database;
using AutoDealersHelper.Database.Objects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace AutoDealersHelper.TelegramBot.Commands
{
    public class GearBoxCommand : AbstractCommand, ICommandWithKeyboard
    {
        public ReplyKeyboardMarkup Keyboard => (this as ICommandWithKeyboard).GetKeyboard(AvailableCommands, PreviousCommand);

        public override string Name => CommandName(CommandNameId.C_GEARBOX);

        public override ChatStates RequiredStateForRun => ChatStates.S_FILTER_SETTING_MENU;

        public override AbstractCommand PreviousCommand => new FilterSettingCommand();

        public override ChatStates CurrentState => ChatStates.S_SET_GEARBOX;

        public override Dictionary<string, AbstractCommand> AvailableCommands => null;

        protected async override Task<Message> Action(Database.Objects.User user, TelegramBotClient client)
        {
            await client.SendTextMessageAsync(user.ChatId, Name, replyMarkup: Keyboard);

            using (BotDbContext db = new BotDbContext())
            {
                await this.SendCollectionAsList<GearBox>(user.ChatId, db.GearBoxes, client);
            }
            return await this.SendExplanationStringForDbSet(user.ChatId, client);
        }
    }
}
