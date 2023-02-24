using Microsoft.EntityFrameworkCore;
using PaymentService.Context;
using PaymentService.Models;
using PaymentService.MQ;
using PaymentService.ViewModels;

namespace PaymentService.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly PaymentContext _context;
        private readonly IRabbitMQManager _rabbitMQManager;
        public PaymentService(PaymentContext context, IRabbitMQManager rabbitMQManager)
        {
            _context = context;
            _rabbitMQManager = rabbitMQManager;
        }
        public async Task<IEnumerable<PaymentViewModel>> GetAsync()
        {
            return await _context.Payments.Select(payment => new PaymentViewModel
            {
                Id = payment.Id,
                InvoiceNumber = payment.InvoiceNumber,
                GSTIN = payment.GSTIN,
                Gateway = payment.Gateway,
                Status = payment.Status,
                Timestamp = payment.Timestamp,
            }).ToListAsync();
        }
        public async Task<PaymentViewModel> GetByIdAsync(int id)
        {
            var payment = await GetPaymentAsync(id);
            PublishMessage(Command.Get, $"Get Payment", payment);
            return ToViewModel(payment);
        }
        public async Task<PaymentViewModel> CreateAsync(PaymentCreateViewModel payment)
        {
            var paymentEntity = new Payment
            {
                InvoiceNumber = payment.InvoiceNumber,
                GSTIN = payment.GSTIN,
                Gateway = payment.Gateway,
                Status = payment.Status,
                Timestamp = payment.Timestamp,
            };
            await _context.AddAsync(paymentEntity);
            await _context.SaveChangesAsync();

            PublishMessage(Command.Post, $"New Payment with {paymentEntity.Gateway}", paymentEntity);
            return ToViewModel(paymentEntity);
        }
        public async Task<PaymentViewModel> UpdateAsync(int id, PaymentStatus status)
        {
            var payment = await GetPaymentAsync(id);
            payment.Status = status;
            await _context.SaveChangesAsync();

            PublishMessage(Command.Put, "Update Payment", payment);
            return ToViewModel(payment);
        }

        private async Task<Payment> GetPaymentAsync(int id)
        {
            var payment = await _context.Payments.FirstOrDefaultAsync(p => p.Id == id);
            if (payment is null)
            {
                throw new Exception($"Could not find the Payment with id: {id}");
            }
            return payment;
        }

        private PaymentViewModel ToViewModel(Payment payment)
        {
            return new PaymentViewModel
            {
                Id = payment.Id,
                InvoiceNumber = payment.InvoiceNumber,
                GSTIN = payment.GSTIN,
                Gateway = payment.Gateway,
                Status = payment.Status,
                Timestamp = payment.Timestamp,
            };
        }

        //  Send message to Message Service
        private void PublishMessage(Command command, string title, Payment payment)
        {
            var messageModel = new Message
            {
                Id = Guid.NewGuid(),
                Command = command,
                Title = title,
                Body = $"Payment with id: {payment.Id} is {payment.Status} at {payment.Timestamp}",
                CreatedOn = DateTime.UtcNow,
            };
            _rabbitMQManager.Publish(messageModel, "ms-exchange", "topic", "ms-b-routing");
        }
    }
}
