using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jet.Framework.Utility
{
    /// <summary>
    /// 定值随机概率
    /// </summary>
    public class ValueProbability<T>
    {
        public ValueProbability()
        {
        }

        public ValueProbability(T value, int probability)
        {
            this.Value = value;
            this.Probability = probability;
        }

        public T Value { get; set; }
        public int Probability { get; set; }
    }

    /// <summary>
    /// 按概率生成随机数
    /// </summary>
    public class CodyRandomer<T>
    {
        private List<T> ValueList = new List<T>();

        public CodyRandomer(List<ValueProbability<T>> valueProbabilityList)
        {
            foreach (var vp in valueProbabilityList)
            {
                for (int i = 0; i < vp.Probability; i++)
                {
                    this.ValueList.Add(vp.Value);
                }
            }
        }

        public T Random()
        {
            return this.ValueList[RandomHelper.RandomInt(0, this.ValueList.Count)];
        }
    }
}
