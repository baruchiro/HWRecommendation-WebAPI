using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace AlgorithmManager.ModelAttributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public abstract class ArrayAttribute : ValidationAttribute
    {
        protected abstract string MethodName { get; }

        public object ApplyMethod(object array)
        {
            if (array == null) throw new ArgumentNullException(nameof(array));
            if ((array as Array)?.Length == 0)
                return null;

            var sumMethod = typeof(Enumerable).GetMethod(MethodName,
                                new[] {array.GetType()}) ??
                            throw new MissingMethodException(typeof(Enumerable).FullName, MethodName);
            return sumMethod.Invoke(array, new[] {array});
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (validationContext.ObjectType.IsArray)
            {
                return new ValidationResult(
                    $"The property {validationContext.DisplayName} " +
                    $"that marked as {nameof(SumAttribute)} " +
                    $"can't be an {validationContext.ObjectType} (Array)");
            }
            return base.IsValid(value, validationContext);
        }
    }
}
