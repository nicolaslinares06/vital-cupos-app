using Microsoft.AspNetCore.Mvc;
using Web.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Text;
using System.Net.Http.Headers;
using Repository.Helpers;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using static CUPOS_FRONT.Models.Requests;

namespace Web.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class CrearUsuarioController : Controller
    {
        private readonly ILogger<CrearUsuarioController> _logger;
        private readonly string UrlApi;
        readonly string RUTAAPI = Environment.GetEnvironmentVariable("RUTAAPI") ?? "";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        public CrearUsuarioController(ILogger<CrearUsuarioController> logger)
        {
            UrlApi = string.IsNullOrEmpty(RUTAAPI) ? new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build().GetValue<string>("Variables:RutaApi") : RUTAAPI;
            _logger = logger;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public HttpClient GetHttpClient()
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            return new HttpClient(clientHandler);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult CreacionUsuario()
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
        /// <param name="_datosUsu"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Crear(CreacionUsu _datosUsu)
        {
            try
            {
                _logger.LogInformation("method called");
                if (HttpContext.Session.GetString("CrearUsuario") != "True")
                {
                    ViewBag.MostrarMensaje = "true";
                    ViewBag.Mensaje = "El usuario no cuenta con los permisos para Crear Usuarios";

                    return View("CreacionUsuario");
                }

                string token = HttpContext.Session.GetString("token") ?? "";

                if (token == "")
                {
                    HttpContext.Session.Remove("token");
                    return RedirectToAction("Index", "Login");
                }
                else
                {
                    string URI = UrlApi + "/User/Create";
                    var httpClient = GetHttpClient();
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var response = httpClient.PostAsJsonAsync(URI, _datosUsu).Result;

                    string responseString = response.Content.ReadAsStringAsync().Result;
                    Responses resp = JsonConvert.DeserializeObject<Responses>(responseString) ?? new Responses();

                    if (response.IsSuccessStatusCode)
                    {
                        HttpContext.Session.SetString("token", resp.Token);
                        ViewBag.MostrarMensaje = "true";
                        ViewBag.Mensaje = resp.Message;
                        return View("CreacionUsuario");
                    }
                    return View("CreacionUsuario");
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
        public List<ReqDocumentType> ConsultarDocumentos()
        {
            try
            {
                _logger.LogInformation("method called");
                string token = HttpContext.Session.GetString("token") ?? "";

                string URI = UrlApi + "/Parametric/ConsultDocumentType";
                var httpClient = GetHttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = httpClient.GetAsync(URI).Result;

                string responseString = response.Content.ReadAsStringAsync().Result;
                Responses respuesta = JsonConvert.DeserializeObject<Responses>(responseString) ?? new Responses();
                List<ReqDocumentType> docs = JsonConvert.DeserializeObject<List<ReqDocumentType>>(respuesta.Response.ToString() ?? "") ?? new List<ReqDocumentType>();


                return docs;
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
        public List<ReqEstadoCertificado> ConsultarDependencia()
        {
            try
            {
                _logger.LogInformation("method called");
                string token = HttpContext.Session.GetString("token") ?? "";

                string URI = UrlApi + "/Parametric/ConsultDependence";
                var httpClient = GetHttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = httpClient.GetAsync(URI).Result;

                string responseString = response.Content.ReadAsStringAsync().Result;
                Responses respuesta = JsonConvert.DeserializeObject<Responses>(responseString) ?? new Responses();
                List<ReqEstadoCertificado> docs = JsonConvert.DeserializeObject<List<ReqEstadoCertificado>>(respuesta.Response.ToString() ?? "") ?? new List<ReqEstadoCertificado>();


                return docs;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                throw;
            }
        }
    }
}
