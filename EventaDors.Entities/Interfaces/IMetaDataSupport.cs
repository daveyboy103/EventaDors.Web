using System.Collections.Generic;

namespace EventaDors.Entities.Interfaces
{
    public interface IMetaDataSupport
    {
        IDictionary<string, IMetaDataItem> MetaData { get; }
    }
}