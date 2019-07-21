using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace AlgorithmManager.ModelAttributes
{
    [AttributeUsage(AttributeTargets.Property)]
    sealed class SumAttribute : ArrayAttribute
    {
        protected override string MethodName { get; } = "Sum";
    }
}
