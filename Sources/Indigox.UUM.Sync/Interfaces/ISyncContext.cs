using System;

namespace Indigox.UUM.Sync.Interfaces
{
    public interface ISyncContext
    {
        object Get( string key );

        void Set( string key, object value );
    }
}