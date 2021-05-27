using System;
using Indigox.Common.ADAccessor.ObjectModel;

namespace Indigox.UUM.AD.Application.WebServices

{
    internal class ADAccessorUtil
    {
        public static T GetADObjectByID<T>(string id)
        {
            string type = typeof(T).Name;
            string[] ids = id.Split(',');
            Guid guid = Guid.Empty;

            if (ids.Length == 0) return default(T);

            if (type.Equals("Group"))
            {
                string groupID = ids[0];
                guid = new Guid(groupID);
            }
            else if (type.Equals("OrganizationalUnit"))
            {
                if (ids.Length == 2)
                {
                    string ouID = ids[1];
                    guid = new Guid(ouID);
                }
            }
            else
            {
                guid = new Guid(id);
            }
            if (guid.Equals(Guid.Empty))
            {
                return default(T);
            }

            T instance = Activator.CreateInstance<T>();
            Entry entry = instance as Entry;
            entry.ID = guid;
            return instance;
        }
    }
}
