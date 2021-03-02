using AutoDealersHelper.Database;
using AutoDealersHelper.Database.Objects;
using AutoDealersHelper.TelegramBot.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutoDealersHelper.TelegramBot.Setters
{
    public class StatesSetter : AbstractSetter
    {
        public override ChatStates RequiredStateForRun => ChatStates.S_SET_STATE;

        public override AbstractCommand NextCommand => new FilterSettingCommand();

        public override void Action(User user, string text, BotDbContext db)
        {
            List<int> indexes = GetListIndexesFromString(text);

            List<BaseType> states = GetCollectionOfItemByIndexes(indexes, db.States);

            Filter filter = user.Filter;
            filter.States = states;
            user.Filter = filter;
        }
    }
}
