using Newtonsoft.Json;

namespace AutoDealersHelper.Database.Objects
{
    public class BaseType
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("value")]
        public int Number { get; set; }
        public int Id { get; set; }

        public BaseType()
        {
            Id = -1;
            Number = 0;
            Name = "Любой";
        }
    }
}
