using HWWebApi.Models;
using HWWebApi.Models.ModelEqualityComparer;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace HWWebApi.IntegrationTest
{
    public class ComputerControllerTests
    {
        [Fact]
        public async Task Post()
        {
            var dbName = DateTime.Now.ToString(CultureInfo.CurrentCulture);
            var factory = new WebApplicationFactory<Startup>().WithWebHostBuilder(config =>
                {
                    config.ConfigureServices(services =>
                    {
                        services.AddDbContext<HardwareContext>(options =>
                        {
                            options.UseInMemoryDatabase(dbName);
                        });
                    });
                });
            var client = factory.CreateClient();
            var computer = TestUtils.TestUtils.GenerateComputer();

            var content = new StringContent(JsonConvert.SerializeObject(computer), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("api/computers/Body", content);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            var actual = await response.Content.ReadAsAsync<Computer>();
            Assert.Equal(computer, actual, new ModelEqualityByMembers<Computer>());

            response = await client.GetAsync($"api/computers/{actual.Id}");
            Assert.True(response.IsSuccessStatusCode, $"Response is {response.StatusCode}");

            actual = await response.Content.ReadAsAsync<Computer>();
            Assert.Equal(computer, actual, new ModelEqualityByMembers<Computer>());
        }
    }
}
