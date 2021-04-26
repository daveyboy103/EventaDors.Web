using System;

namespace EventaDors.Entities.Classes
{
    public class Chat
    {
        public string Message { get; set; }
        public User Sender { get; set; }
        public User Receiver { get; set; }
        public string Link { get; set; }
        public int SequenceId { get; set; }
        public Guid MessageId { get; set; }
        public QuoteRequestElementResponse QuoteRequestElementResponse { get; set; }
    }
}