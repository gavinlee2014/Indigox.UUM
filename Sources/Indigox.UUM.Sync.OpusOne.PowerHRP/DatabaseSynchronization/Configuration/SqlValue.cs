using System;

namespace Indigox.UUM.Sync.OpusOne.PowerHRP.DatabaseSynchronization.Configuration
{
    internal class SqlValue
    {
        private object value;

        public SqlValue( object value )
        {
            this.value = value;
        }

        public string ToSqlString()
        {
            string retStr = "";
            if ( value == null )
            {
                // null
                retStr = "null";
            }
            else if ( value is int || value is short || value is long ||
                      value is uint || value is ushort || value is ulong ||
                      value is double || value is float || value is decimal )
            {
                // 数字类型
                retStr = value.ToString();
            }
            else if ( value is bool )
            {
                // 是否类型
                retStr = ( (bool)value ) ? "1" : "0";
            }
            else if ( value is DateTime )
            {
                // 日期类型
                retStr = string.Format( "'{0}'", ( (DateTime)value ).ToString( "yyyy-MM-dd HH:mm:ss.fff" ) );
            }
            else if ( value is Guid )
            {
                // GUID类型
                retStr = string.Format( "'{0}'", ( (Guid)value ).ToString() );
            }
            else if ( value.GetType().IsEnum )
            {
                // ENUM
                retStr = string.Format( "{0}", Convert.ToInt32( value ) );
            }
            else
            {
                // 字符串和其它类型
                retStr = string.Format( "'{0}'", value.ToString().Replace( "'", "''" ) );
            }
            return retStr;
        }
    }
}