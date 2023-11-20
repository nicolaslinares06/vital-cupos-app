using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Repository.Helpers;
using System.Net.Http.Headers;
using Web.Models;

namespace Web.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class AdministracionTecnicaController : Controller
    {
        private readonly ILogger<AdministracionTecnicaController> _logger;
        private readonly string UrlApi;
        readonly string RUTAAPI = Environment.GetEnvironmentVariable("RUTAAPI") ?? "";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        public AdministracionTecnicaController(ILogger<AdministracionTecnicaController> logger)
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

                return View(filtradoInicial());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                return StatusCode(500, "An error occurred.");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult AgregarValor()
        {
            try
            {
                return Vistas();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                return StatusCode(500, "An error occurred.");
            }
        }

        public IActionResult Ayuda()
        {
            try
            {
                return Vistas();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                return StatusCode(500, "An error occurred");
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
                return StatusCode(500, "An error occurred.");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        public Technical filtradoInicial()
        {
            try
            {
                _logger.LogInformation("method called");
                
                string URI = UrlApi + "/TechnicalAdmin/Consult";
                var r = Busquedas(URI);

                foreach (var tecnico in r)
                {
                    List<SelectListItem> resultadoListado = new List<SelectListItem>();

                    if (tecnico.a007estadoRegistro.ToUpper() == "ACTIVO")
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

                    tecnico.estate = resultadoListado;
                }

                Technical listado = new()
                {
                    technicalList = r
                };

                return listado;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                return new Technical();
            }
        }

        public List<ReqValorTecnica> filtroValor(string nombreValor)
        {
            try
            {
                _logger.LogInformation("method called");

                string URI = UrlApi + "/TechnicalAdmin/Consult?value=" + nombreValor;
                var r = Busquedas(URI);
                
                return r;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                return new List<ReqValorTecnica>();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_datosAct"></param>
        /// <returns></returns>
        [HttpPost]
        public string Actualizar(AdminTecnicaReq _datosAct)
        {
            try
            {
                _logger.LogInformation("method called");
                if (HttpContext.Session.GetString("ActualizarTecnica") != "True")
                {
                    return StringHelper.msgNoPermisoActualizar;
                }

                string r = "";
                string URI = UrlApi + "/TechnicalAdmin/Update";
                var req = new
                {
                    code = _datosAct.codigo,
                    name = _datosAct.nombre,
                    value = _datosAct.valor,
                    description = _datosAct.descripcion,
                    registrationStatus = _datosAct.estadoRegistro
                };

                var respuesta = Respuesta(URI, false, req);
                r = respuesta.Message;
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
        /// <param name="idEliminar"></param>
        /// <returns></returns>
        [HttpPost]
        public string Eliminar(ReqId idEliminar)
        {
            try
            {
                _logger.LogInformation("method called");
                if (HttpContext.Session.GetString("EliminarTecnica") != "True")
                {
                    return StringHelper.msgNoPermisoEliminar;
                }

                string r = "";
                string URI = UrlApi + "/TechnicalAdmin/Delete";
                var respuesta = Respuesta(URI, false, idEliminar);
                HttpContext.Session.SetString("token", respuesta.Token);

                r = respuesta.Message;
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
        public IActionResult Crear(AdminTecnicaReq _datosCreacion)
        {
            try
            {
                _logger.LogInformation("method called");
                if (HttpContext.Session.GetString("ActualizarTecnica") != "True")
                {
                    ViewBag.ErrorCreacion = "false";
                    ViewBag.RespuestaCreacionTabla = StringHelper.msgNoPermisoActualizar;
                    return View("AgregarValor");
                }

                string URI = UrlApi + "/TechnicalAdmin/Create";
                var req = new
                {
                    code = _datosCreacion.codigo,
                    name = _datosCreacion.nombre,
                    value = _datosCreacion.valor,
                    description = _datosCreacion.descripcion,
                    registrationStatus = _datosCreacion.estadoRegistro
                };
                var respuesta = Respuesta(URI, false, req);
                HttpContext.Session.SetString("token", respuesta.Token);

                if (!respuesta.Error)
                {
                    ViewBag.ErrorCreacion = "false";
                    ViewBag.RespuestaCreacionTabla = respuesta.Message;
                    return View("AgregarValor");
                }
                else
                {
                    ViewBag.RespuestaCreacionTabla = respuesta.Message;
                    return View("AgregarValor");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                return StatusCode(500, "An error occurred.");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Produces("application/json")]
        [HttpGet]
        public List<string> BuscarValoresTecnicos()
        {
            try
            {
                _logger.LogInformation("method called");
                List<string> valores = new();

                string term = HttpContext.Request.Query["term"].ToString();
                string URI = UrlApi + "/TechnicalAdmin/ConsultTechnicalValues?parameter=" + term;
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
            string? token = HttpContext.Session.GetString("token");

            if (token == null)
                return View("Views/Login/Index.cshtml");

            _logger.LogInformation("method called");
            return View();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private List<ReqValorTecnica> Busquedas(string URI)
        {
            _logger.LogInformation("method called");

            var respuesta = Respuesta(URI, true);
            List<ReqValorTecnica> r = JsonConvert.DeserializeObject<List<ReqValorTecnica>>(respuesta.Response.ToString() ?? "") ?? new List<ReqValorTecnica>();
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
