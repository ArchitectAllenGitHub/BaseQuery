using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AllenDynamicExpressions
{
    /// <summary>
    /// 返回类型
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AllenExpressionResult<T>
    {
        public bool Condition { get; set; }

        public Expression<Func<T, bool>> Expression { get; set; }
    }

    /// <summary>
    /// 排序返回类型
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AllenOrderByExpressionResult<T>
    {
        /// <summary>
        /// 排序表达式目录树
        /// </summary>
        public Expression<Func<T, object>> OrderByExpression { get; set; }

        /// <summary>
        /// 是否升序
        /// </summary>
        [Required]
        public bool Ascending { get; set; }
    }
}
