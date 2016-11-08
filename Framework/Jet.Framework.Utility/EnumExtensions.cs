using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Jet.Framework.Utility
{
    public static class EnumExtensions
    {




        /// <summary>
        /// 枚举中文值,如果未找到中文值，返回NULL
        /// </summary>
        /// <param name="oEnum"></param>
        /// <returns></returns>
        public static string GetText(this Enum oEnum)
        {
            return oEnum.GetAttachedData<string>(AttachDataType.Text);
        }


        /// <summary>
        /// 根据枚举文本获取枚举对象
        /// </summary>
        public static T ParseText<T>(this Type enumType, string text) where T : struct
        {
            if (enumType.IsEnum)
            {
                foreach (int val in enumType.GetValues())
                {
                    if (text == ((Enum)Enum.Parse(enumType, val.ToString(), true)).GetText())
                    {
                        return (T)Enum.ToObject(enumType, val);
                    }
                }

                return default(T);
            }

            throw new ArgumentException("此方法仅支持枚举类型");
        }

        /// <summary>
        /// 获得枚举的所有名称
        /// </summary>
        /// <param name="oEnum"></param>
        /// <returns></returns>
        public static List<string> GetNames(this Type enumtype)
        {
            List<string> enumNames = new List<string>();

            foreach (FieldInfo fi in enumtype.GetFields(BindingFlags.Static | BindingFlags.Public))
            {
                enumNames.Add(fi.Name);
            }

            return enumNames;
        }

        /// <summary>
        /// 获得枚举的所有的AttachData文本
        /// </summary>
        public static List<string> GetTexts(this Type enumtype)
        {
            List<string> enumNames = new List<string>();

            foreach (var val in Enum.GetValues(enumtype))
            {
                enumNames.Add(((Enum)val).GetText());
            }

            return enumNames;
        }

        /// <summary>
        /// 获得枚举的所有值
        /// </summary>
        public static List<int> GetValues(this Type enumtype)
        {
            List<int> enumNames = new List<int>();

            foreach (var val in Enum.GetValues(enumtype))
            {
                enumNames.Add((int)val);
            }

            return enumNames;
        }



        public static List<int> GetGroupEnum(this Enum oEnum, string groupName)
        {
            List<int> enumValues = new List<int>();

            foreach (FieldInfo fi in oEnum.GetType().GetFields(BindingFlags.Static | BindingFlags.Public))
            {
                var temp = fi.GetCustomAttributes(typeof(AttachGroupAttribute), false);
                if (temp != null)
                {
                    if (((AttachGroupAttribute[])temp).Any(s => s.GroupName == groupName))
                    {
                        enumValues.Add((int)Enum.Parse(oEnum.GetType(), fi.Name, false));
                    }
                }
            }

            return enumValues;
        }
    }

    public enum AttachDataType
    {
        Text
    }

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class AttachDataAttribute : Attribute
    {
        public AttachDataAttribute(object value)
        {
            this.Key = AttachDataType.Text;
            this.Value = value;
        }

        public AttachDataAttribute(object key, object value)
        {
            this.Key = key;
            this.Value = value;
        }

        public object Key { get; private set; }
        public object Value { get; private set; }
    }

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class AttachGroupAttribute : Attribute
    {
        public AttachGroupAttribute(string groupName)
        {
            this.GroupName = groupName;
        }

        public string GroupName { get; set; }
    }

    public static class AttachDataExtensions
    {
        public static object GetAttachedData(this ICustomAttributeProvider provider, object key)
        {
            var attributes = (AttachDataAttribute[])provider.GetCustomAttributes(
                typeof(AttachDataAttribute), false);
            return attributes.First(a => a.Key.Equals(key)).Value;
        }

        public static T GetAttachedData<T>(this ICustomAttributeProvider provider, object key)
        {
            return (T)provider.GetAttachedData(key);
        }

        public static object GetAttachedData(this Enum value, object key)
        {
            ICustomAttributeProvider provider = value.GetType().GetField(value.ToString());
            if (provider != null)
                return provider.GetAttachedData(key);
            else
                return null;
        }

        public static T GetAttachedData<T>(this Enum value, object key)
        {
            return (T)value.GetAttachedData(key);
        }
    }











}
