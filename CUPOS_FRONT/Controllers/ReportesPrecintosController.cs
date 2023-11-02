using ClosedXML.Excel;
using CUPOS_FRONT.Utilities;
using iText.Kernel.Pdf;
using iText.Layout;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Repository.Helpers;
using System.Data;
using System.Net.Http.Headers;
using static CUPOS_FRONT.Models.ReportesPrecintosModels;


namespace CUPOS_FRONT.Controllers
{
    public class ReportesPrecintosController : Controller
    {
        private readonly ILogger<ReportesPrecintosController> _logger;
        private readonly string rutaAPI;
        readonly string RUTAAPI = Environment.GetEnvironmentVariable("RUTAAPI") ?? "";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="metodosPDF"></param>
        /// <param name="logger"></param>
        public ReportesPrecintosController(ILogger<ReportesPrecintosController> logger  )
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
        public IActionResult Index()
        {
            try
            {
                _logger.LogInformation("method called");
                var modelo = new ReportesPrecintosViewModel
                {
                    Establecimientos = ObtenerEstablecimientosSelect()
                };
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
        public JsonResult ObtenerDatosPrecintos(FiltrosPrecintos filtros)
        {
            try
            {
                _logger.LogInformation("method called");

                var datosEmpresas = ObtenerDatosPrecintosAPI(filtros);

                return new JsonResult(new { listado = datosEmpresas });
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
        public List<DatosPrecintosModel> ObtenerDatosPrecintosAPI(FiltrosPrecintos? filtros)
        {
            try
            {
                _logger.LogInformation("method called");
                var datosEmpresas = new List<DatosPrecintosModel>();

                string? token = HttpContext.Session.GetString("token");
                string URI = $"{rutaAPI}/SealsReports/ConsultDataSeals";
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
                        datosEmpresas = JsonConvert.DeserializeObject<List<DatosPrecintosModel>>(respuesta.Response.ToString() ?? "") ?? new List<DatosPrecintosModel>();
                    }
                    else
                    {
                        HttpContext.Session.SetString("token", "");
                    }
                }

                if (datosEmpresas.Count == 0)
                    datosEmpresas = new List<DatosPrecintosModel>();

                return datosEmpresas;
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
        /// <returns></returns>
        private IEnumerable<SelectListItem> ObtenerEstablecimientosSelect()
        {
            try
            {
                _logger.LogInformation("method called");
                List<EstablecimientosPrecintos> tiposEstablecimientos = new();

                string token = HttpContext.Session.GetString("token") ?? "";
                string URI = rutaAPI + "/ReportesPrecintos/ConsultBussinesSeals";
                var httpClient = GetHttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = httpClient.GetAsync(URI).Result;

                if (response.IsSuccessStatusCode)
                {
                    string responseString = response.Content.ReadAsStringAsync().Result;
                    Responses respuesta = JsonConvert.DeserializeObject<Responses>(responseString) ?? new Responses();

                    if (!String.IsNullOrEmpty(respuesta.Token))
                    {
                        HttpContext.Session.SetString("token", respuesta.Token);
                        tiposEstablecimientos = JsonConvert.DeserializeObject<List<EstablecimientosPrecintos>>(respuesta.Response.ToString() ?? "") ?? new List<EstablecimientosPrecintos>();
                    }
                    else
                    {
                        HttpContext.Session.SetString("token", "");
                    }


                }

                var resultadoListado = tiposEstablecimientos.Select(x => new SelectListItem(x.EstablishmentName, x.EstablishmentId.ToString())).ToList();

                var opcionPorDefecto = new SelectListItem("Seleccione un Establecimiento", "0", true);
                resultadoListado.Insert(0, opcionPorDefecto);



                return resultadoListado;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                throw;
            }
        }
    }
}
