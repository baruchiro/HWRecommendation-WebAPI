using AlgorithmManager.Model;
using Microsoft.ML;
using Models;
using System;
using System.Collections.Generic;

namespace AlgorithmManager.Factories
{
    public class AlgorithmManagerFactory
    {
        private readonly MLContext _mlContext;
        private readonly MLModelConverter _mlModelConverter;

        public AlgorithmManagerFactory(MLContext mlContext)
        {
            _mlContext = mlContext ?? throw new ArgumentNullException(nameof(mlContext));
            _mlModelConverter = new MLModelConverter();
        }

        public IDataView CreateDataView(IEnumerable<(Person, Computer)> personComputerPairs)
        {
            return CreateDataView(_mlModelConverter.Convert(personComputerPairs));
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

        public MLPersonComputerModel PersonToMLModelPersonComputer(Person person)
        {
            return _mlModelConverter.Convert((person, new Computer()));
        }
    }
}
