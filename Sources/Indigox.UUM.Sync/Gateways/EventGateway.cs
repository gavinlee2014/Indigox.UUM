using System;
using System.Collections.Generic;
using Indigox.Common.DomainModels.Factory;
using Indigox.Common.DomainModels.Repository.Interface;
using Indigox.Common.EventBus.Interface.Event;
using Indigox.Common.Logging;
using Indigox.Common.Membership.Interfaces;
using Indigox.UUM.Sync.Interfaces;
using Indigox.UUM.Sync.Model;
using Indigox.UUM.Sync.Tasks;
using Indigox.UUM.Sync.Tasks.Builders;

namespace Indigox.UUM.Sync.Gateways
{
    public class EventGateway : IEventGateway
    {
        public void Notify( object source, IEvent evt )
        {
            IList<SysConfiguration> targets = GetSortedTargetSysConfigurations();

            Dictionary<string, ISyncTask> tasks = new Dictionary<string, ISyncTask>();
            
            for ( int i = 0; i < targets.Count; i++ )
            {
                SysConfiguration target = targets[ i ];

                string clientName = target.ClientName;

                List<ISyncTask> dependTasks = new List<ISyncTask>();
                foreach ( SysConfiguration dependTarget in target.Dependencies )
                {
                    if (!tasks.ContainsKey(dependTarget.ClientName))
                    {
                        continue;
                    }
                    dependTasks.Add( tasks[ dependTarget.ClientName ] );
                }
                if (dependTasks.Count < target.Dependencies.Count)
                {
                    continue;
                }

                ISyncTask task = BuildTask( (IPrincipal)source, evt, target, dependTasks );
                if ( task == null )
                {
                    continue;
                }

                SyncManager.AddTask( clientName, task );

                tasks.Add( clientName, task );
            }
        }

        private IList<SysConfiguration> GetSortedTargetSysConfigurations()
        {
            IList<SysConfiguration> sysConfigurations = SysConfigurationService.Instance.GetSysConfigurations( SyncType.Target );
            sysConfigurations = new SysConfigurationSorter().Sort( sysConfigurations );
            return sysConfigurations;
        }

        private ISyncTask BuildTask( IPrincipal source, IEvent evt, SysConfiguration sysConfiguration, List<ISyncTask> dependencies )
        {
            AbstractPrincipalEventTaskBuilder builder = TaskBuilderFactory.Instance.GetTaskBuilder( evt ) as AbstractPrincipalEventTaskBuilder;

            builder.System = sysConfiguration;
            builder.Source = source;
            builder.Event = evt;
            builder.Dependencies = dependencies;

            ISyncTask task = builder.Build();

            return task;
        }
    }
}