using System;
using EventaDors.Entities.Factories;
using EventaDors.Entities.Interfaces;
using NUnit.Framework;

namespace EventaDors.Test
{
    [TestFixture]
    public class MetaDataTests
    {
        [Test]
        public void ShouldRetrieveCorrectTypeFromFactory()
        {
            var factory = new MetaDataTypeFactory();
            Assert.AreEqual(typeof(System.Boolean), factory.GetType(MetaDataType.Boolean));
            Assert.AreEqual(typeof(System.Int64), factory.GetType(MetaDataType.Long));
            Assert.AreEqual(typeof(System.String), factory.GetType(MetaDataType.String));
            Assert.AreEqual(typeof(System.Int32), factory.GetType(MetaDataType.Integer));
            Assert.AreEqual(typeof(System.DateTime), factory.GetType(MetaDataType.Date));
            Assert.AreEqual(typeof(System.Double), factory.GetType(MetaDataType.Double));
        }
    }
}