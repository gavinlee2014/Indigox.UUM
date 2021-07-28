using System;
using Indigox.Common.DomainModels.Factory;
using Indigox.Common.DomainModels.Repository.Interface;
using Indigox.UUM.Sync.Interfaces;
using Indigox.UUM.Sync.Model;
using Indigox.Common.Data.Interface;
using Indigox.Common.Data;

namespace Indigox.UUM.Sync.Executors
{
    public class SaveIDWebServiceExecutor : WebServiceExecutor
    {
        public string InternalID { get; set; }
        public string SysClientName { get; set; }

        public override void Execute( ISyncContext context )
        {
            string id = (string)InvokeWebService( context );
            SaveID( InternalID, id, SysClientName );
        }

        protected virtual void SaveID( string internalID, string externalID, string sysClientName )
        {
            //IRepository<SysKeyMapping> repos = RepositoryFactory.Instance.CreateRepository<SysKeyMapping>();
            //SysConfiguration externalSystem = SysConfigurationService.Instance.GetSysConfiguration( sysClientName );
            //var mapping = new SysKeyMapping( internalID, externalID, externalSystem );
            //repos.Add( mapping );

            if (String.IsNullOrEmpty(externalID))
            {
                return;
            }

            IDatabase db = new DatabaseFactory().CreateDatabase("UUM");

            string sql = @"insert into SysKeyMapping 
( InternalID, ExternalID, ExternalSystem)
select @InternalID as internalID, @ExternalID as externalID, ID as externalSystem 
from sysConfiguration
where clientName = @ExternalSystem ";

            ICommand cmd = db.CreateTextCommand(sql)
                .AddParameter("@InternalID varchar(500)", internalID)
                .AddParameter("@ExternalID varchar(500)", externalID)
                .AddParameter("@ExternalSystem varchar(500)", sysClientName);

            db.Execute(cmd);
        }
    }
}