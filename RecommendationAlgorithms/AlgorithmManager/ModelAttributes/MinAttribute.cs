﻿using System;
using System.Linq;

namespace AlgorithmManager.ModelAttributes
{
    [AttributeUsage(AttributeTargets.Property)]
    sealed class MinAttribute : ArrayAttribute
    {
        protected override object Invoke(object array)
        {
            var method = typeof(Enumerable).GetMethod("Min",
                    new[] { array.GetType() }) ??
                throw new MissingMethodException(typeof(Enumerable).FullName, "Min");
            return method.Invoke(array, new[] { array });
        }
    }
}
