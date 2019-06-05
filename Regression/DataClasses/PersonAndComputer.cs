using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.ML.Data;

namespace Regression.DataClasses
{
    public class PersonAndComputer
    {
        [LoadColumn(0)]
        public string GpuProcessor { get; set; }

        [LoadColumn(1)]
        public string GpuName { get; set; }

        [LoadColumn(2)]
        public float MotherBoardSataConnections { get; set; }

        [LoadColumn(3)]
        public float MotherBoardMaxRam { get; set; }

        [LoadColumn(4)]
        public float MotherBoardDdrSockets { get; set; }

        [LoadColumn(5)]
        public string MotherBoardName { get; set; }

        [LoadColumn(6)]
        public string DiskCapacity { get; set; }

        [LoadColumn(7)]
        public float DiskRpm { get; set; }

        [LoadColumn(8)]
        public string DiskType { get; set; }

        [LoadColumn(9)]
        public string DiskModel { get; set; }

        [LoadColumn(10)]
        public float MemoryMHz { get; set; }

        [LoadColumn(11)]
        public string MemoryType { get; set; }

        [LoadColumn(12)]
        public string MemoryCapacity { get; set; }

        [LoadColumn(13)]
        public string ProcessorArchitecture { get; set; }

        [LoadColumn(14)]
        public float ProcessorNumOfCores { get; set; }

        [LoadColumn(15)]
        public float ProcessorGHz { get; set; }

        [LoadColumn(16)]
        public string ProcessorName { get; set; }

        [LoadColumn(17)]
        public float Age { get; set; }

        [LoadColumn(18)]
        public string FieldInterest { get; set; }

        [LoadColumn(19)]
        public string MainUse { get; set; }

        [LoadColumn(20)]
        public string Gender { get; set; }

        [LoadColumn(21)]
        public string ComputerType { get; set; }

        [LoadColumn(22)]
        public float Price { get; set; }
    }

    public class ComputerPrediction
    {
        [ColumnName("GpuProcessor")]
        public string GpuProcessor { get; set; }

        [ColumnName("GpuName")]
        public string GpuName { get; set; }

        [ColumnName("MotherBoardSataConnections")]
        public float MotherBoardSataConnections { get; set; }

        [ColumnName("MotherBoardMaxRam")]
        public float MotherBoardMaxRam { get; set; }

        [ColumnName("MotherBoardDdrSockets")]
        public float MotherBoardDdrSockets { get; set; }

        [ColumnName("MotherBoardName")]
        public string MotherBoardName { get; set; }

        [ColumnName("DiskCapacity")]
        public string DiskCapacity { get; set; }

        [ColumnName("DiskRpm")]
        public float DiskRpm { get; set; }

        [ColumnName("DiskType")]
        public string DiskType { get; set; }

        [ColumnName("DiskModel")]
        public string DiskModel { get; set; }

        [ColumnName("MemoryMHz")]
        public float MemoryMHz { get; set; }

        [ColumnName("MemoryType")]
        public string MemoryType { get; set; }

        [ColumnName("MemoryCapacity")]
        public string MemoryCapacity { get; set; }

        [ColumnName("ProcessorArchitecture")]
        public string ProcessorArchitecture { get; set; }

        [ColumnName("ProcessorNumOfCores")]
        public float ProcessorNumOfCores { get; set; }

        [ColumnName("ProcessorGHz")]
        public float ProcessorGHz { get; set; }

        [ColumnName("ProcessorName")]
        public string ProcessorName { get; set; }
    }
}
