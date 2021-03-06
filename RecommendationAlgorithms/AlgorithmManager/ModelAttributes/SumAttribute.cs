﻿using System;
using System.Linq;

namespace AlgorithmManager.ModelAttributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class SumAttribute : ArrayAttribute
    {
        protected override object Invoke(object array)
        {
            var method = typeof(Enumerable).GetMethod("Sum",
                             new[] { array.GetType() }) ??
                         throw new MissingMethodException(typeof(Enumerable).FullName, "Sum");
            return method.Invoke(array, new[] { array });
        }
    }
}
