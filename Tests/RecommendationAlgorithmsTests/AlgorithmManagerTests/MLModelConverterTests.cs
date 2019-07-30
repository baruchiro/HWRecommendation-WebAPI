using System;
using System.Linq;
using System.Reflection;
using AlgorithmManager.Extensions;
using AlgorithmManager.Model;
using AlgorithmManager.ModelAttributes;
using AlgoTestUtils;
using Models;
using Xunit;
using TypeExtensions = AlgorithmManager.Extensions.TypeExtensions;

namespace AlgorithmManagerTests
{
    public class MLModelConverterTests
    {
        [Fact]
        public void MLModels_PersonComputer_ValidateNamesAndTypes()
        {
            AssertComparePropertyNamesTypesAgainstMLType<PersonComputerStructureModel, MLPersonComputerModel>();
        }

        [Fact]
        public void MLModels_Computer_ValidateNamesAndTypes()
        {
            AssertComparePropertyNamesTypesAgainstMLType<Computer, MLComputerModel>();
        }

        [Fact]
        public void MLModels_Gpu_ValidateNamesAndTypes()
        {
           AssertComparePropertyNamesTypesAgainstMLType<Gpu, MLGpuModel>();
        }

        [Fact]
        public void MLModelConverter_Gpu_ValidateMemberOfMember()
        {
            var gpu = TestUtils.TestUtils.GenerateGpu();
            gpu.AssertNotDefault();

            var gpuMLModel = TypeExtensions.CreateFilledMLObject<MLGpuModel, Gpu>(gpu);
            gpuMLModel.AssertEqual(gpu);
        }

        [Fact]
        public void MLModelConverter_PersonComputer_ValidateCollections()
        {
            var personComputer = new PersonComputerStructureModel
            {
                Computer = TestUtils.TestUtils.GenerateComputer(),
                Person = TestUtils.TestUtils.GeneratePerson()
            };
            personComputer.AssertNotDefault();


            var personComputerMLModel =
                TypeExtensions.CreateFilledMLObject<MLPersonComputerModel, PersonComputerStructureModel>(
                    personComputer);

            personComputerMLModel.AssertEqual(personComputer);
        }

        [Fact]
        public void MLModelConverter_PersonComputer_ValidateEmptyComponents()
        {
            var personComputer = new PersonComputerStructureModel
            {
                Computer = TestUtils.TestUtils.GenerateEmptyComponentsComputer(),
                Person = TestUtils.TestUtils.GeneratePerson()
            };


            var personComputerMLModel =
                TypeExtensions.CreateFilledMLObject<MLPersonComputerModel, PersonComputerStructureModel>(
                    personComputer);

            personComputerMLModel.AssertEqual(personComputer);
        }

        [Fact]
        public void MLModelConverter_PersonComputer_ValidateEmpty()
        {
            var personComputer = new PersonComputerStructureModel
            {
                Computer = TestUtils.TestUtils.GenerateEmptyComputer(),
                Person = TestUtils.TestUtils.GeneratePerson()
            };
            
            var personComputerMLModel =
                TypeExtensions.CreateFilledMLObject<MLPersonComputerModel, PersonComputerStructureModel>(
                    personComputer);

            personComputerMLModel.AssertEqual(personComputer);
        }

        private void AssertComparePropertyNamesTypesAgainstMLType<TType, TMLModel>()
        {
            var expectedPropsNames = typeof(TType).ResolveRecursiveNamesAndType(true)
                .ToDictionary(
                    pair => pair.Key,
                    pair => ConvertNumericToSingle(pair.Value));
            var actualPropsNames = typeof(TMLModel).GetInstanceOrPublicProperties()
                .ToDictionary(p =>
                    p.Name,
                    p => p.GetCustomAttribute<ArrayAttribute>() != null ?
                        p.PropertyType.MakeArrayType() :
                        p.PropertyType);

            var expectedButNotInActual = expectedPropsNames.Except(actualPropsNames).ToList();
            var actualButNotInExpected = actualPropsNames.Except(expectedPropsNames);

            var toWrite = string.Join('\n', expectedButNotInActual.Select(s => $"public {s.Value} {s.Key} {{get; set;}}"));
            Assert.Empty(expectedButNotInActual);
            Assert.Empty(actualButNotInExpected);
        }

        private Type ConvertNumericToSingle(Type type)
        {
            if (type.IsNumeric(true))
            {
                return type.IsArray ? typeof(float).MakeArrayType() : typeof(float);
            }

            return type;
        }
    }
}
