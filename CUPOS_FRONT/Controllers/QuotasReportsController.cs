using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Repository.Helpers;
using Repository.Helpers.Models;
using System.Data;
using System.Net.Http.Headers;
using Web.Models;

namespace WebFront.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class QuotasReportsController : Controller
    {

        private readonly string UrlApi;
        private readonly ILogger<QuotasReportsController> _logger;
        readonly string RUTAAPI = Environment.GetEnvironmentVariable("RUTAAPI");

        public QuotasReportsController(ILogger<QuotasReportsController> logger)
        {
            UrlApi = string.IsNullOrEmpty(RUTAAPI) ? new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build().GetValue<string>("Variables:RutaApi") : RUTAAPI;
            _logger = logger;
        }
        /// <summary>
        /// htttp client
        /// </summary>
        /// <returns></returns>
        public HttpClient getHttpClient()
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            return new HttpClient(clientHandler);
        }
        /// <summary>
        /// retornar vista
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            try
            {
                _logger.LogInformation("method called");
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                throw;
            }
        }
        /// <summary>
        /// consultar resoluciones
        /// </summary>
        /// <param name="resolutionNumber"></param>
        /// <param name="BussinesName"></param>
        /// <param name="BussinesNit"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public object ConsultResolutions(string? resolutionNumber, string? BussinesName, string? BussinesNit, DateTime? fromDate, DateTime? toDate)
        {
            try
            {
                _logger.LogInformation("method called");
                List<Cupos>? datos = new List<Cupos>();
                string? token = HttpContext.Session.GetString("token");
                string URI = UrlApi + "/QuotasReports/ConsultResolutions?resolutionNumber=" + resolutionNumber + "&BussinesName=" + BussinesName + "&BussinesNit=" + BussinesNit + "&fromDate=" + fromDate + "&toDate=" + toDate;
                var httpClient = getHttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = httpClient.GetAsync(URI).Result;
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return new
                    {
                        volverInicio = true
                    };
                }
                if (response.IsSuccessStatusCode)
                {
                    string responseString = response.Content.ReadAsStringAsync().Result;
                    Responses? respuesta = JsonConvert.DeserializeObject<Responses>(responseString);
                    if (respuesta.Response != null)
                    {
                        datos = JsonConvert.DeserializeObject<List<Cupos>>(respuesta.Response.ToString() ?? "");
                        HttpContext.Session.SetString("token", respuesta.Token);
                    }
                }
                return datos ?? new object { };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                throw;
            }
        }
    }
}
