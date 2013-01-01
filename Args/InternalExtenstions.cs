using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Collections;

namespace Args
{
    internal static class MemberInfoExtensions
    {
        internal static Type GetDeclaredType(this MemberInfo info)
        {
            var propertyInfo = info as PropertyInfo;

            if (propertyInfo != null) return propertyInfo.PropertyType;

            var fieldInfo = info as FieldInfo;

            if (fieldInfo != null) return fieldInfo.FieldType;

            throw new InvalidOperationException("Provided MemberInfo does not have a declared Type");
        }

        internal static void SetValue(this MemberInfo info, object obj, object value)
        {
            var propertyInfo = info as PropertyInfo;

            if (propertyInfo != null)
            {
                propertyInfo.SetValue(obj, value, null);
                return;
            }

            var fieldInfo = info as FieldInfo;

            if (fieldInfo != null)
            {
                fieldInfo.SetValue(obj, value);
                return;
            }

            throw new InvalidOperationException("Provided MemberInfo can not have a value set");
        }
    }

    internal static class TypeExtensions
    {
        internal static object GetDefaultValue(this Type type)
        {
            return type.IsValueType ? Activator.CreateInstance(type) : null;
        }

        /// <summary>
        /// Returns a type that represents the implemented IEnumberable&lt;&gt; (e.g. IEnumerable&lt;string&gt;)
        /// Returns null if the provided type is System.String
        /// </summary>
        /// <param name="type"></param>
        /// <returns>Returns null if none is defined</returns>
        internal static Type GetGenericIEnumerable(this Type type)
        {
            if (type == typeof(string)) return null;

            return type.GetInterfaces()
                    .Union(new[] { type })
                    .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                    .SingleOrDefault();
        }
    }

    internal static class CollectionExtensions
    {
        internal static void AddRange<T>(this ICollection<T> source, IEnumerable<T> items)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            items.ForEach(source.Add);
        }
    }

    internal static class EnumerableExtensions
    {
        internal static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            foreach (var item in source)
                action(item);
        }
    }

    internal static class ExpressionExtensions
    {
        internal static MemberInfo GetMemberInfoFromExpression<T, TReturn>(this Expression<Func<T, TReturn>> expression)
        {
            var member = expression.Body as MemberExpression;
            if (member == null) throw new InvalidOperationException("Only member access expressions may be used.");
            return member.Member;
        }
    }

    internal static class ArrayExtensions
    {
        internal static Array Convert<T>(this T[] source, Type arrayType)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (arrayType == null) throw new ArgumentNullException("arrayType");

            var returnvalue = Array.CreateInstance(arrayType, source.Length);

            for(var i = 0; i < source.Length; i++)
                returnvalue.SetValue(source[i], i);

            return returnvalue;
        }
    }
}
