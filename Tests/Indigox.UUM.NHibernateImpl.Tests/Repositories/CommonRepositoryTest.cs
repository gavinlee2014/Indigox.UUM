using System;
using NUnit.Framework;

namespace Indigox.UUM.NHibernateImpl.NUnitTest.Repositories
{
    [TestFixture]
    public class CommonRepositoryTest : BaseRepositoryTestFixture
    {
        protected override void ClearTestData()
        {
            // ignore
        }

        [Test]
        public void CreateDatabaseSchema()
        {
        }
    }
}