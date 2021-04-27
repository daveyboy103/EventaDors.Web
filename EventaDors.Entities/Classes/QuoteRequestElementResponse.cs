using System;
using System.Collections.Generic;

namespace EventaDors.Entities.Classes
{
    public class QuoteRequestElementResponse : MetaDataSupport
    {
        public QuoteRequestElementResponse()
        {
            Images = new List<string>();
            ChatHistory = new List<Chat>();
        }

        public int Id { get; set; }
        public User Owner { get; set; }
        public bool Accepted { get; set; }
        public bool Estimate { get; set; }
        public double? AmountLow { get; set; }
        public double? AmountHigh { get; set; }
        public string Notes { get; set; }
        public string Link { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public DateTime? Submitted { get; set; }
        public QuoteElement ParentElement { get; set; }
        public List<string> Images { get; set; }
        public List<Chat> ChatHistory { get; }
    }
}