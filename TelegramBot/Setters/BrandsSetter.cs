using AutoDealersHelper.Database;
using AutoDealersHelper.Database.Objects;
using AutoDealersHelper.TelegramBot.Commands;
using System.Collections.Generic;

namespace AutoDealersHelper.TelegramBot.Setters
{
    public class BrandsSetter : AbstractSetter
    {
        public override ChatStates RequiredStateForRun => ChatStates.S_SET_BRAND;
        public override AbstractCommand NextCommand => new FilterSettingCommand();
        public override void Action(User user, string text, BotDbContext db)
        {
            List<int> indexes = GetListIndexesFromString(text);

            List<BaseType> brands = GetCollectionOfItemByIndexes<Brand>(indexes, db.Brands);

            Filter filter = user.Filter;
            filter.Brands = brands;
            filter.Models = new List<BaseType>() { new BaseType() };
            user.Filter = filter;
        }
    }
}
