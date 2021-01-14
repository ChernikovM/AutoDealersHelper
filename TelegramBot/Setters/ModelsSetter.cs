using AutoDealersHelper.Database;
using AutoDealersHelper.Database.Objects;
using AutoDealersHelper.TelegramBot.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutoDealersHelper.TelegramBot.Setters
{
    public class ModelsSetter : AbstractSetter
    {
        public override ChatStates RequiredStateForRun => ChatStates.S_SET_MODEL;
        public override AbstractCommand NextCommand => new FilterSettingCommand();
        public override void Action(User user, string text, BotDbContext db)
        {
            List<int> indexes = GetListIndexesFromString(text);
            List<Model> modelsDbSet = new List<Model>();

            var brands = user.Filter.Brands;
            foreach (var n in brands)
            {
                var brand = db.Brands.First(x => x.Number == n.Number);
                modelsDbSet.AddRange(db.Models.Where(x => x.ParrentId == n.Id).ToList());
            }
            List<BaseType> models = GetCollectionOfItemByIndexes<Model>(indexes, modelsDbSet);

            Filter filter = user.Filter;
            filter.Models = models;
            user.Filter = filter;
        }
    }
}
