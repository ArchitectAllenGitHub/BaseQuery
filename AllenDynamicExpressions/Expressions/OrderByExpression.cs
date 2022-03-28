using AllenDynamicExpressions.Model;
using System.Linq.Expressions;

namespace AllenDynamicExpressions.Expressions
{
    /// <summary>
    /// 为OrderBy封装
    /// Expression<Func<T,object>>
    /// </summary>
    public class OrderByExpression
    {
        /// <summary>
        /// 获取分页表达式
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static IEnumerable<AllenOrderByExpressionResult<T>> GetOrderBy<T>(IEnumerable<OrderByEntity> input)
        {
            var res = new List<AllenOrderByExpressionResult<T>>();

            ParameterExpression sParameter = Expression.Parameter(typeof(T), "a");
            foreach (var item in input)
            {
                res.Add(new AllenOrderByExpressionResult<T>
                {
                    Ascending = item.Ascending,
                    OrderByExpression = Expression.Lambda<Func<T, object>>(Expression.Property(sParameter, typeof(T).GetProperty(item.OrderField)), sParameter)
                });
            }

            return res;
        }
    }
}
