using AutoDealersHelper.Database;
using AutoDealersHelper.Database.Objects;
using AutoDealersHelper.TelegramBot.Commands;
using System;
using System.Collections.Generic;
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

            foreach (var n in indexes)
            {
                var item = dbSet.ToList()[n - 1];
                //var item = dbSet.FirstOrDefault(x => x.Id == n);

                if (item == null)
                    continue;

                collection.Add(item);
            }

            if (collection.Count == 0)
                collection.Add(new BaseType());
            return collection;
        }

        private List<string> GetSplitTrimDistinctStringsList(string text)
        {
            string[] array = text.Split(',', StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < array.Length; ++i)
                array[i] = array[i].Trim();

            array = array.Distinct().ToArray();

            return array.ToList();
        }

        protected List<int> GetListIndexesFromString(string text)
        {
            if (IsValidString<int>(text) == false)
                throw new ArgumentException(); //TODO: InvalidArgumentException

            List<string> list = GetSplitTrimDistinctStringsList(text);
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
            if (IsValidString<double>(text) == false)
                throw new ArgumentException(); //TODO: InvalidArgumentException

            List<string> list = GetSplitTrimDistinctStringsList(text);
            List<double> result = new List<double>();

            foreach (var digit in list)
            {
                if (double.TryParse(digit, out double d) == false)
                    continue;
                result.Add(d);
            }

            result = result.OrderBy(digit => digit).ToList();

            if (result.Count != 2)
                throw new ArgumentException(); //InvalidArgumentsCountException

            return result;
        }

        protected List<int> GetIntPairFromString(string text)
        {
            if (IsValidString<int>(text) == false)
                throw new ArgumentException(); //TODO: InvalidArgumentException

            List<string> list = GetSplitTrimDistinctStringsList(text);
            List<int> result = new List<int>();

            foreach (var digit in list)
            {
                if (int.TryParse(digit, out int d) == false)
                    continue;
                result.Add(d);
            }

            result = result.OrderBy(digit => digit).ToList();

            if (result.Count != 2)
                throw new ArgumentException(); //InvalidArgumentsCountException

            return result;
        }

        protected bool IsValidString<T>(string text)
            where T: struct
        {
            foreach (var n in text)
            {
                if (!char.IsWhiteSpace(n) && !n.Equals(',') && !char.IsDigit(n))
                {
                    if(!(n == '.' && typeof(T) == typeof(double)))
                        return false;
                }
            }
            return true;
        }

        public abstract void Action(Database.Objects.User user, string text, BotDbContext db);
    }
}
