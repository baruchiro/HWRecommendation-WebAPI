using System;
using System.Linq;

namespace AlgorithmManager.ModelAttributes
{
    [AttributeUsage(AttributeTargets.Property)]
    sealed class AllMustBeSameAttribute : ArrayAttribute
    {
        protected override object Invoke(object array)
        {
            var distinctMethod = typeof(Enumerable).GetMethods()
                             .FirstOrDefault(c => c.Name == "Distinct" && c.GetParameters().Length == 1)?
                             .MakeGenericMethod(array.GetType().GetElementType()) ??
                         throw new MissingMethodException(typeof(Enumerable).FullName, "Distinct");

            var countMethod = typeof(Enumerable).GetMethods()
                                  .FirstOrDefault(c => c.Name == "Count" && c.GetParameters().Length == 1)?
                                  .MakeGenericMethod(array.GetType().GetElementType()) ??
                              throw new MissingMethodException(typeof(Enumerable).FullName, "Count");

            var firstMethod = typeof(Enumerable).GetMethods()
                                  .FirstOrDefault(c => c.Name == "First" && c.GetParameters().Length == 1)?
                                  .MakeGenericMethod(array.GetType().GetElementType()) ??
                              throw new MissingMethodException(typeof(Enumerable).FullName, "First");

            var distinctValues = distinctMethod.Invoke(array, new[] { array });

            if (Convert.ToInt32(countMethod.Invoke(distinctValues, new[] { distinctValues })) != 1)
                throw new ArgumentOutOfRangeException($"Array of {array.GetType()} must be with one distinct value");

            return firstMethod.Invoke(distinctValues, new[] { distinctValues });
        }
    }
}
