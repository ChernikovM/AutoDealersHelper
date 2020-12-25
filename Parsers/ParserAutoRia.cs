using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using Microsoft.EntityFrameworkCore;
using AutoDealersHelper.Database.Objects;
using AutoDealersHelper.Database;
using Newtonsoft.Json;

namespace AutoDealersHelper.Parsers
{
    public class ParserAutoRia
    {
        #region Singleton
        private static ParserAutoRia _instance;
        private ParserAutoRia() { return; }
        private ParserAutoRia(string token, NLog.Logger logger)
        {
            _token = token;
            _logger = logger;
        }
        public static ParserAutoRia GetInstance(string apiKey, NLog.Logger logger)
        {
            if (_instance == null)
                _instance = new ParserAutoRia(apiKey, logger);

            return _instance;
        }

        #endregion

        private readonly string _token;
        private NLog.Logger _logger;
        private string GetJson(string link) //вернет жсон строку с ответом на запрос
        {
            string json = null;

            using (WebClient wc = new WebClient())
            {
                wc.Encoding = Encoding.UTF8;
                try
                {
                    json = wc.DownloadString(link + "?api_key=" + _token);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex);
                }
            }
            return json;
        }

        private IEnumerable<BaseType> Deserialize(string json)
        {
            if (json == null)
                return null;

            IEnumerable<BaseType> data = null;
            try
            {
                data = JsonConvert.DeserializeObject<IEnumerable<BaseType>>(json);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
            return data;
        }

        public void FillSupportDb(BotDbContext db)
        {
            try
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
            catch (DbUpdateException ex)
            {
                _logger.Fatal(ex);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }

        private List<T> GetCollection<T>(string link) 
            where T: BaseType, new()
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
            where T: BaseType, IDependentType<U>, new()
            where U: BaseType
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
    }
}
