using AutoDealersHelper.Database;
using AutoDealersHelper.Database.Objects;
using AutoDealersHelper.Exceptions;
using AutoDealersHelper.TelegramBot.Commands;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace AutoDealersHelper.TelegramBot.Setters
{
    public abstract class AbstractSetter
    {
        public abstract ChatStates RequiredStateForRun { get; }
        public abstract AbstractCommand NextCommand { get; }

        public async Task<Message> Run(Database.Objects.User user, string text, TelegramBotClient client)
        {
            using (BotDbContext db = new BotDbContext())
            {
                Action(user, text, db);
                await db.SaveChangesAsync();
            }

            return await (NextCommand).Run(user, client);
        }

        protected List<BaseType> GetCollectionOfItemByIndexes<T>(List<int> indexes, IEnumerable<T> dbSet)
            where T : BaseType
        {
            List<BaseType> collection = new List<BaseType>();

            List<T> dbSetAsList = dbSet.ToList();

            foreach (var n in indexes)
            {
                if (n > dbSetAsList.Count || n == 0)
                    continue;

                var item = dbSetAsList[n - 1];

                collection.Add(item);
            }

            if (collection.Count == 0)
                collection.Add(new BaseType());

            return collection;
        }

        private List<string> GetSplitTrimStringsList(string text)
        {
            string[] array = text.Split(',', StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < array.Length; ++i)
                array[i] = array[i].Trim();

            return array.ToList();
        }

        protected List<int> GetListIndexesFromString(string text)
        {
            IsValidString<int>(text);

            List<string> list = GetSplitTrimStringsList(text);
            list = list.Distinct().ToList();
            List<int> result = new List<int>();

            foreach (var digit in list)
            {
                if (int.TryParse(digit, out int d) == false)
                    continue;
                result.Add(d);
            }

            result = result.OrderBy(digit => digit).ToList();
            if (result.Count > 1 && result[0] == 0)
                result = result.Skip(1).ToList();

            return result;
        }

        protected List<double> GetDoublePairFromString(string text)
        {
            IsValidString<double>(text);

            List<string> list = GetSplitTrimStringsList(text);
            List<double> result = new List<double>();

            foreach (var digit in list)
            {
                if (double.TryParse(digit, System.Globalization.NumberStyles.Number, CultureInfo.InvariantCulture, out double d) == true)
                    result.Add(d);
                else
                    throw new InvalidArgumentException(digit);
            }

            switch (result.Count)
            {
                case 1:
                    if (result[0] == 0)
                        return result;
                    result.Add(result[0]);
                    return result;
                case 2:
                    return result;
                default:
                    throw new IncorrectArgumentsCountException(2, result.Count);
            }
        }

        protected List<int> GetIntPairFromString(string text)
        {
            IsValidString<int>(text);

            List<string> list = GetSplitTrimStringsList(text);
            List<int> result = new List<int>();

            foreach (var digit in list)
            {
                if (int.TryParse(digit, out int d) == true)
                    result.Add(d);
                else
                    throw new InvalidArgumentException(digit);
            }

            result = result.OrderBy(digit => digit).ToList();

            switch (result.Count)
            {
                case 1:
                    if (result[0] == 0)
                        return result;
                    result.Add(result[0]);
                    return result;
                case 2:
                    return result;
                default:
                    throw new IncorrectArgumentsCountException(2, result.Count);
            }
        }

        protected bool IsValidString<T>(string text)
            where T: struct
        {
            foreach (var n in text)
            {
                if (!char.IsWhiteSpace(n) && !n.Equals(',') && !char.IsDigit(n))
                {
                    if (!(n == '.' && typeof(T) == typeof(double)))
                        throw new InvalidCharacterException(n);
                }
            }
            return true;
        }

        public abstract void Action(Database.Objects.User user, string text, BotDbContext db);
    }
}
