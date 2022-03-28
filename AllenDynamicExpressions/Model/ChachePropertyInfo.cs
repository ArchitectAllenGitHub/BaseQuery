using System.Linq.Expressions;
using System.Reflection;

namespace AllenDynamicExpressions
{
    /// <summary>
    /// 缓存属性
    /// </summary>
    public class ChachePropertyInfo
    {
        /// <summary>
        /// 参数属性
        /// </summary>
        public PropertyInfo IPropertyInfo { get; set; }

        /// <summary>
        /// 实体属性
        /// </summary>
        public PropertyInfo TPropertyInfo { get; set; }

        /// <summary>
        /// 表达式类型
        /// 默认:Equal
        /// </summary>
        public ExpressionType ExpressionType { get; set; } = ExpressionType.Equal;

        /// <summary>
        /// 方法
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// 缓存表达式信息
        /// </summary>
        public ChacheExpressionInfo ExpressionInfo { get; set; }
    }

    /// <summary>
    /// 缓存表达式目录树
    /// </summary>
    public class ChacheExpressionInfo
    {
        /// <summary>
        /// 参数
        /// </summary>
        public ParameterExpression parameter { get; set; }

        /// <summary>
        /// 左边表达式
        /// </summary>
        public Expression LeftExpression { get; set; }
    }
}