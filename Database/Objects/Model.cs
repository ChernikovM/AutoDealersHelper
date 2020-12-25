namespace AutoDealersHelper.Database.Objects
{
    public class Model : BaseType, IDependentType<Brand>
    {
        /*
        public int BrandId { get; set; }
        public virtual Brand Brand { get; set; }
        */
        public int ParrentId { get; set; }
        public virtual Brand Parrent { get; set; }
    }
}
