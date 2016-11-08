using Jet.Framework.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace Jet.Framework.ObjectMapping
{
    public abstract partial class DatabaseEngine
    {
        private Dictionary<Type, DataTable> DataTableDict = new Dictionary<Type, DataTable>();

        public virtual DataTable GetTableSchema(TableMapping tablemapping)
        {
            Type t = tablemapping.ObjectType;
            DataTable dt;
            if (this.DataTableDict.ContainsKey(t))
                dt = this.DataTableDict[t];
            else
            {
                string strSQL = string.Format("select top 1 * from {0} where 1 <> 1", this.GetTableName(tablemapping.Name));
                dt = this.ExecuteDataSet(strSQL).Tables[0];
                this.DataTableDict.Add(t, dt);
            }

            return SerializeHelper.DeepClone(dt);//因为使用中会修改这个DataTable,所以返回克隆对象
        }

        public virtual DataTable GetFillTable(TableMapping tableMapping, IList list)
        {
            DataTable dataTable = this.GetTableSchema(tableMapping);
            object[] values = new object[dataTable.Columns.Count];
            dataTable.BeginInit();
            dataTable.BeginLoadData();
            foreach (object obj in list)
            {
                for (int i = 0; i < dataTable.Columns.Count; i++)
                {
                    var ColumnName = dataTable.Columns[i].ColumnName;
                    var col = tableMapping.GetColumnMappingByColumn(ColumnName);
                    if (col != null)
                        values[i] = col.GetValue(obj);
                }

                dataTable.LoadDataRow(values, false);
            }
            dataTable.EndLoadData();
            dataTable.EndInit();
            return dataTable;
        }

        public virtual void BulkCopy(TableMapping tableMapping, IList list, Dictionary<string, object> bulkCopyConfig = null)
        {
            this.PrepareBulkCopy(tableMapping, list);
            foreach (var obj in list)
            {
                this.Create(tableMapping, obj);
            }
        }

        public virtual void PrepareBulkCopy(TableMapping tableMapping, IList list)
        {
            //使用种子工厂--------------------------------------------------------------------------------
            if (tableMapping.ColumnPK.IsUseSeedFactory == false)
                return;
            int SeedPoolSize = Math.Max(100, list.Count);
            foreach (var obj in list)
            {
                int tempid = (int)tableMapping.ColumnPK.GetValue(obj);
                if (tempid == 0)
                {
                    tempid = SeedService.Instance.Generator.GetSeed(tableMapping.Name, tableMapping.ColumnPK.Name, SeedPoolSize);
                    tableMapping.ColumnPK.PropertyInfo.SetValue(obj, tempid, null);
                }
            }
        }
    }
}
