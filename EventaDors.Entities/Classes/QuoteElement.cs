using System;

namespace EventaDors.Entities.Classes
{
    public class QuoteElement
    {
        public QuoteElementType Type { get; set; }
        public string Name { get; set; }
        public string Notes { get; set; }
        public double? Budget { get; set; }
        public double? BudgetTolerance { get; set; }
        public double Quantity { get; set; }
        public int Id { get; set; }
        public int? LeadWeeks { get; set;}
        public bool Completed { get; set; }
        public DateTime? DueDate { get; set; }
        public bool Exclude { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Modified { get; set; }
        public DateTime? Submitted { get; set; }
    }
}