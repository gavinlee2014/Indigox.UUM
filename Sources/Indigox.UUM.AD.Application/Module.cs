using System;
using Indigox.Common.Data;
using Indigox.Common.Data.Interface;

internal class Module
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