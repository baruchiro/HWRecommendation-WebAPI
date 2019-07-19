using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using AlgorithmManager.Interfaces;
using EnumsNET;

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


            var nullableUnderlyingType = Nullable.GetUnderlyingType(prop.PropertyType);
            var type = nullableUnderlyingType ?? prop.PropertyType;

            if (!type.IsComplexObject())
            {
                var resultValue = prop.GetValue(obj);
                if (type.IsEnum && enumToInt)
                {
                    resultValue = (int) resultValue;
                }

                yield return (prop.Name, resultValue);
            }
            else if (typeof(IEnumerable).IsAssignableFrom(type))
            {
                type = type.GetGenericArguments()[0];
                var typeProps = type.ResolveRecursiveNamesAndType(enumToInt).ToList();
                var propNamesAndCollectionValues = typeProps.ToDictionary(tp => tp.Key,
                    tp =>CreateCollectionOfType(tp.Value, enumToInt));

                var allItems = prop.GetValue(obj) as IEnumerable ??
                               throw new ArgumentNullException(
                                   $"The {obj.GetType().Name}.{prop.Name} " +
                                   "can't converted to IEnumarable");
                foreach (var values in allItems)
                {
                    foreach (var nameValue in values.ResolveRecursiveNamesAndValue(enumToInt))
                    {
                        propNamesAndCollectionValues[nameValue.Key].GetType().GetMethod("Add")
                                ?.Invoke(propNamesAndCollectionValues[nameValue.Key], new[] {nameValue.Value});
                    }
                }

                foreach (var namesAndCollectionValue in propNamesAndCollectionValues)
                {
                    yield return (prop.Name + namesAndCollectionValue.Key, namesAndCollectionValue.Value);
                }
            }
            else
            {
                foreach (var namesAndValue in prop.GetValue(obj).ResolveRecursiveNamesAndValue(enumToInt)
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

            var nullableUnderlyingType = Nullable.GetUnderlyingType(prop.PropertyType);
            var type = nullableUnderlyingType ?? prop.PropertyType;

            if (!type.IsComplexObject())
            {
                var resultType = prop.PropertyType;
                if (type.IsEnum && enumToInt)
                {
                    resultType = nullableUnderlyingType != null
                        ? typeof(Nullable<>).MakeGenericType(typeof(int))
                        : typeof(int);
                }

                yield return (prop.Name, resultType);
            }
            else if (typeof(IEnumerable).IsAssignableFrom(type))
            {
                var genericType = type.GetGenericArguments()[0];
                foreach (var namesAndValue in genericType.ResolveRecursiveNamesAndType(enumToInt)
                    .Select(p => (
                        prop.Name + p.Key,
                        typeof(ICollection<>).MakeGenericType(p.Value)
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
                .SelectMany(p=> ResolveRecursiveNamesAndTypes(p, enumToInt))
                .ToDictionary(t => t.Item1, t => t.Item2);
        }

        public static TMLModel CreateFilledMLObject<TMLModel, TSource>(TSource source)
            where TMLModel : IMLModel, new()
        {
            var values = source.ResolveRecursiveNamesAndValue(true);
            var mlModel = new TMLModel();
            foreach (var keyValue in values)
            {
                typeof(TMLModel).GetProperty(keyValue.Key)?.SetValue(mlModel, keyValue.Value);
            }

            return mlModel;
        }
    }
}
