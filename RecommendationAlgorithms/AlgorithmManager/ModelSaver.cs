using System;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Threading;
using AlgorithmManager.Interfaces;
using Microsoft.ML;
using Newtonsoft.Json;

namespace AlgorithmManager
{
    public class ModelSaver
    {
        private readonly MLContext _mlContext;
        private readonly string _outputDir;
        private readonly string _dateFormat = "yyMMdd-HHmmss";
        private readonly IFileSystem _fileSystem;

        public ModelSaver(MLContext mlContext, string outputDir, IFileSystem fileSystem = null)
        {
            _fileSystem = fileSystem ?? new FileSystem();
            _outputDir = _fileSystem.Directory.CreateDirectory(outputDir).FullName;

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

        public LearningResult LoadModel(string modelName)
        {
            var newestJson = GetNewestFile(modelName, "json");
            var newestZip = GetNewestFile(modelName, "zip");

            var json = _fileSystem.File.ReadAllText(newestJson);
            var learningResult = JsonConvert.DeserializeObject<LearningResult>(json);

            using (var stream = _fileSystem.FileStream.Create(newestZip, FileMode.Open))
            {
                learningResult.Model = _mlContext.Model.Load(stream, out var inputSchema);

                if (inputSchema != learningResult.Schema)
                    throw new JsonSerializationException(
                        "Input schema loaded by MLContext is different from the Input schema loaded by Json");
            }

            return learningResult;
        }

        private string GetNewestFile(string fileStartWith, string extension)
        {
            return _fileSystem.Directory.EnumerateFiles(_outputDir, $"{fileStartWith}_*.{extension}")
                .OrderBy(GetModelCreationTime).First();
        }

        public DateTime GetModelCreationTime(string filename)
        {
            var filenameWithoutExtension = _fileSystem.Path.GetFileNameWithoutExtension(filename);
            return DateTime.ParseExact(filenameWithoutExtension?.Split('_').Last(),
                _dateFormat, null); 
        }
    }
}