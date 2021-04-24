using System;
using System.Collections.Generic;

namespace EventaDors.Entities.Classes
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