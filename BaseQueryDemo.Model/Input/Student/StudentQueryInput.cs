using BaseQueryDemo.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseQueryDemo.Model.Input
{
    /// <summary>
    /// 学生查询
    /// </summary>
    public class StudentQueryInput : StudentEntity
    {
        /// <summary>
        /// 包含xx名称--后缀[Contains]
        /// </summary>
        public string NameContains { get; set; }

        /// <summary>
        /// 名称以xx开头--后缀[StartsWith]
        /// </summary>
        public string NameStartsWith { get; set; }

        /// <summary>
        /// 名称以xx结尾--后缀[EndsWith]
        /// </summary>
        public string NameEndsWith { get; set; }

        /// <summary>
        /// 集合包含---后缀[_Contains]
        /// </summary>
        public List<string> Name_Contains { get; set; }

        /// <summary>
        /// 集合包含---后缀[_Contains]
        /// </summary>
        public string[] Code_Contains { get; set; }

        /// <summary>
        /// 集合包含---后缀[_Contains]
        /// </summary>
        public IEnumerable<string> CreateUserId_Contains { get; set; }

        /// <summary>
        /// 集合包含---后缀[_Contains]
        /// </summary>
        public List<int?> Id_Contains { get; set; }

        /// <summary>
        /// 集合包含---后缀[_Contains]
        /// </summary>
        public int?[] Age_Contains { get; set; }

        /// <summary>
        /// 集合包含---后缀[_Contains]
        /// </summary>
        public IEnumerable<int?> Sex_Contains { get; set; }

        /// <summary>
        /// 错误示范 实体定义的是int? 这边定义int 是不行的.
        /// </summary>
        //public List<int> Age_Contains { get; set; }

        /// <summary>
        /// 学校名称Join使用
        /// Entity无效字段
        /// </summary>
        public IEnumerable<string> SchoolName_Contains { get; set; }

        /// <summary>
        /// 学校编码Join使用
        /// Entity无效字段
        /// </summary>
        public int? SchoolCode { get; set; }

        /// <summary>
        /// 学校地址
        /// </summary>
        public string SchoolAdress { get; set; }

        /// <summary>
        /// ID 大于
        /// </summary>
        public int? IdGreaterThan { get; set; }
    }
}
