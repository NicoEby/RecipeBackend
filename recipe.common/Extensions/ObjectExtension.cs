using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using ch.thommenmedia.common.Utils;

namespace ch.thommenmedia.common.Extensions
{
    public static class ObjectExtensions
    {
        private static readonly MethodInfo CloneMethod =
            typeof(object).GetMethod("MemberwiseClone", BindingFlags.NonPublic | BindingFlags.Instance);

        public static bool IsPrimitive(this Type type)
        {
            if (type == typeof(string) || type == typeof(bool) || type == typeof(bool?)) return true;
            return type.IsValueType & type.IsPrimitive;
        }

        private static string GetPropertyName<TPropertySource>
            (this object targetObject, Expression<Func<TPropertySource, object>> expression)
        {
            var lambda = expression as LambdaExpression;
            MemberExpression memberExpression;
            if (lambda.Body is UnaryExpression)
            {
                var unaryExpression = lambda.Body as UnaryExpression;
                memberExpression = unaryExpression.Operand as MemberExpression;
            }
            else
            {
                memberExpression = lambda.Body as MemberExpression;
            }

            var propertyInfo = memberExpression?.Member as PropertyInfo;

            return propertyInfo?.Name;
        }

        public static bool IsNullable<T>(T obj)
        {
            if (obj == null) return true; // obvious
            var type = typeof(T);
            if (!type.IsValueType) return true; // ref-type
            if (Nullable.GetUnderlyingType(type) != null) return true; // Nullable<T>
            return false; // value-type
        }

        public static object Copy(this object originalObject)
        {
            return InternalCopy(originalObject, new Dictionary<object, object>(new ReferenceEqualityComparer()));
        }

        private static object InternalCopy(object originalObject, IDictionary<object, object> visited)
        {
            if (originalObject == null) return null;
            var typeToReflect = originalObject.GetType();
            if (IsPrimitive(typeToReflect)) return originalObject;
            if (visited.ContainsKey(originalObject)) return visited[originalObject];
            var cloneObject = CloneMethod.Invoke(originalObject, null);
            if (typeToReflect.IsArray)
            {
                var arrayType = typeToReflect.GetElementType();
                if (IsPrimitive(arrayType) == false)
                {
                    var clonedArray = (Array) cloneObject;
                    clonedArray.ForEach((array, indices) =>
                        array.SetValue(InternalCopy(clonedArray.GetValue(indices), visited), indices));
                }
            }
            visited.Add(originalObject, cloneObject);
            CopyFields(originalObject, visited, cloneObject, typeToReflect);
            RecursiveCopyBaseTypePrivateFields(originalObject, visited, cloneObject, typeToReflect);
            return cloneObject;
        }

        private static void RecursiveCopyBaseTypePrivateFields(object originalObject,
            IDictionary<object, object> visited, object cloneObject, Type typeToReflect)
        {
            if (typeToReflect.BaseType != null)
            {
                RecursiveCopyBaseTypePrivateFields(originalObject, visited, cloneObject, typeToReflect.BaseType);
                CopyFields(originalObject, visited, cloneObject, typeToReflect.BaseType,
                    BindingFlags.Instance | BindingFlags.NonPublic, info => info.IsPrivate);
            }
        }

        private static void CopyFields(object originalObject, IDictionary<object, object> visited, object cloneObject,
            Type typeToReflect,
            BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public |
                                        BindingFlags.FlattenHierarchy, Func<FieldInfo, bool> filter = null)
        {
            foreach (var fieldInfo in typeToReflect.GetFields(bindingFlags))
            {
                if (filter != null && filter(fieldInfo) == false) continue;
                if (IsPrimitive(fieldInfo.FieldType)) continue;
                var originalFieldValue = fieldInfo.GetValue(originalObject);
                var clonedFieldValue = originalFieldValue == null ? null : InternalCopy(originalFieldValue, visited);
                fieldInfo.SetValue(cloneObject, clonedFieldValue);
            }
        }

        public static T Copy<T>(this T original)
        {
            return (T) Copy((object) original);
        }


        /// <summary>
        ///     Gets the name of the property.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        public static string GetPropertyName<T, TProperty>(Expression<Func<T, TProperty>> expression)
        {
            MemberExpression memberExpression;
            if (expression.Body.NodeType == ExpressionType.MemberAccess)
                memberExpression = expression.Body as MemberExpression;
            else if (expression.Body.NodeType == ExpressionType.Convert)
            {
                var unaryExpression = expression.Body as UnaryExpression;
                memberExpression = unaryExpression.Operand as MemberExpression;
            }
            else
                throw new ArgumentException();

            if (memberExpression != null)
                return memberExpression.Member.Name;
            return null;
        }


        /// <summary>
        ///     determine if there is a property with the given name
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static bool HasProperty(this object obj, string propertyName)
        {
            return obj.GetType().GetProperties().Any(q => q.Name == propertyName);
        }

        public static void SetValue(this object obj, string propertyName, object value)
        {
            var propertyInfo = obj.GetType().GetProperties().FirstOrDefault(p => p.Name == propertyName);
            if (propertyInfo == null)
                throw new ArgumentException("Property not found");
            if (!propertyInfo.CanWrite)
                throw new ArgumentException("Property not writeable");

            propertyInfo.SetValue(obj, value, null);
        }

        public static string GetPropertyName<T>(this T obj, Expression<Func<T, object>> expression)
        {
            return GetPropertyName(expression);
        }

        public static int? ToIntNullable(this string valueToParse)
        {
            int result;
            if (int.TryParse(valueToParse, out result))
                return result;
            return null;
        }

        public static decimal? ToDecimalNullable(this string valueToParse)
        {
            decimal result;
            if (decimal.TryParse(valueToParse, out result))
                return result;
            return null;
        }

        public static bool? ToBoolNullable(this string valueToParse)
        {
            bool result;
            if (bool.TryParse(valueToParse, out result))
                return result;
            return null;
        }

        public static string ToStringOrEmpty(this object obj)
        {
            return obj != null ? obj.ToString() : string.Empty;
        }

        public static string ToStringOrNull(this object obj)
        {
            return obj != null ? obj.ToString() : null;
        }

        public static bool IsNullOrEmpty(this string origString)
        {
            return string.IsNullOrEmpty(origString);
        }

        public static bool IsNotNullOrEmpty(this string origString)
        {
            return !string.IsNullOrEmpty(origString);
        }

        public static string ToMonthName(this int value)
        {
            return value >= 1 && value <= 12
                ? CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(value)
                : "";
        }

        public static DateTime? ToDateTimeNullable(this string valueToParse)
        {
            DateTime result;
            if (DateTime.TryParse(valueToParse, out result))
                return result;
            return null;
        }

        /// <summary>
        /// will not throw an error if the field is not found instead you receive a NULL
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static object GetValueSafe(this object obj, string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
                return null;
            var property = obj.GetType().GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public);
            if (property == null)
                property = obj.GetType().GetProperty(propertyName, BindingFlags.Instance | BindingFlags.NonPublic);
            if (property == null)
                return null;
            if (!property.GetIndexParameters().Any())
                return property.GetValue(obj, null);
            return null;
        }

        public static object GetValue(this object obj, string propertyName)
        {
            var property = obj.GetType().GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public);
            if (property == null)
                property = obj.GetType().GetProperty(propertyName, BindingFlags.Instance | BindingFlags.NonPublic);
            if (property == null)
                throw new ArgumentException("Property {0} on {1} not found".Apply(propertyName, obj.GetType().Name));
            if (!property.GetIndexParameters().Any())
                return property.GetValue(obj, null);
            return null;
        }

        public static bool IsNotNullAnd<TModel>(this TModel obj, Func<TModel, bool> andCondition) where TModel : class
        {
            return obj != null && andCondition(obj);
        }

        public static IList<T> Clone<T>(this IList<T> listToClone) where T : ICloneable
        {
            return listToClone.Select(item => (T) item.Clone()).ToList();
        }

        public static bool ImplementsInterface(this Type t, Type i)
        {
            return t != null && t.GetInterfaces().Any(q => q == i);
        }

        public static bool ImplementsInterface<TModel>(this TModel obj, Type t)
        {
            return obj.GetType().GetInterfaces().Any(q => q == t);
        }

        public static IEnumerable<IEnumerable<T>> InBatchesOf<T>(this IEnumerable<T> items, int batchSize)
        {
            List<T> batch = new List<T>(batchSize);
            foreach (var item in items)
            {
                batch.Add(item);

                if (batch.Count >= batchSize)
                {
                    yield return batch;
                    batch = new List<T>();
                }
            }

            if (batch.Count != 0)
            {
                //can't be batch size or would've yielded above
                batch.TrimExcess();
                yield return batch;
            }
        }
    }
}