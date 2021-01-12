using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutoDealersHelper.TelegramBot.Setters
{
    public static class SetterExtensions
    {
        public static List<int> GetIndexesListFromString(this ISetter setter, string text)
        {
            if (IsValidString(text) == false)
                throw new ArgumentException(); //TODO: InvalidArgumentException

            string[] list = text.Split(',', StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < list.Length; ++i)
                list[i] = list[i].Trim();

            list = list.Distinct().ToArray();

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

        private static bool IsValidString(string text)
        {
            foreach (var n in text)
            {
                if (!char.IsWhiteSpace(n) && !n.Equals(',') && !char.IsDigit(n))
                {
                    return false;
                }
            }

            return true;
        }

        public static List<int> DeserializeFromJsonToList(this ISetter setter, string json)
        {
            if (json == null)
                return null;

            List<int> data;
            try
            {
                data = JsonConvert.DeserializeObject<List<int>>(json);
            }
            catch (Exception) //TODO: если ошибка сериализации - вернуть сообщение об ошибке пользователю и обнулить в базе значение
            {
                throw; //TODO: отловить ошибку выше
            }

            return data;
        }

        public static string SerializeFromListToJson(this ISetter setter,List<int> list)
        {
            string data;
            try
            {
                data = JsonConvert.SerializeObject(list);
            }
            catch (Exception) //если ошибка сериализации - вернуть сообщ пользователю с просьбой повторить ИЛИ присвоить значение 0
            {
                throw; //TODO: отловаить ошибку выше
            }
            return data;
        }
    }
}
