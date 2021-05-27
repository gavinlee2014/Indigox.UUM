using Indigox.Common.Data.Interface;
using Indigox.UUM.Sync.Interfaces;
using Indigox.UUM.Sync.Model;
using Indigox.UUM.Sync.OpusOne.PowerHRP.Synchronizers.TaskBuilders;
using Indigox.UUM.Sync.OpusOne.PowerHRP.Utils;

namespace Indigox.UUM.Sync.OpusOne.PowerHRP.Synchronizers
{
    internal class DeleteOrganizationSynchronizer : ISynchronizer
    {
        public void Synchronize(string id)
        {
            IRecordSet rs = Databases.UUM.QueryText(
                "select code,typeid,c_name from opusOneOrganization where code='" + id + "'");

            if (rs.Records.Count > 0)
            {
                IRecord record = rs.Records[0];
                this.DeleteBackup(record.GetString("code"));
            }
        }
        private void DeleteBackup(string id)
        {
            string sql = "delete from opusOneOrganization where code='" + id + "'";
            Databases.UUM.ExecuteText(sql);
        }
    }
}
