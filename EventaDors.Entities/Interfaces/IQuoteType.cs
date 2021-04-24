using System.Collections.Generic;

namespace EventaDors.Entities.Interfaces
{
    public interface IQuoteType
    {
        string Name { get; }
        string Notes { get; }
        string Link { get; }
        IList<IQuoteSubType> SubTypes { get; }
    }
}