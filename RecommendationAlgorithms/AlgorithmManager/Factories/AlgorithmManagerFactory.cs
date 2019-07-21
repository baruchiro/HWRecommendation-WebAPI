using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AlgorithmManager.Model;
using Microsoft.ML;
using Models;

namespace AlgorithmManager.Factories
{
    public class AlgorithmManagerFactory
    {
        private readonly MLContext _mlContext;
        private MLModelConverter MLModelConverter { get; set; }

        public AlgorithmManagerFactory(MLContext mlContext)
        {
            _mlContext = mlContext ?? throw new ArgumentNullException(nameof(mlContext));
            MLModelConverter = new MLModelConverter();
        }

        public IDataView CreateDataView(IEnumerable<(Person, Computer)> personComputerPairs)
        {
            return CreateDataView(MLModelConverter.Convert(personComputerPairs));
        }

        public IDataView CreateDataView(IEnumerable<MLPersonComputerModel> mlPersonComputerModels)
        {
            return _mlContext.Data.LoadFromEnumerable(mlPersonComputerModels);
        }

        public IDataView CreateDataViewFromCsv(string csvFilePath, string csvDtypesFilePath)
        {
            var dataLoader = new DataLoader(_mlContext, csvFilePath, csvDtypesFilePath);
            return CreateDataView(dataLoader.EnumerateData());
        }
    }
}
