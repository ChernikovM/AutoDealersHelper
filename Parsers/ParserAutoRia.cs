using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using Microsoft.EntityFrameworkCore;
using AutoDealersHelper.Database.Objects;
using AutoDealersHelper.Database;
using Newtonsoft.Json;
using System.Linq;

//TODO: добавить в базу таблицу для хранения последнего прочитанного ИД обьявления. Считать при создании парсера значение LastId и спарсить объявления после него.
//при выключении сервера обновить в базе значения ластИд.
//при получении Идов брать в рассчет только те что после ластИд и обрабатывать их.

namespace AutoDealersHelper.Parsers
{
    public class ParserAutoRia : AbstractParser
    {
        #region Singleton
        private static ParserAutoRia _instance;
        private ParserAutoRia() { return; }
        private ParserAutoRia(string token)
        {
            _token = token;
            _lastSearchId = 29017097; // get value from db
        }
        public static ParserAutoRia GetInstance(string apiKey)
        {
            if (_instance == null)
                _instance = new ParserAutoRia(apiKey);

            return _instance;
        }

        #endregion

        private readonly string _token;
        private int _lastSearchId;
        #region FillSupportDb
        private IEnumerable<BaseType> Deserialize(string json)
        {
            if (json == null)
                return null;

            IEnumerable<BaseType> data = null;
            data = JsonConvert.DeserializeObject<IEnumerable<BaseType>>(json);

            return data;
        }

        public void FillSupportDb(BotDbContext db)
        {
            db.Brands.AddRange(GetCollection<Brand>($"https://developers.ria.com/auto/categories/1/marks/"));
            db.States.AddRange(GetCollection<State>($"https://developers.ria.com/auto/states"));

            db.SaveChanges();

            db.Fuels.AddRange(GetCollection<Fuel>($"https://developers.ria.com/auto/type/"));
            db.GearBoxes.AddRange(GetCollection<GearBox>($"https://developers.ria.com/auto/categories/1/gearboxes"));
            db.Models.AddRange(GetDependantCollection<Model, Brand>($"https://developers.ria.com/auto/categories/1/marks/", db.Brands, "models"));
            db.Cities.AddRange(GetDependantCollection<City, State>($"https://developers.ria.com/auto/states/", db.States, "cities"));

            db.SaveChangesAsync();
        }

        private List<T> GetCollection<T>(string link)
            where T : BaseType, new()
        {
            IEnumerable<BaseType> dataCollection = Deserialize(GetJson(link));
            List<T> list = new List<T>();
            foreach (var n in dataCollection)
            {
                list.Add(new T { Name = n.Name, Number = n.Number });
            }
            return list;
        }

        private List<T> GetDependantCollection<T, U>(string link, IEnumerable<U> baseCollection, string tag)
            where T : BaseType, IDependentType<U>, new()
            where U : BaseType
        {
            if (baseCollection == null || baseCollection.GetEnumerator().MoveNext() == false)
                throw new ArgumentNullException(baseCollection.ToString());

            List<T> list = new List<T>();
            foreach (U baseItem in baseCollection)
            {
                IEnumerable<BaseType> dataCollection = Deserialize(GetJson(link + baseItem.Number + "/" + tag + "/"));
                foreach (var item in dataCollection)
                {
                    T temp = new T() { Name = item.Name, Number = item.Number };
                    (temp as IDependentType<U>).ParrentId = baseItem.Id;
                    list.Add(temp);
                }
            }
            return list;
        }

#endregion

        private string GetJson(string link, string parameters = "") //вернет жсон строку с ответом на запрос
        {
            string json = null;

            using (WebClient wc = new WebClient())
            {
                wc.Encoding = Encoding.UTF8;
                json = wc.DownloadString($"{link}?api_key={_token}&{parameters}");
            }
            return json;
        }


        public override List<Advertisement> Parse()
        {
            List<int> newIds = GetNewIds();
            if (newIds == null) // нет новых объявлений
            {
                Program.logger.Info($"AutoRia: нет новых объявлений. LastId = [{_lastSearchId}]");
                return new List<Advertisement>();
            }

            return GetNewAdvertisements(newIds);
        }

        private List<int> GetNewIds()
        {
            string link = $"https://developers.ria.com/auto/search";
            string parameters = $"top=1&countpage=100&page=0&order_by=11";

            string json = GetJson(link, parameters);
            List<int> ids = new List<int>();
            List<int> result = new List<int>();

            AutoRiaResponse response = JsonConvert.DeserializeObject<AutoRiaResponse>(json);

            if (response.Result.SearchResult.Count == 0)
            {
                return null;
            }
            ids.AddRange(response.Result.SearchResult.Ids);

            int indexOfLastSearchId = ids.FindIndex(x => x == _lastSearchId);

            result.AddRange(ids.Take(indexOfLastSearchId).ToList());
            if (result.Count == 0)
            {
                return null;
            }

            _lastSearchId = result[0];

            return result;
        }
        private List<Advertisement> GetNewAdvertisements(List<int> ids)
        {
            string link = "https://developers.ria.com/auto/info";
            string parameters;
            List<Advertisement> result = new List<Advertisement>();
            string json;

            foreach (var n in ids)
            {
                parameters = $"auto_id={n}";
                json = GetJson(link, parameters);
                result.Add(JsonConvert.DeserializeObject<Advertisement>(json));
            }

            return result;
        }
    }
}
