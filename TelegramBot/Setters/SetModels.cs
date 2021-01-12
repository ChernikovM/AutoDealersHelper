using AutoDealersHelper.Database;
using AutoDealersHelper.Database.Objects;
using AutoDealersHelper.TelegramBot.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutoDealersHelper.TelegramBot.Setters
{
    public class SetModels : ISetter
    {
        public ChatStates RequiredState => ChatStates.S_SET_MODEL;

        public AbstractCommand NextCommand => new FilterSettingCommand();

        public void Action(User user, List<int> indexes, BotDbContext db)
        {
            List<int> brandsNumbers = this.DeserializeFromJsonToList(user.Brand);
            List<Model> allModels = new List<Model>();
            foreach (int brandNum in brandsNumbers)
            {
                allModels.AddRange(db.Brands.First(brand => brand.Number == brandNum).Models.ToList());
            }

            user = db.Users.First(x => x.ChatId == user.ChatId);

            List<int> userModels = new List<int>();

            foreach (int index in indexes)
            {
                userModels.Add(allModels[index].Number);
            }

            user.Model = this.SerializeFromListToJson(userModels);
        }

        public bool Validate(User user, List<int> list, BotDbContext db)
        {
            List<int> brands = this.DeserializeFromJsonToList(user.Brand);
            int countOfModels = 0;

            foreach (int brandNumber in brands)
                countOfModels += db.Brands.First(x => x.Number == brandNumber).Models.Count;

            foreach (int n in list)
            {
                if (n > countOfModels)
                    return false;
            }

            return true;
        }
    }
}
