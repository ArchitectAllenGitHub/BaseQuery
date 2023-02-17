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
        public static ChachePropertyInfo Mapping<T, I>(ChachePropertyInfo input)
        {
            //Input中的属性
            var i = input.IPropertyInfo;
            input.TPropertyInfo = typeof(T).GetProperty(i.Name);

            //如果名称一致,等于
            if (input.TPropertyInfo != null)
            {
                if (i.PropertyType != input.TPropertyInfo.PropertyType)
                    throw ExceptionMessage.ExceptionPropertyTypeInconsistency4<T, I>(input.TPropertyInfo.Name, i.Name);

                input.ExpressionType = ExpressionType.Equal;
                return input;
            }

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
                        else
                        {
                            input.ExpressionType = ExpressionType.Call;
                        }

                        if (input.TPropertyInfo != null)
                        {
                            if (i.PropertyType != input.TPropertyInfo.PropertyType)
                                throw ExceptionMessage.ExceptionPropertyTypeInconsistency4<T, I>(input.TPropertyInfo.Name, i.Name);
                        }
                        return input;
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

                return input;
            }

            if (i.Name.EndsWith(AllenConstant._Contains))
            {
                input.Method = AllenConstant._Contains;
                input.TPropertyInfo = typeof(T).GetProperty(i.Name.SplitEnd(AllenConstant._Contains));
                input.ExpressionType = ExpressionType.Call;

                CheckICollectionType(input);
            }

            return input;
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
        /// 校验集合类型和实体属性类型是否一致
        /// </summary>
        /// <param name="input"></param>
        public static void CheckICollectionType<T, I>(ChachePropertyInfo input)
        {
            if (input.TPropertyInfo == null)
            {
                return;
            }

            if (input.IPropertyInfo.PropertyType.IsArray)
            {
                MappingArray<T, I>(input);
            }

            if (input.IPropertyInfo.PropertyType.IsGenericType)
            {
                MappingGeneric<T, I>(input);
            }
        }

        /// <summary>
        /// 映射泛型,类型校验
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
        /// 映射数组,类型校验
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
