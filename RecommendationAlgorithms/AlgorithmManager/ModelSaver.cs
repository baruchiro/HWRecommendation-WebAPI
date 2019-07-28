using System;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Reflection;
using System.Threading;
using AlgorithmManager.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.ML;
using Newtonsoft.Json;

namespace AlgorithmManager
{
    public class ModelSaver
    {
        public const string KEY_MODEL_SAVE_PATH = "MODEL_SAVE_PATH";
        private const string MODEL_SAVE_PATH = "Models";
        private readonly MLContext _mlContext;
        private readonly string _outputDir;
        private readonly string _dateFormat = "yyMMdd-HHmmss";
        private readonly IFileSystem _fileSystem;

        public ModelSaver(MLContext mlContext, IConfiguration configuration, IFileSystem fileSystem = null)
        {
            _fileSystem = fileSystem ?? new FileSystem();
            var modelSavePath = configuration?["MODEL_SAVE_PATH"] ?? MODEL_SAVE_PATH;
            var fullPath = _fileSystem.Path.Combine(
                _fileSystem.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                modelSavePath);
            _outputDir = _fileSystem.Directory.CreateDirectory(fullPath).FullName;

            _mlContext = mlContext;
        }

        public void SaveModel(LearningResult learningResult)
        {
            if (learningResult == null) throw new ArgumentNullException(nameof(learningResult));

            var filename = _fileSystem.Path.Combine(_outputDir,
                $"{learningResult.Name}_{DateTime.Now.ToString(_dateFormat)}");

            var jsonFile = $"{filename}.json";
            var modelPath = $"{filename}.zip";

            var json = JsonConvert.SerializeObject(learningResult, Formatting.Indented);
            _fileSystem.File.WriteAllText(jsonFile, json);

            _mlContext.Model.Save(learningResult.Model, learningResult.Schema, modelPath);

            using (var stream = _fileSystem.FileStream.Create(modelPath, FileMode.Create))
            {
                _mlContext.Model.Save(learningResult.Model, learningResult.Schema, stream);
            }
        }

        public LearningResult LoadLearningResult(string modelName)
        {
            var newestJson = GetNewestFile(modelName, "json") ?? throw new FileNotFoundException(
                                 $"The saved model: {modelName} is not exist",
                                 _fileSystem.Path.Combine(_outputDir, modelName + "*.json"));
            var newestZip = GetNewestFile(modelName, "zip") ?? throw new FileNotFoundException(
                                $"The saved model: {modelName} is not exist",
                                _fileSystem.Path.Combine(_outputDir, modelName + "*.zip"));;

            var json = _fileSystem.File.ReadAllText(newestJson);
            var learningResult = JsonConvert.DeserializeObject<LearningResult>(json);

            using (var stream = _fileSystem.FileStream.Create(newestZip, FileMode.Open))
            {
                learningResult.Model = _mlContext.Model.Load(stream, out _);
            }

            return learningResult;
        }

        private string GetNewestFile(string fileStartWith, string extension)
        {
            return _fileSystem.Directory.EnumerateFiles(_outputDir, $"{fileStartWith}_*.{extension}")
                .OrderBy(GetModelCreationTime).FirstOrDefault();
        }

        public DateTime GetModelCreationTime(string filename)
        {
            var filenameWithoutExtension = _fileSystem.Path.GetFileNameWithoutExtension(filename);
            return DateTime.ParseExact(filenameWithoutExtension?.Split('_').Last(),
                _dateFormat, null); 
        }
    }
}