using System.Collections.Generic;

namespace AutoDealersHelper.Database.Objects
{
    public class Brand : BaseType
    {
        public ICollection<Model> Models { get; set; }
    }
}
