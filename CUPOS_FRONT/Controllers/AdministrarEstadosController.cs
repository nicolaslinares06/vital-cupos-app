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
    public class AdministrarEstadosController : Controller
    {
        private readonly ILogger<AdministrarEstadosController> _logger;
        private readonly string UrlApi;
        readonly string RUTAAPI = Environment.GetEnvironmentVariable("RUTAAPI") ?? "";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        public AdministrarEstadosController(ILogger<AdministrarEstadosController> logger)
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

                return View(actividadesSinFiltro());
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
        public IActionResult AgregarEstado()
        {
            try
            {
                return Vistas();
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
        /// <returns></returns>
        public IActionResult Ayuda()
        {
            try
            {
                return Vistas();
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
        /// <param name="nombreEstado"></param>
        /// <param name="estadoRegistro"></param>
        /// <returns></returns
        public Estados actividadesSinFiltro()
        {
            string URI = UrlApi + "/Estate/Consult";

            var r = Busquedas(URI);

            var estadosCertificado = ConsultarEstadosCertificado();

            foreach (var actividad in r)
            {
                List<SelectListItem> resultadoListado = new List<SelectListItem>();
                List<SelectListItem> resultadoEstadosCert = new List<SelectListItem>();
                int contador = 0;

                if (actividad.a008estadoRegistro.ToUpper() == "ACTIVO")
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

                foreach (var estadoCert in estadosCertificado.Select(x => x.nombre))
                {
                    if (estadoCert == actividad.a008codigoParametricaEstado)
                    {
                        var optionSelected = new SelectListItem(estadoCert, estadoCert, true);
                        resultadoEstadosCert.Insert(contador, optionSelected);
                    }
                    else
                    {
                        var option = new SelectListItem(estadoCert, estadoCert, false);
                        resultadoEstadosCert.Insert(contador, option);
                    }
                    contador++;
                }

                actividad.estate = resultadoListado;
                actividad.stage = resultadoEstadosCert;
            }

            Estados listado = new()
            {
                estateList = r
            };

            return listado;
        }

        public List<ReqEstados> filtroEstados(string nombreEstado, string estadoRegistro)
        {
            try
            {
                _logger.LogInformation("method called");

                string URI = UrlApi + "/Estate/Consult?name=" + nombreEstado + "&registrationStatus=" + estadoRegistro;

                return Busquedas(URI);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                return new List<ReqEstados>();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_datosAct"></param>
        /// <returns></returns>
        [HttpPost]
        public string ActualizarEstado(ReqEstado _datosAct)
        {
            try
            {
                _logger.LogInformation("method called");
                if (HttpContext.Session.GetString("ActualizarEstados") != "True")
                {
                    return StringHelper.msgNoPermisoActualizar;
                }

                string r = "";

                if (_datosAct.description == null)
                {
                    _datosAct.description = "";
                }
                
                string URI = UrlApi + "/Estate/Update";
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
        public IActionResult Crear(ReqAdminEstados _datosCreacion)
        {
            try
            {
                _logger.LogInformation("method called");

                string URI = UrlApi + "/Estate/Create";

                var req = new
                {
                    stage = _datosCreacion.etapa,
                    description = _datosCreacion.descripcion,
                    state = _datosCreacion.estado
                };
                var respuesta = Respuesta(URI, false, req);
                
                if(!respuesta.Error)
                    HttpContext.Session.SetString("token", respuesta.Token);

                ViewBag.ErrorCreacionEstado = "false";
                ViewBag.RespuestaCreacionEstado = respuesta.Message;
                return View("AgregarEstado");
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
        public List<ReqEstadoCertificado> ConsultarEstadosCertificado()
        {
            try
            {
                _logger.LogInformation("method called");

                string URI = UrlApi + "/Parametric/ConsultEstateCertificate";
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
        [Produces("application/json")]
        [HttpGet]
        public List<string> BuscarEstados()
        {
            try
            {
                _logger.LogInformation("method called");
                List<string> valores = new List<string>();

                string term = HttpContext.Request.Query["term"].ToString();

                string URI = UrlApi + "/Estate/ConsultEstates?parameter=" + term;
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
        private List<ReqEstados> Busquedas(string URI)
        {
            _logger.LogInformation("method called");

            var respuesta = Respuesta(URI, true);
            List<ReqEstados> r = JsonConvert.DeserializeObject<List<ReqEstados>>(respuesta.Response.ToString() ?? "") ?? new List<ReqEstados>();
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
