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
                string? token = HttpContext.Session.GetString("token");

                if (token == null)
                    return View("Views/Login/Index.cshtml");

                ViewBag.nombreRolFuncionalidades = rol;
                ViewBag.idRolFuncionalidades = idRol;
                ViewBag.cargoFuncionalidades = cargo;
                ViewBag.estadoFuncionalidades = estado;
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
                string token = HttpContext.Session.GetString("token") ?? "";

                string URI = UrlApi + "/Rol/ConsultByRol?rol=" + codRol + "&charge=" + nomCargo;
                var httpClient = getHttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = httpClient.GetAsync(URI).Result;

                string responseString = response.Content.ReadAsStringAsync().Result;
                Responses respuesta = JsonConvert.DeserializeObject<Responses>(responseString) ?? new Responses();

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
                throw;
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
                    return "El usuario no cuenta con los permisos para Actualizar funcionalidades";
                }
                string r = "";
                string token = HttpContext.Session.GetString("token") ?? "";

                if (token == "")
                {
                    HttpContext.Session.Remove("token");
                }
                else
                {
                    string URI = UrlApi + "/Rol/UpdateFunctionality";
                    var httpClient = getHttpClient();
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var response = httpClient.PutAsJsonAsync(URI, _datos).Result;

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
    }
}
