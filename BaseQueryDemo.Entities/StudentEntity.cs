using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseQueryDemo.Entities
{
    /// <summary>
    /// 学生实体
    /// </summary>
    [SugarTable("Student", TableDescription = "学生表")]
    public class StudentEntity : BaseEntity
    {
        /// <summary>
        /// 姓名
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 学号
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 年龄
        /// </summary>
        public int? Age { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public int? Sex { get; set; }

        /// <summary>
        /// 学校Id
        /// </summary>
        public int? SchoolId { get; set; }
    }
}
