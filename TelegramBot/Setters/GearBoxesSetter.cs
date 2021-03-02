using AutoDealersHelper.Database;
using AutoDealersHelper.Database.Objects;
using AutoDealersHelper.TelegramBot.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutoDealersHelper.TelegramBot.Setters
{
    public class GearBoxesSetter : AbstractSetter
    {
        public override ChatStates RequiredStateForRun => ChatStates.S_SET_GEARBOX;

        public override AbstractCommand NextCommand => new FilterSettingCommand();

        public override void Action(User user, string text, BotDbContext db)
        {
            List<int> indexes = GetListIndexesFromString(text);

            List<BaseType> gearboxes = GetCollectionOfItemByIndexes(indexes, db.GearBoxes);

            Filter filter = user.Filter;
            filter.GearBoxes = gearboxes;
            user.Filter = filter;
        }
    }
}
