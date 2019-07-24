using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;

namespace AlgorithmManager.ModelAttributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public abstract class ArrayAttribute : ValidationAttribute
    {
        protected abstract object Invoke(object array);
        public object ApplyMethod(object array)
        {
            if (array == null) throw new ArgumentNullException(nameof(array));
            if ((array as Array)?.Length == 0)
                return null;

            return Invoke(array);
            //var sumMethod = GetMethodInfo(array); 
            //return sumMethod.Invoke(array, new[] {array});
        }

        public override bool IsValid(object value)
        {
            return !value?.GetType().IsArray ?? false;
            //if (validationContext != null && validationContext.ObjectType.IsArray)
            //{
            //    return new ValidationResult(
            //        $"The property {validationContext.DisplayName} " +
            //        $"that marked as {nameof(SumAttribute)} " +
            //        $"can't be an {validationContext.ObjectType} (Array)");
            //}
            //return ValidationResult.Success;
        }
    }
}
