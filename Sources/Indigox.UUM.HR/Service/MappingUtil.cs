using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Indigox.UUM.HR.Service
{
    class MappingUtil
    {
        public static void DeleteMapping(string organizationalPersonID)
        {
            Module.Db.ExecuteText("delete from HRMapping where PrincipalID='" + organizationalPersonID + "'");
        }

        public static void CreateMapping(string employeeID, string organizationalPersonID)
        {
            Module.Db.ExecuteText("delete HRMapping where PrincipalID = '"
                + organizationalPersonID + "' and HRObjectID != '" + employeeID + "'");

            Module.Db.ExecuteText("insert into HRMapping (HRObjectID,PrincipalID) "
                + "values ('" + employeeID + "','" + organizationalPersonID + "')");
        }

        public static string GetPrincipalIDByHRObjectID(string employeeID)
        {
            return (string)Module.Db.ScalarText("select PrincipalID from HRMapping where HRObjectID='" + employeeID + "'");
        }
    }
}
