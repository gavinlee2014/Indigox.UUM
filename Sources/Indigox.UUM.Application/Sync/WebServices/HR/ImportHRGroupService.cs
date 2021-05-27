using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Services;
using Indigox.Common.Logging;
using Indigox.UUM.Sync.Interface.Client;

namespace Indigox.UUM.Application.Sync.WebServices.HR
{
    [WebServiceBinding(Name = "ImportGroupService", Namespace = Consts.Namespace_HR)]
    public class ImportHRGroupService :  IImportGroupService
    {
        public void AddOrganizationalRole(string groupID, string organizationalRoleID)
        {
            Log.Debug(String.Format(@"ImportHRGroupService AddOrganizationalRole {0} {1}",groupID,organizationalRoleID));
        }

        public void AddOrganizationalUnit(string groupID, string organizationalUnitID)
        {
            Log.Debug(String.Format(@"ImportHRGroupService AddOrganizationalUnit {0} {1}", groupID, organizationalUnitID));
        }

        public void AddUser(string groupID, string userID)
        {
            Log.Debug(String.Format(@"ImportHRGroupService AddUser {0} {1}", groupID, userID));
        }

        public void ChangeProperty(string groupID, UUM.Sync.Interface.PropertyChangeCollection propertyChanges)
        {
            Log.Debug(String.Format(@"ImportHRGroupService ChangeProperty {0} {1}", groupID, propertyChanges));
        }

        public string Create(string nativeID, string name, string email, string description, double orderNum)
        {
            Log.Debug(String.Format(@"ImportHRGroupService Create {0} {1} {2} {3} {4}", nativeID, name, email, description, orderNum));
            return nativeID;
        }

        public void Delete(string groupID)
        {
            Log.Debug(String.Format(@"ImportHRGroupService Delete {0}", groupID));
        }

        public void RemoveOrganizationalRole(string groupID, string organizationalRoleID)
        {
            Log.Debug(String.Format(@"ImportHRGroupService RemoveOrganizationalRole {0} {1}", groupID, organizationalRoleID));
        }

        public void RemoveOrganizationalUnit(string groupID, string organizationalUnitID)
        {
            Log.Debug(String.Format(@"ImportHRGroupService RemoveOrganizationalUnit {0} {1}", groupID, organizationalUnitID));
        }

        public void RemoveUser(string groupID, string userID)
        {
            Log.Debug(String.Format(@"ImportHRGroupService RemoveUser {0} {1}", groupID, userID));
        }

        public string SyncGroup(string nativeID, string name, string email, string description, double orderNum)
        {
            Log.Debug(String.Format(@"ImportHRGroupService SyncGroup {0} {1} {2} {3} {4}", nativeID, name, email, description, orderNum));
            return nativeID;
        }
    }
}
