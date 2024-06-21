namespace Order.API.Models.ViewModels
{
    public class CreateOrderVM
    {
        public Guid BuyerID { get; set; }
        public List<CreateOrderItemVM> OrderItems { get; set; }
    }

}
