using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AlgorithmManager.Extensions;
using AlgorithmManager.Interfaces;
using AlgorithmManager.Model;
using Microsoft.ML.Data;
using Models;

namespace AlgorithmManager.Factories
{
    // TODO: Move to root folder
    public class MLModelConverter
    {
        public TOut Convert<T, TOut>(T model) where TOut : IMLModel, new()
        {
            return TypeExtensions.CreateFilledMLObject<TOut, T>(model);
        }

        public IEnumerable<TOut> Convert<T, TOut>(IEnumerable<T> modelEnumerable) where TOut : IMLModel, new()
        {
            return modelEnumerable.Select(Convert<T, TOut>);
        }

        public MLPersonComputerModel Convert((Person person, Computer computer) tuple)
        {
            var structural = new PersonComputerStructureModel
            {
                Computer = tuple.computer,
                Person = tuple.person
            };
            return TypeExtensions.CreateFilledMLObject<MLPersonComputerModel, PersonComputerStructureModel>(
                structural);
        }

        public IEnumerable<MLPersonComputerModel> Convert(IEnumerable<(Person, Computer)> tupleEnumerable)
        {
            return tupleEnumerable.Select(Convert);
        }
    }
}
