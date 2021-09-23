using System;
using System.Data;

namespace EventaDors.Entities.Classes
{
    public class QuoteElement : MetaDataSupport
    {
        public QuoteElementType Type { get; init; }
        public string Name { get; init; }
        public string Notes { get; init; }
        public string UnderlyingElementNotes { get; set; }
        public double? BudgetTolerance { get; init; }
        public decimal? Budget { get; init; }
        public double Quantity { get; init; }
        public int Id { get; init; }
        public bool Exclude { get; init; }
        public int? LeadWeeks { get; init;}
        public DateTime? Submitted { get; set; }
        public bool InheritTopLevelQuantity { get; init; }
        public bool Completed { get; set; }
        public DateTime? DueDate { get; set; }
    }
}