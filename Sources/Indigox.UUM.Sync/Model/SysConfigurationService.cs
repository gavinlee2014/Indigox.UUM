using System;
using System.Collections.Generic;
using Indigox.Common.DomainModels.Factory;
using Indigox.Common.DomainModels.Interface.Specifications;
using Indigox.Common.DomainModels.Queries;
using Indigox.Common.DomainModels.Specifications;

namespace Indigox.UUM.Sync.Model
{
    public class SysConfigurationService
    {
        private static SysConfigurationService instance = new SysConfigurationService();

        private SysConfigurationService()
        {
        }

        public static SysConfigurationService Instance
        {
            get
            {
                return instance;
            }
        }

        public IList<SysConfiguration> GetSysConfigurations( SyncType syncType )
        {
            if ( syncType == SyncType.None )
            {
                throw new ApplicationException( "Can't find by SyncType.None." );
            }

            var repository = RepositoryFactory.Instance.CreateRepository<SysConfiguration>();
            var condition = new Query();
            ISpecification spec = Specification.And( Specification.Equal( "Enabled", true ) );
            if ( syncType != SyncType.All )
            {
                spec = Specification.And( spec, Specification.Equal( "SyncType", syncType ) );
            }
            condition.FindByCondition( spec );
            var sysConfigurations = repository.Find( condition );

            return sysConfigurations;
        }

        public SysConfiguration GetSysConfiguration( int id )
        {
            var repository = RepositoryFactory.Instance.CreateRepository<SysConfiguration>();
            return repository.Get( id );
        }

        public SysConfiguration GetSysConfiguration( string sysClientName )
        {
            var repository = RepositoryFactory.Instance.CreateRepository<SysConfiguration>();
            return repository.First( new Query()
                .FindByCondition(
                    Specification.And( Specification.Equal( "ClientName", sysClientName ) )
                )
            );
        }
    }
}