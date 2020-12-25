namespace AutoDealersHelper.Database.Objects
{
    public class City : BaseType, IDependentType<State>
    {
        /*
        public int StateId { get; set; }

        public virtual State State { get; set; }
        */
        public int ParrentId { get; set; }
        public State Parrent { get; set; }
    }
}
