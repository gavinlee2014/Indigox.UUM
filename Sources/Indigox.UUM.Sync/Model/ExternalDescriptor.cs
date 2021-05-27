using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Indigox.Common.Data.Interface;
using Indigox.Common.Data;

namespace Indigox.UUM.Sync.Model
{
    public class ExternalObjectDescriptor
    {
        public string InternalID { get; set; }
        public int SysID { get; set; }

        public string GetExternalID()
        {
            IDatabase db = new DatabaseFactory().CreateDatabase("UUM");
            IRecordSet rs = db.QueryText("select * from SysKeyMapping where internalid='" + this.InternalID + "' and externalSystem=" + SysID);
            string externalID = "";
            if (rs.Records.Count > 0)
            {
                externalID = rs.Records[0].GetString("ExternalID");
            }
            return externalID;
        }
        
    }
}
