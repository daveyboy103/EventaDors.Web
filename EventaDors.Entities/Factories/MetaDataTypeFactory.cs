using System;
using EventaDors.Entities.Interfaces;

namespace EventaDors.Entities.Factories
{
    public class MetaDataTypeFactory
    {
        public Type GetType(MetaDataType sourceType)
        {
            Type ret = null;

            switch (sourceType)
            {
                case MetaDataType.Boolean:
                    ret = typeof(bool);
                    break;
                case MetaDataType.String:
                    ret = typeof(string);
                    break;
                case MetaDataType.Double:
                    ret = typeof(double);
                    break;
                case MetaDataType.Long:
                    ret = typeof(long);
                    break;
                case MetaDataType.Integer:
                    ret = typeof(int);
                    break;
                case MetaDataType.Date:
                    ret = typeof(DateTime);
                    break;
            }

            return ret;
        }
    }
}