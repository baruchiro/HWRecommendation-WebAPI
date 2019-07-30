using System;
using System.Linq;
using System.Threading.Tasks;
using HWWebApi.Controllers;
using HWWebApi.Models;
using HWWebApi.UnitTest.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace HWWebApi.UnitTest
{
    public class ScansControllerTests
    {
        private readonly DbContextOptions<HardwareContext> _options;
        public ScansControllerTests()
        {
            _options = TestUtils.TestUtils.GetInMemoryDbContextOptions<HardwareContext>().Options;
        }

        [Theory]
        [ClassData(typeof(ScanTestData))]
        public async Task Get_GetScan_ValidateReturn(Scan scan)
        {
            //Given
            using (var context = new HardwareContext(_options))
            {
                context.Scans.Add(scan);
                context.SaveChanges();
            }

            //When
            using (var context = new HardwareContext(_options))
            {
                Assert.Equal(1, context.Scans.Count());
            }

            ActionResult<Scan> result;
            //Then
            using (var context = new HardwareContext(_options))
            {
                var scansController = new ScansController(context);
                result = await scansController.GetScan(scan.Id);
            }

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var value = Assert.IsType<Scan>(okResult.Value);
            Assert.Equal(scan.Computer?.Id, value.Computer?.Id);
            Assert.Equal(scan.User?.Id, value.User?.Id);
            Assert.Equal(scan.CreationDateTime, value.CreationDateTime);
        }

        [Theory]
        [ClassData(typeof(ScanTestData))]
        public async Task Post_InsertScan_ValidateExists(Scan scan)
        {
            var now = DateTime.Now;

            using (var context = new HardwareContext(_options))
            {
                var scansController = new ScansController(context);
                await scansController.PostNewScanByComputer(scan.Computer);
            }

            using (var context = new HardwareContext(_options))
            {
                Assert.Equal(1, context.Scans.Count());
                var actual = context.Scans.Include(s => s.User)
                    .Include(s => s.Computer).Single();

                Assert.NotEqual(Guid.Empty, actual.Id);
                Assert.Null(actual.User);
                Assert.True(actual.CreationDateTime > now, $"{actual.CreationDateTime} > {now}");
                Assert.True(actual.CreationDateTime < DateTime.Now, $"{actual.CreationDateTime} < {DateTime.Now}");
                Assert.Equal(scan.Computer?.Id, actual.Computer?.Id);
            }
        }

        //TODO: Test for data laked scan

        //TODO: Test for add components?
    }
}
