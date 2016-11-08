using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Jet.Framework.ObjectMapping
{
    public class MappingService
    {
        public static readonly MappingService Instance = new MappingService();
        private static object LockedObject = new object();

        private List<TableMapping> Tables;
        private Dictionary<Type, TableMapping> TableDict;

        private MappingService()
        {
            Tables = new List<TableMapping>();
            TableDict = new Dictionary<Type, TableMapping>();
        }

        public TableMapping GetTableMapping(object obj, bool throwerror = true)
        {
            return GetTableMapping(obj.GetType(), throwerror);
        }

        public TableMapping GetTableMapping(string type, bool throwerror = true)
        {
            return GetTableMapping(Type.GetType(type), throwerror);
        }

        public TableMapping GetTableMapping(Type type, bool throwerror = true)
        {
            lock (LockedObject)
            {
                TableMapping tablemapping = null;

                if (type != null)
                {
                    if (this.TableDict.ContainsKey(type))
                        tablemapping = this.TableDict[type];
                    else
                    {
                        TableAttribute tableattribute = Attribute.GetCustomAttribute(type, typeof(TableAttribute), false) as TableAttribute;
                        if (tableattribute != null)
                        {
                            tablemapping = CreateTableMapping(tableattribute, type);
                            this.Tables.Add(tablemapping);
                            this.TableDict.Add(type, tablemapping);
                        }
                    }
                }

                if (throwerror && tablemapping == null)
                    throw new ObjectMappingException("not find tablemapping define");

                return tablemapping;
            }
        }

        private void GetProperties(TableMapping table, Type type, List<PropertyInfo> list)
        {
            list.AddRange(type.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public));
        }

        private TableMapping CreateTableMapping(TableAttribute tableattribute, Type type)
        {
            TableMapping table = new TableMapping();
            table.Name = tableattribute.Name;
            table.ObjectType = type;

            if (string.IsNullOrEmpty(table.Name))
                table.Name = table.ObjectType.Name;

            List<PropertyInfo> props = new List<PropertyInfo>();
            GetProperties(table, type, props);

            foreach (PropertyInfo prop in props)
            {
                ColumnAttribute columnattribute = Attribute.GetCustomAttribute(prop, typeof(ColumnAttribute)) as ColumnAttribute;
                if (columnattribute == null)
                    table.OtherPropertys.Add(prop);
                else
                {
                    ColumnMapping column = new ColumnMapping();
                    column.Name = columnattribute.Name;
                    column.PropertyInfo = prop;
                    column.IsViewColumn = columnattribute.IsViewColumn;

                    if (string.IsNullOrEmpty(column.Name))
                    {
                        column.Name = prop.Name;
                    }

                    column.IsAutoIncrement = columnattribute.IsAutoIncrement;
                    column.IsPK = columnattribute.IsPK;
                    column.IsUseSeedFactory = columnattribute.IsUseSeedFactory;
                    //--------------------------------------------------------------------------------------------------------------------
                    Attribute[] attrs = Attribute.GetCustomAttributes(prop);
                    foreach (Attribute attr in attrs)
                    {
                        if (attr is DataInterceptAttribute)
                        {
                            if (column.DataIntercept == null)
                            {
                                DataInterceptAttribute dataintercept = attr as DataInterceptAttribute;
                                dataintercept.ColumnMapping = column;
                                column.DataIntercept = dataintercept;
                            }
                            else
                                throw new ObjectMappingException("Property have more DataInterceptAttribute define");
                        }
                    }
                    //--------------------------------------------------------------------------------------------------------------------
                    table.AddColumnMapping(column);
                }
            }

            if (table.ColumnPK == null)
                throw new ObjectMappingException("not find primary key");


            return table;
        }
    }
}
