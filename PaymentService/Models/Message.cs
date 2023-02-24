﻿namespace PaymentService.Models
{
    public class Message
    {
        public Guid Id { get; set; }
        public Command Command { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public DateTime CreatedOn { get; set; }

    }
}