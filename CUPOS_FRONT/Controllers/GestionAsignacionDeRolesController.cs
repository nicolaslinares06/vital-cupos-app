using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Repository.Helpers;
using System.Net.Http.Headers;
using Web.Models;
using static CUPOS_FRONT.Models.Requests;

namespace Web.Controllers
{
    //[Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class GestionAsignacionDeRolesController : Controller
    {
        private readonly ILogger<GestionAsignacionDeRolesController> _logger;
        private readonly string UrlApi;
        readonly string RUTAAPI = Environment.GetEnvironmentVariable("RUTAAPI") ?? "";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        public GestionAsignacionDeRolesController(ILogger<GestionAsignacionDeRolesController> logger)
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
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public IActionResult Index()
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
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
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
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public IActionResult Ayuda()
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
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <returns></returns>
        public IActionResult AsignarRol(string enc)
        {
            try
            {
                _logger.LogInformation("method called");

                enc = "eyJjb2RlIjoiMDUiLCJzdGF0dXMiOiJTdWNjZXNzIiwicGVybWlzc2lvbnMiOiJDSVRFUyIsIm1lc3NhZ2UiOm51bGwsIklEIjo2MjQ3NCwiVXNlciI6Ijc4Nzg3ODc4NzgiLCJOYW1lIjoiTWFkZXJhcyAgUHJ1ZWJhcyIsIkRvY3VtZW50IjoiNzg3ODc4Nzg3OCIsIkVNYWlsIjoiYWlwYXJyYUBtaW5hbWJpZW50ZS5nb3YuY28iLCJMYXN0TG9naW4iOiIyMDIzLTA4LTA0VDE1OjIwOjUyLjczNzAwMCIsIkFjdGl2ZSI6IlQiLCJFbmFibGVkIjoiVCIsIk1vZHVsZSI6IkNJVEVTIiwiVXJsIjoiaHR0cDovL3Rlc3Qtc3VubC1hcGkubWluYW1iaWVudGUuZ292LmNvL2FwaS8iLCJUb2tlbiI6IjlkNzczNDBjLWQzMDEtNGI4Ni1hMjhmLTkyMGFjODEzNzQxYyIsIlVybEVycm9yIjoiaHR0cDovL3ZpdGFsLm1pbmFtYmllbnRlLmdvdi5jby8ifQ==";
                var datos = System.Text.Encoding.UTF8.GetString(System.Convert.FromBase64String(enc));

                VitalReq datosVital = new();
                datosVital = JsonConvert.DeserializeObject<VitalReq>(datos.ToString() ?? "") ?? new VitalReq();
                datosVital.rols = ConsultarRoles();

                //Verificar si ya se encuentra en el sistema
                string URI = UrlApi + "/AssignmentRequest/VerifyDocument?document=" + datosVital.Document;
                var httpClient = GetHttpClient();

                var response = httpClient.GetAsync(URI).Result;
                if (response.IsSuccessStatusCode)
                {
                    string responseString = response.Content.ReadAsStringAsync().Result;
                    Responses respuesta = JsonConvert.DeserializeObject<Responses>(responseString) ?? new Responses();

                    if (respuesta.Error)
                    {
                        if (respuesta.Message.Contains("activa"))
                        {
                            ViewBag.mostrarModalAlerta = "false";
                            ViewBag.mostrarModalCorreo = "true";
                            ViewBag.mensajeModal = respuesta.Message;
                        }
                        else
                        {
                            ViewBag.mostrarModalAlerta = "true";
                            ViewBag.mensajeModal = respuesta.Message;
                        }
                    }
                }

                return View(datosVital);

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
        /// <param name="nombre"></param>
        /// <returns></returns>
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public List<ReqGesAsigRoles> filtroValor(string nombre)
        {
            try
            {
                _logger.LogInformation("method called");
                List<ReqGesAsigRoles> r = new();

                string token = HttpContext.Session.GetString("token") ?? "";

                string URI = UrlApi + "/AssignmentRequest/Consult?user=" + nombre;
                var httpClient = GetHttpClient();

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
                        r = JsonConvert.DeserializeObject<List<ReqGesAsigRoles>>(respuesta.Response.ToString() ?? "") ?? new List<ReqGesAsigRoles>();
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
        /// <param name="_solicitud"></param>
        /// <returns></returns>
        [HttpPost]
        public List<string> Actualizar(AssignmentUpdateReq _solicitud)
        {
            try
            {
                _logger.LogInformation("method called");

                List<string> r = new();


                if (HttpContext.Session.GetString("ActualizarAsignacionRol") != "True")
                {
                    r.Add("El usuario no cuenta con los permisos para Actualizar el registro");
                    return r;
                }

                string token = HttpContext.Session.GetString("token") ?? "";

                string URI = UrlApi + "/AssignmentRequest/UpdateStatus";
                var httpClient = GetHttpClient();

                if (token == "")
                {
                    HttpContext.Session.Remove("token");
                    return r;
                }
                else
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var response = httpClient.PutAsJsonAsync(URI, _solicitud).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        string responseString = response.Content.ReadAsStringAsync().Result;
                        string jsonInput = responseString;
                        Responses respuesta = JsonConvert.DeserializeObject<Responses>(jsonInput) ?? new Responses();

                        r.Add(respuesta.Message);
                        r.Add(respuesta.Error.ToString());

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
        /// <returns></returns>
        public List<SelectListItem> ConsultarRoles()
        {
            try
            {
                _logger.LogInformation("method called");

                string token = HttpContext.Session.GetString("token") ?? "";

                string URI = UrlApi + "/Rol/ConsultRolsAssign";
                var httpClient = GetHttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = httpClient.GetAsync(URI).Result;

                string responseString = response.Content.ReadAsStringAsync().Result;
                Responses respuesta = JsonConvert.DeserializeObject<Responses>(responseString) ?? new Responses();
                List<ReqRoles> docs = JsonConvert.DeserializeObject<List<ReqRoles>>(respuesta.Response.ToString() ?? "") ?? new List<ReqRoles>();

                var resultadoListado = docs.Select(x => new SelectListItem(x.name, x.id.ToString())).ToList();

                var opcionPorDefecto = new SelectListItem("Seleccione El Rol Solicitado", "0", true);
                resultadoListado.Insert(0, opcionPorDefecto);

                return resultadoListado;

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
        /// <param name="asignacion"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult SolicitarRol(VitalReq asignacion)
        {
            try
            {
                _logger.LogInformation("method called");

                AssignRol datos = new()
                {
                    code = asignacion.code,
                    status = asignacion.status,
                    permissions = asignacion.permissions,
                    message = asignacion.message,
                    id = asignacion.ID,
                    user = asignacion.User,
                    name = asignacion.Name,
                    document = asignacion.Document,
                    eMail = asignacion.EMail,
                    lastLogin = asignacion.LastLogin,
                    active = asignacion.Active,
                    enabled = asignacion.Enabled,
                    module = asignacion.Module,
                    url = asignacion.Url,
                    token = asignacion.Token,
                    urlError = asignacion.UrlError,
                    rol = (int)asignacion.rol
                };

                string URI = UrlApi + "/AssignmentRequest/Assign";
                var httpClient = GetHttpClient();

                var response = httpClient.PostAsJsonAsync(URI, datos).Result;

                if (response.IsSuccessStatusCode)
                {
                    string responseString = response.Content.ReadAsStringAsync().Result;
                    Responses resp = JsonConvert.DeserializeObject<Responses>(responseString) ?? new Responses();

                    if (resp.Error)
                    {
                        ViewBag.mostrarModal = "true";
                        ViewBag.mensajeSolicitud = resp.Message;
                        asignacion.rols = ConsultarRoles();
                        return View("AsignarRol", asignacion);
                    }
                    ViewBag.mostrarModal = "true";
                    ViewBag.mensajeSolicitud = resp.Message;
                    asignacion.rols = ConsultarRoles();
                    return View("AsignarRol", asignacion);
                }

                asignacion.rols = ConsultarRoles();
                return View("AsignarRol", asignacion);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                throw;
            }
        }
    }
}
