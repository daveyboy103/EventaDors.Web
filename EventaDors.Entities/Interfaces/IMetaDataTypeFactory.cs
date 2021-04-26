using System;
using System.Collections;

namespace EventaDors.Entities.Interfaces
{
    public interface IMetaDataTypeFactory
    {
        Type GetType(MetaDataType sourceType);
    }
}