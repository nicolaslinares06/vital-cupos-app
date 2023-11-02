using ClosedXML.Excel;
using CUPOS_FRONT.Utilities;
using iText.Kernel.Pdf;
using iText.Layout;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Repository.Helpers;
using System.Data;
using System.Net.Http.Headers;
using static CUPOS_FRONT.Models.ReportesMarquillasModels;

namespace CUPOS_FRONT.Controllers
{
    public class ReportesMarquillasController : Controller
    {
        private readonly ILogger<ReportesMarquillasController> _logger;
        private readonly string rutaAPI;
        readonly string RUTAAPI = Environment.GetEnvironmentVariable("RUTAAPI") ?? "";

        public ReportesMarquillasController(ILogger<ReportesMarquillasController> logger    )
        {
            rutaAPI = string.IsNullOrEmpty(RUTAAPI) ? new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build().GetValue<string>("Variables:RutaApi") : RUTAAPI;
            _logger = logger;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public HttpClient GetHttpClient()
        {
            HttpClientHandler clientHandler = new()
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
            };
            return new HttpClient(clientHandler);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            try
            {
                _logger.LogInformation("method called");
                var modelo = new FiltrosMarquillas();
                return View(modelo);
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
        /// <param name="filtros"></param>
        /// <returns></returns>
        [HttpPost]
        public List<DatosMarquillaModel> ObtenerDatosMarquillas(FiltrosMarquillas filtros)
        {
            try
            {
                _logger.LogInformation("method called");
                List<DatosMarquillaModel> datos = new();

                string? token = HttpContext.Session.GetString("token");
                string URI = $"{rutaAPI}/TagsReports/ConsultDataTags";
                var httpClient = GetHttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = httpClient.PostAsJsonAsync(URI, filtros).Result;

                if (response.IsSuccessStatusCode)
                {
                    string responseString = response.Content.ReadAsStringAsync().Result;
                    Responses respuesta = JsonConvert.DeserializeObject<Responses>(responseString) ?? new Responses();

                    if (!String.IsNullOrEmpty(respuesta.Token))
                    {
                        HttpContext.Session.SetString("token", respuesta.Token);
                        datos = JsonConvert.DeserializeObject<List<DatosMarquillaModel>>(respuesta.Response.ToString() ?? "") ?? new List<DatosMarquillaModel>();
                    }
                    else
                    {
                        HttpContext.Session.SetString("token", "");
                    }
                }

                if (datos.Count == 0)
                    datos = new List<DatosMarquillaModel>();

                return datos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                throw;
            }
        }
    }
}
