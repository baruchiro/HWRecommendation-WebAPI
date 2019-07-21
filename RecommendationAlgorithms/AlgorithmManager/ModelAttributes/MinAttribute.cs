using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AlgorithmManager.ModelAttributes
{
    [AttributeUsage(AttributeTargets.Property)]
    sealed class MinAttribute : ArrayAttribute
    {
        protected override string MethodName { get; } = "Min";
    }
}
