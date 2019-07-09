using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.ML;
using Microsoft.ML.Data;

namespace DataTestsUtils
{
    public class DataLoader
    {
        private readonly string _fakeDataFilePath = Path.Combine("Resources", "fake-data-out.csv");
        private readonly string _fakeDataDtypesFilePath = Path.Combine("Resources", "fake-data-out.dtypes.csv");
        private readonly MLContext _mlContext;
        private TextLoader.Column[] _columns;
        public IDataView Data { get; }

        public DataLoader(MLContext mlContext)
        {
            _mlContext = mlContext;
            _columns = ReadColumnsFromDtypesFile();
            Data = _mlContext.Data.LoadFromTextFile(_fakeDataFilePath, _columns, ',', true);
        }

        private readonly IDictionary<string, DataKind> _strToDtypes = new Dictionary<string, DataKind>
        {
            {"int64", DataKind.Int64},
            {"object", DataKind.String},
            {"float64", DataKind.Double}
        };


        private TextLoader.Column[] ReadColumnsFromDtypesFile()
        {
            return File.ReadAllLines(_fakeDataDtypesFilePath)
                .Select(l => l.Split(','))
                .Select((line, index) =>
                    new TextLoader.Column(line[0],
                        _strToDtypes[line[1].ToLower()],
                        index))
                .ToArray();
        }
    }
}
