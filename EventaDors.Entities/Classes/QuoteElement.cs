using System;

namespace EventaDors.Entities.Classes
{
    public class QuoteElement : MetaDataSupport
    {
        public QuoteElementType Type { get; set; }
        public string Name { get; set; }
        public string Notes { get; set; }
        
        public string UnderlyingElementNotes { get; set; }
        public double? BudgetTolerance { get; set; }
        public double Quantity { get; set; }
        public int Id { get; set; }
        
        public bool Exclude { get; set; }
        public int? LeadWeeks { get; set;}
        public DateTime? Submitted { get; set; }
        public bool InheritTopLevelQuantity { get; set; }
    }
}