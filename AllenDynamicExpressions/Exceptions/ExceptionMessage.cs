using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllenDynamicExpressions.Exceptions
{
    /// <summary>
    /// 异常消息
    /// </summary>
    internal class ExceptionMessage
    {
        /// <summary>
        /// 属性类型不一致
        /// </summary>
        private const string PropertyTypeInconsistency4 = "实体{0}与对应的查询{1}参数中[{2},{3}],类型不一致";

        /// <summary>
        /// 关键字
        /// </summary>
        private const string Keywords4 = "实体{0}与对应的查询{1}参数中[{2}],存在关键字符{3}";

        /// <summary>
        /// 获取关键词异常
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="I"></typeparam>
        /// <param name="tName"></param>
        /// <param name="iName"></param>
        /// <returns></returns>
        public static Exception ExceptionKeywords4<T, I>(string iName, string key)
        {
            return Exception(Keywords4, typeof(T).Name, typeof(I).Name, iName, key);
        }

        /// <summary>
        /// 属性不一致异常
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="I"></typeparam>
        /// <param name="tName"></param>
        /// <param name="iName"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static Exception ExceptionPropertyTypeInconsistency4<T, I>(string tName, string iName)
        {
            return Exception(PropertyTypeInconsistency4, typeof(T).Name, typeof(I).Name, tName, iName);
        }

        /// <summary>
        /// 返回异常
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="para"></param>
        /// <returns></returns>
        public static Exception Exception(string msg, params string[] para)
        {
            return new Exception(GetMessage(msg, para));
        }

        /// <summary>
        /// 返回消息
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="para"></param>
        /// <returns></returns>
        public static string GetMessage(string msg, params string[] para)
        {
            return string.Format(msg, para);
        }
    }
}
