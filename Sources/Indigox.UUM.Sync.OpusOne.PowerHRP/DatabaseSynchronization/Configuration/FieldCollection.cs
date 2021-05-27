using System;
using System.Collections;
using System.Collections.Generic;

namespace Indigox.UUM.Sync.OpusOne.PowerHRP.DatabaseSynchronization.Configuration
{
    public class FieldCollection : IEnumerable<Field>
    {
        private IDictionary<string, Field> dictionary = new Dictionary<string, Field>();
        private IEnumerable<Field> enumerable;

        public FieldCollection( IEnumerable<Field> enumerable )
        {
            this.enumerable = enumerable;
            foreach ( var item in enumerable )
            {
                dictionary.Add( item.Name, item );
            }
        }

        public bool Contains( string field )
        {
            return dictionary.ContainsKey( field );
        }

        public bool ContainsAll( string[] fields )
        {
            foreach ( string field in fields )
            {
                if ( !dictionary.ContainsKey( field ) )
                {
                    return false;
                }
            }
            return true;
        }

        public bool ContainsAny( string[] fields )
        {
            foreach ( string field in fields )
            {
                if ( dictionary.ContainsKey( field ) )
                {
                    return true;
                }
            }
            return false;
        }

        IEnumerator<Field> IEnumerable<Field>.GetEnumerator()
        {
            return enumerable.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return enumerable.GetEnumerator();
        }
    }
}