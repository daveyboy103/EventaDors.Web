using System.Collections.Generic;
using EventaDors.Entities.Interfaces;

namespace EventaDors.Entities.Classes
{
    public abstract class MetaDataSupport 
    {
        protected MetaDataSupport()
        {
            MetaData = new Dictionary<string, MetaDataItem>();
        }
        public IDictionary<string, MetaDataItem> MetaData { get; }
    }

    public class MetaDataItem
    {
        public string Name { get; }
        public string Value { get; }
        public MetaDataType Type { get; }
    }
}