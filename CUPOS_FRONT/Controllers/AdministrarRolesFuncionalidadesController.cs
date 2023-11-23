using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Repository.Helpers;
using Repository.Helpers.Models;
using System.Net.Http.Headers;
using static CUPOS_FRONT.Models.Requests;

namespace Web.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class AdministrarRolesFuncionalidadesController : Controller
    {
        private readonly ILogger<AdministrarRolesFuncionalidadesController> _logger;
        private readonly string UrlApi;
        readonly string RUTAAPI = Environment.GetEnvironmentVariable("RUTAAPI") ?? "";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        public AdministrarRolesFuncionalidadesController(ILogger<AdministrarRolesFuncionalidadesController> logger)
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
        /// <param name="rol"></param>
        /// <param name="cargo"></param>
        /// <param name="estado"></param>
        /// <param name="idRol"></param>
        /// <returns></returns>
        public IActionResult Index(string rol, string cargo, string estado, decimal idRol)
        {
            try
            {
                _logger.LogInformation("method called");

                ViewBag.nombreRolFuncionalidades = rol;
                ViewBag.idRolFuncionalidades = idRol;
                ViewBag.cargoFuncionalidades = cargo;
                ViewBag.estadoFuncionalidades = estado;
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
            return Vistas();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="codRol"></param>
        /// <param name="nomCargo"></param>
        /// <returns></returns>
        public List<ReqModulos> consultarModulos(decimal codRol, string nomCargo)
        {
            try
            {
                _logger.LogInformation("method called");
                string URI = UrlApi + "/Rol/ConsultByRol?rol=" + codRol + "&charge=" + nomCargo;
                var respuesta = Response(URI, true);

                if (respuesta.Response == null)
                {
                    return new List<ReqModulos>();
                }

                List<ReqModulos> modulos = JsonConvert.DeserializeObject<List<ReqModulos>>(respuesta.Response.ToString() ?? "") ?? new List<ReqModulos>();

                return modulos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                return new List<ReqModulos>();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="codRol"></param>
        /// <param name="nomCargo"></param>
        /// <returns></returns>
        public List<ModulosReq> consultar(decimal codRol, string nomCargo)
        {
            try
            {
                _logger.LogInformation("method called");
                string URI = UrlApi + "/Module/Consult";
                var respuesta = Response(URI, true);

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
                return new List<ModulosReq>();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_datos"></param>
        /// <returns></returns>
        [HttpPost]
        public string actualizar(RolModPermition _datos)
        {
            try
            {
                _logger.LogInformation("method called");
                if (HttpContext.Session.GetString("ActualizarFuncionalidades") != "True")
                {
                    return StringHelper.msgNoPermisoActualizar;
                }
                string r = "";
                string URI = UrlApi + "/Rol/UpdateFunctionality";
                var resp = Response(URI, false, _datos);

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
        private Responses Response(string URI, bool get, Object data = null)
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
