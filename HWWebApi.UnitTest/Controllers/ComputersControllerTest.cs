using System;
using System.Collections.Generic;
using Xunit;
using HWWebApi.Controllers;
using HWWebApi.Models;
using FakeItEasy;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace HWWebApi.UnitTest.Controllers
{
    public class ComputersControllerTest
    {
        private DbContextOptions<HardwareContext> options;
        public ComputersControllerTest()
        {
            options = new DbContextOptionsBuilder<HardwareContext>()
                .UseInMemoryDatabase(databaseName: "Add_writes_to_database")
                .Options;
        }

        [Fact]
        public void Test_Post_ValidateID()
        {
            var memory = new Memory()
            {
                Capacity = 300,
                type = RAMType.DDR3,
                ghz = 5000004
            };
            var disk = new Disk()
            {
                type = DiskType.HDD,
                rpm = 5000,
                capacity = 1000000
            };
            var proccesor = new Processor()
            {
                Name = "Intel core i6",
                ghz = 500045546,
                numOfCores = 4,
                architacture = Architacture.x64
            };
            var mobo = new MotherBoard()
            {
                ddrSockets = 2,
                maxRam = 400000,
                sataConnections = 2,
                architacture = Architacture.x64
            };
            var gpu = new GPU() { cores = 2 };
            Computer expectedComputer = new Computer()
            {
                memories = new[] { memory },
                disks = new[] { disk },
                processor = proccesor,
                motherBoard = mobo,
                gpus = new[] { gpu }
            };

            using (var context = new HardwareContext(options))
            {
                var computersController = new ComputersController(context);
                computersController.Post(expectedComputer);
            }

            using (var context = new HardwareContext(options))
            {
                Assert.Equal(1, context.Computers.Count());
                Assert.Equal(expectedComputer, context.Computers.Single());
            }
        }
    }
}
