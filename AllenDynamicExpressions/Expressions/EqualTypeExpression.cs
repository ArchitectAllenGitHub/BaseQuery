using System.Linq.Expressions;

namespace AllenDynamicExpressions.Expressions
{
    /// <summary>
    /// 相等类型的表达式目录树
    /// </summary>
    public class EqualTypeExpression<T>
    {
        /// <summary>
        /// 拼装
        /// </summary>
        /// <param name="query"></param>
        /// <param name="tPropertyInfo"></param>
        /// <param name="iPropertyInfo"></param>
        /// <param name="iValue"></param>
        public static Expression<Func<T, bool>> Generate(KeyValuePair<string, ChachePropertyInfo> cacheInfo, object iValue)
        {
            switch (cacheInfo.Value.IPropertyInfo.PropertyType.Name)
            {
                case "String":
                    if (!string.IsNullOrEmpty(iValue.ToString()))
                        return EqualExpression(cacheInfo, iValue);
                    break;
                default:
                    if (iValue != null)
                        return EqualExpression(cacheInfo, iValue);
                    break;
            }
            return default;
        }

        /// <summary>
        /// 拼装表达式目录树
        /// </summary>
        /// <param name="property"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        private static Expression<Func<T, bool>> EqualExpression(KeyValuePair<string, ChachePropertyInfo> cacheInfo, object iValue)
        {
            var body = Expression.Equal(cacheInfo.Value.ExpressionInfo.LeftExpression, Expression.Constant(iValue, cacheInfo.Value.TPropertyInfo.PropertyType));

            return Expression.Lambda<Func<T, bool>>(body, new ParameterExpression[1] { cacheInfo.Value.ExpressionInfo.parameter });
        }
    }
}
