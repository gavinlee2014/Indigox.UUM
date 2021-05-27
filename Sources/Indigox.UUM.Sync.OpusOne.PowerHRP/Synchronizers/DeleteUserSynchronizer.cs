using Indigox.Common.Data.Interface;
using Indigox.UUM.Sync.Interfaces;
using Indigox.UUM.Sync.Model;
using Indigox.UUM.Sync.OpusOne.PowerHRP.Synchronizers.TaskBuilders;
using Indigox.UUM.Sync.OpusOne.PowerHRP.Utils;

namespace Indigox.UUM.Sync.OpusOne.PowerHRP.Synchronizers
{
    class DeleteUserSynchronizer : ISynchronizer
    {
        public void Synchronize(string id)
        {
            IRecordSet rs = Databases.UUM.QueryText(
                 "select structurecode,structureorder,typeid,orgcode1,orgcode2,orgcode3,orgcode4,orgcode5 from OpusOneOrganizationStructure where structurecode='" + id + "'");

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

            ISyncTaskBuilder taskBuilder = new DeleteUserTaskBuilder()
            {
                Source = source,
                UserID = record.GetString("serialnumber")
            };

            SyncManager.AddTask(source.ClientName, taskBuilder.Build());
        }
    }
}
