using AllenDynamicExpressions.Consts;
using AllenDynamicExpressions.Model;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace AllenDynamicExpressions.Expressions
{
    /// <summary>
    /// 包含Contains
    /// Expression<Func<T,bool>>
    /// </summary>
    public class CallExpression<T>
    {
        /// <summary>
        /// 生成
        /// </summary>
        /// <param name="cacheInfo"></param>
        /// <param name="iValue"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> Generate(KeyValuePair<string, ChachePropertyInfo> cacheInfo, object iValue)
        {
            switch (cacheInfo.Value.Method)
            {
                case AllenConstant.Contains:
                case AllenConstant.StartsWith:
                case AllenConstant.EndsWith:
                    return StringContains(cacheInfo, iValue);
                case AllenConstant._Contains:
                    return CollectionContains(cacheInfo, iValue);
                case AllenConstant.GreaterThan:
                    return GreaterThan(cacheInfo, iValue);
                default:
                    throw new NotImplementedException(cacheInfo.Value.Method);
            }
        }

        /// <summary>
        /// 大于
        /// </summary>
        /// <param name="cacheInfo"></param>
        /// <param name="iValue"></param>
        private static Expression<Func<T, bool>> GreaterThan(KeyValuePair<string, ChachePropertyInfo> cacheInfo, object iValue)
        {

        }

        /// <summary>
        /// 字符串的包含
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private static Expression<Func<T, bool>> StringContains(KeyValuePair<string, ChachePropertyInfo> cacheInfo, object iValue)
        {
            var body = Expression.Call(
                                  cacheInfo.Value.ExpressionInfo.LeftExpression,
                                  GetStringMethodInfo(cacheInfo.Value.Method),
                                  new Expression[]
                                  {
                                      Expression.Constant(iValue, cacheInfo.Value.TPropertyInfo.PropertyType)
                                  });

            return Expression.Lambda<Func<T, bool>>(body, new ParameterExpression[1] { cacheInfo.Value.ExpressionInfo.parameter });
        }

        /// <summary>
        /// In
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private static Expression<Func<T, bool>> CollectionContains(KeyValuePair<string, ChachePropertyInfo> cacheInfo, object iValue)
        {
            var prop = cacheInfo.Value.TPropertyInfo;

            var body = Expression.Call(null, AllenConstant.CollectionContainsMethod.MakeGenericMethod(
                prop.PropertyType),
                new Expression[2]
                {
                    Expression.Constant(iValue, typeof(IEnumerable<>).MakeGenericType(prop.PropertyType)),
                    Expression.Property(cacheInfo.Value.ExpressionInfo.parameter, prop)
                });

            return Expression.Lambda<Func<T, bool>>(body, new ParameterExpression[1]
            {
                cacheInfo.Value.ExpressionInfo.parameter
            });
        }

        /// <summary>
        /// 获取方法信息
        /// </summary>
        /// <param name="methodName"></param>
        /// <returns></returns>
        private static MethodInfo GetStringMethodInfo(string methodName)
        {
            return AllenConstant.StringContainsMethods.GetOrAdd(methodName, typeof(string).GetMethod(methodName, new Type[] { typeof(string) }));
        }
    }
}
