using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jet.Framework.Utility
{
    public static class SqlCommon
    {
        public enum SqlErrorCode : int
        {
            /// <summary>
            /// 外键约束错误
            /// </summary>
            ErrorCode_547 = 547

            
        }

        public static readonly DateTime SqlDateTime_Min = new DateTime(1900, 1, 1);

        public static readonly DateTime SqlDateTime_Max = new DateTime(2100, 12, 31);

        public static string SqlIn(string TableName, string ColumnName, List<int> ids)
        {
            return SqlIn(TableName, ColumnName, ids.ListToStringAppendComma());
        }

        public static string SqlIn(string TableName, string ColumnName, string strids)
        {
            return string.Format(" EXISTS(SELECT Id FROM SplitFn('{0}') A WHERE A.Id = {1}.{2}) ", strids, TableName, ColumnName);
        }
    }
}
