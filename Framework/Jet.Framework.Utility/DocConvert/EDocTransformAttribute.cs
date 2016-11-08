using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jet.Framework.Utility
{
    public enum EDocType
    {
        None = 0,
        Word = 1,
        Excel = 2,
        PowerPoint = 3,
        Media = 4,
        Pdf = 5,
    }

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class EDocTransformAttribute : Attribute
    {
        public EDocTransformAttribute(EDocType transformType)
        {
            this.TransformType = transformType;
        }
        public EDocTransformAttribute(EDocType transformType, EDocType targetType)
            : this(transformType)
        {
            this.TargetType = targetType;
        }
        /// <summary>
        /// 原始类型
        /// </summary>
        public EDocType TransformType { get; set; }

        /// <summary>
        /// 转换目标类型
        /// </summary>
        public EDocType TargetType { get; set; }
    }
}
