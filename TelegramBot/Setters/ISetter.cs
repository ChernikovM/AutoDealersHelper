using AutoDealersHelper.Database;
using AutoDealersHelper.Database.Objects;
using AutoDealersHelper.TelegramBot.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace AutoDealersHelper.TelegramBot.Setters
{
    public interface ISetter
    {
        ChatStates RequiredState { get; }
        AbstractCommand NextCommand { get; }

        bool Validate(Database.Objects.User user, List<int> list, BotDbContext db);

        async Task<Message> Run(Database.Objects.User user, string text, TelegramBotClient client)
        {
            List<int> indexes = this.GetIndexesListFromString(text);

            using (BotDbContext db = new BotDbContext())
            {
                if (Validate(user, indexes, db) == false)
                    throw new ArgumentException(); //TODO: InvalidArgumentException

                Action(user, indexes, db);
                await db.SaveChangesAsync();
            }

            return await (NextCommand).Run(user, client);
        }

        void Action(Database.Objects.User user, List<int> indexes, BotDbContext db);
    }
}
