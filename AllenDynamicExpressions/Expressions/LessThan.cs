using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AllenDynamicExpressions.Expressions
{
    /// <summary>
    /// 小于表达式目录树
    /// </summary>
    public class LessThan<T>
    {
        /// <summary>
        /// 生成小于表达式
        /// </summary>
        /// <param name="cacheInfo"></param>
        /// <param name="iValue"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> Generate(KeyValuePair<string, ChachePropertyInfo> cacheInfo, object iValue)
        {
            var body = Expression.LessThan(cacheInfo.Value.ExpressionInfo.LeftExpression, Expression.Constant(iValue, cacheInfo.Value.TPropertyInfo.PropertyType));

            return Expression.Lambda<Func<T, bool>>(body, new ParameterExpression[1] { cacheInfo.Value.ExpressionInfo.parameter });
        }
    }
}
