using System;
using System.Collections.Generic;
using Indigox.Common.Data.Interface;

namespace Indigox.UUM.Sync.OpusOne.PowerHRP.DatabaseSynchronization.Configuration
{
    public static class TableExtension
    {
        public static string GetSelectSourceRecordSql( this Table table, string keyValue )
        {
            string[] fields = new string[ table.Fields.Count ];
            string[] wheres = new string[ table.Key.Fields.Count ];

            int i = 0;
            foreach ( Field field in table.Fields )
            {
                string fieldName = field.Name;
                fields[ i ] = field.Name;
                i++;
            }

            i = 0;
            foreach ( Field field in table.Key.Fields )
            {
                string fieldName = field.Name;
                object value = keyValue;
                wheres[ i ] = field.Name + ( ( value == null ) ? " IS NULL" : ( "=" + new SqlValue( value ).ToSqlString() ) );
                break;
            }

            string sql = "SELECT " + string.Join( ", ", fields ) + " FROM " + table.Name + " WHERE " + wheres[ 0 ];

            return sql;
        }

        //public static string GetSelectSourceRecordsSql( this Table table, string whereClause )
        //{
        //    string[] fields = new string[ table.Fields.Count ];
        //
        //    int i = 0;
        //    foreach ( Field field in table.Fields )
        //    {
        //        string fieldName = field.Name;
        //        fields[ i ] = field.Name;
        //        i++;
        //    }
        //
        //    string sql = "SELECT " + string.Join( ", ", fields ) + " FROM " + table.Name + " WHERE " + whereClause;
        //
        //    return sql;
        //}

        public static string GetSelectBackupRecordSql( this Table table, string keyValue )
        {
            string[] fields = new string[ table.Fields.Count ];
            string[] wheres = new string[ table.Key.Fields.Count ];

            int i = 0;
            foreach ( Field field in table.Fields )
            {
                string fieldName = field.Name;
                fields[ i ] = field.Name;
                i++;
            }

            i = 0;
            foreach ( Field field in table.Key.Fields )
            {
                string fieldName = field.Name;
                object value = keyValue;
                wheres[ i ] = field.Name + ( ( value == null ) ? " IS NULL" : ( "=" + new SqlValue( value ).ToSqlString() ) );
                break;
            }

            string sql = "SELECT " + string.Join( ", ", fields ) + " FROM " + table.BackupTable + " WHERE " + wheres[ 0 ];

            return sql;
        }

        //public static string GetSelectBackupRecordsSql( this Table table, string whereClause )
        //{
        //    string[] fields = new string[ table.Fields.Count ];
        //
        //    int i = 0;
        //    foreach ( Field field in table.Fields )
        //    {
        //        string fieldName = field.Name;
        //        fields[ i ] = field.Name;
        //        i++;
        //    }
        //
        //    string sql = "SELECT " + string.Join( ", ", fields ) + " FROM " + table.BackupTable + " WHERE " + whereClause;
        //
        //    return sql;
        //}

        public static string GetInsertBackupRecordSql( this Table table, IRecord record )
        {
            string[] fields = new string[ table.Fields.Count ];
            string[] values = new string[ table.Fields.Count ];

            int i = 0;
            foreach ( Field field in table.Fields )
            {
                string fieldName = field.Name;
                object value = record.GetValue( fieldName );
                fields[ i ] = fieldName;
                values[ i ] = new SqlValue( value ).ToSqlString();
                i++;
            }

            string sql = "INSERT INTO " + table.BackupTable + " (" + string.Join( ", ", fields ) + ") VALUES (" + string.Join( ", ", values ) + ")";

            return sql;
        }

        public static string GetDeleteBackupRecordSql( this Table table, IRecord record )
        {
            string[] wheres = new string[ table.Key.Fields.Count ];

            int i = 0;
            foreach ( Field keyField in table.Key.Fields )
            {
                string fieldName = keyField.Name;
                object value = record.GetValue( fieldName );
                wheres[ i ] = keyField.Name + ( ( value == null ) ? " IS NULL" : ( "=" + new SqlValue( value ).ToSqlString() ) );
                i++;
            }

            string sql = "DELETE FROM " + table.BackupTable + " WHERE " + string.Join( " AND ", wheres );

            return sql;
        }

        public static string GetUpdateBackupRecordSql( this Table table, IRecord record, IList<Field> changedFields )
        {
            string[] sets = new string[ changedFields.Count ];
            string[] wheres = new string[ table.Key.Fields.Count ];

            int i = 0;
            foreach ( Field field in changedFields )
            {
                string fieldName = field.Name;
                object value = record.GetValue( fieldName );
                sets[ i ] = field.Name + "=" + new SqlValue( value ).ToSqlString();
                i++;
            }

            i = 0;
            foreach ( Field keyField in table.Key.Fields )
            {
                string fieldName = keyField.Name;
                object value = record.GetValue( fieldName );
                wheres[ i ] = keyField.Name + ( ( value == null ) ? " IS NULL" : ( "=" + new SqlValue( value ).ToSqlString() ) );
                i++;
            }

            string sql = "UPDATE " + table.BackupTable + " SET " + string.Join( ", ", sets ) + " WHERE " + string.Join( " AND ", wheres );

            return sql;
        }
    }
}