using AutoDealersHelper.Database;
using AutoDealersHelper.Database.Objects;
using AutoDealersHelper.TelegramBot.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutoDealersHelper.TelegramBot.Setters
{
    public class PriceSetter : AbstractSetter
    {
        public override ChatStates RequiredStateForRun => ChatStates.S_SET_PRICE;

        public override AbstractCommand NextCommand => new FilterSettingCommand();

        public override void Action(User user, string text, BotDbContext db)
        {
            List<int> pair = GetIntPairFromString(text);

            Filter filter = user.Filter;
            filter.Price = pair;
            user.Filter = filter;
        }
    }
}
