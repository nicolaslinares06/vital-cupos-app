using Microsoft.AspNetCore.Mvc;
using System;

namespace Web.Controllers
{
    public class ManageCitesPermits : Controller
    {
        private readonly string UrlApi;
        private readonly ILogger<CvController> _logger;
        readonly string RUTAAPI = Environment.GetEnvironmentVariable("RUTAAPI");
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        public ManageCitesPermits(ILogger<CvController> logger)
        {
            UrlApi = string.IsNullOrEmpty(RUTAAPI) ? new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build().GetValue<string>("Variables:RutaApi") : RUTAAPI;
            _logger = logger;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="manage"></param>
        /// <returns></returns>
        public IActionResult Index(int manage = 0)
        {
            try
            {
                _logger.LogInformation("method called");
                ViewBag.manage = manage;
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                throw;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="manage"></param>
        /// <returns></returns>
        public IActionResult Home(int manage = 0)
        {
            try
            {
                _logger.LogInformation("method called");
                ViewBag.manage = manage;
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                throw;
            }
        }
    }
}
