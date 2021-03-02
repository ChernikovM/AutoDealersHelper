using AutoDealersHelper.Database;
using AutoDealersHelper.Database.Objects;
using AutoDealersHelper.TelegramBot.Commands;
using System;
using System.Collections.Generic;

namespace AutoDealersHelper.TelegramBot.Setters
{
    public class YearSetter : AbstractSetter
    {
        public override ChatStates RequiredStateForRun => ChatStates.S_SET_YEAR;

        public override AbstractCommand NextCommand => new FilterSettingCommand();

        public override void Action(User user, string text, BotDbContext db)
        {
            List<int> pair = GetIntPairFromString(text);

            Filter filter = user.Filter;
            filter.Year = pair;
            user.Filter = filter;
        }
    }
}
