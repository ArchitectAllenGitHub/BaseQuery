using BaseQueryDemo.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseQueryDemo.Model.Response
{
    /// <summary>
    /// 学生返回类型
    /// </summary>
    public class StudentResponse : StudentEntity
    {
        /// <summary>
        /// 学校名称
        /// </summary>
        public string SchoolName { get; set; }

        /// <summary>
        /// 学校编码Join使用
        /// Entity无效字段
        /// </summary>
        public int? SchoolCode { get; set; }

        /// <summary>
        /// 学校地址
        /// </summary>
        public string SchoolAdress { get; set; }
    }
}
