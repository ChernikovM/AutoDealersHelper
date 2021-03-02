using AutoDealersHelper.Exceptions;
using AutoDealersHelper.TelegramBot.Commands;
using AutoDealersHelper.TelegramBot.Setters;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;

namespace AutoDealersHelper.TelegramBot
{
    public sealed class Bot
    {
        #region Singleton

        private static Bot _bot;
        private Bot(NLog.Logger logger)
        {
            Client = new TelegramBotClient(Program.Config.TelegramToken);
            this.logger = logger;

            Client.OnMessage += ClientOnMessageReceived;
            
            CommandsInit();
            SettersInit();
            _messageHandler = new MessageHandler(_commands, _setters, Client);
        }

        public static Bot GetInstance(NLog.Logger logger)
        {
            if (_bot == null)
                _bot = new Bot(logger);

            return _bot;
        }
        #endregion

        private readonly NLog.Logger logger;

        private List<AbstractCommand> _commands;
        private List<AbstractSetter> _setters;
        private readonly MessageHandler _messageHandler;

        public TelegramBotClient Client { get; private set; }

        public void StartReceiving()
        {
            logger.Info("Bot started");
            Client.StartReceiving();
        }
        public void StopReceiving()
        {
            logger.Info("Bot stopped");
            Client.StopReceiving();
        }

        private void CommandsInit() //TODO: тут добалвяются все команды
        {
            _commands = new List<AbstractCommand>
            {
                new StartCommand(),
                new MenuCommand(),
                new FilterSettingCommand(),
                new BrandCommand(),
                new ModelCommand(),
                new CarSearchMenuCommand(),
                new StateCommand(),
                new CityCommand(),
                new FuelCommand(),
                new GearBoxCommand(),
                new PriceCommand(),
                new MileageCommand(),
                new YearCommand(),
                new VolumeCommand(),
                new ResetFilterCommand(),
                new NotImplementedCommand()
            };
        }

        private void SettersInit()
        {
            _setters = new List<AbstractSetter>
            {
                new BrandsSetter(),
                new MileageSetter(),
                new ModelsSetter(),
                new YearSetter(),
                new PriceSetter(),
                new VolumeSetter(),
                new StatesSetter(),
                new CitiesSetter(),
                new GearBoxesSetter(),
                new FuelsSetter(),
            };

        }

        private async void ClientOnMessageReceived(object sender, MessageEventArgs e)
        {
            try
            {
                logger.Info($"Message: \"{e.Message.Text}\" FROM ChatID: {e.Message.Chat.Id}");

                await _messageHandler.Process(e.Message);
            }
            catch (Exception ex)
            {
                string errorMessage = ExceptionHandler.Execute(ex, logger);

                if(errorMessage != null)
                    await CommandHelper.SendErrorMessage(null, e.Message.Chat.Id, errorMessage, Client);
            }
        }

        #region FormattedSenders

        public async void SendAsvertisement(long chatId, string link)
        {
            await Client.SendTextMessageAsync(chatId, link);
        }

        public static async Task<Message> SendTextFormattedCode(long chatId, string text, TelegramBotClient client)
        {
            return await client.SendTextMessageAsync(chatId, $"<code>{text}</code>", parseMode: Telegram.Bot.Types.Enums.ParseMode.Html);
        }

        public static async Task<Message> SendTextFormattedBold(long chatId, string text, TelegramBotClient client)
        {
            return await client.SendTextMessageAsync(chatId, $"<b>{text}</b>", parseMode: Telegram.Bot.Types.Enums.ParseMode.Html);
        }

        public static async Task<Message> SendTextFormattedItalic(long chatId, string text, TelegramBotClient client)
        {
            return await client.SendTextMessageAsync(chatId, $"<i>{text}</i>", parseMode: Telegram.Bot.Types.Enums.ParseMode.Html);
        }
        #endregion
    }
}