using System;
using System.Collections.Generic;
using Xunit;
using HWWebApi.Controllers;
using HWWebApi.Models;
using FakeItEasy;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using HWWebApi.Models.ModelEqualityComparer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using TestUtils;

namespace HWWebApi.UnitTest.Controllers
{
    public class ComputersControllerTest
    {
        private DbContextOptions<HardwareContext> options;
        private Computer computer;
        public ComputersControllerTest()
        {
            options = TestUtils.TestUtils.GetInMemoryDbContextOptions().Options;

            computer = TestUtils.TestUtils.GenerateComputer();

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
            Assert.Equal(computer, value, new ModelEqualityComparer<Computer>());
        }

        [Fact]
        public void Post_InsertComputer_ValidateExists()
        {

            using (var context = new HardwareContext(options))
            {
                var computersController = new ComputersController(context);
                computersController.PostBody(computer);
            }

            using (var context = new HardwareContext(options))
            {
                Assert.Equal(1, context.Computers.Count());
                Assert.Equal(computer, context.Computers.Include(c => c.Disks)
                    .Include(c => c.GPUs)
                    .Include(c => c.Memories)
                    .Include(c => c.MotherBoard)
                    .Include(c => c.Processor)
                    .Single(), new ModelEqualityComparer<Computer>());
            }
        }

        // Test for data laked computer

        // Test for add components?
    }
}
