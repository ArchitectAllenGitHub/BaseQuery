using AllenDynamicExpressions.Consts;
using AllenDynamicExpressions.Expressions;
using AllenDynamicExpressions.Model;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace AllenDynamicExpressions
{
    /// <summary>
    /// 抽象动态表达式
    /// 
    /// 还需要实现两个
    /// 2.集合IN
    /// 3.Like实现.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="I"></typeparam>
    public class BaseDynamicExpression<T, I>
    {
        static BaseDynamicExpression()
        {
            Init();
        }

        /// <summary>
        /// 条件结果
        /// </summary>
        public AllenExpressionResult<T> allenExpressionResult { get; private set; }

        /// <summary>
        /// 缓存内容
        /// </summary>
        private static readonly CacheAll _Chache = new CacheAll();

        /// <summary>
        /// 条件集合
        /// </summary>
        private List<Expression<Func<T, bool>>> _expressionList = new List<Expression<Func<T, bool>>>();

        ///// <summary>
        ///// 获取排序表达式目录树
        ///// </summary>
        ///// <param name="input"></param>
        ///// <returns></returns>
        //public IEnumerable<AllenOrderByExpressionResult<T>> GetOrderBy(IEnumerable<OrderByEntity> input)
        //{
        //    return OrderByExpression.GetOrderBy<T>(input);
        //}

        /// <summary>
        /// 获取表达式目录树
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public AllenExpressionResult<T> GetExpression(I i)
        {
            Expression<Func<T, bool>> expression;
            foreach (var item in _Chache.PropertyInfoChache)
            {
                var value = item.Value.IPropertyInfo.GetValue(i);
                if (value == null)
                    continue;

                switch (item.Value.ExpressionType)
                {
                    case ExpressionType.Call:
                        expression = CallExpression<T>.Generate(item, value);
                        break;
                    case ExpressionType.GreaterThan:
                        expression = GreaterThanExpression<T>.Generate(item, value);
                        break;
                    case ExpressionType.GreaterThanOrEqual:
                        expression = GreaterThanOrEqual<T>.Generate(item, value);
                        break;
                    case ExpressionType.LessThan:
                        expression = LessThan<T>.Generate(item, value);
                        break;
                    case ExpressionType.LessThanOrEqual:
                        expression = LessThanOrEqual<T>.Generate(item, value);
                        break;
                    case ExpressionType.NotEqual:
                        expression = NotEqual<T>.Generate(item, value);
                        break;
                    case ExpressionType.Equal:
                    default:
                        expression = EqualTypeExpression<T>.Generate(item, value);
                        break;
                }

                if (expression != null)
                    _expressionList.Add(expression);
            }

            return ConcatExpression();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="i">input入参</param>
        /// <param name="t">实体</param>
        private static void Init()
        {
            if (_Chache.PropertyInfoChache.Count == 0)
                foreach (var property in typeof(I).GetProperties())
                {
                    var info = AllenRuleValidation.Mapping<T, I>(new ChachePropertyInfo
                    {
                        IPropertyInfo = property
                    });

                    if (info.TPropertyInfo != null)
                    {
                        var para = Expression.Parameter(typeof(T), "a");
                        info.ExpressionInfo = new ChacheExpressionInfo
                        {
                            parameter = para,
                            LeftExpression = Expression.Property(para, info.TPropertyInfo)
                        };

                        _Chache.PropertyInfoChache.Add(property.Name, info);
                    }
                }
        }

        /// <summary>
        /// 拼装查询条件
        /// </summary>
        /// <returns></returns>
        private AllenExpressionResult<T> ConcatExpression()
        {
            allenExpressionResult = new AllenExpressionResult<T>
            {
                Condition = _expressionList.Count > 0,
            };

            foreach (var item in _expressionList)
                allenExpressionResult.Expression = AndTypeExpression.And(allenExpressionResult.Expression, item);

            _expressionList.Clear();
            return allenExpressionResult;
        }
    }
}
