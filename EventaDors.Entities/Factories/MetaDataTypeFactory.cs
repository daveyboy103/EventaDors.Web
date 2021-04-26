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
                    ret = typeof(System.Boolean);
                    break;                
                case MetaDataType.String:
                    ret = typeof(System.String);
                    break;
                case MetaDataType.Double:
                    ret = typeof(System.Double);
                    break;
                case MetaDataType.Long:
                    ret = typeof(System.Int64);
                    break;               
                case MetaDataType.Integer:
                    ret = typeof(System.Int32);
                    break;
                case MetaDataType.Date:
                    ret = typeof(System.DateTime);
                    break;
            }

            return ret;
        }
    }
}