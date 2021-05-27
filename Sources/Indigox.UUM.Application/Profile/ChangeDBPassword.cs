using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Indigox.Common.Data.Interface;
using Indigox.Common.Data;

namespace Indigox.UUM.Application.Profile
{
    public class ChangeDBPassword
    {
        public void ChangePassword(string accountName, string oldPwd, string newPwd)
        {
            if (!CheckPassword(accountName, oldPwd)) { throw new Exception(); }
            else
            {
                string sql = "update users set accountpassword = @password where accountname = @username";
                IDatabase db = new DatabaseFactory().CreateDatabase("UUM");
                db.ExecuteText(sql, "@username varchar(100),@password varchar(100)", accountName, newPwd);
            }
        }

        public void SetPassword(string accountName, string pwd)
        {
            string sql = "update users set accountpassword = @password where accountname = @username";
            IDatabase db = new DatabaseFactory().CreateDatabase("UUM");
            db.ExecuteText(sql, "@username varchar(100),@password varchar(100)", accountName, pwd);
        }

        public bool CheckPassword(string accountName, string pwd)
        {
            string querysql = "select '1' from users where accountname=@username and accountpassword=@password";
            IDatabase db = new DatabaseFactory().CreateDatabase("UUM");
            object val = db.ScalarText(querysql, "@username varchar(100),@password varchar(100)", accountName, pwd);
            return (val != null);

        }
    }
}
