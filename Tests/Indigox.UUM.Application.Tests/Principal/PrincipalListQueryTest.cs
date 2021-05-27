using System;
using System.IO;
using Indigox.Common.NHibernateFactories.Configuration;
using Indigox.UUM.Application.Principal;
using NUnit.Framework;

namespace Indigox.UUM.Application.Test.Principal
{
    [TestFixture]
    public class PrincipalListQueryTest
    {
        //by Yi
        //[Test]
        public void TestList()
        {
            PrincipalListQuery query = new PrincipalListQuery();
            query.List();
        }

        [Test]
        public void Pass()
        {

        }

        [SetUp]
        protected void SetUp()
        {
            string fullpath = Path.Combine( AppDomain.CurrentDomain.BaseDirectory, "config\\factories.xml" );
            XmlConfigurator configurator = new XmlConfigurator( fullpath );
            configurator.Configure();
        }
    }
}