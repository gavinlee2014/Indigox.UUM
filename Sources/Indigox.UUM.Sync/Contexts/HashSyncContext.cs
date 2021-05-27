using System;
using System.Collections.Generic;
using Indigox.UUM.Sync.Interfaces;

namespace Indigox.UUM.Sync.Contexts
{
    public class HashSyncContext : Dictionary<string, object>, ISyncContext
    {
        public HashSyncContext()
            : base( StringComparer.CurrentCultureIgnoreCase )
        {
        }

        public object Get( string key )
        {
            if ( this.ContainsKey( key ) )
            {
                return this[ key ];
            }
            else
            {
                return null;
                //throw new KeyNotFoundException( string.Format( "找不到 key:{0} 对应的值。", key ) );
            }
        }

        public void Set( string key, object value )
        {
            this[ key ] = value;
        }
    }
}