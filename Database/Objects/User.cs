using System.ComponentModel.DataAnnotations;

namespace AutoDealersHelper.Database.Objects
{
    public class User
    {
        public int Id { get; set; }
        public long ChatId { get; set; }

        public string Brand { get; set; }
        public string Model { get; set; }
        public string State {get; set;}
        public string City { get; set; }
        public string Fuel { get; set; }
        public string GearBox { get; set; }
        public string Year { get; set; }
        public string Price { get; set; }
        public string Mileage { get; set; }
        public string Volume { get; set; }

        public string ChatStateId { get; set; }

    }
}
