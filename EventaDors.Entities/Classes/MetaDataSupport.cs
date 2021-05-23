using System;
using System.Collections.Generic;

namespace EventaDors.Entities.Classes
{
    public abstract class MetaDataSupport : CreatedModifiedBase
    {
        protected MetaDataSupport()
        {
            MetaData = new Dictionary<string, MetaDataItem>();
        }

        public IDictionary<string, MetaDataItem> MetaData { get; }
    }
}