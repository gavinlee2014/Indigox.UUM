using System;
using System.Collections.Generic;

namespace Indigox.UUM.Sync.Interface
{
    public class PropertyChangeCollection
    {
        private List<PropertyChange> collection;

        public PropertyChangeCollection()
        {
            this.collection = new List<PropertyChange>();
        }

        public PropertyChangeCollection( IDictionary<string, object> dictionary )
        {
            this.collection = ConvertFromDictionary( dictionary );
        }

        public PropertyChangeCollection(IDictionary<string, string> dictionary)
        {
            Dictionary<string, object> sd = new Dictionary<string, object>();
            foreach (KeyValuePair<string, string> keyValuePair in dictionary)
            {
                sd.Add(keyValuePair.Key, keyValuePair.Value);
            }
            this.collection = ConvertFromDictionary(sd);
        }

        public List<PropertyChange> PropertyChanges
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

        public bool Contains( string propertyName )
        {
            return GetItem( propertyName ) != null;
        }

        public object Get( string propertyName )
        {
            PropertyChange item = GetItem( propertyName );
            if ( item != null )
            {
                return item.Value;
            }
            return null;
        }

        public void Set( string propertyName, object propertyValue )
        {
            PropertyChange item = GetItem( propertyName );
            if ( item == null )
            {
                item = new PropertyChange();
                collection.Add( item );
            }
            item.Value = propertyValue;
        }

        public IDictionary<string, object> ToDictionary()
        {
            return ConvertToDictionary(this.collection);
        }

        public IDictionary<string, string> ToStringValues()
        {
            IDictionary<string, object> dictionary = this.ToDictionary();
            IDictionary<string, string> sd = new Dictionary<string, string>();
            foreach (KeyValuePair<string, object> keyValuePair in dictionary)
            {
                sd.Add(keyValuePair.Key, keyValuePair.Value == null ? "" : keyValuePair.Value.ToString());
            }
            return sd;
        }

        private PropertyChange GetItem( string propertyName )
        {
            foreach ( PropertyChange item in collection )
            {
                if ( item.Name == propertyName )
                {
                    return item;
                }
            }
            return null;
        }

        private static List<PropertyChange> ConvertFromDictionary( IDictionary<string, object> dictionary )
        {
            List<PropertyChange> collection = new List<PropertyChange>();
            foreach ( KeyValuePair<string, object> item in dictionary )
            {
                collection.Add( new PropertyChange( item.Key, item.Value ) );
            }
            return collection;
        }

        private static IDictionary<string, object> ConvertToDictionary( List<PropertyChange> collection )
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            foreach ( PropertyChange propertyChange in collection )
            {
                dictionary.Add( propertyChange.Name, propertyChange.Value );
            }
            return dictionary;
        }
    }
}