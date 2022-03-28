using BaseQueryDemo.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseQueryDemo.Model.Input
{
    /// <summary>
    /// 学校查询
    /// </summary>
    public class SchoolQueryInput : SchoolEntity
    {
        /// <summary>
        /// 枚举 IN
        /// </summary>
        public IEnumerable<SchoolCategoryEnum?> Category_Contains { get; set; }
    }
}
