using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AllenDynamicExpressions.Consts
{
    /// <summary>
    /// 常量
    /// </summary>
    internal class AllenConstant
    {
        //var method3 = typeof(Enumerable).GetMethod("ToList", BindingFlags.Static | BindingFlags.Public);//OK
        //var method1 = typeof(Enumerable).GetMethod("Contains", new Type[] { typeof(IEnumerable<>).MakeGenericType(typeof(string)), typeof(string) });
        //var method2 = typeof(Enumerable).GetMethod("Contains", BindingFlags.Static | BindingFlags.Public);
        //var method2 = typeof(Enumerable).GetMethod("Contains", BindingFlags.Public | BindingFlags.Static, new Type[] { typeof(IEnumerable<string>), typeof(string) });
        /// <summary>
        /// 集合方法
        /// 运行时方法,好像无法通过上述方式获取
        /// 全局泛型Contains方法
        /// </summary>
        public static readonly MethodInfo CollectionContainsMethod = typeof(Enumerable).GetMethods().FirstOrDefault(e => e.Name == "Contains" && e.GetParameters().Length == 2);

        /// <summary>
        /// String方法
        /// </summary>
        public static readonly ConcurrentDictionary<string, MethodInfo> StringContainsMethods = new ConcurrentDictionary<string, MethodInfo>();

        public const string Contains = "Contains";

        /// <summary>
        /// 集合包含In
        /// </summary>
        public const string _Contains = "_Contains";

        /// <summary>
        /// 以xx开头
        /// </summary>
        public const string StartsWith = "StartsWith";

        /// <summary>
        /// 以xx结尾
        /// </summary>
        public const string EndsWith = "EndsWith";

        /// <summary>
        /// 字符串类型
        /// </summary>
        public static List<string> StringTypes = new List<string>()
        {
            Contains,
            StartsWith,
            EndsWith,
        };
    }
}
