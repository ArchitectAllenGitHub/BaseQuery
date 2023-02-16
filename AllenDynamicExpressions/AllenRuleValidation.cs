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

                        if (item == AllenConstant.NotEqual)
                            input.ExpressionType = GetExpressionType(input.IPropertyInfo).Value;

                        if (input.TPropertyInfo != null)
                        {
                            if (i.PropertyType != input.TPropertyInfo.PropertyType)
                                throw ExceptionMessage.ExceptionPropertyTypeInconsistency4<T, I>(input.TPropertyInfo.Name, i.Name);
                        }
                        return;
                    }
                }
            }
            else if (i.PropertyType == typeof(int?))
            {
                var type = GetExpressionType(input.IPropertyInfo);

                if (type != null)
                {
                    input.ExpressionType = type.Value;
                    input.Method = input.ExpressionType.ToString();
                    input.TPropertyInfo = typeof(T).GetProperty(i.Name.SplitEnd(input.Method));
                    if (input.TPropertyInfo != null)
                    {
                        if (i.PropertyType != input.TPropertyInfo.PropertyType)
                            throw ExceptionMessage.ExceptionPropertyTypeInconsistency4<T, I>(input.TPropertyInfo.Name, i.Name);
                    }
                }

                return;
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
        /// 获取表达式目录树类型
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <returns></returns>
        public static ExpressionType? GetExpressionType(PropertyInfo propertyInfo)
        {
            if (propertyInfo.Name.EndsWith(AllenConstant.LessThan))
            {
                return ExpressionType.LessThan;
            }
            else if (propertyInfo.Name.EndsWith(AllenConstant.LessThanOrEqual))
            {
                return ExpressionType.LessThanOrEqual;
            }
            else if (propertyInfo.Name.EndsWith(AllenConstant.GreaterThan))
            {
                return ExpressionType.GreaterThan;
            }
            else if (propertyInfo.Name.EndsWith(AllenConstant.GreaterThanOrEqual))
            {
                return ExpressionType.GreaterThanOrEqual;
            }
            else if (propertyInfo.Name.EndsWith(AllenConstant.NotEqual))
            {
                return ExpressionType.NotEqual;
            }
            else
            {
                return default;
            }
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
