using AutoDealersHelper.Database;
using AutoDealersHelper.Database.Objects;
using AutoDealersHelper.TelegramBot.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutoDealersHelper.TelegramBot.Setters
{
    public class CitiesSetter : AbstractSetter
    {
        public override ChatStates RequiredStateForRun => ChatStates.S_SET_CITY;

        public override AbstractCommand NextCommand => new FilterSettingCommand();

        public override void Action(User user, string text, BotDbContext db)
        {
            List<int> indexes = GetListIndexesFromString(text);
            List<City> citiesDbSet = new List<City>();

            var states = user.Filter.States;
            foreach (var n in states)
            {
                var state = db.States.First(x => x.Number == n.Number);
                citiesDbSet.AddRange(db.Cities.Where(x => x.ParrentId == n.Id).ToList());
            }
            List<BaseType> cities = GetCollectionOfItemByIndexes<City>(indexes, citiesDbSet);

            Filter filter = user.Filter;
            filter.Cities = cities;
            user.Filter = filter;
        }
    }
}
