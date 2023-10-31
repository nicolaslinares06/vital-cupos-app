﻿using AutoFixture;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;
using Web.Controllers;
using Web.Models;
using Microsoft.Extensions.Logging;
using WebFront.Controllers;
using Microsoft.Extensions.Configuration;

namespace TestUnit.WEB.Controllers
{
    public class QuotasReportsControllerTest
    {
        //Se debe colocar un usuario administrador valido para la ejecucion de las pruebas unitarias
        private QuotasReportsController controller;
        private Fixture _fixture;
        private readonly string token;

        public QuotasReportsControllerTest()
        {
            controller = new QuotasReportsController(new LoggerFactory().CreateLogger<QuotasReportsController>());
            _fixture = new Fixture();

            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, "Administrador")
        };
            var identity = new ClaimsIdentity(claims, "someAuthTypeName");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            var httpContext = new DefaultHttpContext
            {
                User = claimsPrincipal,
                Session = new FakeSession()
            };

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            token = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.Development.json").Build().GetValue<string>("Variables:Token");

        }

        [Fact]
        public void Index()
        {
            var r = controller.Index();
            Assert.True(r != null);

            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            r = controller.Index();
            Assert.True(r != null);
        }

        [Fact]
        public void Crear()
        {
            string? resolutionNumber = _fixture.Create<string>(); 
            string? BussinesName = _fixture.Create<string>(); 
            string? BussinesNit = _fixture.Create<string>();
            DateTime? fromDate = DateTime.Now;
            DateTime? toDate = DateTime.Now;
            controller.ControllerContext.HttpContext.Session.SetString("token", token);

            var r = controller.ConsultResolutions(resolutionNumber, BussinesName, BussinesNit, fromDate, toDate);
            Assert.True(r != null);
        }
    }
}
