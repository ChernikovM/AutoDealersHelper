namespace AutoDealersHelper.Database.Objects
{
    interface IDependentType<T> where T: BaseType
    {
        int ParrentId { get; set; }
        T Parrent { get; set; }
    }
}
