using Indigox.Common.Data;
using Indigox.Common.Data.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Indigox.UUM
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
