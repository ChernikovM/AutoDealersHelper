using AutoDealersHelper.TelegramBot.Commands;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AutoDealersHelper.Database.Objects
{
    public class User
    {
        public int Id { get; set; }
        public long ChatId { get; set; }
        public string ChatStateId { get; set; }

        public string Brand { get; set; }
        public string Model { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Fuel { get; set; }
        public string GearBox { get; set; }
        public string Year { get; set; }
        public string Price { get; set; }
        public string Mileage { get; set; }
        public string Volume { get; set; }

        public User(long chatId)
        {
            ChatId = chatId;
            Brand = "[0]";
            Model = "[0]";
            State = "[0]";
            City = "[0]";
            Fuel = "[0]";
            GearBox = "[0]";
            Year = "[0]";
            Price = "[0]";
            Mileage = "[0]";
            Volume = "[0]";
        }

        public ChatStates GetChatStateId()
        {
            if (Enum.TryParse(ChatStateId, out ChatStates chatState) == false) //TODO: вернуть сообщение с ошибкой и просьбой перезагрузить бота
                throw new ArgumentException(); //TODO: InvalidStateIdException

            return chatState;
        }
        public async Task<int> SetChatState(ChatStates state)
        {
            using var db = new BotDbContext();

            db.Users.FirstAsync(x => x.ChatId == this.ChatId).Result.ChatStateId = state.ToString();
            return await db.SaveChangesAsync();
        }

        public List<int> GetBrands()
        {
            return DeserializeFromJsonToList(Brand);
        }
        public void SetBrands(List<int> list)
        {
            int maxValue = BotDbContext.BrandsCount;
            foreach (int n in list)
            {
                if (n > maxValue || n < 0) //TODO:   отловить аргументЭксепшн в сеттере и вернуть сообщение с текстом ошибки
                    throw new ArgumentException("Одно или несколько значений находится вне диапазона допустимых значений.");
            }
            Brand = SerializeFromListToJson(list);
        }

        public List<int> GetModels()
        {
            return DeserializeFromJsonToList(Brand);
        }

        public void SetModels(List<int> list)
        {
            int countOfModels = 0;
            using (BotDbContext db = new BotDbContext())
            {
                countOfModels = db.Models.Where(x => GetBrands().Exists(brandId => brandId == x.ParrentId)).ToList().Count();
            };

            foreach(int n in list)
            {
                if (n > countOfModels || n < 0)
                    throw new ArgumentException("Одно или несколько значений находится вне диапазона допустимых значений.");
            }
        }

        public List<int> GetStates()
        {
            return DeserializeFromJsonToList(State);
        }
        public void SetSates(List<int> list)
        {
            int maxValue = BotDbContext.StatesCount;
            foreach (int n in list)
            {
                if (n > maxValue || n < 0)
                    throw new ArgumentException();
            }
            State = SerializeFromListToJson(list);
        }
        public List<int> GetCities()
        {
            return DeserializeFromJsonToList(City);
        }
        public void SetCities(List<int> list)
        {
            int countOfCities;
            using (BotDbContext db = new BotDbContext())
            {
                countOfCities = db.Cities.Where(x => GetStates().Exists(stateId => stateId == x.ParrentId)).ToList().Count();
            }

            foreach (int n in list)
            {
                if (n > countOfCities || n < 0)
                    throw new ArgumentException();
            }

            City = SerializeFromListToJson(list);
        }

        public List<int> GetPrice()
        {
            return DeserializeFromJsonToList(Price);
        }

        public void SetPrice(List<int> list) //TODO: проследить чтобы передавалось только 2 числа в списке
        {
            Price = SerializeFromListToJson(list);
        }

        public List<int> GetVolume()
        {
            return DeserializeFromJsonToList(Volume);
        }
        public void SetVolume(List<int> list) // TODO: отдельный валидатор для объема чтобы можно было писать дроби
        {
            Volume = SerializeFromListToJson(list);
        }
        public List<int> GetGearboxes()
        {
            return DeserializeFromJsonToList(GearBox);
        }
        public void SetGearboxes(List<int> list)
        {
            int maxValue = BotDbContext.GearBoxesCount;
            foreach (var n in list)
            {
                if (n > maxValue || n < 0)
                    throw new ArgumentException();
            }

            GearBox = SerializeFromListToJson(list);
        }

        public List<int> GetMileage()
        {
            return DeserializeFromJsonToList(Mileage);
        }
        public void SetMileage(List<int> list)//TODO: убрать во всех сеттерах проверку на <0 т.к. валидатор заведомо не пропустит знак -
        {
            if (list[0] > list[1] || list[1] > 100000)
                throw new ArgumentException();

            Mileage = SerializeFromListToJson(list);
        }

        public List<int> GetYear()
        {
            return DeserializeFromJsonToList(Year);
        }
        public void SetYear(List<int> list)//TODO: сделать кастомный ексепшн на проверку годов
        {
            if (list[0] > list[1] || list[0] < 1900 || list[1] > DateTime.Now.Year)
                throw new ArgumentException();

            Year = SerializeFromListToJson(list);
        }

        public List<int> GetFuel()
        {
            return DeserializeFromJsonToList(Fuel);
        }
        public void SetFuel(List<int> list)//TODO: убрать проверку на ПЕРВЫЙ_ЭЛЕМЕНТ > ВТОРОЙ_ЭЛЕМЕНТ т.к. они сортируются по возрастанию в преобразователе
        {
            int maxValue = BotDbContext.FuelsCount;
            foreach (int n in list)
            {
                if (n > maxValue || n < 0)
                    throw new ArgumentException();
            }
            Fuel = SerializeFromListToJson(list);
        }

        private List<int> DeserializeFromJsonToList(string json)
        {
            if (json == null)
                return null;

            List<int> data;
            try
            {
                data = JsonConvert.DeserializeObject<List<int>>(json);
            }
            catch (Exception) //TODO: если ошибка сериализации - вернуть сообщение об ошибке пользователю и обнулить в базе значение
            {
                throw; //TODO: отловить ошибку выше
            }

            return data;
        }

        private static string SerializeFromListToJson(List<int> list)
        {
            string data;
            try
            {
                data = JsonConvert.SerializeObject(list);
            }
            catch (Exception) //если ошибка сериализации - вернуть сообщ пользователю с просьбой повторить ИЛИ присвоить значение 0
            {
                throw; //TODO: отловаить ошибку выше
            }
            return data;
        }

    }
}
