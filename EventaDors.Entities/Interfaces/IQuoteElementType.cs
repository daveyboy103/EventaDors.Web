using System;
using System.Collections.Generic;

namespace EventaDors.Entities.Interfaces
{
    public interface IQuoteElementType
    {
        string Name { get; }
        string Notes { get; }
        string Link { get; }
        int Id { get; }
    }

    public interface IQuote
    {
        IUser Owner { get; }
        DateTime Created { get; }
        DateTime Modified { get; }
        DateTime Submitted { get; }
        string Notes { get; }
        IQuoteType Type { get; }
        IList<IQuoteElement> Elements { get; }
    }
}