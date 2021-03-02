using AutoDealersHelper.Exceptions;
using AutoDealersHelper.Parsers;
using AutoDealersHelper.TelegramBot.Commands;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

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
                this.FilterString = JsonConvert.SerializeObject(value);
                db.Users.FirstAsync(x => x.ChatId == ChatId).Result.FilterString = this.FilterString;
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

        public User Update()
        {
            using var db = new BotDbContext();

            var user = db.Users.First(x => x.ChatId == this.ChatId);
            user = this;

            db.SaveChanges();

            return this;
        }

        public bool CheckAdvertisement(Advertisement ad)
        {
            if (this.Filter.Brands.Exists(x => x.Number == ad.Brand || x.Number == 0))
            {
                if (this.Filter.Models.Exists(x => x.Number == ad.Model || x.Number == 0))
                {
                    if (this.Filter.States.Exists(x => x.Number == ad.StateData.State || x.Number == 0))
                    {
                        if (this.Filter.Cities.Exists(x => x.Number == ad.StateData.City || x.Number == 0))
                        {
                            if (this.Filter.GearBoxes.Exists(x => x.Number == ad.AutoData.GearBox || x.Number == 0))
                            {
                                if (this.Filter.Fuels.Exists(x => x.Number == ad.AutoData.Fuel || x.Number == 0))
                                {
                                    if ( (this.Filter.Year.Count > 1 && (ad.AutoData.Year >= this.Filter.Year[0] && ad.AutoData.Year <= this.Filter.Year[1])) || (this.Filter.Year.Count == 1 && this.Filter.Year[0] == 0))
                                    {
                                        if ((this.Filter.Mileage.Count > 1 && (ad.AutoData.Mileage >= this.Filter.Mileage[0] && ad.AutoData.Mileage <= this.Filter.Mileage[1])) || (this.Filter.Mileage.Count == 1 &&  this.Filter.Mileage[0] == 0))
                                        {
                                            if ((this.Filter.Price.Count > 1 && (ad.Price >= this.Filter.Price[0] && ad.Price <= this.Filter.Price[1])) || (this.Filter.Price.Count == 1 &&  this.Filter.Price[0] == 0))
                                            {
                                                return true;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }
    }
}
