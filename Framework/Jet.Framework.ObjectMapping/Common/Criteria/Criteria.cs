using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Jet.Framework.ObjectMapping
{
    public partial class Criteria
    {
        public TableMapping TableMapping { get; protected set; }
        public string TableName { get; set; }
        public List<SqlCondition> Conditions { get; set; }
        public List<SqlOrder> Orders { get; set; }
        public HashSet<string> Columns { get; set; }
        public List<SqlParameter> UpdateParameters { get; set; }
        public List<SqlParameter> UpdateCustomParameters { get; set; }
        public List<String> UpdateCustomSqls { get; set; }
        public int TakeCount { get; set; }
        public int PageIndex { get; set; }
        public int PageCount { get; set; }
        public int PageMaxRowCount { get; set; }
        public int PageSize { get; set; }
        public string PageMaxRowCountSqlString { get; set; }
        public string PageGetPageDataSqlString { get; set; }

        public String PkColumn;

        public int PageMinRowNum
        {
            get
            {
                return this.PageSize * this.PageIndex;
            }
        }

        public int PageMaxRowNum
        {
            get
            {
                int maxRownum = this.PageSize * (this.PageIndex + 1);
                return this.PageMaxRowCount < maxRownum ? this.PageMaxRowCount : maxRownum;
            }
        }

        public Criteria()
        {
            this.Conditions = new List<SqlCondition>();
            this.Orders = new List<SqlOrder>();
            this.Columns = new HashSet<string>();
            this.UpdateParameters = new List<SqlParameter>();
            this.UpdateCustomParameters = new List<SqlParameter>();
            this.UpdateCustomSqls = new List<string>();
            this.PageIndex = -1;
            this.PageSize = 15;
        }

        public Criteria(TableMapping tableMapping)
            : this()
        {
            this.TableMapping = tableMapping;
        }

        public Criteria AddOrderColumn(string column, SqlOrderType type = SqlOrderType.ASC)
        {
            this.Orders.Add(new SqlOrder(column, type));
            return this;
        }

        public Criteria AddColumn(string column)
        {
            this.Columns.Add(column);
            return this;
        }

        public Criteria AddUpdateColumn(string column, object val)
        {
            this.UpdateParameters.Add(new SqlParameter(column, val));
            return this;
        }

        public Criteria AddUpdateCustom(string strsql, params SqlParameter[] paras)
        {
            this.UpdateCustomSqls.Add(strsql);
            this.UpdateCustomParameters.AddRange(paras);
            return this;
        }

        public IList ToList()
        {
            IList list = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(TableMapping.ObjectType));
            DatabaseEngine db = DatabaseEngineFactory.GetDatabaseEngine();
            if (this.PageIndex == -1)
                db.Load(this, list);
            else
                db.LoadPage(this, list);
            return list;
        }

        public object FirstOrDefault()
        {
            List<object> list = new List<object>();
            DatabaseEngine db = DatabaseEngineFactory.GetDatabaseEngine();
            int bakTakeCount = this.TakeCount;
            try
            {
                this.TakeCount = 1;
                db.Load(this, list);
            }
            finally
            {
                this.TakeCount = bakTakeCount;
            }
            return list.FirstOrDefault();
        }

        public object Max(string column)
        {
            DatabaseEngine db = DatabaseEngineFactory.GetDatabaseEngine();
            return db.Max(this, column);
        }

        public int Count()
        {
            DatabaseEngine db = DatabaseEngineFactory.GetDatabaseEngine();
            return db.Count(this);
        }

        public int Delete()
        {
            return DatabaseEngineFactory.GetDatabaseEngine().Delete(this);
        }
    }
}
