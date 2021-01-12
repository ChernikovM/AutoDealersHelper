using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace AutoDealersHelper.TelegramBot.Commands
{
    class SetModelCommand : AbstractCommand, ICommandWithKeyboard, ICommandValidatable
    {
        public override string Name => this.CommandName(CommandNameId.C_MODEL);
        public override ChatStates RequiredStateForRun => ChatStates.S_SETTING_FILTER_MENU;
        public override ChatStates CurrentState => ChatStates.S_SET_MODEL;
        public override AbstractCommand PreviousCommand => new FilterSettingCommand();
        public override Dictionary<string, AbstractCommand> AvailableCommands => null;

        public ReplyKeyboardMarkup Keyboard => (this as ICommandWithKeyboard).GetKeyboard(AvailableCommands, PreviousCommand);

        protected override async Task<Message> Action(Database.Objects.User user, TelegramBotClient client)
        {
            await client.SendTextMessageAsync(user.ChatId, Name, replyMarkup: Keyboard);
            //тут получаем список брендов авто, генерируем список моделей и отправляем ответ
            return await this.SendExplanationString(user.ChatId, client);
        }

        public bool Validate(Database.Objects.User user)
        {
            List<int> brands = JsonConvert.DeserializeObject(user.Brand, typeof(List<int>)) as List<int>;

            if (brands.Count == 0 || brands[0] == 0)
                throw new ArgumentException(); //TODO: BrandException

             return true;
        }
    }
}
