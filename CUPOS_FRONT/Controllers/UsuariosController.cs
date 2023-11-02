using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Security.Claims;
using Web.Models;
using Repository.Helpers;
using Repository.Helpers.Models;
using Microsoft.AspNetCore.Authorization;
using static CUPOS_FRONT.Models.Requests;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Web.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class UsuariosController : Controller
    {
        private readonly ILogger<UsuariosController> _logger;
        private readonly string UrlApi;
        readonly string RUTAAPI = Environment.GetEnvironmentVariable("RUTAAPI") ?? "";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        public UsuariosController(ILogger<UsuariosController> logger)
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
            HttpClientHandler clientHandler = new()
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
            };
            return new HttpClient(clientHandler);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult GestionUsuarios()
        {
            try
            {
                _logger.LogInformation("method called");
                string? token = HttpContext.Session.GetString("token");

                if (token == null)
                    return View("Views/Login/Index.cshtml");

                ListaUsuarios listado = Filtrar("");
                return View(listado);
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
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult CambiarContrasena(int id)
        {
            try
            {
                _logger.LogInformation("method called");
                string? token = HttpContext.Session.GetString("token");

                if (token == null)
                    return View("Views/Login/Index.cshtml");

                HttpContext.Session.SetString("idEdicion", id.ToString());
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
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult EditarUsuario(int id)
        {
            try
            {
                _logger.LogInformation("method called");
                string? token = HttpContext.Session.GetString("token");

                if (token == null)
                    return View("Views/Login/Index.cshtml");

                HttpContext.Session.SetString("idEdicionUsuario", id.ToString());
                ReqUser model = ConsultarEdit();
                model.documentType = ObtenerTiposDocumentos();
                model.dependenceType = ObtenerDependencia();
                return View(model);
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
        public IActionResult ReturnAdministracion()
        {
            return RedirectToAction("Index", "Home");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="nombreBus"></param>
        /// <returns></returns>
        [HttpPost]
        public List<GestionUsuario> FiltrarUsuarios(string nombre)
        {
            try
            {
                _logger.LogInformation("method called");
                string URI = UrlApi + "/User/Consult?stringSearch=" + nombre;
                var httpClient = GetHttpClient();
                string token = HttpContext.Session.GetString("token") ?? "";


                List<GestionUsuario> usuarios = new();
                if (token == "")
                {
                    HttpContext.Session.Remove("token");
                    return usuarios;
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
                        usuarios = JsonConvert.DeserializeObject<List<GestionUsuario>>(respuesta.Response.ToString() ?? "") ?? new List<GestionUsuario>();
                        HttpContext.Session.SetString("token", respuesta.Token);

                        return usuarios;
                    }

                    return usuarios;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                throw;
            }
        }

        public ListaUsuarios Filtrar(string nombreBus)
        {
            string URI = UrlApi + "/User/Consult?stringSearch=" + nombreBus;
            var httpClient = GetHttpClient();
            string token = HttpContext.Session.GetString("token") ?? "";

            List<GestionUsuario> usuarios = new();
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
                    usuarios = JsonConvert.DeserializeObject<List<GestionUsuario>>(respuesta.Response.ToString() ?? "") ?? new List<GestionUsuario>();
                    HttpContext.Session.SetString("token", respuesta.Token);
                }
            }

            ListaUsuarios listado = new()
            {
                Usuarios = usuarios
            };

            var rols = ConsultarRoles();

            foreach(var usuario in usuarios)
            {
                if (usuario.roles != null)
                {
                    var roles = usuario.roles.Split("|");
                    var stringRoles = "";
                    var cantidad = 0;
                    rols.ForEach(el =>
                    {
                        if (roles.Contains(el.id.ToString()))
                        {
                            if (cantidad > 0)
                            {
                                stringRoles += ", ";
                            }
                            stringRoles += el.name;
                            cantidad += 1;
                        }

                    });
                    usuario.roles = stringRoles;
                }
                else
                    usuario.roles = "";
            }

            return listado;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="datosUsu"></param>
        /// <returns></returns>
        [HttpPost]
        public string Crear(ReqUser datosUsu)
        {
            try
            {
                _logger.LogInformation("method called");
                if (HttpContext.Session.GetString("CrearUsuario") != "True")
                {
                    var resp = "El usuario no cuenta con los permisos para Crear Usuarios";

                    return resp;
                }

                string token = HttpContext.Session.GetString("token") ?? "";
                string respuesta = "";

                if (token == "")
                {
                    HttpContext.Session.Remove("token");
                    respuesta = "sin token";
                    return respuesta;
                }
                else
                {
                    string URI = UrlApi + "/User/Create";
                    var httpClient = GetHttpClient();
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var response = httpClient.PostAsJsonAsync(URI, datosUsu).Result;

                    string responseString = response.Content.ReadAsStringAsync().Result;
                    Responses resp = JsonConvert.DeserializeObject<Responses>(responseString) ?? new Responses();

                    if (response.IsSuccessStatusCode)
                    {
                        HttpContext.Session.SetString("token", resp.Token);
                        respuesta = "exito";
                    }
                }
                return respuesta;
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
        /// <param name="pass"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult CambiarPass(actualizarRequest pass)
        {
            try
            {
                _logger.LogInformation("method called");
                if (HttpContext.Session.GetString("ActualizarUsuario") != "True")
                {
                    ViewBag.RespuestaCambioPass = "El usuario no cuenta con los permisos para Actualizar usuarios";
                    return View("CambiarContrasena");
                }

                string token = HttpContext.Session.GetString("token") ?? "";
                pass.code = Int32.Parse(HttpContext.Session.GetString("idEdicion") ?? "");
                ViewBag.AlertPass = null;
                pass.registrationStatus = true;

                if (token == "")
                {
                    HttpContext.Session.Remove("token");
                    ViewBag.AlertaPass = "false";
                    ViewBag.RespuestaCambioPass = "Sesion caducada";
                    return View("CambiarContrasena");
                }
                else
                {
                    if (pass.password == pass.dependence)
                    {
                        pass.dependence = null;
                        string URI = UrlApi + "/User/Update";
                        var httpClient = GetHttpClient();
                        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                        var response = httpClient.PutAsJsonAsync(URI, pass).Result;

                        string responseString = response.Content.ReadAsStringAsync().Result;
                        Responses resp = JsonConvert.DeserializeObject<Responses>(responseString) ?? new Responses();

                        ViewBag.RespuestaCambioPass = resp.Message;
                        if (!resp.Error)
                        {
                            ViewBag.AlertaPass = "false";
                            HttpContext.Session.SetString("token", resp.Token);
                        }
                    }
                    else
                    {
                        @ViewBag.AlertaPass = "Las contraseñas deben coincidir.";
                    }

                }
                return View("CambiarContrasena");
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
        /// <param name="_users"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Actualizar(ReqUser _users)
        {
            try
            {
                _logger.LogInformation("method called");
                ViewBag.mostrarMensajeEditar = "True";
                if (HttpContext.Session.GetString("ActualizarUsuario") != "True")
                {
                    ViewBag.msjEditarUsuario = "El usuario no cuenta con los permisos para Actualizar usuarios";
                    return View("EditarUsuario");
                }

                if (_users.estate == "ACTIVO")
                {
                    _users.registrationStatus = true;
                }
                else
                {
                    _users.registrationStatus = false;
                }

                string token = HttpContext.Session.GetString("token") ?? "";

                if (token == "")
                {
                    HttpContext.Session.Remove("token");
                    ViewBag.AlertaPass = "false";
                    ViewBag.RespuestaCambioPass = "Sesion caducada";
                }
                else
                {
                    string URI = UrlApi + "/User/Update";
                    var httpClient = GetHttpClient();
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var response = httpClient.PutAsJsonAsync(URI, _users).Result;

                    string responseString = response.Content.ReadAsStringAsync().Result;
                    Responses resp = JsonConvert.DeserializeObject<Responses>(responseString) ?? new Responses();

                    ViewBag.msjEditarUsuario = resp.Message;
                    if (!resp.Error)
                    {
                        HttpContext.Session.SetString("token", resp.Token);
                    }
                }
                _users.documentType = ObtenerTiposDocumentos();
                _users.dependenceType = ObtenerDependencia();
                return View("EditarUsuario", _users);
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
        public List<ReqRoles> ConsultarRoles()
        {
            try
            {
                _logger.LogInformation("method called");
                string token = HttpContext.Session.GetString("token") ?? "";

                string URI = UrlApi + "/Rol/ConsultRols";
                var httpClient = GetHttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = httpClient.GetAsync(URI).Result;

                string responseString = response.Content.ReadAsStringAsync().Result;
                Responses respuesta = JsonConvert.DeserializeObject<Responses>(responseString) ?? new Responses();
                List<ReqRoles> docs = JsonConvert.DeserializeObject<List<ReqRoles>>(respuesta.Response.ToString() ?? "") ?? new List<ReqRoles>();


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
        [HttpPost]
        public ReqUser ConsultarEdit()
        {
            try
            {
                _logger.LogInformation("method called");
                decimal id = Int32.Parse(HttpContext.Session.GetString("idEdicionUsuario") ?? "");

                ReqUser r = new();
                string URI = UrlApi + "/User/ConsultEdit?id=" + id;
                var httpClient = GetHttpClient();
                string token = HttpContext.Session.GetString("token") ?? "";
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
                        r = JsonConvert.DeserializeObject<ReqUser>(respuesta.Response.ToString() ?? "") ?? new ReqUser();
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
        private List<SelectListItem> ObtenerTiposDocumentos()
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

                var resultadoListado = docs.Select(x => new SelectListItem(x.type, x.id.ToString())).ToList();

                var opcionPorDefecto = new SelectListItem("Seleccione un Tipo de Documento", "0", true);
                resultadoListado.Insert(0, opcionPorDefecto);

                return resultadoListado;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                throw;
            }
        }

        private List<SelectListItem> ObtenerDependencia()
        {
            string token = HttpContext.Session.GetString("token") ?? "";

            string URI = UrlApi + "/Parametric/ConsultDependence";
            var httpClient = GetHttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = httpClient.GetAsync(URI).Result;

            string responseString = response.Content.ReadAsStringAsync().Result;
            Responses respuesta = JsonConvert.DeserializeObject<Responses>(responseString) ?? new Responses();
            List<ReqEstadoCertificado> docs = JsonConvert.DeserializeObject<List<ReqEstadoCertificado>>(respuesta.Response.ToString() ?? "") ?? new List<ReqEstadoCertificado>();

            var resultadoListado = docs.Select(x => new SelectListItem(x.nombre, x.codigo.ToString())).ToList();

            var opcionPorDefecto = new SelectListItem("Seleccione una dependencia", "0", true);
            resultadoListado.Insert(0, opcionPorDefecto);

            return resultadoListado;
        }
    }
}
