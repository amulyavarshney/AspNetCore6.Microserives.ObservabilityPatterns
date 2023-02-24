using PaymentService.Models;
using System.ComponentModel.DataAnnotations;

namespace PaymentService.ViewModels
{
    public class PaymentCreateViewModel
    {
        [Required]
        public string InvoiceNumber { get; set; }

        [Required]
        public string GSTIN { get; set; }
        public PaymentStatus Status { get; set; } = PaymentStatus.Pending;
        public Gateway Gateway { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
