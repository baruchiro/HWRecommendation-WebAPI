using System;

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
