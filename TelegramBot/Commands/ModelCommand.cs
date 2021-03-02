using AutoDealersHelper.Database;
using AutoDealersHelper.Database.Objects;
using AutoDealersHelper.Exceptions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace AutoDealersHelper.TelegramBot.Commands
{
    class ModelCommand : AbstractCommand, ICommandWithKeyboard, ICommandValidatable, IExplanationString
    {
        public override string Name => this.CommandName(CommandNameId.C_MODEL);
        public override ChatStates RequiredStateForRun => ChatStates.S_FILTER_SETTING_MENU;
        public override ChatStates CurrentState => ChatStates.S_SET_MODEL;
        public override AbstractCommand PreviousCommand => new FilterSettingCommand();
        public override Dictionary<string, AbstractCommand> AvailableCommands => null;
        public ExplanationStringsId ExpStringId => ExplanationStringsId.EX_S_DBSET;
        public ReplyKeyboardMarkup Keyboard => (this as ICommandWithKeyboard).GetKeyboard(AvailableCommands, PreviousCommand);

        protected override async Task<Message> Action(Database.Objects.User user, TelegramBotClient client)
        {
            await client.SendTextMessageAsync(user.ChatId, Name, replyMarkup: Keyboard);

            List<BaseType> brands = user.Filter.Brands;
            List<Model> modelsDbSet = new List<Model>();
            using (var db = new BotDbContext())
            {
                foreach (var n in brands)
                    modelsDbSet.AddRange(db.Models.Where(x => x.ParrentId == n.Id));
            }

            await this.SendCollection<Model>(user.ChatId, modelsDbSet, client);
            return await this.SendExplanationString(user.ChatId, client);
        }

        public bool Validate(Database.Objects.User user)
        {
            var brands = user.Filter.Brands;

            if (brands.Count == 0 || (brands.Count == 1 && brands[0].Number == 0))
                throw new UndecidedParameterException("Марка");

             return true;
        }
    }
}
