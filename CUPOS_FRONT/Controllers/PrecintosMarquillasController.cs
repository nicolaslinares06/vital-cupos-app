using DocumentFormat.OpenXml.Drawing;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Org.BouncyCastle.Ocsp;
using Repository.Helpers;
using Repository.Models;
using System.Drawing;
using System.Net.Http.Headers;
using Web.Models;

namespace Web.Controllers
{
    public class PrecintosMarquillasController : Controller
    {
        private readonly ILogger<PrecintosMarquillasController> _logger;
        private readonly string UrlApi;
        readonly string RUTAAPI = Environment.GetEnvironmentVariable("RUTAAPI");
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        public PrecintosMarquillasController(ILogger<PrecintosMarquillasController> logger)
        {
            UrlApi = string.IsNullOrEmpty(RUTAAPI) ? new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build().GetValue<string>("Variables:RutaApi") : RUTAAPI;
            _logger = logger;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public HttpClient getHttpClient()
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            return new HttpClient(clientHandler);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult Home()
        {
            try
            {
                _logger.LogInformation("method called");
                return View("Views/Home/Index.cshtml");
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
        public IActionResult Index()
        {
            try
            {
                _logger.LogInformation("method called");
                string? token = HttpContext.Session.GetString("token");

                if (token == null)
                    return View("Views/Login/Index.cshtml");

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
        /// <returns></returns>
        public IActionResult Ayuda()
        {
            try
            {
                _logger.LogInformation("method called");
                string? token = HttpContext.Session.GetString("token");

                if (token != null)
                    return View("Views/PrecintosMarquillas/Partials/Ayuda.cshtml");

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
        /// <returns></returns>
        [HttpPost] 
        public List<CuposV001Precintoymarquilla> Filtrar(string tipoDocumento, DateTime fechaInicial, string numero, string numeroDocumento, DateTime fechaFinal, string color, string nombreEmpresa, string vigencia)
        {
            try
            {

                var parametros = new FiltrosPrecintosMarquillas();
                var fechaActual = DateTime.Now;
                _logger.LogInformation("method called");
                if (fechaInicial == DateTime.MinValue)
                    fechaInicial = fechaActual.AddYears(-100);

                if (fechaFinal == DateTime.MinValue)
                    fechaFinal = fechaActual.AddYears(500);

                fechaFinal = fechaFinal.AddDays(1).AddSeconds(-1);

                parametros.documentType = tipoDocumento;
                parametros.initialDate = fechaInicial;
                parametros.number = numero;
                parametros.documentNumber = numeroDocumento;
                parametros.finalDate = fechaFinal;
                parametros.color = color;
                parametros.companyName = nombreEmpresa;
                parametros.validity = vigencia;

              

                string URI = UrlApi + "/SealsLabels/Consult";
                var httpClient = getHttpClient();
                string? token = HttpContext.Session.GetString("token");

                List<CuposV001Precintoymarquilla> listaDatos = new List<CuposV001Precintoymarquilla>();

                if (token == null)
                {
                    HttpContext.Session.Remove("token");
                    return listaDatos;
                }
                else
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var response = httpClient.PostAsJsonAsync(URI, parametros).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        string responseString = response.Content.ReadAsStringAsync().Result;
                        string jsonInput = responseString;
                        Responses respuesta = JsonConvert.DeserializeObject<Responses>(jsonInput);
                        listaDatos = JsonConvert.DeserializeObject<List<CuposV001Precintoymarquilla>>(respuesta.Response.ToString());
                        HttpContext.Session.SetString("token", respuesta.Token);

                        return listaDatos;
                    }

                    int statusCode = (int)response.StatusCode;
                    string statusCodeStr = statusCode.ToString();
                    CuposV001Precintoymarquilla datos = new CuposV001Precintoymarquilla();
                    datos.CANTIDAD = 10;
                    datos.COLOR = statusCodeStr;
                    datos.ESPECIE = token;
                    datos.NOMBRE = URI;
                    datos.CANTIDAD = 1000000;
                    listaDatos.Add(datos);

                    return listaDatos;
                }
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
        [HttpPost]
        public List<TiposDocumentosEmpresas> TipoDocumentoEmpresa()
        {
            try
            {
                _logger.LogInformation("method called");
                string URI = UrlApi + "/SealsLabels/CompanyDocumentType";
                var httpClient = getHttpClient();
                string? token = HttpContext.Session.GetString("token");

                List<TiposDocumentosEmpresas> tiposDocumentosEmpresas = new List<TiposDocumentosEmpresas>();
                if (token == null)
                {
                    HttpContext.Session.Remove("token");
                    return tiposDocumentosEmpresas;
                }
                else
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var response = httpClient.GetAsync(URI).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        string responseString = response.Content.ReadAsStringAsync().Result;
                        string jsonInput = responseString;
                        Responses respuesta = JsonConvert.DeserializeObject<Responses>(jsonInput);
                        tiposDocumentosEmpresas = JsonConvert.DeserializeObject<List<TiposDocumentosEmpresas>>(respuesta.Response.ToString());
                        HttpContext.Session.SetString("token", respuesta.Token);

                        return tiposDocumentosEmpresas;
                    }

                    return tiposDocumentosEmpresas;
                }
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
        [HttpPost]
        public List<Colores> Color()
        {
            try
            {
                _logger.LogInformation("method called");
                string URI = UrlApi + "/SealsLabels/Colors";
                var httpClient = getHttpClient();
                string? token = HttpContext.Session.GetString("token");

                List<Colores> colores = new List<Colores>();
                if (token == null)
                {
                    HttpContext.Session.Remove("token");
                    return colores;
                }
                else
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var response = httpClient.GetAsync(URI).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        string responseString = response.Content.ReadAsStringAsync().Result;
                        string jsonInput = responseString;
                        Responses respuesta = JsonConvert.DeserializeObject<Responses>(jsonInput);
                        colores = JsonConvert.DeserializeObject<List<Colores>>(respuesta.Response.ToString());
                        HttpContext.Session.SetString("token", respuesta.Token);

                        return colores;
                    }

                    return colores;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                throw;
            }
        }
    }
}