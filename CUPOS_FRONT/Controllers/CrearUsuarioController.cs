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
                string token = HttpContext.Session.GetString("token") ?? "";
                if (!String.IsNullOrEmpty(token))
                    return RedirectToAction("GestionUsuarios", "Usuarios");
                else
                    return RedirectToAction("Index", "Login");
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
                    ViewBag.Mensaje = StringHelper.msgCrearUsuarios;

                    return View("CreacionUsuario");
                }

                string URI = UrlApi + "/User/Create";
                var resp = Respuesta(URI, false, _datosUsu);

                if (resp.Message != null)
                {
                    HttpContext.Session.SetString("token", resp.Token);
                    ViewBag.MostrarMensaje = "true";
                    ViewBag.Mensaje = resp.Message;
                    return View("CreacionUsuario");
                }
                return View("CreacionUsuario");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                string token = HttpContext.Session.GetString("token") ?? "";
                if (!String.IsNullOrEmpty(token))
                    return RedirectToAction("Index", "Home");
                else
                    return RedirectToAction("Index", "Login");
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

                string URI = UrlApi + "/Parametric/ConsultDocumentType";
                var respuesta = Respuesta(URI, true);
                List<ReqDocumentType> docs = JsonConvert.DeserializeObject<List<ReqDocumentType>>(respuesta.Response.ToString() ?? "") ?? new List<ReqDocumentType>();

                return docs;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                return new List<ReqDocumentType>();
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

                string URI = UrlApi + "/Parametric/ConsultDependence";
                var respuesta = Respuesta(URI, true);
                List<ReqEstadoCertificado> docs = JsonConvert.DeserializeObject<List<ReqEstadoCertificado>>(respuesta.Response.ToString() ?? "") ?? new List<ReqEstadoCertificado>();

                return docs;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                return new List<ReqEstadoCertificado>();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private Responses Respuesta(string URI, bool get, Object data = null)
        {
            var httpClient = GetHttpClient();
            string token = HttpContext.Session.GetString("token") ?? "";

            if (token == "")
            {
                HttpContext.Session.Remove("token");
                return new Responses();
            }
            else
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage response;

                if (get)
                {
                    response = httpClient.GetAsync(URI).Result;
                }
                else
                {
                    response = httpClient.PutAsJsonAsync(URI, data).Result;
                }

                if (response.IsSuccessStatusCode)
                {
                    string responseString = response.Content.ReadAsStringAsync().Result;
                    string jsonInput = responseString;
                    return JsonConvert.DeserializeObject<Responses>(jsonInput) ?? new Responses();
                }
            }

            return new Responses();
        }
    }
}
