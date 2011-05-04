using System;
using Moq;
using NUnit.Framework;
using SisoDb.Core;
using SisoDb.Providers.DbSchema;
using SisoDb.Structures.Schemas;
using SisoDb.Tests.IntegrationTests.Providers.Sql2008Provider;

namespace SisoDb.Tests.IntegrationTests.Providers
{
    [TestFixture]
    public class DbSchemaManagerTests : SqlIntegrationTestBase
    {
        private IStructureSchema _structureSchema;

        protected override void OnFixtureInitialize()
        {
            DropStructureSet<Class_53966417_B25D_49E1_966B_58754110781C>();

            _structureSchema = Database.StructureSchemas.GetSchema(
                TypeFor<Class_53966417_B25D_49E1_966B_58754110781C>.Type);
        }

        protected override void OnTestFinalize()
        {
            DropStructureSet<Class_53966417_B25D_49E1_966B_58754110781C>();
        }

        [Test]
        public void UpsertStructureSet_WhenNeverCalled_UpserterIsCalledOnce()
        {
            var upserterFake = new Mock<IDbSchemaUpserter>();

            var manager = new DbSchemaManager();
            manager.UpsertStructureSet(_structureSchema, upserterFake.Object);

            upserterFake.Verify(f => f.Upsert(_structureSchema), Times.Once());
        }

        [Test]
        public void UpsertStructureSet_WhenCalledTwice_UpserterIsCalledOnceNotTwice()
        {
            var upserterFake = new Mock<IDbSchemaUpserter>();

            var manager = new DbSchemaManager();
            manager.UpsertStructureSet(_structureSchema, upserterFake.Object);
            manager.UpsertStructureSet(_structureSchema, upserterFake.Object);

            upserterFake.Verify(f => f.Upsert(_structureSchema), Times.Once());
        }

        [Test]
        public void DropStructureSet_WhenCalledTwice_DropperIsCalledTwice()
        {
            var dropperFake = new Mock<IDbSchemaDropper>();

            var manager = new DbSchemaManager();
            manager.DropStructureSet(_structureSchema, dropperFake.Object);
            manager.DropStructureSet(_structureSchema, dropperFake.Object);

            dropperFake.Verify(f => f.Drop(_structureSchema), Times.Exactly(2));
        }

        private class Class_53966417_B25D_49E1_966B_58754110781C
        {
            public Guid SisoId { get; set; }

            public string IndexableMember1 { get; set; }

            public int IndexableMember2 { get; set; }
        }
    }
}