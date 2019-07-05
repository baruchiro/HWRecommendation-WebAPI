using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.ML;
using Microsoft.ML.Data;

namespace MultiRegressionTF
{
    public class MultiRegression
    {
        private readonly IDictionary<string, DataKind> _strToDtypes = new Dictionary<string, DataKind>
        {
            {"int64", DataKind.Int64},
            {"object", DataKind.String},
            {"float64", DataKind.Double}
        };

        private TextLoader.Column[] ReadColumnsFromList(string[] columns)
        {
            return columns.Select(l => l.Split(','))
                .Select((line, index) => new TextLoader.Column(line[0], _strToDtypes[line[1].ToLower()], index))
                .ToArray();
        }

        public void CreateModel(IFileHandle trainDataCsv, string[] trainDtypesCsv)
        {
            //Create ML Context with seed for repeteable/deterministic results
            MLContext mlContext = new MLContext(seed: 0);

            // STEP 1: Common data loading configuration
            TextLoader textLoader = mlContext.Data.CreateTextLoader(ReadColumnsFromList(trainDtypesCsv),
                hasHeader: true,
                separatorChar: ','
            );

            IMultiStreamSource multiStream = new FileHandleSource(trainDataCsv);
            IDataView fakedDataView = textLoader.Load(multiStream);
        }
    }
}
