using AutoDealersHelper.Database.Objects;
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

    public enum ExplanationStringsId
    { 
        EX_S_DBSET,
        EX_S_YEAR,
        EX_S_MILEAGE,
        EX_S_VOLUME,
        EX_S_PRICE,
    }

    public static class CommandHelper
    {
        #region Explanation strings
        private static readonly string _explanationStringDbSet = $"Выберите один или несколько вариантов из списка выше и отправьте мне их номера через запятую.\n" +
                                                  $"\n" +
                                                  $"Примеры сообщений:\n" +
                                                  $"\n" +
                                                  $"10      (если один вариант)\n" +
                                                  $"9, 56, 84 (если несколько вариантов)\n" +
                                                  $"0       (значение 'Любой')\n";
        private static readonly string _explanationStringYear = $"Введите пару годов через запятую для поиска от/до.\n" +
                                                $"\n" +
                                                $"Примеры сообщений: \n" +
                                                $"\n" +
                                                $"{DateTime.Now.Year - 10}, {DateTime.Now.Year}  (с {DateTime.Now.Year - 10}г. по {DateTime.Now.Year}г.)\n" +
                                                $"0           (значение 'Любой')\n";
        private static readonly string _explanationStringMileage = $"Введите пару значений пробега в тыс.км. через запятую для поиска от/до.\n" +
            $"\n" +
            $"Примеры сообщений:\n" +
            $"\n" +
            $"25, 200   (от 25.000 км. до 200.000 км.)\n" +
            $"0, 0      (без пробега)\n" +
            $"0         (значение 'Любой')\n";

        private static readonly string _explanationStringVolume = $"Введите пару значений объема двигателя в литрах через запятую для поиска от/до.\n" +
            $"\n" +
            $"Примеры сообщений:\n" +
            $"\n" +
            $"1.6, 2.5 (от 1.6 л. до 2.5 л.)\n" +
            $"2, 5     (от 2 л. до 5 л.)\n" +
            $"0        (значение 'Любой')\n";
        private static readonly string _explanationStringPrice = $"Введите пару цен в долларах через запятую для поиска от/до.\n" +
            $"\n" +
            $"Примеры сообщений:\n" +
            $"\n" +
            $"10000, 15000  (от 10.000$ до 15.000$)\n" +
            $"0, 1000       (до 1000$)\n" +
            $"0             (значение 'Любой')\n";

        #endregion

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
        private static readonly IReadOnlyDictionary<ExplanationStringsId, string> explanationString = new Dictionary<ExplanationStringsId, string>
        {
            { ExplanationStringsId.EX_S_DBSET, _explanationStringDbSet},
            { ExplanationStringsId.EX_S_MILEAGE, _explanationStringMileage},
            { ExplanationStringsId.EX_S_YEAR, _explanationStringYear},
            { ExplanationStringsId.EX_S_PRICE, _explanationStringPrice},
            { ExplanationStringsId.EX_S_VOLUME, _explanationStringVolume},
        };

        public static async Task<Message> SendCollection<T>(this AbstractCommand command, long chatId, IEnumerable<T> collection, TelegramBotClient client)
            where T : BaseType
        {
            StringBuilder text = new StringBuilder();

            int i = 0;
            text.Append($"{i++}. Любой (Сбросить фильтр). {Environment.NewLine}");
            foreach (var item in collection)
            {
                text.Append($"{i++}.{item.Name} {Environment.NewLine}");
            }

            return await Bot.SendTextFormattedItalic(chatId, text.ToString(), client);
        }

        public static async Task<Message> SendErrorMessage(this AbstractCommand command, long chatId, string text, TelegramBotClient client)
        {
            return await Bot.SendTextFormattedBold(chatId, $"⛔️ {text}", client);
        }

        public static async Task<Message> SendExplanationString(this AbstractCommand command, long chatId, TelegramBotClient client)
        {
            if (command is IExplanationString != true)
                return null;

            string text = "📝 " + explanationString[(command as IExplanationString).ExpStringId];

            return await Bot.SendTextFormattedCode(chatId, text, client);
        }

    }
}
