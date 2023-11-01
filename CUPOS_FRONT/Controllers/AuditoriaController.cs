using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Repository.Helpers;
using System.Net.Http.Headers;
using Web.Models;
using static CUPOS_FRONT.Models.Requests;

namespace Web.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class AuditoriaController : Controller
    {
        private readonly ILogger<AuditoriaController> _logger;
        private readonly string UrlApi;
        readonly string RUTAAPI = Environment.GetEnvironmentVariable("RUTAAPI") ?? "";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        public AuditoriaController(ILogger<AuditoriaController> logger)
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
        public IActionResult GestionAuditoria()
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
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult Ayuda()
        {
            try
            {
                string? token = HttpContext.Session.GetString("token");

                if (token == null)
                    return View("Views/Login/Index.cshtml");

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
        /// 
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public IActionResult DetalleAuditoria(string fecha)
        {
            try
            {
                _logger.LogInformation("method called");
                ViewBag.FechaAuditoria = fecha;
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
        public IActionResult returnAdministracion()
        {
            try
            {
                _logger.LogInformation("method called");
            return RedirectToAction("Index", "Home");
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
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFinal"></param>
        /// <returns></returns>
        public List<Auditoria> Consultar(DateTime fechaInicio, DateTime fechaFinal)
        {
            try
            {
                _logger.LogInformation("method called");
                string nuevaFechaInicio = fechaInicio.ToString("yyyy-MM-dd HH:mm:ss.fff");
                string nuevaFechaFinal = fechaFinal.ToString("yyyy-MM-dd HH:mm:ss.fff");

                string URI = UrlApi + "/Audit/Consult?startDate=" + nuevaFechaInicio + "&endDate=" + nuevaFechaFinal + "&page=";
                var httpClient = getHttpClient();
                string token = HttpContext.Session.GetString("token") ?? "";

                List<Auditoria> r = new();

                if (token == "")
                {
                    HttpContext.Session.Remove("token");
                    return r;
                }
                else
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var response = httpClient.GetAsync(URI).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        string responseString = response.Content.ReadAsStringAsync().Result;
                        string jsonInput = responseString;
                        Responses respuesta = JsonConvert.DeserializeObject<Responses>(jsonInput) ?? new Responses();
                        r = JsonConvert.DeserializeObject<List<Auditoria>>(respuesta.Response.ToString() ?? "") ?? new List<Auditoria>();
                        HttpContext.Session.SetString("token", respuesta.Token);

                        return r;
                    }
                }

                return r;
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
        /// <param name="fecha"></param>
        /// <returns></returns>
        public List<Auditoria> ConsultarDetalles(string fecha)
        {
            try
            {
                _logger.LogInformation("method called");
                string URI = UrlApi + "/Audit/ConsultDetails?date=" + fecha;
                var httpClient = getHttpClient();
                string token = HttpContext.Session.GetString("token") ?? "";

                List<Auditoria> r = new();

                if (token == "")
                {
                    HttpContext.Session.Remove("token");
                    return r;
                }
                else
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var response = httpClient.GetAsync(URI).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        string responseString = response.Content.ReadAsStringAsync().Result;
                        string jsonInput = responseString;
                        Responses respuesta = JsonConvert.DeserializeObject<Responses>(jsonInput) ?? new Responses();
                        r = JsonConvert.DeserializeObject<List<Auditoria>>(respuesta.Response.ToString() ?? "") ?? new List<Auditoria>();
                        HttpContext.Session.SetString("token", respuesta.Token);

                        return r;
                    }
                }

                return r;
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
        public List<ModulosReq> ConsultarModulos()
        {
            try
            {
                _logger.LogInformation("method called");
                string token = HttpContext.Session.GetString("token") ?? "";

                string URI = UrlApi + "/Module/Consult";
                var httpClient = getHttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = httpClient.GetAsync(URI).Result;

                string responseString = response.Content.ReadAsStringAsync().Result;
                Responses respuesta = JsonConvert.DeserializeObject<Responses>(responseString) ?? new Responses();

                if (respuesta.Response == null)
                {
                    return new List<ModulosReq>();
                }

                List<ModulosReq> modulos = JsonConvert.DeserializeObject<List<ModulosReq>>(respuesta.Response.ToString() ?? "") ?? new List<ModulosReq>();

                return modulos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                throw;
            }
        }
    }
}