using AutoDealersHelper.Database;
using AutoDealersHelper.Database.Objects;
using AutoDealersHelper.TelegramBot.Commands;
using System.Collections.Generic;

//TODO: при вводе пары 0, 0 - искать авто без пробега
//      при вводе 0 - ставить значение любой

namespace AutoDealersHelper.TelegramBot.Setters
{
    public class MileageSetter : AbstractSetter
    {
        public override ChatStates RequiredStateForRun => ChatStates.S_SET_MILEAGE;

        public override AbstractCommand NextCommand => new FilterSettingCommand();

        public override void Action(User user, string text, BotDbContext db)
        {
            List<int> pair = GetIntPairFromString(text);

            Filter filter = user.Filter;
            filter.Mileage = pair;
            user.Filter = filter;
        }

    }
}
