using System;
using Indigox.Common.DomainModels.Factory;
using Indigox.Common.DomainModels.Queries;
using Indigox.Common.DomainModels.Specifications;

namespace Indigox.UUM.Sync.Model
{
    public class SysKeyMappingService
    {
        private static SysKeyMappingService instance = new SysKeyMappingService();

        private SysKeyMappingService()
        {
        }

        public static SysKeyMappingService Instance
        {
            get
            {
                return instance;
            }
        }

        public void SetExternalID( string internalID, string externalID, SysConfiguration externalSystem )
        {
            var repository = RepositoryFactory.Instance.CreateRepository<SysKeyMapping>();
            SysKeyMapping mapping = repository.First(new Query()
                .FindByCondition(
                    Specification.And(
                        Specification.Equal("InternalID", internalID),
                        Specification.Equal("ExternalSystem", externalSystem)
                    )
                )
            );

            if (mapping != null)
            {
                mapping.ExternalID = externalID;
                repository.Update(mapping);
            }
            else
            {
                mapping = new SysKeyMapping(internalID, externalID, externalSystem);
                repository.Add(mapping);
            }
        }

        public string GetExternalID( string internalID, SysConfiguration externalSystem )
        {
            var repository = RepositoryFactory.Instance.CreateRepository<SysKeyMapping>();
            var mapping = repository.First( new Query()
                .FindByCondition(
                    Specification.And(
                        Specification.Equal( "InternalID", internalID ),
                        Specification.Equal( "ExternalSystem", externalSystem )
                    )
                )
            );

            if ( mapping != null )
            {
                return mapping.ExternalID;
            }
            else
            {
                return null;
            }
        }
    }
}