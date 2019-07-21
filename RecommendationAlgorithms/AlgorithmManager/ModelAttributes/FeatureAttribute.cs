using System;
using System.Collections.Generic;
using System.Text;

namespace AlgorithmManager.ModelAttributes
{
    [AttributeUsage(AttributeTargets.Property)]
    sealed class FeatureAttribute : Attribute
    {
        public FeatureAttribute()
        {
            
        }
    }
}
