using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace AlgorithmManager.Extensions
{
    public static class TypeExtensions
    {
        public static PropertyInfo[] GetInstanceOrPublicProperties(this Type type)
        {
            return type?.GetProperties(BindingFlags.Instance | BindingFlags.Public);
        }

        public static bool IsLikePrimitive(this Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            return type.IsPrimitive || type.IsEnum || type == typeof(string);
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

            if (type.IsLikePrimitive())
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


        public static IEnumerable<(string, object)> ResolveRecursiveNamesAndValues<T>(PropertyInfo prop, T obj)
        {
            if (prop == null) throw new ArgumentNullException(nameof(prop));
            if (obj == null) throw new ArgumentNullException(nameof(obj));


            var type = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;

            if (type.IsLikePrimitive())
            {
                yield return (prop.Name, prop.GetValue(obj));
            }
            else if (typeof(IEnumerable).IsAssignableFrom(type))
            {
                type = type.GetGenericArguments()[0];
                var typeProps = type.ResolveRecursiveNamesAndType().ToList();
                var propNamesAndCollectionValues = typeProps.ToDictionary(tp => tp.Key,
                    tp => Activator.CreateInstance(typeof(List<>).MakeGenericType(tp.Value)));

                var allItems = prop.GetValue(obj) as IEnumerable ??
                               throw new ArgumentNullException(
                                   $"The {obj.GetType().Name}.{prop.Name} " +
                                   "can't converted to IEnumarable");
                foreach (var values in allItems)
                {
                    foreach (var nameValue in values.ResolveRecursiveNamesAndValue())
                    {
                        (propNamesAndCollectionValues[nameValue.Key] as ICollection<object>)?.Add(nameValue.Value);
                    }
                }

                foreach (var namesAndCollectionValue in propNamesAndCollectionValues)
                {
                    yield return (prop.Name + namesAndCollectionValue.Key, namesAndCollectionValue.Value);
                }
            }
            else
            {
                foreach (var namesAndValue in prop.GetValue(obj).ResolveRecursiveNamesAndValue()
                    .Select(p => (prop.Name + p.Key, p.Value)))
                {
                    yield return namesAndValue;
                }
            }
        }

        public static IEnumerable<(string, Type)> ResolveRecursiveNamesAndTypes(PropertyInfo prop)
        {
            if (prop == null) throw new ArgumentNullException(nameof(prop));


            var type = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;

            if (type.IsLikePrimitive())
            {
                yield return (prop.Name, prop.PropertyType);
            }else if (typeof(IEnumerable).IsAssignableFrom(type))
            {
                var genericType = type.GetGenericArguments()[0];
                foreach (var namesAndValue in genericType.ResolveRecursiveNamesAndType()
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
                foreach (var namesAndValue in type.ResolveRecursiveNamesAndType().Select(p => (prop.Name + p.Key, p.Value)))
                {
                    yield return namesAndValue;
                }
            }
        }

        public static IDictionary<string, object> ResolveRecursiveNamesAndValue(this object obj)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));

            var x = obj.GetType().GetInstanceOrPublicProperties()
                .SelectMany(p => ResolveRecursiveNamesAndValues(p, obj)).ToList();
            return x
                .ToDictionary(t => t.Item1, t => t.Item2);
        }

        public static IDictionary<string, Type> ResolveRecursiveNamesAndType(this Type type)
        {
            return type.GetInstanceOrPublicProperties()
                .SelectMany(ResolveRecursiveNamesAndTypes)
                .ToDictionary(t => t.Item1, t => t.Item2);
        }

        public static TFlatten CreateFilledFlattenObject<TFlatten, TSource>(TSource source)
            where TFlatten : new()
        {
            var values = source.ResolveRecursiveNamesAndValue();
            var flatten = new TFlatten();
            foreach (var keyValue in values)
            {
                typeof(TFlatten).GetProperty(keyValue.Key)?.SetValue(flatten, keyValue.Value);
            }

            return flatten;
        }
    }
}
