using AutoDealersHelper.Database;
using AutoDealersHelper.Database.Objects;
using AutoDealersHelper.TelegramBot.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutoDealersHelper.TelegramBot.Setters
{
    public class FuelsSetter : AbstractSetter
    {
        public override ChatStates RequiredStateForRun => ChatStates.S_SET_FUEL;

        public override AbstractCommand NextCommand => new FilterSettingCommand();

        public override void Action(User user, string text, BotDbContext db)
        {
            List<int> indexes = GetListIndexesFromString(text);

            List<BaseType> fuels = GetCollectionOfItemByIndexes(indexes, db.Fuels);

            Filter filter = user.Filter;
            filter.Fuels = fuels;
            user.Filter = filter;
        }
    }
}
