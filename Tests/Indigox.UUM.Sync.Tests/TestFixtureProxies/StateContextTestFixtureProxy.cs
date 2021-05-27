using System;
using Indigox.Common.StateContainer;
using Indigox.Common.StateContainer.CurrentUserProviders;
using Indigox.TestUtility.TestFixtures;

namespace Indigox.UUM.Sync.Tests.TestFixtureProxies
{
    public class StateContextTestFixtureProxy : BaseTestFixtureProxy
    {
        public override void OnSetUp()
        {
            InitStateContainer();
        }

        public override void OnTearDown()
        {
            DisposeStateContainer();
        }

        private void InitStateContainer()
        {
            StateContext.Current.BeginApplication();
            StateContext.Current.BeginSession( new MutableCurrentUserProvider() );
            StateContext.Current.BeginTransaction();
        }

        private void DisposeStateContainer()
        {
            StateContext.Current.EndApplication();
            StateContext.Current.EndSession();
            StateContext.Current.EndTransaction();
        }
    }
}