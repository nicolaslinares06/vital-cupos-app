using AutoFixture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebFront.Models;

namespace TestUnit.WEB.Models
{

	public class ErrorViewModelTest
	{
        private Fixture _fixture;

        [Fact]
        public void ErrorViewModel()
        {
            ErrorViewModel model = new ErrorViewModel
            {
                RequestId = "1234567890"
            };

            var type = Assert.IsType<ErrorViewModel>(model);
            Assert.NotNull(type);
            Assert.True(model.ShowRequestId);
        }

        [Fact]
        public void ErrorViewModel_NoRequestId()
        {
            ErrorViewModel model = new ErrorViewModel
            {
                RequestId = null
            };

            var type = Assert.IsType<ErrorViewModel>(model);
            Assert.NotNull(type);
            Assert.False(model.ShowRequestId);
        }
    }
}
