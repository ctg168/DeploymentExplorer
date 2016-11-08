using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jet.Framework.Utility
{
    public static class RandomHelper
    {
        private static Random RandomObj = new Random((int)DateTime.Now.Ticks);

        public static int RandomInt(int minValue, int maxValue)
        {
            return RandomObj.Next(minValue, maxValue);
        }

        /// <summary>
        /// 将集合打乱顺序
        /// </summary>
        public static void RandomList<T>(this IList<T> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                int index = RandomHelper.RandomInt(0, list.Count);
                T temp = list[i];
                list[i] = list[index];
                list[index] = temp;
            }
        }

        /// <summary>
        /// 获得随机的子集合
        /// </summary>
        public static List<T> GetSubRandomList<T>(this IList<T> list, int subListCount)
        {
            if (subListCount > list.Count)
                throw "子集合大小不能大于父集合大小".CreateException();

            List<T> subList = new List<T>();
            List<T> listTemp = new List<T>(list);
            for (int i = 0; i < subListCount; i++)
            {
                int index = RandomHelper.RandomInt(0, listTemp.Count);
                subList.Add(listTemp[index]);
                listTemp.RemoveAt(index);
            }
            return subList;
        }

        /// <summary>
        /// 获得随机的子集合
        /// </summary>
        public static T GetRandomObj<T>(this IList<T> list)
        {
            return list[RandomHelper.RandomInt(0, list.Count)];
        }

        /// <summary>
        /// 根据百分比概率取得值范围内的一个值
        /// </summary>
        /// <param name="minValue">最小值</param>
        /// <param name="middleValue">通过值</param>
        /// <param name="maxValue">最大值</param>
        /// <param name="greaterPercent">通过百分比概率</param>
        /// <returns></returns>
        public static int GetRangeRandomValue(int minValue, int middleValue, int maxValue, int greaterPercent)
        {
            if (GetChance(greaterPercent))
            {
                decimal value = (decimal)(RandomHelper.RandomInt(100, 201)) / 100;//100-200
                decimal temp = 1 - (value - 1) * (value - 1);
                return middleValue + (int)Math.Round((maxValue - middleValue) * temp);
            }
            else
            {
                decimal value = (decimal)(RandomHelper.RandomInt(1, 100)) / 100;//1-99
                decimal temp = 1 - (value - 1) * (value - 1);
                return (int)Math.Round(middleValue * temp);
            }
        }

        /// <summary>
        /// 根据百分比概率取得值范围内的一个值
        /// </summary>
        /// <param name="minValue">最小值</param>
        /// <param name="middleValue">通过值</param>
        /// <param name="maxValue">最大值</param>
        /// <param name="greaterPercent">通过百分比概率</param>
        /// <returns></returns>
        public static decimal GetRangeRandomValue(decimal minValue, decimal middleValue, decimal maxValue, int greaterPercent)
        {
            if (GetChance(greaterPercent))
            {
                decimal value = (decimal)(RandomHelper.RandomInt(100, 201)) / 100;//100-200
                decimal temp = 1 - (value - 1) * (value - 1);
                return middleValue + (((maxValue - middleValue) * temp));
            }
            else
            {
                decimal value = (decimal)(RandomHelper.RandomInt(1, 100)) / 100;//1-99
                decimal temp = 1 - (value - 1) * (value - 1);
                return middleValue * temp;
            }
        }

        /// <summary>
        /// 获得一个百分比概率的机会
        /// </summary>
        public static bool GetChance(int passValue, int maxValue = 100)
        {
            return RandomHelper.RandomInt(0, maxValue) <= passValue;
        }

        public static List<T> GetRandomSubList<T>(List<T> list, int count)
        {
            List<T> listTemp = new List<T>(list);
            List<T> sublist = new List<T>();

            for (int i = 0; i < count; i++)
            {
                int index = RandomHelper.RandomInt(0, listTemp.Count);
                sublist.Add(listTemp[index]);
                listTemp.RemoveAt(index);
            }
            return sublist;
        }
    }
}
