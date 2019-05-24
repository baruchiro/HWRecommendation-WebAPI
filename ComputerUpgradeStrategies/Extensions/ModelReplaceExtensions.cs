using System;
using System.Collections.Generic;
using System.Text;
using Models;

namespace ComputerUpgradeStrategies.Extensions
{
    public static class ModelReplaceExtensions
    {
        public static T Replace<T>(this T model, Action<T> replacement)
        {
            var result = model;

            replacement(result);

            return result;
        }
    }
}
