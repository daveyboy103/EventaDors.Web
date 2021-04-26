namespace EventaDors.Entities.Interfaces
{
    public interface IMetaDataItem
    {
        string Name { get; }
        string Value { get; }
        MetaDataType Type { get; }
    }
}