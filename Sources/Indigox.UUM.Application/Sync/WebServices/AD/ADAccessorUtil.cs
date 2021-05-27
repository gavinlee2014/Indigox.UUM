using System;
using Indigox.Common.ADAccessor.ObjectModel;
using Indigox.Common.Logging;

namespace Indigox.UUM.Application.Sync.WebServices.AD
{
    class ADAccessorUtil
    {
        public static T GetADObjectByID<T>(string id)
        {
            string type = typeof(T).Name;
            //Guid guid = new Guid(Convert.ToString(Module.Db.ScalarText(
            //    "select ADEntryID from ADEntryMapping where ADEntryType='" + type + "' and SourceID='" + id + "'")));
            //if (guid.Equals(Guid.Empty))
            //{
            //    return default(T);
            //}
            Log.Error("*************" + id);

            Guid guid = new Guid(id);
            if (type.Equals("OrganizationalUnit"))
            {
                Log.Error("*************type.Equals(\"OrganizationalUnit\")");
                string ouid = Convert.ToString(Module.Db.ScalarText(
                    "select OrganizationalUnitID from ADGroupToOU where GroupID='" + id + "'"));
                if (!String.IsNullOrEmpty(ouid))
                {
                    guid = new Guid(ouid);
                }
                else
                {
                    guid = Guid.Empty;
                }
            }

            if (guid.Equals(Guid.Empty))
            {
                Log.Error("*************guid.Equals(Guid.Empty)");
                return default(T);
            }

            T instance = Activator.CreateInstance<T>();
            Entry entry = instance as Entry;
            entry.ID = guid;
            return instance;
        }

        public static void CreateADMapping(string id, Entry entry)
        {
            Module.Db.ExecuteText("insert into ADEntryMapping (SourceID,ADEntryID,ADEntryType) values ('"
                + id + "','" + entry.ID.ToString() + "','" + entry.GetType().Name + "')");

        }

        public static void MappingGroupAndOU(string groupID,string ouID)
        {
            Module.Db.ExecuteText("delete from ADGroupToOU where GroupID='"+ groupID + "'");
            Module.Db.ExecuteText("insert into ADGroupToOU (GroupID,OrganizationalUnitID) values ('"
                + groupID + "','" + ouID + "')");
        }
    }
}
