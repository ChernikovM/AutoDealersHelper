using AutoDealersHelper.Database;
using AutoDealersHelper.Database.Objects;
using AutoDealersHelper.TelegramBot.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace AutoDealersHelper.TelegramBot.Setters
{
    public class SetBrands : ISetter
    {
        public ChatStates RequiredState => ChatStates.S_SET_BRAND;

        public AbstractCommand NextCommand => new FilterSettingCommand();

        public void Action(Database.Objects.User user, List<int> indexes, BotDbContext db)
        {
            List<int> brandsNumbers = new List<int>();
            foreach (var n in indexes)
            {
                brandsNumbers.Add( db.Brands.First( x => x.Id == n).Number);
            }

            db.Users.First(x => x.ChatId == user.ChatId).Brand = this.SerializeFromListToJson(brandsNumbers);
        }

        public bool Validate(Database.Objects.User user, List<int> indexes, BotDbContext db)
        {
            foreach (int n in indexes)
            {
                if (n > BotDbContext.BrandsCount)
                    return false;
            }

            return true;
        }
    }
}
