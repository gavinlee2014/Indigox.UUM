using System;
using System.IO;
using Indigox.Common.DomainModels.Configuration.Generator;
using Indigox.Common.Membership.Interfaces;
using Indigox.UUM.Factory;
using NUnit.Framework;

namespace Indigox.UUM.Test.Factory
{
    [TestFixture]
    public class GroupFactoryTest
    {
        [Test]
        public void TestCreate()
        {
            IGroup item = new GroupFactory().Create();
            Assert.NotNull( item );
        }

        [SetUp]
        protected void SetUp()
        {
            string fullpath = Path.Combine( AppDomain.CurrentDomain.BaseDirectory, "config\\DBIdGenertators.xml" );
            DBIdGenertatorConfigurator configurator = new DBIdGenertatorConfigurator( fullpath );
            configurator.Configure();
        }
    }
}