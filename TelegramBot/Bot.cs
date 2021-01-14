using AutoDealersHelper.Database;
using AutoDealersHelper.Exceptions;
using AutoDealersHelper.Parsers;
using AutoDealersHelper.TelegramBot.Commands;
using AutoDealersHelper.TelegramBot.Setters;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private Bot(Config config, NLog.Logger logger)
        {
            Client = new TelegramBotClient(config.TelegramToken);
            this.logger = logger;
            _autoRiaParser = ParserAutoRia.GetInstance(config.AutoRiaToken, logger);

            BotDbContext.InitStaticFields();

            Client.OnMessage += ClientOnMessageReceived;
            
            CommandsInit();
            SettersInit();
            _messageHandler = new MessageHandler(_commands, _setters, Client);
        }

        public static Bot GetInstance(Config config, NLog.Logger logger)
        {
            if (_bot == null)
                _bot = new Bot(config, logger);

            return _bot;
        }
        #endregion

        private ParserAutoRia _autoRiaParser;

        private readonly NLog.Logger logger;

        private List<AbstractCommand> _commands;
        private List<ISetter> _setters;
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
            _commands = new List<AbstractCommand>();
            
            _commands.Add(new StartCommand());
            _commands.Add(new MenuCommand());
            _commands.Add(new FilterSettingCommand());
            _commands.Add(new BrandCommand());
            _commands.Add(new ModelCommand());
            _commands.Add(new CarSearchMenuCommand());
            _commands.Add(new StateCommand());
            _commands.Add(new CityCommand());
            _commands.Add(new FuelCommand());
            _commands.Add(new GearBoxCommand());
            _commands.Add(new PriceCommand());
            _commands.Add(new MileageCommand());
            _commands.Add(new YearCommand());
            _commands.Add(new VolumeCommand());
            _commands.Add(new ResetFilterCommand());
            _commands.Add(new NotImplementedCommand());
        }

        private void SettersInit()
        {
            _setters = new List<ISetter>();

            _setters.Add(new SetBrands());
            _setters.Add(new SetModels());
            
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