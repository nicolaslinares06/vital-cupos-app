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
        readonly string RUTAAPI = Environment.GetEnvironmentVariable("RUTAAPI");
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
        public IActionResult AsignarRol(string enc = null)
        {
            try
            {
                _logger.LogInformation("method called");

                enc = "eyJjb2RlIjoiMDUiLCJzdGF0dXMiOiJTdWNjZXNzIiwicGVybWlzc2lvbnMiOiJDSVRFUyIsIm1lc3NhZ2UiOm51bGwsIklEIjo2MjQ3NCwiVXNlciI6Ijc4Nzg3ODc4NzgiLCJOYW1lIjoiTWFkZXJhcyAgUHJ1ZWJhcyIsIkRvY3VtZW50IjoiNzg3ODc4Nzg3OCIsIkVNYWlsIjoiYWlwYXJyYUBtaW5hbWJpZW50ZS5nb3YuY28iLCJMYXN0TG9naW4iOiIyMDIzLTA4LTA0VDE1OjIwOjUyLjczNzAwMCIsIkFjdGl2ZSI6IlQiLCJFbmFibGVkIjoiVCIsIk1vZHVsZSI6IkNJVEVTIiwiVXJsIjoiaHR0cDovL3Rlc3Qtc3VubC1hcGkubWluYW1iaWVudGUuZ292LmNvL2FwaS8iLCJUb2tlbiI6IjlkNzczNDBjLWQzMDEtNGI4Ni1hMjhmLTkyMGFjODEzNzQxYyIsIlVybEVycm9yIjoiaHR0cDovL3ZpdGFsLm1pbmFtYmllbnRlLmdvdi5jby8ifQ==";
                var datos = System.Text.Encoding.UTF8.GetString(System.Convert.FromBase64String(enc));

                VitalReq datosVital = new VitalReq();
                datosVital = JsonConvert.DeserializeObject<VitalReq>(datos.ToString());
                datosVital.rols = ConsultarRoles();

                //Verificar si ya se encuentra en el sistema
                string URI = UrlApi + "/AssignmentRequest/VerifyDocument?document=" + datosVital.Document;
                var httpClient = getHttpClient();

                var response = httpClient.GetAsync(URI).Result;
                if (response.IsSuccessStatusCode)
                {
                    string responseString = response.Content.ReadAsStringAsync().Result;
                    Responses respuesta = JsonConvert.DeserializeObject<Responses>(responseString);

                    if(respuesta != null)
                    {
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
                List<ReqGesAsigRoles> r = new List<ReqGesAsigRoles>();

                string token = HttpContext.Session.GetString("token");

                string URI = UrlApi + "/AssignmentRequest/Consult?user=" + nombre;
                var httpClient = getHttpClient();

                if (token == null)
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
                        Responses respuesta = JsonConvert.DeserializeObject<Responses>(jsonInput);
                        r = JsonConvert.DeserializeObject<List<ReqGesAsigRoles>>(respuesta.Response.ToString());
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

                List<string> r = new List<string>();


                if (HttpContext.Session.GetString("ActualizarAsignacionRol") != "True")
                {
                    r.Add("El usuario no cuenta con los permisos para Actualizar el registro");
                    return r;
                }

                string token = HttpContext.Session.GetString("token");

                string URI = UrlApi + "/AssignmentRequest/UpdateStatus";
                var httpClient = getHttpClient();

                if (token == null)
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
                        Responses respuesta = JsonConvert.DeserializeObject<Responses>(jsonInput);

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

                string token = HttpContext.Session.GetString("token");

                string URI = UrlApi + "/Rol/ConsultRolsAssign";
                var httpClient = getHttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = httpClient.GetAsync(URI).Result;

                string responseString = response.Content.ReadAsStringAsync().Result;
                Responses respuesta = JsonConvert.DeserializeObject<Responses>(responseString);
                List<ReqRoles> docs = JsonConvert.DeserializeObject<List<ReqRoles>>(respuesta.Response.ToString());

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

                AssignRol datos = new AssignRol();
                datos.code = asignacion.code;
                datos.status = asignacion.status;
                datos.permissions = asignacion.permissions;
                datos.message = asignacion.message;
                datos.id = asignacion.ID;
                datos.user = asignacion.User;
                datos.name = asignacion.Name;
                datos.document = asignacion.Document;
                datos.eMail = asignacion.EMail;
                datos.lastLogin = asignacion.LastLogin;
                datos.active = asignacion.Active;
                datos.enabled = asignacion.Enabled;
                datos.module = asignacion.Module;
                datos.url = asignacion.Url;
                datos.token = asignacion.Token;
                datos.urlError = asignacion.UrlError;
                datos.rol = (int)asignacion.rol;

                string URI = UrlApi + "/AssignmentRequest/Assign";
                var httpClient = getHttpClient();

                var response = httpClient.PostAsJsonAsync(URI, datos).Result;

                if (response.IsSuccessStatusCode)
                {
                    string responseString = response.Content.ReadAsStringAsync().Result;
                    Responses resp = JsonConvert.DeserializeObject<Responses>(responseString);

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
