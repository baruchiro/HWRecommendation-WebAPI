using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AlgorithmManager.Interfaces;
using AlgorithmManager.ModelAttributes;

namespace AlgorithmManager.Extensions
{
    public static class TypeExtensions
    {
        public static PropertyInfo[] GetInstanceOrPublicProperties(this Type type)
        {
            return type?.GetProperties(BindingFlags.Instance | BindingFlags.Public);
        }

        public static bool IsComplexObject(this Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            return !type.IsPrimitive && !type.IsEnum && type != typeof(string);
        }

        public static IEnumerable<string> ResolveRecursiveNames(this Type type, string prefix = null)
        {
            var names = type.GetInstanceOrPublicProperties().SelectMany(ResolveRecursiveNames);
            return !string.IsNullOrEmpty(prefix) ? names.Select(p => prefix + p) : names;
        }

        public static IEnumerable<string> ResolveRecursiveNames(PropertyInfo prop)
        {
            if (prop == null) throw new ArgumentNullException(nameof(prop));


            var type = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;

            if (!type.IsComplexObject())
            {
                yield return prop.Name;
            }
            else
            {
                if (typeof(IEnumerable).IsAssignableFrom(type))
                    type = type.GetGenericArguments()[0];

                foreach (var name in type.ResolveRecursiveNames(prop.Name))
                {
                    yield return name;
                }
            }
        }


        public static IEnumerable<(string, object)> ResolveRecursiveNamesAndValues<T>(PropertyInfo prop, T obj,
            bool enumToInt = false)
        {
            if (prop == null) throw new ArgumentNullException(nameof(prop));
            if (obj == null) throw new ArgumentNullException(nameof(obj));


            var realType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;

            if (!realType.IsComplexObject())
            {
                var resultValue = prop.GetValue(obj);
                // ReSharper disable once UseNullPropagation
                if (realType.IsEnum && enumToInt && resultValue != null)
                {
                    resultValue = (int) resultValue;
                }

                yield return (prop.Name, resultValue);
            }
            else if (typeof(IEnumerable).IsAssignableFrom(realType))
            {
                realType = realType.GetGenericArguments()[0];
                var typeProps = realType.ResolveRecursiveNamesAndType(enumToInt).ToList();
                var propNamesAndCollectionValues = typeProps.ToDictionary(tp => tp.Key,
                    tp => CreateCollectionOfType(tp.Value, enumToInt));

                var allItems = prop.GetValue(obj) as IEnumerable ??
                               throw new ArgumentNullException(
                                   $"The {obj.GetType().Name}.{prop.Name} " +
                                   "can't converted to IEnumarable");

                foreach (var item in allItems)
                {
                    foreach (var nameValue in item.ResolveRecursiveNamesAndValue(enumToInt))
                    {
                        propNamesAndCollectionValues[nameValue.Key].GetType().GetMethod("Add")
                            ?.Invoke(propNamesAndCollectionValues[nameValue.Key], new[] {nameValue.Value});
                    }
                }

                foreach (var namesAndCollectionValue in propNamesAndCollectionValues)
                {
                    var arrayType = namesAndCollectionValue.Value.GetType();
                    var array = arrayType.GetMethod("ToArray")?
                                    .Invoke(namesAndCollectionValue.Value, null) ??
                                throw new MissingMethodException(arrayType.FullName, "ToArray");
                    yield return (prop.Name + namesAndCollectionValue.Key, array);
                }
            }
            else
            {
                var propertyValue = prop.GetValue(obj);
                if (propertyValue == null) yield break;

                foreach (var namesAndValue in propertyValue.ResolveRecursiveNamesAndValue(enumToInt)
                    .Select(p => (prop.Name + p.Key, p.Value)))
                {
                    yield return namesAndValue;
                }
            }
        }

        private static object CreateCollectionOfType(Type type, bool enumToInt = false)
        {
            if (enumToInt && type.IsEnum)
            {
                type = typeof(int);
            }

            return Activator.CreateInstance(typeof(List<>).MakeGenericType(type));
        }

        public static IEnumerable<(string, Type)> ResolveRecursiveNamesAndTypes(PropertyInfo prop,
            bool enumToInt = false)
        {
            if (prop == null) throw new ArgumentNullException(nameof(prop));

            var type = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;

            if (!type.IsComplexObject())
            {
                if (type.IsEnum && enumToInt)
                {
                    type = typeof(int);
                }

                yield return (prop.Name, type);
            }
            else if (typeof(IEnumerable).IsAssignableFrom(type))
            {
                var genericType = type.GetGenericArguments()[0];
                foreach (var namesAndValue in genericType.ResolveRecursiveNamesAndType(enumToInt)
                    .Select(p => (
                        prop.Name + p.Key,
                        p.Value.MakeArrayType()
                    )))
                {
                    yield return namesAndValue;
                }
            }
            else
            {
                foreach (var namesAndValue in type.ResolveRecursiveNamesAndType(enumToInt)
                    .Select(p => (prop.Name + p.Key, p.Value)))
                {
                    yield return namesAndValue;
                }
            }
        }

        public static IDictionary<string, object> ResolveRecursiveNamesAndValue(this object obj, bool enumToInt = false)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));

            return obj.GetType().GetInstanceOrPublicProperties()
                .SelectMany(p => ResolveRecursiveNamesAndValues(p, obj, enumToInt))
                .ToDictionary(t => t.Item1, t => t.Item2);
        }

        public static IDictionary<string, Type> ResolveRecursiveNamesAndType(this Type type, bool enumToInt = false)
        {
            return type.GetInstanceOrPublicProperties()
                .SelectMany(p => ResolveRecursiveNamesAndTypes(p, enumToInt))
                .ToDictionary(t => t.Item1, t => t.Item2);
        }

        public static TMLModel CreateFilledMLObject<TMLModel, TSource>(TSource source)
            where TMLModel : IMLModel, new()
        {
            var values = source.ResolveRecursiveNamesAndValue(true);
            var mlModel = new TMLModel();
            foreach (var keyValue in values)
            {
                var property = typeof(TMLModel).GetProperty(keyValue.Key) ??
                               throw new MissingMemberException(typeof(TMLModel).FullName, keyValue.Key);

                var arrayAttribute = property.GetCustomAttribute<ArrayAttribute>();
                if (arrayAttribute != null)
                {
                    var result = arrayAttribute.ApplyMethod(keyValue.Value);
                    property.SetValue(mlModel, result);
                }
                else
                {
                    var sourceType = keyValue.Value?.GetType();
                    if (sourceType != null && sourceType.IsNumeric(true) &&
                        (sourceType != typeof(float[]) || sourceType != typeof(float)))
                    {
                        if (sourceType.IsArray)
                        {
                            var array = (Array) keyValue.Value;

                            var newArray = new float[array.Length];
                            for (var i = 0; i < array.Length; i++)
                            {
                                newArray[i] = Convert.ToSingle(array.GetValue(i));
                            }

                            property.SetValue(mlModel, newArray);
                        }
                        else
                        {
                            property.SetValue(mlModel, Convert.ToSingle(keyValue.Value));
                        }
                    }
                    else
                    {
                        property.SetValue(mlModel, keyValue.Value);
                    }
                }
            }

            return mlModel;
        }

        public static List<string> GetFeatureColumns<T>() where T : IMLModel
        {
            return typeof(T).GetProperties().Where(p => p.GetCustomAttribute<FeatureAttribute>() != null)
                .Select(p => p.Name).ToList();
        }

        public static IEnumerable<string> GetPropertiesNamesByAttribute<TAttr>(this Type type)
            where TAttr : Attribute
        {
            return type.GetInstanceOrPublicProperties()
                .Where(p => p.GetCustomAttribute<TAttr>() != null)
                .Select(p => p.Name);
        }

        public static bool IsNumeric(this Type type, bool includeArray = false)
        {
            var validateType = type;
            if (includeArray)
            {
                validateType = type.IsArray ? type.GetElementType() : type;
            }
            
            switch (Type.GetTypeCode(validateType))
            {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Single:
                    return true;
                default:
                    return false;
            }
        }
    }
}
