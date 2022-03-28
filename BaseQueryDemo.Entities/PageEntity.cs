using AllenDynamicExpressions.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BaseQueryDemo.Entities
{
    /// <summary>
    /// 分页实体
    /// </summary>
    public class PageEntity<T> where T : class, new()
    {
        /// <summary>
        /// 排序集合
        /// </summary>
        public IEnumerable<OrderByEntity> OrderByEntity { get; set; }

        /// <summary>
        /// 分页大小
        /// </summary>
        [Required]
        [Range(1, int.MaxValue)]
        public int PageSize { get; set; } = 30;

        /// <summary>
        /// 当前页码
        /// </summary>
        [Required]
        [Range(1, int.MaxValue)]
        public int PageIndex { get; set; }

        /// <summary>
        /// 总条数，不需要填写
        /// </summary>
        [JsonIgnore]
        public int Total { get; set; }

        /// <summary>
        /// 总页数，不需要填写
        /// </summary>
        [JsonIgnore]
        public int TotalPage => (int)(Total / ((PageSize == 0) ? 30 : PageSize) + ((Total % ((PageSize == 0) ? 30 : PageSize) != 0L) ? 1 : 0));

        /// <summary>
        /// 数据
        /// </summary>
        public T Data { get; set; }
    }
}
