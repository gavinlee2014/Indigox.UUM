using Indigox.Common.Data.Interface;
using Indigox.UUM.Sync.Interfaces;
using Indigox.UUM.Sync.Model;
using Indigox.UUM.Sync.OpusOne.PowerHRP.Synchronizers.TaskBuilders;
using Indigox.UUM.Sync.OpusOne.PowerHRP.Utils;

namespace Indigox.UUM.Sync.OpusOne.PowerHRP.Synchronizers
{
    internal class DeleteOrganizationStructureSynconizer : ISynchronizer
    {
         public void Synchronize(string id)
        {
            IRecordSet rs = Databases.UUM.QueryText(
                "select structurecode,structureorder,typeid,orgcode1,orgcode2,orgcode3,orgcode4,orgcode5 from OpusOneOrganizationStructure " +
            " where structurecode ='" + id + "'");

            if (rs.Records.Count > 0)
            {
                IRecord record = rs.Records[0];
                this.BuildTask(record);
                this.DeleteBackup(record.GetString("structurecode"));
            }
        }
        private void DeleteBackup(string id)
        {
            string sql = "delete from OpusOneOrganizationStructure where structurecode='" + id + "'";
            Databases.UUM.ExecuteText(sql);
        }
        private void BuildTask(IRecord record)
        {
            SysConfiguration source = RegisteredSysConfiguration.Get();

            ISyncTaskBuilder taskBuilder = new DeleteOrganizationTaskBuilder()
            {
                Source = source,
                OrganizationalUnitID = GetOrganizationID(record, record.GetInt("typeid")),
                Name = GetOrganizationName(record)
            };

            SyncManager.AddTask(source.ClientName, taskBuilder.Build());
        }
        private string GetOrganizationName(IRecord record)
        {
            string organizationName = "";
            string typeid = record.GetString("typeid");
            string orgCode = record.GetString("orgcode" + typeid);

            IRecordSet recordSet = Databases.OpusOnePowerHRP.QueryText("select c_name from organization where code = '" + orgCode + "'");
            if (recordSet.Records.Count > 0)
            {
                organizationName = recordSet.Records[0].GetString("c_name");
            }

            return organizationName;
        }
        private string GetOrganizationID(IRecord record, int typeid)
        {
            string id = "";
            for (int i = 1; i <= typeid; i++)
            {
                id += record.GetString("orgcode" + i);
            }
            return id;
        }
    }
}
