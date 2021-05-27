using System;
using System.Collections.Generic;
using Indigox.Common.EventBus.Interface.Event;
using Indigox.Common.Membership.Interfaces;
using Indigox.UUM.Sync.Contexts;
using Indigox.UUM.Sync.Executors;
using Indigox.UUM.Sync.Interfaces;
using Indigox.UUM.Sync.Model;
using Indigox.UUM.Sync.Mail;
using Indigox.Common.Logging;

namespace Indigox.UUM.Sync.Tasks.Builders
{
    internal abstract class AbstractPrincipalEventTaskBuilder : ISyncTaskBuilder
    {
        private List<ISyncTask> dependencies;
        private IEvent evt;
        private IPrincipal source;
        private SysConfiguration sys;

        public List<ISyncTask> Dependencies
        {
            get { return dependencies; }
            set { dependencies = value; }
        }

        public IEvent Event
        {
            get { return evt; }
            set { evt = value; }
        }

        public IPrincipal Source
        {
            get { return source; }
            set { source = value; }
        }

        public SysConfiguration System
        {
            get { return sys; }
            set { sys = value; }
        }

        public ISyncTask Build()
        {
            if ( !SyncEnabled )
            {
                return null;
            }

            ISyncContext context = BuildSyncContext();
            ISyncExecutor executor = BuildSyncExecutor();

            ISyncTask task = new SyncTask();
            task.Tag = this.sys.ClientName;
            task.Description = this.evt.ToString();
            task.Dependencies = dependencies;
            task.Context = context;
            task.Executor = executor;

            //MailService mailService = new MailService();
            //mailService.SendMail(this.sys.Email, task);

            return task;
        }

        protected virtual ISyncContext BuildSyncContext()
        {
            ISyncContext context = new HashSyncContext();
            return context;
        }

        protected virtual ISyncExecutor BuildSyncExecutor()
        {
            ISyncExecutor executor = new WebServiceExecutor();
            return executor;
        }

        protected ExternalObjectDescriptor GetExternalObject( string internalID )
        {
            ExternalObjectDescriptor externalDescriptor = new ExternalObjectDescriptor()
            {
                InternalID=internalID,
                SysID=sys.ID
            };
            return externalDescriptor;
        }

        protected void SetExternalID( string internalID, string externalID )
        {
            SysKeyMappingService.Instance.SetExternalID( internalID, externalID, sys );
        }

        protected virtual bool SyncEnabled
        {
            get
            {
                return true;
            }
        }
        protected string GetExternalID(string internalID)
        {
            string externalID = SysKeyMappingService.Instance.GetExternalID(internalID, sys);
            return externalID;
        }
    }
}