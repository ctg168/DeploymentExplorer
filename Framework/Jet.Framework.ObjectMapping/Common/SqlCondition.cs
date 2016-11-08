using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jet.Framework.ObjectMapping
{
    public enum SqlConditionType
    {
        EqualTo,
        GreaterThan,
        GreaterThanAndEqualTo,
        NotEqualTo,
        LessThan,
        LessThanAndEqualTo,
        Match,
        MatchPrefix,
        In,
        NotMatch,
        NotMatchPrefix,
        NotIn,
        MatchSuffix,
        NotMatchSuffix,
        MatchFullText,
        Custom
    }
    public class SqlCondition
    {
        public SqlCondition()
        {
        }

        /// <summary>
        /// 条件类型
        /// </summary>
        public SqlConditionType Type { get; set; }

        /// <summary>
        /// 列名
        /// </summary>
        public string Column { get; set; }

        /// <summary>
        /// 条件数值
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// 条件数值2    (Between需要两个条件参数)
        /// </summary>
        public object Value2 { get; set; }

        /// <summary>
        /// 条件数值集合
        /// </summary>
        public IList Values { get; set; }

        /// <summary>
        /// 参数集合
        /// </summary>
        public SqlParameterCollection Parameters { get; set; }
    }
}
