using System;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using HWWebApi.Models;
using HWWebApi.UnitTest.Utils;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Models;
using Models.ModelEqualityComparer;
using Newtonsoft.Json;
using TestUtils;
using Xunit;

namespace HWWebApi.IntegrationTest
{
    public class ScansControllerTests
    {
        [Theory]
        [ClassData(typeof(ScanTestData))]
        public async Task Post(Scan scan)
        {
            if (scan.Computer == null) return;

            var now = DateTime.Now;

            var dbName = DateTime.Now.ToString(CultureInfo.CurrentCulture);
            var factory = new WebApplicationFactory<Startup>().WithWebHostBuilder(config =>
            {
                config.ConfigureServices(services =>
                {
                    services.AddDbContext<HardwareContext>(options => { options.UseInMemoryDatabase(dbName); });
                });
            });
            var client = factory.CreateClient();

            var content = new StringContent(JsonConvert.SerializeObject(scan.Computer), Encoding.UTF8,
                "application/json");
            var response = await client.PostAsync("api/Scans", content);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            var actual = await response.Content.ReadAsAsync<Scan>();
            Assert.Equal(scan.Computer, actual.Computer, new ModelEqualityByMembers<Computer>());

            response = await client.GetAsync($"api/scans/{actual.Id}");
            Assert.True(response.IsSuccessStatusCode, $"Response is {response.StatusCode}");

            actual = await response.Content.ReadAsAsync<Scan>();
            Assert.NotEqual(Guid.Empty, actual.Id);
            Assert.Null(actual.User);
            Assert.True(actual.CreationDateTime > now, $"{actual.CreationDateTime} > {now}");
            Assert.True(actual.CreationDateTime < DateTime.Now, $"{actual.CreationDateTime} < {DateTime.Now}");
            Assert.Equal(scan.Computer == null, actual.Computer == null);
        }
    }
}
