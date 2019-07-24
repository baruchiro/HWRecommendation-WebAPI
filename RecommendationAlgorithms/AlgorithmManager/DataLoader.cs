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

        public IEnumerable<(Person, Computer)> EnumerateData() => File.ReadLines(_dataFilePath)
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

        private (Person, Computer) CreateTupleFromStringArray(string[] arg)
        {
            try
            {
                var person = new Person
                {
                    Age = StringToInt(arg[0]),
                    WorkArea = arg[6],
                    Gender = ParseEnum<Gender>(arg[7]),
                    MainUse = arg[14],
                    Price = StringToInt(arg[21]),
                };
                var disk = new Disk
                {
                    Capacity = StringToLong(arg[2]),
                    Model = arg[3],
                    Rpm = StringToInt(arg[4]),
                    Type = ParseEnum<DiskType>(arg[5])
                };
                var gpuMemory = new Memory
                {
                    Capacity = StringToLong(arg[10]),
                    Generation = StringToInt(arg[11]),
                    Type = ParseEnum<RamType>(arg[12])
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
                    Type = ParseEnum<RamType>(arg[18])
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
                    ComputerType = ParseEnum<ComputerType>(arg[1]),
                    Disks = new List<Disk> {disk},
                    Gpus = new List<Gpu> {gpu},
                    Memories = new List<Memory> {memory},
                    MotherBoard = motherBoard,
                    Processor = processor
                };
                return (person, computer);

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine(arg);
                throw;
            }
        }

        private T ParseEnum<T>(string field) where T : struct, Enum
        {
            try
            {
                Enums.TryParse(field, true, out T t);
                return t;
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private static int StringToInt(string number)
        {
            return Convert.ToInt32(StringToDouble(number));
        }

        private static long StringToLong(string number)
        {
            return Convert.ToInt64(StringToDouble(number));
        }

        private static double StringToDouble(string number)
        {
            try
            {
                return string.IsNullOrEmpty(number) ? 0 : Convert.ToDouble(number);
            }
            catch (FormatException e)
            {
                Console.WriteLine(e);
                Console.WriteLine($"param: {number}");
                throw;
            }
        }
    }
}
