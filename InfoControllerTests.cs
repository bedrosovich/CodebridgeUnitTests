using Codebridge.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;

namespace UnitTests
{
    public class InfoControllerTests
    {
        [Fact]
        public void PingResult()
        {
            InfoController controller = new InfoController();

            JsonResult result = controller.Ping();
     
            Assert.Equal("Dogs house service. Version 1.0.1", result.Value);
        }

        [Fact]
        public void PingResultNotNull()
        {
            InfoController controller = new InfoController();

            JsonResult result = controller.Ping();

            Assert.NotNull(result);
        }

       
    }
}
