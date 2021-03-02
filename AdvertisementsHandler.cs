using AutoDealersHelper.Database;
using AutoDealersHelper.Parsers;
using System.Collections.Generic;

namespace AutoDealersHelper
{
    public class AdvertisementsHandler
    {
        public static void Run(List<Advertisement> list)
        {
            using var db = new BotDbContext();

            foreach (var n in list)
            {
                foreach (var user in db.Users)
                {
                    if (user.CheckAdvertisement(n) == true)
                    {
                        Program.bot.SendAsvertisement(user.ChatId, "https://auto.ria.com/uk" + n.Link); //TODO: костыль... много костылей...
                    }
                }
            }
        }
    }
}
