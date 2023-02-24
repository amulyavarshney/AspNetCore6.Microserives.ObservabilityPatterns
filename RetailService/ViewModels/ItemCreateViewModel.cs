namespace RetailService.ViewModels
{
    public class ItemCreateViewModel
    {
        public string Name { get; set; }
        public string? Description { get; set; } = null;
        public decimal BasePrice { get; set; }
        public int Quantity { get; set; }
        public decimal TaxRate { get; set; }
    }
}
