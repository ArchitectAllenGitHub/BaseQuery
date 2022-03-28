using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseQueryDemo.Entities
{
    /// <summary>
    /// 学校
    /// </summary>
    [SugarTable("School", TableDescription = "学校表")]
    public class SchoolEntity : BaseEntity
    {
        /// <summary>
        /// 学校名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 学校代码
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string Adress { get; set; }

        /// <summary>
        /// 类别
        /// </summary>
        public SchoolCategoryEnum? Category { get; set; }
    }
}
