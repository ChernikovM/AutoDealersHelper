using AutoDealersHelper.Database;
using AutoDealersHelper.Database.Objects;
using AutoDealersHelper.Exceptions;
using AutoDealersHelper.TelegramBot.Commands;
using AutoDealersHelper.TelegramBot.Setters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace AutoDealersHelper.TelegramBot
{
    public class MessageHandler
    {
        private Dictionary<string, AbstractCommand> _command;
        private Dictionary<ChatStates, ISetter> _setter;
        private TelegramBotClient _client;

        public MessageHandler(List<AbstractCommand> commands, List<ISetter> setters, TelegramBotClient client)
        {
            _command = new Dictionary<string, AbstractCommand>();
            _setter = new Dictionary<ChatStates, ISetter>();

            foreach (var com in commands)
                _command.Add(com.Name, com);

            foreach (var setter in setters)
                _setter.Add(setter.RequiredState, setter);

            _client = client;
        }

        public async Task<Database.Objects.User> GetUserProfile(long chatId)
        {
            Database.Objects.User user;

            using (var db = new BotDbContext())
            {
                user = db.Users.FirstOrDefault(x => x.ChatId == chatId);

                if (user == null)
                {
                    user = new Database.Objects.User(chatId);
                    db.Users.Add(user);
                    await db.SaveChangesAsync();
                }
            }

            return user;
        }

        public async Task<Message> Process(Message message)
        {
            if (message.Type != MessageType.Text)
                throw new InvalidMessageTypeException(message.Type);

            Database.Objects.User user = GetUserProfile(message.Chat.Id).Result;

            return await CommandHandler(user, message);
        }

        private async Task<Message> BackButtonRun(Database.Objects.User user, ChatStates chatState)
        {
            AbstractCommand currentCommand = _command.FirstOrDefault(x => x.Value.CurrentState == chatState).Value;

            if (currentCommand.PreviousCommand == null)
                return null;

            return await currentCommand.PreviousCommand.Run(user, _client);
        }

        private async Task<Message> CommandHandler(Database.Objects.User user, Message message)
        {
            if (message.Text == CommandHelper.commandNames[CommandNameId.C_START])
            {
                return await _command[message.Text].Run(user, _client);
            }

            ChatStates chatState = user.ChatState;

            if (message.Text == CommandHelper.commandNames[CommandNameId.C_BACK])
                return await BackButtonRun(user, chatState);

            AbstractCommand com;

            if (_command.TryGetValue(message.Text, out com) == false)
            {
                if (chatState >= ChatStates.S_SETTER_FIRST
                    && chatState <= ChatStates.S_SETTER_LAST)
                {
                    return await SetterHandler(user, chatState, message);
                }

                return null;                
            }

            if (IsCommandCanBeExecuted(com, chatState) == false)
                return null;

            if (com is ICommandValidatable)
                (com as ICommandValidatable).Validate(user);            

            return await com.Run(user, _client);
        }

        private async Task<Message> SetterHandler(Database.Objects.User user, ChatStates userChatState, Message message)
        {
            if (_setter.TryGetValue(userChatState, out ISetter setter) == false)
            {
                throw new ArgumentException(); //TODO:  UnknownSetterException
            }

            return await setter.Run(user, message.Text, _client);            
        }

        private bool IsCommandCanBeExecuted(AbstractCommand com, ChatStates userChatState)
        {
            if (com.RequiredStateForRun != ChatStates.S_ANY)
                if (com.RequiredStateForRun != userChatState)
                    return false;

            return true;
        }
    }
}
