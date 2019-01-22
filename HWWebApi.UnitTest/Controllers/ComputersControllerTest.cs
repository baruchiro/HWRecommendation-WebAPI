using System;
using System.Collections.Generic;
using Xunit;
using HWWebApi.Controllers;
using HWWebApi.Models;
using FakeItEasy;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace HWWebApi.UnitTest.Controllers
{
    public class ComputersControllerTest
    {
        private DbContextOptions<HardwareContext> options;
        private Computer computer;
        public ComputersControllerTest()
        {
            options = new DbContextOptionsBuilder<HardwareContext>()
                .UseInMemoryDatabase(databaseName: DateTime.Now.ToString())
                .Options;

            var memory = new Memory
            {
                Capacity = 300,
                Type = RamType.DDR3,
                Ghz = 5000004
            };
            var disk = new Disk()
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
            var gpu = new GPU { Cores = 2 };

            computer = new Computer
            {
                Memories = new[] { memory },
                Disks = new[] { disk },
                Processor = processor,
                MotherBoard = motherBoard,
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

            ActionResult<Computer> result;
            //Then
            using (var context = new HardwareContext(options))
            {
                var computersController = new ComputersController(context);
                result = computersController.Get(computer.Id);     
            }

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var value = Assert.IsType<Computer>(okResult.Value);
            Assert.Equal(computer, value);
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
                Assert.Equal(computer, context.Computers.Include(c => c.Disks)
                    .Include(c => c.GPUs)
                    .Include(c => c.Memories)
                    .Include(c => c.MotherBoard)
                    .Include(c => c.Processor)
                    .Single());
            }
        }

        // Test for data laked computer

        // Test for add components?
    }
}
