using System;
using Indigox.Common.Logging;
using Indigox.Common.StateContainer;

namespace Indigox.UUM.Sync.Web
{
    public class Initializer : IStateContextListener
    {
        #region IStateContextListener members

        public void OnApplicationBegin( IApplicationState application )
        {
            Initialize();
        }

        public void OnApplicationEnd( IApplicationState application )
        {
        }

        public void OnSessionBegin( ISessionState session )
        {
        }

        public void OnSessionEnd( ISessionState session )
        {
        }

        public void OnTransactionBegin( ITransactionState transaction )
        {
        }

        public void OnTransactionEnd( ITransactionState transaction )
        {
        }

        #endregion IStateContextListener members

        private void Initialize()
        {
            Log.Debug( "Indigox.UUM.Sync initialize begin..." );

            NHibernateFactoryInvoker.Instance.InitCurrentSession();

            SyncManager.RestoreFromRepository();

            Log.Debug( "Indigox.UUM.Sync initialize end." );
        }
    }
}