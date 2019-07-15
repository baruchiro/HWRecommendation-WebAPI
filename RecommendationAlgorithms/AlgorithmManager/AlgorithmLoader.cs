using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using AlgorithmManager.Interfaces;

namespace AlgorithmManager
{
    public class AlgorithmLoader
    {
        public IEnumerable<IRecommendationAlgorithmLearner> LoadAllRegressionAlgorithms()
        {
            return Directory.EnumerateFiles(".", "*.dll")
                .Select(Path.GetFullPath)
                .Select(Assembly.LoadFile)
                .SelectMany(GetTypeFromAssembly<IRecommendationAlgorithmLearner>);
        }

        private IEnumerable<T> GetTypeFromAssembly<T>(Assembly assembly)
        {
            var relevantTypes = assembly.GetTypes()
                .Where(t => typeof(T).IsAssignableFrom(t));

            foreach (var type in relevantTypes)
            {
                if (Activator.CreateInstance(type) is T t)
                    yield return t;
            }
        }
    }
}
