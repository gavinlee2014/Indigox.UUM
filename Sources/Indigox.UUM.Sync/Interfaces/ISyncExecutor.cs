using System;

namespace Indigox.UUM.Sync.Interfaces
{
    public interface ISyncExecutor
    {
        void Execute( ISyncContext context );
    }
}