using AutoDealersHelper.Database;
using AutoDealersHelper.Database.Objects;
using AutoDealersHelper.TelegramBot.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutoDealersHelper.TelegramBot.Setters
{
    public class VolumeSetter : AbstractSetter
    {
        public override ChatStates RequiredStateForRun => ChatStates.S_SET_VOLUME;

        public override AbstractCommand NextCommand => new FilterSettingCommand();

        public override void Action(User user, string text, BotDbContext db)
        {
            List<double> pair = GetDoublePairFromString(text);

            Filter filter = user.Filter;
            filter.Volume = pair;
            user.Filter = filter;
        }
    }
}
