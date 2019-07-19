using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AlgorithmManager.Extensions;
using AlgorithmManager.Model;
using Microsoft.ML.Data;
using Models;

namespace AlgorithmManager.Factories
{
    // TODO: Move to root folder
    class ModelFlatenner
    {
        public TOut Flatten<T, TOut>(T model) where TOut : new()
        {
            return TypeExtensions.CreateFilledFlattenObject<TOut, T>(model);
        }

        public IEnumerable<TOut> Flatten<T, TOut>(IEnumerable<T> modelEnumerable) where TOut : new()
        {
            return modelEnumerable.Select(Flatten<T, TOut>);
        }

        public FlattenPersonComputer Flatten((Person person, Computer computer) tuple)
        {
            var structural = new PersonComputerStructureModel
            {
                Computer = tuple.computer,
                Person = tuple.person
            };
            return TypeExtensions.CreateFilledFlattenObject<FlattenPersonComputer, PersonComputerStructureModel>(
                structural);
        }

        public IEnumerable<FlattenPersonComputer> Flatten(IEnumerable<(Person, Computer)> tupleEnumerable)
        {
            return tupleEnumerable.Select(Flatten);
        }
    }
}
