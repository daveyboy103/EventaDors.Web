using System;

namespace EventaDors.Entities.Classes
{
    public class QuoteElement : MetaDataSupport
    {
        public QuoteElementType Type { get; set; }
        public string Name { get; set; }
        public string Notes { get; set; }
        public double? BudgetTolerance { get; set; }
        public double Quantity { get; set; }
        public int Id { get; set; }
        public int? LeadWeeks { get; set;}
        public DateTime Created { get; set; }
        public DateTime? Modified { get; set; }
        public DateTime? Submitted { get; set; }
        public bool InheritTopLevelQuantity { get; set; }
    }

    public class QuoteRequestElement : QuoteElement
    {
        public int? QuoteRequestElementId { get; set; }
        public Guid? QuoteId { get; set; }
        public DateTime? DueDate { get; set; }
        public bool Completed { get; set; }
        public bool Exclude { get; set; }
        public double? Budget { get; set; }
    }
}