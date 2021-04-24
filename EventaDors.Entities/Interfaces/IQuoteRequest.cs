using System;
using System.Collections.Generic;
using EventaDors.Entities.Classes;

namespace EventaDors.Entities.Interfaces
{
    public interface IQuoteRequest
    {
        Guid QuoteId { get; }
        string Name { get; }
        string Notes { get; }
        IQuoteType Type { get; }
        DateTime Created { get; }
        DateTime Submitted { get; }
        IList<IQuoteElement> Elements { get; }
    }
}