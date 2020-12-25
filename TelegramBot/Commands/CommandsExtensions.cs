using AutoDealersHelper.Database;
using AutoDealersHelper.Database.Objects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;

namespace AutoDealersHelper.TelegramBot.Commands
{
    public static class CommandsExtensions
    {
        public async static Task SendBackButton(this ICommand command, Bot bot, long chatId) //TODO: поправить баг сет_бренд и выше
        {
            switch (command.RequiredStateForRun)
            {
                case ChatStates.S_CAR_SEARCH:
                    await command.SendBackToMainMenuButton(bot, chatId);
                    break;
                case ChatStates.S_SETTING:
                    await command.SendBackToCarSearchButton(bot, chatId);
                    break;
                case ChatStates.S_SET_BRAND:
                    await command.SendBackToSettingsButton(bot, chatId);
                    break;
                case ChatStates.S_SET_MODEL:
                    await command.SendBackToSettingsButton(bot, chatId);
                    break;
                case ChatStates.S_SET_YEAR:
                    await command.SendBackToSettingsButton(bot, chatId);
                    break;
                case ChatStates.S_SET_PRICE:
                    await command.SendBackToSettingsButton(bot, chatId);
                    break;
                case ChatStates.S_SET_MILEAGE:
                    await command.SendBackToSettingsButton(bot, chatId);
                    break;
                case ChatStates.S_SET_STATE:
                    await command.SendBackToSettingsButton(bot, chatId);
                    break;
                case ChatStates.S_SET_CITY:
                    await command.SendBackToSettingsButton(bot, chatId);
                    break;
                case ChatStates.S_SET_FUEL:
                    await command.SendBackToSettingsButton(bot, chatId);
                    break;
                case ChatStates.S_SET_VOLUME:
                    await command.SendBackToSettingsButton(bot, chatId);
                    break;
                case ChatStates.S_SET_GEARBOX:
                    await command.SendBackToSettingsButton(bot, chatId);
                    break;
                default:
                    break;
            }
        }

        private static ReplyKeyboardMarkup GenerateBackButton(string text)
        {
            List<List<KeyboardButton>> rows = new List<List<KeyboardButton>>();
            rows.Add(new List<KeyboardButton>()
            {
                new KeyboardButton(text),
            });

            return new ReplyKeyboardMarkup(rows, resizeKeyboard: true);
        }

        private static async Task SendBackToCarSearchButton(this ICommand command, Bot bot, long chatId)
        {
            var button = GenerateBackButton("Вернуться в меню поиска");
            await bot.Client.SendTextMessageAsync(chatId, command.Name, replyMarkup: button);
        }

        private async static Task SendBackToSettingsButton(this ICommand command, Bot bot, long chatId)
        {
            var button = GenerateBackButton("Вернуться к настройкам");
            await bot.Client.SendTextMessageAsync(chatId, command.Name, replyMarkup: button);
        }

        private async static Task SendBackToMainMenuButton(this ICommand command, Bot bot, long chatId)
        {
            var button = GenerateBackButton("Вернуться в главное меню");
            await bot.Client.SendTextMessageAsync(chatId, command.Name, replyMarkup: button);
        }

        public async static Task ChangeChatState(this ICommand command, long chatId, ChatStates state)
        {
            using (var db = new BotDbContext())
            {
                db.Users.FirstAsync(x => x.ChatId == chatId).Result.ChatStateId = state.ToString();
                await db.SaveChangesAsync();
            }
        }
        public static async Task SendCollectionAsList<T>(this ICommand command, Bot bot, long chatId, DbSet<T> collection)
            where T: BaseType
        {
            StringBuilder text = new StringBuilder();

            int i = 0;
            text.Append($"{i++}. Все {Environment.NewLine}");
            foreach (var item in collection)
            {
                text.Append($"{i++}. {item.Name} {Environment.NewLine}");
            }

            await bot.SendTextFormattedItalic(chatId, text.ToString());
        }

        public static async Task SendErrorMessage(this ICommand command, Bot bot, long chatId, string text)
        {
            await bot.SendTextFormattedBold(chatId, text);
        }

        public static async Task SendExplanationString(this ICommand command, Bot bot, long chatId)
        {
            StringBuilder text = new StringBuilder();

            text.Append("Выберите один или несколько вариантов из списка выше и отправьте мне их номера через запятую, без пробелов.");
            text.Append(Environment.NewLine);
            text.Append(Environment.NewLine);
            text.Append($"Примеры сообщений:{Environment.NewLine}{Environment.NewLine}");
            text.Append($"10      (если один вариант){Environment.NewLine}");
            text.Append($"9,56,84 (если несколько вариантов){Environment.NewLine}");
            text.Append($"0       (чтобы выбрать все варианты){Environment.NewLine}");

            await bot.SendTextFormattedCode(chatId, text.ToString());
        }
    }
}
