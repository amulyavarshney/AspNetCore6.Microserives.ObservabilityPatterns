using PaymentService.Models;

namespace PaymentService.ViewModels
{
    public class PaymentViewModel
    {
        public int Id { get; set; }
        public string InvoiceNumber { get; set; }
        public string GSTIN { get; set; }
        public PaymentStatus Status { get; set; }
        public Gateway Gateway { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
