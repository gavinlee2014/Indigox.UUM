using System;

namespace Indigox.UUM.Sync.OpusOne.PowerHRP.Synchronizers
{
    internal interface ISynchronizer
    {
        void Synchronize(string id);
    }
}