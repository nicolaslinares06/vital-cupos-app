using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace CUPOS_FRONT.Controllers
{
    public class ValidateApiController : Controller
    {
        private readonly string UrlApi;
        readonly string RUTAAPI = Environment.GetEnvironmentVariable("RUTAAPI");

        public ValidateApiController()
        {
            UrlApi = string.IsNullOrEmpty(RUTAAPI) ? new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build().GetValue<string>("Variables:RutaApi") : RUTAAPI;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> ValidateApi()
        {
            using var client = new HttpClient();

            try
            {
                // Se envía una solicitud HTTP GET a la URL del API.
                var response = await client.GetAsync(UrlApi);

                // Si la respuesta de la API tiene un código de estado de éxito, se devuelve un mensaje indicando que el API está funcionando correctamente.
                if (response.IsSuccessStatusCode)
                {
                    return "El API está funcionando correctamente.";
                }
                else
                {
                    return $"La API no está disponible. Código de estado HTTP: {(int)response.StatusCode} - {response.ReasonPhrase}";
                }
            }
            catch (HttpRequestException ex)
            {
                // Si se produce una excepción durante el acceso a la API, se devuelve un mensaje de error.
                return $"Error al acceder a la API: {ex.Message}";
            }
        }
    }
}