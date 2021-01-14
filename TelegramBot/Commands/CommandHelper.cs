using AutoDealersHelper.Database.Objects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace AutoDealersHelper.TelegramBot.Commands
{
    public enum ChatStates
    {
        S_STATE_FIRST = 1,

        S_ANY,

        S_START,

        S_MAIN_MENU,
        S_CAR_SEARCH_MENU,
        S_FILTER_SETTING_MENU,

        S_SETTER_FIRST,
        S_SET_BRAND = S_SETTER_FIRST,
        S_SET_MODEL,
        S_SET_YEAR,
        S_SET_PRICE,
        S_SET_MILEAGE,
        S_SET_STATE,
        S_SET_CITY,
        S_SET_FUEL,
        S_SET_VOLUME,
        S_SET_GEARBOX,
        S_SETTER_LAST = S_SET_GEARBOX,

        S_STATE_LAST
    }

    public enum CommandNameId
    {
        C_START = 0,
        C_MAIN_MENU,
        C_CAR_SEARCH_MENU,
        C_FILTER_SETTING_MENU,
        C_BRAND,
        C_MODEL,
        C_PRICE,
        C_YEAR,
        C_FUEL,
        C_GEARBOX,
        C_STATE,
        C_CITY,
        C_MILEAGE,
        C_VOLUME,
        C_RESET_FILTER,
        C_APPLY_FILTER,
        C_BACK,

        C_EMPTY_BUTTON,
    }

    public static class CommandHelper
    {
        public static IReadOnlyDictionary<CommandNameId, string> commandNames = new Dictionary<CommandNameId, string>()
        {
            { CommandNameId.C_START, "/start"},
            { CommandNameId.C_MAIN_MENU, "🚪 Главное меню"},
            { CommandNameId.C_CAR_SEARCH_MENU, "🔍 Поиск авто"},
            { CommandNameId.C_FILTER_SETTING_MENU, "🛠 Настройки фильтра"},
            { CommandNameId.C_BRAND, "🚖 Марка"},
            { CommandNameId.C_MODEL, "🚗 Модель"},
            { CommandNameId.C_PRICE, "💰 Цена"},
            { CommandNameId.C_YEAR, "📆 Год выпуска"},
            { CommandNameId.C_FUEL, "⛽️ Топливо"},
            { CommandNameId.C_GEARBOX, "🎚 Коробка"},
            { CommandNameId.C_STATE, "🇺🇦 Область"},
            { CommandNameId.C_CITY, "🗺 Город"},
            { CommandNameId.C_MILEAGE, "🔢 Пробег"},
            { CommandNameId.C_VOLUME, "📶 Объем двигателя"},
            { CommandNameId.C_RESET_FILTER, "♻️ Сбросить"},
            { CommandNameId.C_APPLY_FILTER, "✅ Подтвeрдить"},
            { CommandNameId.C_BACK, "🔙 Назад"},
            { CommandNameId.C_EMPTY_BUTTON, "Пустая кнопка"},

        };

        public static async Task<Message> SendCollectionAsList<T>(this AbstractCommand command, long chatId, IEnumerable<T> collection, TelegramBotClient client)
            where T : BaseType
        {
            StringBuilder text = new StringBuilder();

            int i = 0;
            text.Append($"{i++}. Любой (Сбросить фильтр). {Environment.NewLine}");
            foreach (var item in collection)
            {
                text.Append($"{i++}. {item.Name} {Environment.NewLine}");
            }

            return await Bot.SendTextFormattedItalic(chatId, text.ToString(), client);
        }

        public static async Task<Message> SendErrorMessage(this AbstractCommand command, long chatId, string text, TelegramBotClient client)
        {
            return await Bot.SendTextFormattedBold(chatId, $"⛔️ {text}", client);
        }

        public static async Task<Message> SendExplanationStringForDbSet(this AbstractCommand command, long chatId, TelegramBotClient client)
        {
            StringBuilder text = new StringBuilder();

            text.Append("Выберите один или несколько вариантов из списка выше и отправьте мне их номера через запятую, без пробелов.");
            text.Append(Environment.NewLine);
            text.Append(Environment.NewLine);
            text.Append($"Примеры сообщений:{Environment.NewLine}{Environment.NewLine}");
            text.Append($"10      (если один вариант){Environment.NewLine}");
            text.Append($"9,56,84 (если несколько вариантов){Environment.NewLine}");
            text.Append($"0       (чтобы выбрать все варианты){Environment.NewLine}");

            return await Bot.SendTextFormattedCode(chatId, text.ToString(), client);
        }

    }
}
