using EventaDors.Entities.Interfaces;

namespace EventaDors.Entities.Classes
{
    public class MetaDataItem : CreatedModifiedBase
    {
        public string Name { get; init; }
        public string Value { get; set; }
        /// <summary>
        /// When a meta data item is used on a QuoteElement then
        ///The blank meta sata items are also copied to the quote request
        /// items. If this is set to true the user must supply a value for the
        /// item before they can submit for quote.
        /// </summary>
        public bool MandatoryWhenUsed { get; set; }
        public MetaDataType Type { get; set; }
    }
}