using System.Linq;
using HWWebApi.Controllers;
using HWWebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.ModelEqualityComparer;
using TestUtils;
using Xunit;

namespace HWWebApi.UnitTest.Controllers
{
    public class ComputersControllerTest
    {
        private readonly DbContextOptions<HardwareContext> _options;
        public ComputersControllerTest()
        {
            _options = TestUtils.TestUtils.GetInMemoryDbContextOptions<HardwareContext>().Options;
        }

        [Theory]
        [ClassData(typeof(ComputerTestData))]
        public void Get_GetComputer_ValidateReturn(Computer computer)
        {
            //Given
            using (var context = new HardwareContext(_options))
            {
                context.Computers.Add(computer);
                context.SaveChanges();
            }

            //When
            using (var context = new HardwareContext(_options))
            {
                Assert.Equal(1, context.Computers.Count());
            }

            ActionResult<Computer> result;
            //Then
            using (var context = new HardwareContext(_options))
            {
                var computersController = new ComputersController(context);
                result = computersController.Get(computer.Id);     
            }

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var value = Assert.IsType<Computer>(okResult.Value);
            Assert.Equal(computer, value, new ModelEqualityComparer<Computer>());
        }

        [Theory]
        [ClassData(typeof(ComputerTestData))]
        public void Post_InsertComputer_ValidateExists(Computer computer)
        {

            using (var context = new HardwareContext(_options))
            {
                var computersController = new ComputersController(context);
                computersController.PostBody(computer);
            }

            using (var context = new HardwareContext(_options))
            {
                Assert.Equal(1, context.Computers.Count());
                Assert.Equal(computer, context.Computers.Include(c => c.Disks)
                    .Include(c => c.Gpus)
                    .ThenInclude(g=> g.Memory)
                    .Include(c => c.Memories)
                    .Include(c => c.MotherBoard)
                    .Include(c => c.Processor)
                    .Single(), new ModelEqualityComparer<Computer>());
            }
        }

        //TODO: Test for data laked computer

        //TODO: Test for add components?
    }
}
