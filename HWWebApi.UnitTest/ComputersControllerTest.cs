using System;
using System.Collections.Generic;
using Xunit;
using HWWebApi.Controllers;
using HWWebApi.Models;
using FakeItEasy;
using Microsoft.EntityFrameworkCore;

namespace HWWebApi.UnitTest
{
    public class ComputersControllerTest
    {
        private ComputersController computersController;
        public ComputersControllerTest()
        {
            var options = new DbContextOptionsBuilder<HardwareContext>()
                .UseInMemoryDatabase(databaseName: "Add_writes_to_database")
                .Options;

            this.computersController = new ComputersController(new HardwareContext(options));
        }
        [Fact]
        public void Test_Post_ValidateID()
        {
            Computer computer = A.Fake<Computer>();
            var id = computersController.Post(computer);
            var computer2 = computersController.Get(id);

            Assert.Equal(computer, computer2);
        }
    }
}
