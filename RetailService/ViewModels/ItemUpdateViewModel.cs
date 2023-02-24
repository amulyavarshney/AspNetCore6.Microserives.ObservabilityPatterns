namespace RetailService.ViewModels
{
    public class ItemUpdateViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal BasePrice { get; set; }
        public int Quantity { get; set; }
        public decimal TaxRate { get; set; }
    }
}
