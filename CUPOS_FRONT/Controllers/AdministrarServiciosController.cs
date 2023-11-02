using CUPOS_FRONT.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CUPOS_FRONT.Controllers
{
    public class AdministrarServiciosController : Controller
    {
        private readonly ILogger<AdministrarServiciosController> _logger;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        public AdministrarServiciosController(ILogger<AdministrarServiciosController> logger)
        {
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
        public IActionResult Index()
        {
            try
            {
                _logger.LogInformation("method called");
                string token = HttpContext.Session.GetString("token") ?? "";

                if (token == "")
                    return View("Views/AdministrarServicios/Index.cshtml");

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
        public IActionResult DetalleServicio(string name)
        {
            try
            {
                _logger.LogInformation("method called");
                string token = HttpContext.Session.GetString("token") ?? "";

                if (token != "") { 
                    ViewBag.name = name;
                    return View("Views/AdministrarServicios/Partials/DetalleServicio.cshtml");
                }

                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                throw;
            }
        }
        /// <summary>
        /// Evalúa las URL en un arreglo y cambia el estado dependiendo del resultado.
        /// </summary>
        /// <param name="servicios">Arreglo que contiene el nombre del servicio, la URL y el estado.</param>
        /// <returns>Arreglo con los cambios en los estados.</returns>
        public List<Servicio> EvaluarServicios(List<Servicio> servicios)
        {
            try
            {
                foreach (var servicio in servicios)
                {
                    bool tieneConexion = VerificarConexion(servicio.Url);
                    servicio.Estado = tieneConexion ? "Conexión establecida" : "Sin conexión";
                }

                return servicios;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                throw;
            }
        }

        /// <summary>
        /// Verifica si se puede establecer conexión con una URL específica.
        /// </summary>
        /// <param name="url">URL a verificar.</param>
        /// <returns>True si la conexión es exitosa, False de lo contrario.</returns>
        private bool VerificarConexion(string url)
        {
            try
            {
                using var httpClient = new HttpClient();
                var response = httpClient.GetAsync(url).Result;
                if (response.IsSuccessStatusCode || response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

    }
}
