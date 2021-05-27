using System;
using System.Collections.Generic;
using Indigox.Common.NHibernate.Extension.Serialization;
using Indigox.UUM.Sync.Contexts;
using NUnit.Framework;

namespace Indigox.UUM.Sync.Tests.Serialization
{
    [TestFixture]
    public class AnyTypeSerializationTest
    {
        [Test]
        public void TestSerializeHashSyncContext()
        {
            HashSyncContext context = new HashSyncContext();
            context.Set( "k1", 100 );
            context.Set( "k2", "hello!" );
            context.Set( "k3", new Dictionary<string, object>()
            {
                { "k1", 100 },
                { "k2", "hello!" },
                { "k4", null }
            } );
            context.Set( "k4", null );
            context.Set( "k5", new List<string>() { "abc", "def" } );

            string xml = AnyTypeXmlSerializer.Serialize( context );

            Console.WriteLine( xml );

            HashSyncContext restored = (HashSyncContext)AnyTypeXmlSerializer.Deserialize( xml );

            Assert.AreEqual( restored[ "k1" ], 100 );
            Assert.AreEqual( restored[ "k2" ], "hello!" );
            Assert.AreEqual( ( (Dictionary<string, object>)restored[ "k3" ] )[ "k1" ], 100 );
            Assert.AreEqual( ( (Dictionary<string, object>)restored[ "k3" ] )[ "k2" ], "hello!" );
            Assert.AreEqual( restored[ "k4" ], null );
            Assert.AreEqual( ( (List<string>)restored[ "k5" ] )[ 0 ], "abc" );
            Assert.AreEqual( ( (List<string>)restored[ "k5" ] )[ 1 ], "def" );
        }
    }
}