﻿using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Repository.Helpers;
using Repository.Helpers.Models;
using System.Net.Http.Headers;
using Web.Models;
using static CUPOS_FRONT.Models.Requests;

namespace Web.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class AdministrarRolesController : Controller
    {
        private readonly ILogger<AdministrarRolesController> _logger;
        private readonly string UrlApi;
        readonly string RUTAAPI = Environment.GetEnvironmentVariable("RUTAAPI") ?? "";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        public AdministrarRolesController(ILogger<AdministrarRolesController> logger)
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
        public IActionResult Index()
        {
            try
            {
                _logger.LogInformation("method called");
                string? token = HttpContext.Session.GetString("token");

                if (token == null)
                    return View("Views/Login/Index.cshtml");

                return View(cargueInicial());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                string token = HttpContext.Session.GetString("token") ?? "";
                if (!String.IsNullOrEmpty(token))
                    return RedirectToAction("Administracion", "Home");
                else
                    return RedirectToAction("Index", "Login");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult CrearRol()
        {
            try
            {
                return Vistas();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                string token = HttpContext.Session.GetString("token") ?? "";
                if (!String.IsNullOrEmpty(token))
                    return RedirectToAction("Index", "AdministrarRoles");
                else
                    return RedirectToAction("Index", "Login");
            }
        }

        public IActionResult Ayuda()
        {
            return Vistas();
        }

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
                string token = HttpContext.Session.GetString("token") ?? "";
                if (!String.IsNullOrEmpty(token))
                    return RedirectToAction("Index", "AdministrarRoles");
                else
                    return RedirectToAction("Index", "Login");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="nombreRol"></param>
        /// <param name="valEstado"></param>
        /// <returns></returns>
        public RolesList cargueInicial()
        {
            string URI = UrlApi + "/Rol/Consult";

            var r = Busquedas(URI);

            foreach (var rol in r)
            {
                List<SelectListItem> resultadoListado = new List<SelectListItem>();

                if (rol.estado.ToUpper() == "ACTIVO")
                {
                    var opcionSelected = new SelectListItem("ACTIVO", "ACTIVO", true);
                    var opcion2 = new SelectListItem("INACTIVO", "INACTIVO", false);
                    resultadoListado.Insert(0, opcionSelected);
                    resultadoListado.Insert(1, opcion2);
                }
                else
                {
                    var opcionSelected = new SelectListItem("INACTIVO", "INACTIVO", true);
                    var opcion2 = new SelectListItem("ACTIVO", "ACTIVO", false);
                    resultadoListado.Insert(0, opcionSelected);
                    resultadoListado.Insert(1, opcion2);
                }

                rol.estate = resultadoListado;
            }

            RolesList listado = new()
            {
                rolsList = r
            };

            return listado;
        }

        public List<ReqRoles> filtroRoles(string nombreRol, string valEstado)
        {
            try
            {
                _logger.LogInformation("method called");

                string URI = UrlApi + "/Rol/Consult?searchQuery=" + nombreRol + "&state=" + valEstado;

                return Busquedas(URI);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                return new List<ReqRoles>();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_datosAct"></param>
        /// <returns></returns>
        [HttpPost]
        public string ActualizarEstado (ReqRol _datosAct)
        {
            try
            {
                _logger.LogInformation("method called");
                if (HttpContext.Session.GetString("ActualizarRoles") != "True")
                {
                    return StringHelper.msgNoPermisoActualizar;
                }

                string r = "";

                if (_datosAct.description == null)
                {
                    _datosAct.description = "";
                }

                string URI = UrlApi + "/Rol/Update";
                var resp = Respuesta(URI, false, _datosAct);
                r = resp.Message;

                if (!resp.Error)
                {
                    HttpContext.Session.SetString("token", resp.Token);
                }

                return r;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                return "";
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_datosCreacion"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult CreacionRol(CreateRolRequest _datosCreacion)
        {
            try
            {
                _logger.LogInformation("method called");
                if (HttpContext.Session.GetString("CrearRoles") != "True")
                {
                    ViewBag.RespuestaCreacionRol = StringHelper.msgCrearRoles;
                    ViewBag.ErrorCreacion = "false";

                    return View("CrearRol");
                }

                _datosCreacion.description = "";

                string URL = UrlApi + "/Rol/Create";

                var resp = Respuesta(URL, false, _datosCreacion);

                if (!resp.Error)
                {
                    HttpContext.Session.SetString("token", resp.Token);
                }
                ViewBag.RespuestaCreacionRol = resp.Message;
                ViewBag.ErrorCreacion = "false";

                return View("CrearRol");
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
        [Produces("application/json")]
        [HttpGet]
        public List<string> BuscarRoles()
        {
            try
            {
                _logger.LogInformation("method called");
                List<string> valores = new List<string>();

                string term = HttpContext.Request.Query["term"].ToString();

                string URI = UrlApi + "/Rol/ConsultAllRols?parameter=" + term;

                var respuesta = Respuesta(URI, true);
                List<ValorEstado> datos = JsonConvert.DeserializeObject<List<ValorEstado>>(respuesta.Response.ToString() ?? "") ?? new List<ValorEstado>();

                foreach (var etapa in datos)
                {
                    valores.Add(Convert.ToString(etapa.etapa) ?? "");
                }

                return valores;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                return new List<string>();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private IActionResult Vistas()
        {
            _logger.LogInformation("method called");
            string? token = HttpContext.Session.GetString("token");

            if (token == null)
                return View("Views/Login/Index.cshtml");

            return View();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private List<ReqRoles> Busquedas(string URI)
        {
            _logger.LogInformation("method called");

            var respuesta = Respuesta(URI, true);
            List<ReqRoles> r = JsonConvert.DeserializeObject<List<ReqRoles>>(respuesta.Response.ToString() ?? "") ?? new List<ReqRoles>();
            HttpContext.Session.SetString("token", respuesta.Token);

            return r;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private Responses Respuesta(string URI, bool get, Object data = null)
        {
            var httpClient = getHttpClient();
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
