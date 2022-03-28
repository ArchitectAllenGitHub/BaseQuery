using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AllenDynamicExpressions
{
    /// <summary>
    /// 工具类
    /// </summary>
    internal static class Helper
    {
        /// <summary>
        /// 切割尾巴
        /// </summary>
        /// <param name="value"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static string SplitEnd(this string value, string end)
        {
            if (string.IsNullOrWhiteSpace(value) || string.IsNullOrWhiteSpace(end))
                return value;

            if (value.EndsWith(end))
                return value.Substring(0, value.Length - end.Length);

            return value;
        }
    }
}
