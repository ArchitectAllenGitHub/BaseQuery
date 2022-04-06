using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllenDynamicExpressions.Model
{
    /// <summary>
    /// 排序实体
    /// </summary>
    public class OrderByEntity
    {
        /// <summary>
        /// 排序字段
        /// </summary>
        [Required]
        public string OrderField { get; set; }

        ///// <summary>
        ///// 是否升序
        ///// </summary>
        //[Required]
        //public bool Ascending { get; set; }
    }
}
