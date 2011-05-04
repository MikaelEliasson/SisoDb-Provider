using System;

namespace SisoDb.Tests.IntegrationTests.Providers.Sql2008Provider.UnitOfWork.TestModel
{
    [Serializable]
    public class Address
    {
        public string Street { get; set; }

        public string Zip { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public int? AreaCode { get; set; }
    }
}