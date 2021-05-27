using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Indigox.Common.Data.Interface;
using Indigox.Common.Data;

namespace Indigox.UUM.HR
{
    class Module
    {
        private static IDatabase db;

        public static IDatabase Db
        {
            get
            {
                if (db == null)
                {
                    DatabaseFactory factory = new DatabaseFactory();
                    db = factory.CreateDatabase("UUM");
                }
                return db;
            }
        }
    }
}
