using AutoDealersHelper.Database;
using AutoDealersHelper.Parsers;
using AutoDealersHelper.TelegramBot.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;

//TODO: при старте бота сохранить в кеш все таблицы из бд для быстрого обращения
//TODO: добавить в текст эмодзи

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

            Client.OnMessage += ClientOnMessageReceived;
            Client.OnCallbackQuery += ClientOnCallbackQueryReceived;
            
            CommandsInit();
        }

        public static Bot GetInstance(Config config, NLog.Logger logger)
        {
            if (_bot == null)
                _bot = new Bot(config, logger);

            return _bot;
        }
        #endregion

        private ParserAutoRia _autoRiaParser;

        public BotDbContext db;

        public  NLog.Logger logger;

        private List<ICommand> _commands;

        public IReadOnlyDictionary<string, ICommand> CommandsDict { get => _commandsDict; }

        private Dictionary<string, ICommand> _commandsDict;

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
            _commands = new List<ICommand>();
            _commandsDict = new Dictionary<string, ICommand>();

            _commands.Add(new StartCommand());
            _commands.Add(new MenuCommand());
            _commands.Add(new SettingsCommand());
            _commands.Add(new SetBrandCommand());
            _commands.Add(new SetModelCommand());
            _commands.Add(new CarSearchCommand());

            foreach (var com in _commands)
            {
                _commandsDict.Add(com.Name, com);
            }
        }

        private async void ClientOnMessageReceived(object sender, MessageEventArgs e)
        {
            if (e.Message.Type != Telegram.Bot.Types.Enums.MessageType.Text)//TODO: кинуть обратно сообщение (КАК ЖАЛЬ, ЧТО Я НЕ УМЕЮ ЧИТАТЬ ТАКИЕ СООБЩЕНИЯ.....)
            {
                logger.Warn($"Non-text message received from chatId:{e.Message.Chat.Id}");
                return;
            }

            logger.Info($"Message: \"{e.Message.Text}\" FROM ChatID: {e.Message.Chat.Id}");

            if (String.Equals(e.Message.Text, "/start"))
            {
                await CommandsDict[e.Message.Text].Execute(e.Message, _bot);
                return;
            }

            using (var db = new BotDbContext()) //TODO: убрать этот костыль в релизе
            {
                if (db.Users.Any(x => x.ChatId == e.Message.Chat.Id) == false)
                    return;
            }

            ChatStates chatState;
            using (db = new BotDbContext())
            {
                Enum.TryParse(db.Users.First(x => x.ChatId == e.Message.Chat.Id).ChatStateId, out chatState);
            }

            await ChatStateHandler(e.Message, chatState); //TODO: отловить эксепшн и обработать его
        }

        private async Task ChatStateHandler(Message mes, ChatStates chatState)
        {
            ICommand currentCommand;
            try
            {
                currentCommand = CommandsDict[mes.Text];

                if (chatState == currentCommand.RequiredStateForRun)
                    await currentCommand.Execute(mes, _bot);
            }
            catch (Exception ex)
            {
                logger.Warn(ex); //TODO: передать дальше эксепшн
                return;
            }
        }

        private async void ClientOnCallbackQueryReceived(object sender, CallbackQueryEventArgs e)
        {
            logger.Info($"CallbackQuery: \"{e.CallbackQuery.Data}\" FROM ChatID: {e.CallbackQuery.Message.Chat.Id}");

            foreach (ICommand command in _commands)
            {
                if (e.CallbackQuery.Data == command.Name)
                {
                    await command.Execute(e.CallbackQuery.Message, _bot);
                    break;
                }
            }
        }

        #region FormattedSenders

        public async Task SendTextFormattedCode(long chatId, string text)
        {
            await this.Client.SendTextMessageAsync(chatId, $"<code>{text}</code>", parseMode: Telegram.Bot.Types.Enums.ParseMode.Html);
        }

        public async Task SendTextFormattedBold(long chatId, string text)
        {
            await this.Client.SendTextMessageAsync(chatId, $"<b>{text}</b>", parseMode: Telegram.Bot.Types.Enums.ParseMode.Html);
        }

        public async Task SendTextFormattedItalic(long chatId, string text)
        {
            await this.Client.SendTextMessageAsync(chatId, $"<i>{text}</i>", parseMode: Telegram.Bot.Types.Enums.ParseMode.Html);
        }
        #endregion
    }
}