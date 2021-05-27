using System;
using System.Collections.Generic;

namespace Indigox.UUM.Sync.Interface
{
    public class HRPropertyChangeCollection
    {
        private List<HRPropertyChange> collection;

        public HRPropertyChangeCollection()
        {
            this.collection = new List<HRPropertyChange>();
        }

        public HRPropertyChangeCollection(IDictionary<string, string> dictionary)
        {
            this.collection = ConvertFromDictionary(dictionary);
        }

        public List<HRPropertyChange> PropertyChanges
        {
            get
            {
                return collection;
            }
            set
            {
                collection = value;
            }
        }

        public bool Contains(string propertyName)
        {
            return GetItem(propertyName) != null;
        }

        public string Get(string propertyName)
        {
            HRPropertyChange item = GetItem(propertyName);
            if (item != null)
            {
                return item.Value;
            }
            return null;
        }

        public void Set(string propertyName, string propertyValue)
        {
            HRPropertyChange item = GetItem(propertyName);
            if (item == null)
            {
                item = new HRPropertyChange();
                collection.Add(item);
            }
            item.Value = propertyValue;
        }

        public IDictionary<string, string> ToDictionary()
        {
            return ConvertToDictionary(this.collection);
        }

        private HRPropertyChange GetItem(string propertyName)
        {
            foreach (HRPropertyChange item in collection)
            {
                if (item.Name == propertyName)
                {
                    return item;
                }
            }
            return null;
        }

        private static List<HRPropertyChange> ConvertFromDictionary(IDictionary<string, string> dictionary)
        {
            List<HRPropertyChange> collection = new List<HRPropertyChange>();
            foreach (KeyValuePair<string, string> item in dictionary)
            {
                collection.Add(new HRPropertyChange(item.Key, item.Value));
            }
            return collection;
        }

        private static IDictionary<string, string> ConvertToDictionary(List<HRPropertyChange> collection)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            foreach (HRPropertyChange propertyChange in collection)
            {
                dictionary.Add(propertyChange.Name, propertyChange.Value);
            }
            return dictionary;
        }
    }
}