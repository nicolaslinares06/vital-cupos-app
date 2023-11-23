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

                return View(Filtrar(""));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                return StatusCode(500, "An error occurred");
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
                var usuarios = Busqueda(URI);

                return usuarios;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                return new List<GestionUsuario>();
            }
        }

        public ListaUsuarios Filtrar(string nombreBus)
        {
            string URI = UrlApi + "/User/Consult?stringSearch=" + nombreBus;
            var usuarios = Busqueda(URI);

            ListaUsuarios listado = new()
            {
                Usuarios = usuarios
            };

            var rols = ConsultarRoles();

            foreach(var usuario in usuarios)
            {
                ConsultarRoles(rols, usuario);
            }

            return listado;
        }

        private static void ConsultarRoles(List<ReqRoles> rols, GestionUsuario usuario)
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
                    var r = StringHelper.msgCrearUsuarios;

                    return r;
                }

                string respuesta = "";
                string URI = UrlApi + "/User/Create";
                var resp = Respuestas(URI, false, datosUsu);
                respuesta = resp.Message;

                return respuesta;
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
        /// <param name="pass"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult CambiarPass(ActualizarRequest pass)
        {
            try
            {
                _logger.LogInformation("method called");
                if (HttpContext.Session.GetString("ActualizarUsuario") != "True")
                {
                    ViewBag.RespuestaCambioPass = StringHelper.msgNoPermisoActualizar;
                    return View("CambiarContrasena");
                }

                pass.code = Int32.Parse(HttpContext.Session.GetString("idEdicion") ?? "");
                ViewBag.AlertPass = null;
                pass.registrationStatus = true;
                
                if (pass.password == pass.dependence)
                {
                    pass.dependence = null;
                    string URI = UrlApi + "/User/Update";
                    var resp = Respuestas(URI, false, pass);

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

                return View("CambiarContrasena");
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
        [HttpPost]
        public List<ReqDocumentType> ConsultarDocumentos()
        {
            try
            {
                _logger.LogInformation("method called");
                string URI = UrlApi + "/Parametric/ConsultDocumentType";
                var respuesta = Respuestas(URI, true);
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
                    ViewBag.msjEditarUsuario = StringHelper.msgNoPermisoActualizar;
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
                
                string URI = UrlApi + "/User/Update";
                var resp = Respuestas(URI, false, _users);

                ViewBag.msjEditarUsuario = resp.Message;
                if (!resp.Error)
                {
                    HttpContext.Session.SetString("token", resp.Token);
                }
                else
                {
                    ViewBag.AlertaPass = "false";
                    ViewBag.RespuestaCambioPass = "Sesion caducada";
                }
                
                _users.documentType = ObtenerTiposDocumentos();
                _users.dependenceType = ObtenerDependencia();
                return View("EditarUsuario", _users);
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
        [HttpPost]
        public List<ReqRoles> ConsultarRoles()
        {
            try
            {
                _logger.LogInformation("method called");
                string URI = UrlApi + "/Rol/ConsultRols";
                var respuesta = Respuestas(URI, true);
                List<ReqRoles> docs = JsonConvert.DeserializeObject<List<ReqRoles>>(respuesta.Response.ToString() ?? "") ?? new List<ReqRoles>();

                return docs;
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
                var respuesta = Respuestas(URI, true);
                r = JsonConvert.DeserializeObject<ReqUser>(respuesta.Response.ToString() ?? "") ?? new ReqUser();     

                return r;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                return new ReqUser();
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
                string URI = UrlApi + "/Parametric/ConsultDocumentType";
                var respuesta = Respuestas(URI, true);
                List<ReqDocumentType> docs = JsonConvert.DeserializeObject<List<ReqDocumentType>>(respuesta.Response.ToString() ?? "") ?? new List<ReqDocumentType>();
                var resultadoListado = docs.Select(x => new SelectListItem(x.type, x.id.ToString())).ToList();

                var opcionPorDefecto = new SelectListItem("Seleccione un Tipo de Documento", "0", true);
                resultadoListado.Insert(0, opcionPorDefecto);

                return resultadoListado;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                return new List<SelectListItem>();
            }
        }

        private List<SelectListItem> ObtenerDependencia()
        {
            string URI = UrlApi + "/Parametric/ConsultDependence";
            var respuesta = Respuestas(URI, true);
            List<ReqEstadoCertificado> docs = JsonConvert.DeserializeObject<List<ReqEstadoCertificado>>(respuesta.Response.ToString() ?? "") ?? new List<ReqEstadoCertificado>();
            var resultadoListado = docs.Select(x => new SelectListItem(x.nombre, x.nombre.ToString())).ToList();

            var opcionPorDefecto = new SelectListItem("Seleccione una dependencia", "0", true);
            resultadoListado.Insert(0, opcionPorDefecto);

            return resultadoListado;
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
        private List<GestionUsuario> Busqueda(string URI)
        {
            _logger.LogInformation("method called");

            var respuesta = Respuestas(URI, true);
            List<GestionUsuario> r = JsonConvert.DeserializeObject<List<GestionUsuario>>(respuesta.Response.ToString() ?? "") ?? new List<GestionUsuario>();
            HttpContext.Session.SetString("token", respuesta.Token);

            return r;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private Responses Respuestas(string URI, bool get, Object data = null)
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
