using HWWebApi.Controllers;
using HWWebApi.Models;
using HWWebApi.Models.ModelEqualityComparer;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Xunit;

namespace HWWebApi.UnitTest.Controllers
{
    public class WorksControllerTests
    {
        private DbContextOptions<HardwareContext> options;

        public WorksControllerTests()
        {
            options = TestUtils.TestUtils.GetInMemoryDbContextOptions().Options;
        }

        [Fact]
        public void Post_InsertWorkObject_ValidateExists()
        {
            var work = new Work
            {
                Name = "Test Work"
            };

            using (var context = new HardwareContext(options))
            {
                var worksController = new WorksController(context);
                worksController.Post(work);
            }

            using (var context = new HardwareContext(options))
            {
                Assert.Equal(1, context.Works.Count());
                Assert.Equal(work, context.Works.Single(), new ModelEqualityComparer<Work>());
            }
        }

        [Fact]
        public void Post_InsertWorkString_ValidateExists()
        {
            var work = new Work
            {
                Name = "Test Work"
            };

            using (var context = new HardwareContext(options))
            {
                var worksController = new WorksController(context);
                worksController.Post(work.Name);
            }

            using (var context = new HardwareContext(options))
            {
                Assert.Equal(1, context.Works.Count());
                Assert.Equal(work, context.Works.Single(), new ModelEqualityByMembers<Work>());
            }
        }

        [Fact]
        public void Post_InsertDuplicateWork_ShouldThrowException()
        {
            var work = new Work
            {
                Name = "Test Work"
            };

            using (var context = new HardwareContext(options))
            {
                var worksController = new WorksController(context);
                worksController.Post(work.Name);
            }

            using (var context = new HardwareContext(options))
            {
                var worksController = new WorksController(context);
                worksController.Post(work.Name);
            }

            //using (var context = new HardwareContext(options))
            //{
            //    Assert.Equal(1, context.Works.Count());
            //    //Assert.Equal(work, context.Works.Single(), new ModelEqualityByMembers<Work>());
            //}
        }
        // TODO: insert duplicate string
    }
}
