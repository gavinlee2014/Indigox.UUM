using System;
using Indigox.Common.EventBus.Interface.Event;

namespace Indigox.UUM.Sync.Interfaces
{
    internal interface IEventGateway
    {
        void Notify( object source, IEvent evt );
    }
}