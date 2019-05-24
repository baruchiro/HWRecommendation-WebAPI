using System;
using HW.Bot.Model;
using HWWebApi.Models;
using Microsoft.EntityFrameworkCore;
using Models;

namespace TestUtils
{
    public class TestUtils
    {
        public static Computer GenerateComputer()
        {
            var memory = new Memory
            {
                Capacity = 300,
                Type = RamType.DDR3,
                Ghz = 5000004
            };
            var disk = new Disk
            {
                Type = DiskType.HDD,
                Rpm = 5000,
                Capacity = 1000000
            };
            var processor = new Processor
            {
                Name = "Intel core i6",
                GHz = 500045546,
                NumOfCores = 4,
                Architecture = Architecture.X64
            };
            var motherBoard = new MotherBoard
            {
                DdrSockets = 2,
                MaxRam = 400000,
                SataConnections = 2,
                Architecture = Architecture.X64
            };
            var gpu = new Gpu { Cores = 2 };

            var computer = new Computer
            {
                Memories = new[] { memory },
                Disks = new[] { disk },
                Processor = processor,
                MotherBoard = motherBoard,
                Gpus = new[] { gpu }
            };

            return computer;
        }

        public static Scan GenerateScan()
        {
            return new Scan
            {
                Computer = GenerateComputer(),
                CreationDateTime = DateTime.Now,
                User = new User {Age = 25, Gender = Gender.MALE, Name = "User Name", WorkArea = "My work"}
            };
        }
        public static Computer GenerateEmptyComponentsComputer()
        {
            var memory = new Memory();
            var disk = new Disk();
            var processor = new Processor();
            var motherBoard = new MotherBoard();
            var gpu = new Gpu();

            var computer = new Computer
            {
                Memories = new[] { memory },
                Disks = new[] { disk },
                Processor = processor,
                MotherBoard = motherBoard,
                Gpus = new[] { gpu }
            };

            return computer;
        }

        public static Scan GenerateEmptyComponentsScan()
        {
            return new Scan
            {
                Computer = new Computer(),
                CreationDateTime = new DateTime(),
                User = new User()
            };
        }
        public static Computer GenerateEmptyComputer()
        {
            var computer = new Computer();

            return computer;
        }

        public static Scan GenerateEmptyScan()
        {
            return new Scan();
        }
        public static DbContextOptionsBuilder<HardwareContext> GetInMemoryDbContextOptions()=> new DbContextOptionsBuilder<HardwareContext>()
            .UseInMemoryDatabase(databaseName: DateTime.Now.Ticks.ToString());
    }
}
