using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoDealersHelper.Database.Objects
{
    public class Model : BaseType, IDependentType<Brand>
    {
        public int ParrentId { get; set; }
        [JsonIgnore]
        public virtual Brand Parrent { get; set; }

        public Model() : base()
        { }
    }
}
