using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AlgorithmManager.Interfaces;
using AlgorithmManager.Model;
using Xunit;

namespace AlgorithmManagerTests
{
    public class AttributeValidation
    {
        public static IEnumerable<object[]> Models =new List<IMLModel[]>{
            new IMLModel[]{new MLGpuModel()},
            new IMLModel[]{new MLComputerModel()}, 
            new IMLModel[]{new MLPersonComputerModel()}
        };

        [Theory]
        [MemberData(nameof(Models))]
        public void ValidateAllAttributesInIMLModels(IMLModel model)
        {
            Assert.True(Validator.TryValidateObject(model,
                new ValidationContext(model),
                null, true)
            , $"IMLModel {model.GetType()} is not validate");
        }
    }
}
