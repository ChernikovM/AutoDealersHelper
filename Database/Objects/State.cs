using System.Collections.Generic;

namespace AutoDealersHelper.Database.Objects
{
    public class State : BaseType
    {
        public ICollection<City> Cities { get; set; }

        public State() : base()
        { }
    }
}
