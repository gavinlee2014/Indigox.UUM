using System;
using Indigox.UUM.Sync.Interfaces;

namespace Indigox.UUM.Sync.Tests.Model
{
    internal class CallbackExecutor : ISyncExecutor
    {
        private CallbackExecutorHandler callback;

        public CallbackExecutor( CallbackExecutorHandler callback )
        {
            this.callback = callback;
        }

        public void Execute( ISyncContext context )
        {
            Console.WriteLine( "Execute begin..." );
            callback.Invoke();
            Console.WriteLine( "Execute end." );
        }
    }

    internal delegate void CallbackExecutorHandler();
}