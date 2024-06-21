namespace Order.API.Models.ViewModels
{
    public class CreateOrderItemVM
    {

        public string ProductId { get; set; }
        public int Count { get; set; }
        public decimal Price { get; set; }
    }
}
