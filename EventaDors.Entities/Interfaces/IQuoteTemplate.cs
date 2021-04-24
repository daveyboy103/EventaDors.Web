using System;
using System.Collections.Generic;

namespace EventaDors.Entities.Interfaces
{
    public interface IQuoteTemplate
    {
        string Name { get; }
        string Notes { get; }
        IQuoteType Type { get; }
        DateTime Created { get; }
        DateTime Modified { get; }
        IList<IQuoteElement> Elements { get; }
    }
}