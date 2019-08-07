using AlgorithmManager;
using AlgorithmManager.Interfaces;
using FakeItEasy;
using Microsoft.Extensions.Configuration;
using Microsoft.ML;
using System;
using System.Collections.Generic;
using System.IO.Abstractions.TestingHelpers;
using System.Linq;
using Xunit;

namespace AlgorithmManagerTests
{
    public class ModelSaverTests
    {
        private MockFileSystem _fileSystem;
        private ModelSaver _modelSaver;
        private string _outputDir;

        public ModelSaverTests()
        {
            var mlContext = new MLContext(0);
            _fileSystem = new MockFileSystem();
            _outputDir = _fileSystem.Path.GetTempPath();
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddInMemoryCollection(new[]
            {
                new KeyValuePair<string, string>(ModelSaver.KEY_MODEL_SAVE_PATH, _fileSystem.Path.GetTempPath())
            });


            _modelSaver = new ModelSaver(mlContext, configurationBuilder.Build(), _fileSystem);
        }

        [Fact]
        public void SaveModel_FileSystem_AssertJsonZipDates()
        {
            var label = "Label123";
            var learningResult = A.Fake<LearningResult>();
            A.CallTo(() => learningResult.Name).Returns(label);

            var now = DateTime.Now;
            _modelSaver.SaveModel(learningResult);

            var extensions = new List<string> { ".zip", ".json" };
            var files = _fileSystem.Directory.GetFiles(_outputDir, $"{label}*")
                .Where(f => extensions.Contains(_fileSystem.Path.GetExtension(f)))
                .ToList();

            Assert.Equal(2, files.Count);
            Assert.Equal(now, _modelSaver.GetModelCreationTime(files[0]),
                TimeSpan.FromSeconds(1));
            Assert.Equal(now, _modelSaver.GetModelCreationTime(files[1]),
                TimeSpan.FromSeconds(1));
        }
    }
}
