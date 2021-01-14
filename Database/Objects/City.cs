using Newtonsoft.Json;

namespace AutoDealersHelper.Database.Objects
{
    public class City : BaseType, IDependentType<State>
    {
        public int ParrentId { get; set; }
        [JsonIgnore]
        public State Parrent { get; set; }

        public City() : base()
        { }
    }
}
