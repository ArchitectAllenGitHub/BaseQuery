using AllenDynamicExpressions.Consts;
using AllenDynamicExpressions.Exceptions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AllenDynamicExpressions
{
    /// <summary>
    /// 规则校验
    /// </summary>
    public class AllenRuleValidation
    {
        /// <summary>
        /// 根据参数名映射
        /// 对应的属性
        /// 以及关联类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="inputName"></param>
        /// <returns></returns>
        public static void Mapping<T, I>(ChachePropertyInfo input)
        {
            var i = input.IPropertyInfo;
            input.TPropertyInfo = typeof(T).GetProperty(i.Name);

            if (input.TPropertyInfo != null)
            {
                if (i.PropertyType != input.TPropertyInfo.PropertyType)
                    throw ExceptionMessage.ExceptionPropertyTypeInconsistency4<T, I>(input.TPropertyInfo.Name, i.Name);
                return;
            }

            input.ExpressionType = ExpressionType.Call;

            if (i.PropertyType == typeof(string))
            {
                foreach (var item in AllenConstant.StringTypes)
                {
                    if (i.Name.EndsWith(item))
                    {
                        input.Method = item;
                        input.TPropertyInfo = typeof(T).GetProperty(i.Name.SplitEnd(item));

                        if (input.TPropertyInfo != null)
                        {
                            if (i.PropertyType != input.TPropertyInfo.PropertyType)
                                throw ExceptionMessage.ExceptionPropertyTypeInconsistency4<T, I>(input.TPropertyInfo.Name, i.Name);
                        }
                        return;
                    }
                }
            }

            if (i.Name.EndsWith(AllenConstant._Contains))
            {
                input.Method = AllenConstant._Contains;
                input.TPropertyInfo = typeof(T).GetProperty(i.Name.SplitEnd(AllenConstant._Contains));

                if (input.TPropertyInfo != null)
                {
                    if (i.PropertyType.IsArray)
                    {
                        MappingArray<T, I>(input);
                    }

                    if (i.PropertyType.IsGenericType)
                    {
                        MappingGeneric<T, I>(input);
                    }
                }
            }

            return;
        }

        /// <summary>
        /// 映射泛型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="I"></typeparam>
        /// <param name="input"></param>
        private static void MappingGeneric<T, I>(ChachePropertyInfo input)
        {
            var arguments = input.IPropertyInfo.PropertyType.GetGenericArguments();

            if (arguments.Length != 1 || input.TPropertyInfo.PropertyType != arguments[0])
                throw ExceptionMessage.ExceptionPropertyTypeInconsistency4<T, I>(input.TPropertyInfo.Name, input.IPropertyInfo.Name);
        }

        /// <summary>
        /// 映射数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="I"></typeparam>
        /// <param name="input"></param>
        private static void MappingArray<T, I>(ChachePropertyInfo input)
        {
            var type = input.IPropertyInfo.PropertyType.GetElementType();

            if (input.TPropertyInfo.PropertyType != type)
                throw ExceptionMessage.ExceptionPropertyTypeInconsistency4<T, I>(input.TPropertyInfo.Name, input.IPropertyInfo.Name);
        }
    }
}
