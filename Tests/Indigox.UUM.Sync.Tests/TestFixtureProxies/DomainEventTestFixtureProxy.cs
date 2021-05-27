using System;
using Indigox.Common.EventBus.Configuration;
using Indigox.TestUtility.TestFixtures;

namespace Indigox.UUM.Sync.Tests.TestFixtureProxies
{
    public class DomainEventTestFixtureProxy : BaseTestFixtureProxy
    {
        public override void OnTestFixtureSetUp()
        {
            RegisteEvents();
        }

        private void RegisteEvents()
        {
            var configurator = new XmlEventsConfigurator();
            configurator.Configure();
        }
    }
}