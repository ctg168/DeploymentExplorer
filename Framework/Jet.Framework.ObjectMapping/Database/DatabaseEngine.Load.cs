using Jet.Framework.Utility;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Jet.Framework.ObjectMapping
{
    public abstract partial class DatabaseEngine
    {
        public void Load(Criteria criteria, JArray list)
        {
            SqlParameterCollection paras = new SqlParameterCollection();
            string strSQL = this.GetLoadSqlString(criteria, paras);
            using (IDataReader dr = this.ExecuteReader(strSQL, paras.ToArray()))
            {
                int fieldcount = dr.FieldCount;
                List<string> fieldnames = new List<string>();
                for (int i = 0; i < fieldcount; i++)
                {
                    fieldnames.Add(dr.GetName(i));
                }

                while (dr.Read())
                {
                    JObject obj = new JObject();
                    foreach (string fieldname in fieldnames)
                    {
                        object val = dr[fieldname];
                        if (val == DBNull.Value)
                            continue;
                        obj[fieldname] = JToken.FromObject(val);
                    }
                    list.Add(obj);
                }
            }
        }

        public void Load(Criteria criteria, IList list)
        {
            SqlParameterCollection paras = new SqlParameterCollection();
            string strSQL = this.GetLoadSqlString(criteria, paras);
            using (IDataReader dr = this.ExecuteReader(strSQL, paras.ToArray()))
            {
                this.Load(criteria.TableMapping.ObjectType, list, dr);
            }
        }

        public void LoadPage(Criteria criteria, IList list)
        {
            SqlParameterCollection paras = new SqlParameterCollection();
            string strSQL = this.GetLoadPageSqlString(criteria, paras);

            using (IDataReader dr = this.ExecuteReader(strSQL, paras.ToArray()))
            {
                this.Load(criteria.TableMapping.ObjectType, list, dr);
            }
        }

        public void Load(Type objectType, IList list, IDataReader dr)
        {
            if (objectType.IsPrimitive || (objectType == typeof(string)))
            {
                while (dr.Read())
                {
                    object val = dr[0];
                    if (val == DBNull.Value)
                    {
                        if (objectType == typeof(string))
                            list.Add(null);
                        else
                            list.Add(Activator.CreateInstance(objectType));
                    }
                    else
                    {
                        list.Add(Convert.ChangeType(val, objectType));
                    }
                }
            }
            else
            {
                int fieldcount = dr.FieldCount;
                List<string> fieldnames = new List<string>();
                for (int i = 0; i < fieldcount; i++)
                {
                    fieldnames.Add(dr.GetName(i));
                }

                TableMapping tableMapping = MappingService.Instance.GetTableMapping(objectType, false);

                while (dr.Read())
                {
                    object obj = Activator.CreateInstance(objectType);
                    IDynamicProperty dynObj = obj as IDynamicProperty;

                    foreach (string fieldname in fieldnames)
                    {
                        object val = dr[fieldname];

                        ColumnMapping column = null;
                        if (tableMapping != null)
                            column = tableMapping.GetColumnMappingByColumn(fieldname);
                        if (column != null)
                            column.SetValue(obj, val);
                        else if (val != DBNull.Value)
                        {
                            PropertyInfo prop = objectType.GetProperty(fieldname);
                            if (prop == null && dynObj != null)
                                dynObj.SetData(fieldname, val);
                            else if (prop != null)
                            {
                                if (val.GetType() != prop.PropertyType)
                                    val = Convert.ChangeType(val, prop.PropertyType);
                                prop.SetValue(obj, val, null);
                            }
                        }
                    }
                    list.Add(obj);
                }
            }
        }
    }
}
