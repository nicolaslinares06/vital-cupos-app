using Microsoft.AspNetCore.Authentication.Cookies;
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

                RolesList rolesList = cargueInicial();
                return View(rolesList);
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
        public IActionResult CrearRol()
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

        public IActionResult Ayuda()
        {
            string? token = HttpContext.Session.GetString("token");

            if (token == null)
                return View("Views/Login/Index.cshtml");

            return View();
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
                throw;
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
            List<ReqRoles> r = new List<ReqRoles>();

            string token = HttpContext.Session.GetString("token") ?? "";

            string URI = UrlApi + "/Rol/Consult";

            var httpClient = getHttpClient();

            if (token == "")
            {
                HttpContext.Session.Remove("token");
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
                    r = JsonConvert.DeserializeObject<List<ReqRoles>>(respuesta.Response.ToString() ?? "") ?? new List<ReqRoles>();
                    HttpContext.Session.SetString("token", respuesta.Token);
                }
            }

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

            RolesList listado = new RolesList();

            listado.rolsList = r;

            return listado;
        }

        public List<ReqRoles> filtroRoles(string nombreRol, string valEstado)
        {
            try
            {
                _logger.LogInformation("method called");
                List<ReqRoles> r = new List<ReqRoles>();

                string token = HttpContext.Session.GetString("token") ?? "";

                string URI = UrlApi + "/Rol/Consult?searchQuery=" + nombreRol + "&state=" + valEstado;

                var httpClient = getHttpClient();

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
                        r = JsonConvert.DeserializeObject<List<ReqRoles>>(respuesta.Response.ToString() ?? "") ?? new List<ReqRoles>();
                        HttpContext.Session.SetString("token", respuesta.Token);

                        return r;
                    }

                    return r;
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
                    return "El usuario no cuenta con los permisos para Actualizar";
                }

                string r = "";
                string token = HttpContext.Session.GetString("token") ?? "";

                if (_datosAct.description == null)
                {
                    _datosAct.description = "";
                }

                if (token == "")
                {
                    HttpContext.Session.Remove("token");
                }
                else
                {
                    string URI = UrlApi + "/Rol/Update";
                    var httpClient = getHttpClient();
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var response = httpClient.PutAsJsonAsync(URI, _datosAct).Result;

                    string responseString = response.Content.ReadAsStringAsync().Result;
                    Responses resp = JsonConvert.DeserializeObject<Responses>(responseString) ?? new Responses();

                    r = resp.Message;
                    if (!resp.Error)
                    {
                        HttpContext.Session.SetString("token", resp.Token);
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
                    ViewBag.RespuestaCreacionRol = "El usuario no cuenta con los permisos para Crear Roles";
                    ViewBag.ErrorCreacion = "false";

                    return View("CrearRol");
                }

                string token = HttpContext.Session.GetString("token") ?? "";

                _datosCreacion.description = "";

                if (token == "")
                {
                    HttpContext.Session.Remove("token");
                }
                else
                {
                    string URL = UrlApi + "/Rol/Create";
                    var httpClient = getHttpClient();
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var response = httpClient.PostAsJsonAsync(URL, _datosCreacion).Result;

                    string responseString = response.Content.ReadAsStringAsync().Result;
                    Responses resp = JsonConvert.DeserializeObject<Responses>(responseString) ?? new Responses();

                    if (!resp.Error)
                    {
                        HttpContext.Session.SetString("token", resp.Token);
                    }
                    ViewBag.RespuestaCreacionRol = resp.Message;
                    ViewBag.ErrorCreacion = "false";
                }

                return View("CrearRol");
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
        [Produces("application/json")]
        [HttpGet]
        public List<string> BuscarRoles()
        {
            try
            {
                _logger.LogInformation("method called");
                List<string> valores = new List<string>();

                string term = HttpContext.Request.Query["term"].ToString();

                string token = HttpContext.Session.GetString("token") ?? "";

                string URI = UrlApi + "/Rol/ConsultAllRols?parameter=" + term;
                var httpClient = getHttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = httpClient.GetAsync(URI).Result;

                string responseString = response.Content.ReadAsStringAsync().Result;
                Responses respuesta = JsonConvert.DeserializeObject<Responses>(responseString) ?? new Responses();
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
                throw;
            }
        }
    }
}
