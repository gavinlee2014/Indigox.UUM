//using System;
//using Indigox.Common.Data.Interface;
//using Indigox.UUM.Sync.OpusOne.PowerHRP.DatabaseSynchronization.Configuration;
//
//namespace Indigox.UUM.Sync.OpusOne.PowerHRP.DatabaseSynchronization
//{
//    public class RecordRemovedEventArgs : EventArgs
//    {
//        private IRecord oldRecord;
//        private Table table;
//
//        public RecordRemovedEventArgs( Table table, IRecord oldRecord )
//        {
//            this.table = table;
//            this.oldRecord = oldRecord;
//        }
//
//        public IRecord OldRecord
//        {
//            get { return oldRecord; }
//        }
//
//        public Table Table
//        {
//            get { return table; }
//        }
//    }
//}