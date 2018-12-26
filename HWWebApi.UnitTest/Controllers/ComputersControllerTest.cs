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
        private Computer computer;
        public ComputersControllerTest()
        {
            options = new DbContextOptionsBuilder<HardwareContext>()
                .UseInMemoryDatabase(databaseName: "Add_writes_to_database")
                .Options;

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

            computer = new Computer()
            {
                Memories = new[] { memory },
                Disks = new[] { disk },
                Processor = proccesor,
                MotherBoard = mobo,
                GPUs = new[] { gpu }
            };

        }

        [Fact]
        public void Get_GetComputer_ValidateReturn()
        {
            //Given
            using (var context = new HardwareContext(options))
            {
                context.Computers.Add(computer);
                context.SaveChanges();
            }

            //When
            using (var context = new HardwareContext(options))
            {
                Assert.Equal(1, context.Computers.Count());
            }
            //Then
            using (var context = new HardwareContext(options))
            {
                var computersController = new ComputersController(context);
                var actualComputer = computersController.Get(computer.Id).Value;
                Assert.True(computer.Equals(actualComputer));
                Assert.Equal(computer, actualComputer);
            }
        }

        [Fact]
        public void Post_InsertComputer_ValidateExists()
        {

            using (var context = new HardwareContext(options))
            {
                var computersController = new ComputersController(context);
                computersController.Post(computer);
            }

            using (var context = new HardwareContext(options))
            {
                Assert.Equal(1, context.Computers.Count());
                Assert.Equal(computer, context.Computers.Single());
            }
        }
    }
}
