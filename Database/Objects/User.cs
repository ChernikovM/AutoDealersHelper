using AutoDealersHelper.Exceptions;
using AutoDealersHelper.TelegramBot.Commands;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;

namespace AutoDealersHelper.Database.Objects
{
    public class User
    {
        public int Id { get; set; }
        public long ChatId { get; set; }
        public string ChatStateString { get; set; }
        public string FilterString { get; set; }

        [NotMapped]
        public Filter Filter 
        {
            get
            {
                Filter filter = JsonConvert.DeserializeObject<Filter>(FilterString, new JsonSerializerSettings() { ObjectCreationHandling = ObjectCreationHandling.Replace });
                return filter;
            }
            set
            {
                using BotDbContext db = new BotDbContext();
                db.Users.FirstAsync(x => x.ChatId == ChatId).Result.FilterString = JsonConvert.SerializeObject(value);
                db.SaveChangesAsync();
            }
        }

        [NotMapped]
        public ChatStates ChatState 
        {
            get
            {
                if (Enum.TryParse(ChatStateString, out ChatStates chatState) == false)
                    throw new InvalidChatStateException();

                return chatState;
            }
            set
            {
                using BotDbContext db = new BotDbContext();
                db.Users.FirstAsync(x => x.ChatId == ChatId).Result.ChatStateString = value.ToString();
                db.SaveChangesAsync();
            }
        }

        public User(long chatId)
        {
            ChatId = chatId;
            FilterString = JsonConvert.SerializeObject( new Filter() );
        }

        public User()
        { 
        
        }
    }
}
