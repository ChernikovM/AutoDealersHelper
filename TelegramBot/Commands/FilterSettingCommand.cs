using System;
using System.Collections.Generic;
using System.Text;
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
        public override ChatStates CurrentState => ChatStates.S_FILTER_SETTING_MENU;
        public override AbstractCommand PreviousCommand => new CarSearchMenuCommand();
        public override Dictionary<string, AbstractCommand> AvailableCommands => new Dictionary<string, AbstractCommand>()
        {
            { CommandName(CommandNameId.C_BRAND), new BrandCommand() },
            { CommandName(CommandNameId.C_MODEL), new ModelCommand() },
            { CommandName(CommandNameId.C_YEAR), new YearCommand() },
            { CommandName(CommandNameId.C_PRICE), new PriceCommand() },
            { CommandName(CommandNameId.C_STATE), new StateCommand() },
            { CommandName(CommandNameId.C_CITY), new CityCommand() },
            { CommandName(CommandNameId.C_MILEAGE), new MileageCommand() },
            { CommandName(CommandNameId.C_VOLUME), new VolumeCommand() },
            { CommandName(CommandNameId.C_GEARBOX), new GearBoxCommand() },
            { CommandName(CommandNameId.C_FUEL), new FuelCommand() },
            
            { CommandName(CommandNameId.C_RESET_FILTER), new ResetFilterCommand() },
            { CommandName(CommandNameId.C_APPLY_FILTER), new NotImplementedCommand() }, //TODO: доделать кнопку apply
        };

        public ReplyKeyboardMarkup Keyboard => (this as ICommandWithKeyboard).GetKeyboard(AvailableCommands, PreviousCommand);

        protected override async Task<Message> Action(Database.Objects.User user, TelegramBotClient client)
        {
            await client.SendTextMessageAsync(user.ChatId, Name, replyMarkup: Keyboard);

            return await SendCurrentFilter(user, client);
        }

        private async Task<Message> SendCurrentFilter(Database.Objects.User user, TelegramBotClient client)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("Ваши текущие настройки: " + Environment.NewLine + Environment.NewLine);
            sb.Append(user.Filter.ToString() + Environment.NewLine + Environment.NewLine);
            sb.Append(
                $"Вы можете изменить любое поле с помощью соответствующей кнопки на клавиатуре, после чего " +
                $"нажмите кнопку [{CommandName(CommandNameId.C_APPLY_FILTER)}], чтобы бот начал оповещать Вас о подходящих предложениях!");

            return await Bot.SendTextFormattedCode(user.ChatId, sb.ToString(), client);
        }
    }
}
