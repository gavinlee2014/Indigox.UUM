//using System;
//using Indigox.Common.Data.Interface;
//using Indigox.UUM.Sync.OpusOne.PowerHRP.DatabaseSynchronization.Configuration;
//
//namespace Indigox.UUM.Sync.OpusOne.PowerHRP.DatabaseSynchronization
//{
//    public class RecordUpdatedEventArgs : EventArgs
//    {
//        private Field[] changedFields;
//        private IRecord newRecord;
//        private IRecord oldRecord;
//        private Table table;
//
//        public RecordUpdatedEventArgs( Table table, Field[] changedFields, IRecord newRecord, IRecord oldRecord )
//        {
//            this.table = table;
//            this.changedFields = changedFields;
//            this.newRecord = newRecord;
//            this.oldRecord = oldRecord;
//        }
//
//        public Field[] ChangedFields
//        {
//            get { return changedFields; }
//        }
//
//        public IRecord NewRecord
//        {
//            get { return newRecord; }
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