namespace PaymentService.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public string InvoiceNumber { get; set; }
        public string GSTIN { get; set; }
        public PaymentStatus Status { get; set; }
        public Gateway Gateway { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
