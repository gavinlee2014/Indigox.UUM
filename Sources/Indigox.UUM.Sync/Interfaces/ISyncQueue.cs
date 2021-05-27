using System;

namespace Indigox.UUM.Sync.Interfaces
{
    public interface ISyncQueue
    {
        void Push( ISyncTask task );
    }
}