using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using EnumsNET;
using Microsoft.ML;
using Microsoft.ML.Data;
using Models;

namespace AlgorithmManager
{
    public class DataLoader
    {
        private readonly string _dataFilePath;
        private readonly string _dataDtypesFilePath;
        private readonly MLContext _mlContext;
        private readonly TextLoader.Column[] _columns;

        public IEnumerable<Tuple<Person, Computer>> EnumerateData() => File.ReadLines(_dataFilePath)
            .Skip(1)
            .Select(l => l.Split(','))
            .Select(CreateTupleFromStringArray);

        public DataLoader(MLContext mlContext, string dataCsvPath, string dtypesCsvPath)
        {
            _mlContext = mlContext;
            _dataFilePath = dataCsvPath;
            _dataDtypesFilePath = dtypesCsvPath;
            _columns = ReadColumnsFromDtypesFile();
        }

        private readonly IDictionary<string, DataKind> _strToDtypes = new Dictionary<string, DataKind>
        {
            {"int64", DataKind.Int64},
            {"object", DataKind.String},
            {"float64", DataKind.Double}
        };

        private TextLoader.Column[] ReadColumnsFromDtypesFile()
        {
            return File.ReadAllLines(_dataDtypesFilePath)
                .Select(l => l.Split(','))
                .Select((line, index) =>
                    new TextLoader.Column(line[0],
                        _strToDtypes[line[1].ToLower()],
                        index))
                .ToArray();
        }

        private Tuple<Person, Computer> CreateTupleFromStringArray(string[] arg)
        {
            try
            {
                var person = new Person
                {
                    Age = StringToInt(arg[0]),
                    WorkArea = arg[6],
                    Gender = Enums.Parse<Gender>(arg[7], true),
                    MainUse = arg[14],
                    Price = StringToInt(arg[21]),
                };
                var disk = new Disk
                {
                    Capacity = StringToLong(arg[2]),
                    Model = arg[3],
                    Rpm = StringToInt(arg[4]),
                    Type = Enums.Parse<DiskType>(arg[5], true)
                };
                var gpuMemory = new Memory
                {
                    Capacity = StringToLong(arg[10]),
                    Generation = StringToInt(arg[11]),
                    Type = Enums.Parse<RamType>(arg[12], true)
                };
                var gpu = new Gpu
                {
                    Manufacturer = arg[8],
                    Name = arg[9],
                    Memory = gpuMemory,
                    Version = StringToInt(arg[13])
                };
                var memory = new Memory
                {
                    Capacity = StringToLong(arg[15]),
                    Ghz = StringToLong(arg[16]),
                    Generation = StringToInt(arg[17]),
                    Type = Enums.Parse<RamType>(arg[18], true)
                };
                var motherBoard = new MotherBoard
                {
                    DdrSockets = StringToInt(arg[19]),
                    MaxRam = StringToLong(arg[20])
                };
                var processor = new Processor
                {
                    Manufacturer = arg[22],
                    GHz = StringToLong(arg[23]),
                    Name = arg[24],
                    NumOfCores = StringToInt(arg[25])
                };
                var computer = new Computer
                {
                    ComputerType = Enums.Parse<ComputerType>(arg[1], true),
                    Disks = new List<Disk> {disk},
                    Gpus = new List<Gpu> {gpu},
                    Memories = new List<Memory> {memory},
                    MotherBoard = motherBoard,
                    Processor = processor
                };
                return new Tuple<Person, Computer>(person, computer);

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine(arg);
                throw;
            }
        }

        private static int StringToInt(string number)
        {
            return Convert.ToInt32(Convert.ToDouble(number));
        }

        private static long StringToLong(string number)
        {
            return Convert.ToInt64(Convert.ToDouble(number));
        }
    }
}
