using System;
using System.IO;
using Microsoft.ML;
using Microsoft.ML.AutoML;
using Newtonsoft.Json;

namespace AlgorithmManager.Interfaces
{
    public class LearningResult
    {
        public static LearningResult CreateFromRunDetail(RunDetail runDetail,
            double result,
            DataViewSchema schema = null, string name = null)
        {
            var learningResult = new LearningResult
            {
                Estimator = runDetail.Estimator,
                RuntimeInSeconds= runDetail.RuntimeInSeconds,
                TrainerName= runDetail.TrainerName,
                Result = result,
                Schema = schema,
                Name = name
            };
            return learningResult;
        }

        public static LearningResult CreateFromRunDetail<T>(RunDetail<T> runDetail,
            double result, DataViewSchema schema = null, string name = null)
        {
            var learningResult = CreateFromRunDetail(runDetail as RunDetail, result, schema, name);
            learningResult.ValidationMetrics = runDetail.ValidationMetrics;
            learningResult.Exception = runDetail.Exception;
            learningResult.Model = runDetail.Model;

            return learningResult;
        }
        public string Name { get; private set; }
        
        public double Result {  private set; get; }

        [JsonIgnore]
        public ITransformer Model {  private set; get; }

        [JsonConverter(typeof(ExceptionConverter))]
        public Exception Exception {  private set; get; }

        public object ValidationMetrics {  private set; get; }

        public string TrainerName {  private set; get; }

        public double RuntimeInSeconds {  private set; get; }

        [JsonIgnore]
        public IEstimator<ITransformer> Estimator {  private set; get; }

        public DataViewSchema Schema {  private set; get; }
    }

    public class ExceptionConverter : JsonConverter<Exception>
    {
        public override void WriteJson(JsonWriter writer, Exception value, JsonSerializer serializer)
        {
            writer.WriteValue(value.Message);
        }

        public override Exception ReadJson(JsonReader reader, Type objectType, Exception existingValue,
            bool hasExistingValue,
            JsonSerializer serializer)
        {
            var message = reader.Value as string;
            return new Exception(message);
        }
    }
}
