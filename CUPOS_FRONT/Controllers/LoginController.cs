using Microsoft.AspNetCore.Mvc;
using Web.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using System.Text;
using Repository.Helpers;
using Newtonsoft.Json;
using Repository.Helpers.Models;
using Newtonsoft.Json.Linq;
using System.Net;
using static CUPOS_FRONT.Models.Requests;

namespace Web.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILogger<LoginController> _logger;
        private readonly LoginRequest usuario;
        private readonly string UrlApi;
        private readonly string secretKey;
        private readonly string secretKeySite;
        readonly string RUTAAPI = Environment.GetEnvironmentVariable("RUTAAPI") ?? "";
        readonly string RECAPTCHAPRIVATEKEY = Environment.GetEnvironmentVariable("RECAPTCHAPRIVATEKEY") ?? "";
        readonly string RECAPTCHASITEKEY = Environment.GetEnvironmentVariable("RECAPTCHASITEKEY") ?? "";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="httpClientFactory"></param>
        public LoginController(ILogger<LoginController> logger)
        {
            UrlApi = string.IsNullOrEmpty(RUTAAPI) ? new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build().GetValue<string>("Variables:RutaApi") : RUTAAPI;
            secretKey = string.IsNullOrEmpty(RECAPTCHAPRIVATEKEY) ? new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build().GetValue<string>("Recaptcha:reCaptchaPrivateKey") : RECAPTCHAPRIVATEKEY;
            secretKeySite = string.IsNullOrEmpty(RECAPTCHASITEKEY) ? new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build().GetValue<string>("Recaptcha:reCaptchaSiteKey") : RECAPTCHASITEKEY;
            _logger = logger;

            if (usuario == null)
            {
                usuario = new LoginRequest();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public HttpClient GetHttpClient()
        {
            HttpClientHandler clientHandler = new();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            return new HttpClient(clientHandler);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult Index(string? enc = null)
        {
            try
            {
                _logger.LogInformation("method called");
                var url = "/GestionAsignacionDeRoles/AsignarRol?enc= " + enc;
                if (enc != null)
                    return Redirect(url);

                ViewBag.SecretKeySite = secretKeySite;
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
        public IActionResult CambioContrasena()
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
        public IActionResult CambioContrasenaOlvido()
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
        public IActionResult EnvioCorreos()
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
        /// <param name="_usuario"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Ingresar(LoginRequest _usuario)
        {
            try
            {
                _logger.LogInformation("method called");
                if (_usuario.user != null && _usuario.password != null)
                {
                    var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, _usuario.user),
                    new Claim("Usuario", _usuario.user),
                    new Claim(ClaimTypes.Role, "Administrador")
                };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                    LoginRequest datosIng = new()
                    {
                        user = _usuario.user,
                        password = _usuario.password
                    };

                    if (IsReCaptchValid())
                    {
                        string URI = UrlApi + "/User/Authenticate";
                        var httpClient = GetHttpClient();
                        var response = httpClient.PostAsJsonAsync(URI, datosIng).Result;

                        string responseString = response.Content.ReadAsStringAsync().Result;
                        Responses respuesta = JsonConvert.DeserializeObject<Responses>(responseString) ?? new Responses();

                        if (!respuesta.Error)
                        {
                            HttpContext.Session.SetString("token", respuesta.Token);
                            return RedirectToAction("Index", "Home", new { usuario = _usuario.user });
                        }
                        else
                        {
                            if (respuesta.Message.Contains(StringHelper.estadoValidarCorreo.ToString()))
                            {
                                string URI2 = UrlApi + "/User/ConsultTerms?login=" + datosIng.user;
                                var response2 = httpClient.GetAsync(URI2).Result;
                                string responseString2 = response2.Content.ReadAsStringAsync().Result;
                                Responses respuesta2 = JsonConvert.DeserializeObject<Responses>(responseString2) ?? new Responses();
                                ReqAceptarCondiciones req = JsonConvert.DeserializeObject<ReqAceptarCondiciones>(respuesta2.Response.ToString() ?? "") ?? new ReqAceptarCondiciones();

                                if (req.A012aceptaTratamientoDatosPersonales && req.A012aceptaTerminos)
                                {
                                    HttpContext.Session.SetString("User", datosIng.user);
                                    HttpContext.Session.SetString("Password", datosIng.password);
                                    return RedirectToAction("CambioContrasenaOlvido", "Login");
                                }
                                else
                                {
                                    HttpContext.Session.SetString("User", datosIng.user);
                                    HttpContext.Session.SetString("Password", datosIng.password);
                                    return RedirectToAction("CambioContrasena", "Login");
                                }
                            }
                            else
                            {
                                ViewBag.SecretKeySite = secretKeySite;
                                ViewBag.Alert = respuesta.Message;
                                return View("Index");
                            }
                        }
                    }
                    else
                    {
                        ViewBag.SecretKeySite = secretKeySite;
                        ViewBag.Alert = "Comprueba que no eres un robot.";
                        return View("Index");
                    }

                }
                else
                {
                    ViewBag.SecretKeySite = secretKeySite;
                    ViewBag.Alert = "Usuario o Contraseña Incorrecta";
                    return View("Index");
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
        public bool IsReCaptchValid()
        {
            try
            {
                _logger.LogInformation("method called");
                var result = false;
                var captchaResponse = Request.Form["g-recaptcha-response"];
                string? apiUrl = "https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}";
                var requestUrl = string.Format(apiUrl, secretKey, captchaResponse);
                var request = (HttpWebRequest)WebRequest.Create(requestUrl);

                using (WebResponse response = request.GetResponse())
                {
                    using StreamReader stream = new(response.GetResponseStream());
                    JObject jResponse = JObject.Parse(stream.ReadToEnd());
                    var isSuccess = jResponse.Value<bool>("success");
                    result = (isSuccess);
                }

                return result;
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
        /// <param name="_datosPass"></param>
        /// <returns></returns>
        public IActionResult Cambiar(LoginRequest _datosPass)
        {
            try
            {
                _logger.LogInformation("method called");
                string user = HttpContext.Session.GetString("User") ?? "";
                string pass = HttpContext.Session.GetString("Password") ?? "";

                _datosPass.user = user;
                _datosPass.password = pass;

                string URI = UrlApi + "/User/ChangePassword";
                var httpClient = GetHttpClient();
                var response = httpClient.PostAsJsonAsync(URI, _datosPass).Result;

                string responseString = response.Content.ReadAsStringAsync().Result;
                Responses respuesta = JsonConvert.DeserializeObject<Responses>(responseString) ?? new Responses();

                if (!respuesta.Error)
                {
                    ViewBag.ErrorCambio = "false";
                    ViewBag.RespuestaCambio = respuesta.Message;
                    return View("CambioContrasena");
                }
                else
                {
                    ViewBag.AlertCambioPass = respuesta.Message;
                    return View("CambioContrasena");
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
        /// <param name="_datosPass"></param>
        /// <returns></returns>
        public List<string> CambiarOlvido(LoginRequest _datosPass)
        {
            try
            {
                _logger.LogInformation("method called");
                List<string> r = new();

                string user = HttpContext.Session.GetString("User") ?? "";
                string pass = HttpContext.Session.GetString("Password") ?? "";

                _datosPass.user = user;
                _datosPass.password = pass;

                string URI = UrlApi + "/User/ChangePassword";
                var httpClient = GetHttpClient();
                var response = httpClient.PostAsJsonAsync(URI, _datosPass).Result;

                string responseString = response.Content.ReadAsStringAsync().Result;
                Responses respuesta = JsonConvert.DeserializeObject<Responses>(responseString) ?? new Responses();

                r.Add(respuesta.Message);
                r.Add(respuesta.Error.ToString());

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
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        public List<string> EnvioCorreo(string user)
        {
            try
            {
                _logger.LogInformation("method called");
                List<string> r = new();
                ReqSimpleUser usuarioRecu = new()
                {
                    user = user
                };

                if (user == null)
                {
                    r.Add("El campo no puede estar vacio, ingrese un usuario");
                }
                else
                {
                    string URI = UrlApi + "/User/SendEmailChangePassword";
                    var httpClient = GetHttpClient();
                    var response = httpClient.PostAsJsonAsync(URI, usuarioRecu).Result;

                    string responseString = response.Content.ReadAsStringAsync().Result;
                    Responses respuesta = JsonConvert.DeserializeObject<Responses>(responseString) ?? new Responses();

                    if (!respuesta.Error)
                    {
                        r.Add(respuesta.Message);
                        r.Add("false");
                        return r;
                    }
                    else
                    {
                        r.Add(respuesta.Message);
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
        /// <returns></returns>
        public async Task<IActionResult> CerrarSesion()
        {
            try
            {
                //Administrador
                HttpContext.Session.SetString("ConsultarRoles", "False");
                HttpContext.Session.SetString("ConsultarAsignacionRol", "False");
                HttpContext.Session.SetString("ConsultarEstados", "False");
                HttpContext.Session.SetString("ConsultarParametrica", "False");
                HttpContext.Session.SetString("ConsultarAuditoria", "False");
                HttpContext.Session.SetString("ConsultarDocumental", "False");
                HttpContext.Session.SetString("ConsultarTecnica", "False");
                HttpContext.Session.SetString("ConsultarUsuario", "False");
                HttpContext.Session.SetString("ConsultarFuncionalidades", "False");
                HttpContext.Session.SetString("ConsultarServicio", "False");
                //Flujo de Negocio
                HttpContext.Session.SetString("ResAsignacionCUPOS", "False");
                HttpContext.Session.SetString("DocumentosVenta", "False");
                HttpContext.Session.SetString("GestionarEntidad", "False");
                HttpContext.Session.SetString("HojaVidaEntidad", "False");
                HttpContext.Session.SetString("FloraNoMaderable", "False");
                HttpContext.Session.SetString("PresolucionesPecesNivelNacional", "False");
                HttpContext.Session.SetString("ResolucionPecesEntidad", "False");
                HttpContext.Session.SetString("PrecintosMarquillas", "False");
                HttpContext.Session.SetString("PrecintosNacionales", "False");
                HttpContext.Session.SetString("GestionPrecintosNacionalesAnalista", "False");
                HttpContext.Session.SetString("GestionPrecintosNacionalesDirector", "False");
                HttpContext.Session.SetString("ActaVisitaCortes", "False");
                HttpContext.Session.SetString("SolicitudesPermisosCITES", "False");
                HttpContext.Session.SetString("CoordinadorGPNSolicitudes", "False");
                //Reportes
                HttpContext.Session.SetString("consultReportes", "False");
                HttpContext.Session.SetString("consultReporteEmpresa", "False");
                HttpContext.Session.SetString("consultReportePrecintos", "False");
                HttpContext.Session.SetString("consultReportesMarquillas", "False");
                HttpContext.Session.SetString("consultReportesCupos", "False");

                _logger.LogInformation("method called");
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                return RedirectToAction("Index", "Login");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                throw;
            }
        }
    }
}
