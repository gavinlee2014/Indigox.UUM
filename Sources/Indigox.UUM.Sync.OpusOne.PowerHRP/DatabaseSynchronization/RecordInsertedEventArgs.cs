//using System;
//using Indigox.Common.Data.Interface;
//using Indigox.UUM.Sync.OpusOne.PowerHRP.DatabaseSynchronization.Configuration;
//
//namespace Indigox.UUM.Sync.OpusOne.PowerHRP.DatabaseSynchronization
//{
//    public class RecordInsertedEventArgs : EventArgs
//    {
//        private IRecord newRecord;
//        private Table table;
//
//        public RecordInsertedEventArgs( Table table, IRecord newRecord )
//        {
//            this.table = table;
//            this.newRecord = newRecord;
//        }
//
//        public IRecord NewRecord
//        {
//            get { return newRecord; }
//        }
//
//        public Table Table
//        {
//            get { return table; }
//        }
//    }
//}