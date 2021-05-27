using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Indigox.UUM.Util
{
    class PasswordUtil
    {
        public static void UpdatePassword(string organizationalPersonID, string pwd)
        {
            Module.Db.ExecuteText("UPDATE Users SET AccountPassword='" + pwd + "' WHERE ID='" + organizationalPersonID + "'");
        }
        public static void UpdatePasswordByAccount(string accountName, string pwd)
        {
            Module.Db.ExecuteText("UPDATE Users SET AccountPassword='" + pwd + "' WHERE AccountName='" + accountName + "'");
        }
        public static string GetPassword(string organizationalPersonID)
        {
            return (string)Module.Db.ScalarText("SELECT AccountPassword FROM Users WHERE ID='" +
                organizationalPersonID + "'");
        }
    }
}
