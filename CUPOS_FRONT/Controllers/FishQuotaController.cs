using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Repository.Helpers;
using Repository.Helpers.Models;
using System;
using System.Net.Http.Headers;
using Web.Models;

namespace Web.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class FishQuotaController : Controller
    {
        private readonly ILogger<FishQuotaController> _logger;
        private readonly string urlApi;
        readonly string RUTAAPI = Environment.GetEnvironmentVariable("RUTAAPI") ?? "";

        public FishQuotaController(ILogger<FishQuotaController> logger)
        {
            urlApi = string.IsNullOrEmpty(RUTAAPI) ? new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build().GetValue<string>("Variables:RutaApi") : RUTAAPI;
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
        public IActionResult FishQuotas()
        {
            try
            {
                var model = new List<FishQuota>();
                _logger.LogInformation("method called");
                return View(model);
            }
            catch (Exception ex)
            {
                return ManejarExcepcion(ex);
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
                _logger.LogInformation("method called");
                string? token = HttpContext.Session.GetString("token");

                if (token != null)
                    return View("Views/FishQuota/Partials/Ayuda.cshtml");

                return View();
            }
            catch (Exception ex)
            {
                return ManejarExcepcion(ex);
            }
        }
        /// <summary>
        /// Obtiene cuota para peces
        /// </summary>
        /// <param name="numberResolution"></param>
        /// <param name="vigency"></param>
        /// <returns></returns>
        public List<FishQuota> GetFishesQuotas(DateTime? initialValidityDate, DateTime? finalValidityDate, decimal numberResolution = 0)
        {
            try
            {               
                string uri = "";
                if (numberResolution == 0 && initialValidityDate == null && finalValidityDate == null)
                {
                    uri = String.Format("{0}/FishQuota/GetFishesQuotas", urlApi);
                }
                else
                {
                    uri = String.Format("{0}/FishQuota/GetFishesQuotas?initialValidityDate={1}&finalValidityDate={2}&numberResolution={3}", urlApi, initialValidityDate, finalValidityDate, numberResolution);

                }
                var resultado = ProcesarDataApiGet<List<FishQuota>>(uri);
                return resultado ?? new List<FishQuota>();              
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                return new List<FishQuota>();
            }
        }
        /// <summary>
        /// Obtiene cuota para peces por code
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public List<FishQuota> GetFishQuotaByCode(decimal code)
        {
            try
            {             
                string uri = "";
                uri = String.Format("{0}/FishQuota/GetFishQuotaByCode?code=" + code, urlApi);
                var resultado = ProcesarDataApiGet<List<FishQuota>>(uri) ?? new List<FishQuota>();   
                foreach (var l in resultado)
                {
                    if (l.Quota == 0)
                    {
                        l.SpeciesCode = 0;
                        l.speciesNameComun = null;
                        l.SpeciesName = null;
                    }
                }
                return resultado;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                return new List<FishQuota>();
            }
        }
        /// <summary>
        /// Guarda cuota para peces
        /// </summary>
        /// <param name="fishQuota"></param>
        /// <returns></returns>
        public Responses SaveFishQuota(FishQuota fishQuota)
        {
            try
            {              
                string uri = String.Format("{0}/FishQuota/SaveFishQuota", urlApi);
                var req = ObtenerObjetoData(fishQuota);
                var respuesta = ProcesarDataApiPost(uri, req, "No se pudo consumir save servicio API CUPOS");
                return respuesta;   
            }
            catch (Exception ex)
            {
                return ManejarExcepcionResponses(ex);
            }
        }

        /// <summary>
        /// Elimina cuota para peces por code
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public Responses DeleteFishQuota(int code)
        {
            try
            {                
                string uri = String.Format("{0}/FishQuota/DeleteFishQuota?code=" + code, urlApi);
                var resultado = ProcesarDataApiGet<Responses>(uri) ?? new Responses();
              
                if(resultado.Error)
                {
                    resultado.Message = "No se pudo consumir delete servicio API CUPOS";
                }

                return resultado;               
            }
            catch (Exception ex)
            {
                return ManejarExcepcionResponses(ex);
            }
        }

        /// <summary>
        /// Actualiza cuota para peces
        /// </summary>
        /// <param name="fishQuota"></param>
        /// <returns></returns>
        public Responses UpdateFishQuota(FishQuota fishQuota)
        {
            try
            {               
                string uri = String.Format("{0}/FishQuota/UpdateFishQuota", urlApi);
                var req = ObtenerObjetoData(fishQuota);
                var respuesta = ProcesarDataApiPost(uri, req, "No se pudo consumir update servicio API CUPOS");
                return respuesta;            
            }
            catch (Exception ex)
            {
                return ManejarExcepcionResponses(ex);
            }
        }

        /// <summary>
        /// Obtiene especies
        /// </summary>
        /// <returns></returns>
        public List<ElementTypesEspecies> GetSpecies()
        {
            try
            {  
                string uri = String.Format("{0}/FishQuota/GetSpecies", urlApi);
                var resultado = ProcesarDataApiGet<List<ElementTypesEspecies>>(uri) ?? new List<ElementTypesEspecies>();
                return resultado;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                return new List<ElementTypesEspecies>();
            }
        }

        /// <summary>
        /// Obtiene los documentos soporte por codigo
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public List<SupportDocuments> GetSupportDocument(decimal code)
        {
            try
            {
              
                string uri = "";
                uri = String.Format("{0}/FishQuota/GetSupportDocument?code=" + code, urlApi);
                var resultado = ProcesarDataApiGet<List<SupportDocuments>>(uri) ?? new List<SupportDocuments>();
                return resultado;               
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                return new List<SupportDocuments>();
            }
        }

        private IActionResult ManejarExcepcion(Exception ex)
        {
            _logger.LogError(ex, "An error occurred in the method.");
            string token = HttpContext.Session.GetString("token") ?? "";
            if (!String.IsNullOrEmpty(token))
                return RedirectToAction("FlujoNegocio", "Home");
            else
                return RedirectToAction("Index", "Login");
        }

        private T ProcesarDataApiGet<T>(string URI) where T : new()
        {

            var httpClient = ConfigurarHttpClient();
            var response = httpClient.GetAsync(URI).Result;
            var data = ProcessHttpResponse<T>(response);
            return data;
        }

        private Responses ProcesarDataApiPost<T>(string URI, T req, string mensajeError) 
        {

            var httpClient = ConfigurarHttpClient();
            var response = httpClient.PostAsJsonAsync(URI, req).Result;
            var data = ProcessHttpResponsePost(response, mensajeError);
            return data ?? new Responses();
        }

        private T ProcessHttpResponse<T>(HttpResponseMessage response) where T : new()
        {

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return new T();
            }

            if (response.IsSuccessStatusCode)
            {
                string responseString = response.Content.ReadAsStringAsync().Result;
                Responses? respuesta = JsonConvert.DeserializeObject<Responses>(responseString) ?? new Responses();

                if (respuesta.Response != null)
                {
                    HttpContext.Session.SetString("token", respuesta.Token);
                    return JsonConvert.DeserializeObject<T>(respuesta.Response.ToString() ?? "") ?? new T();
                }
            }

            return new T();
        }

        private Responses ProcessHttpResponsePost(HttpResponseMessage response, string mensajeError)
        {
            Responses responseData = new Responses();

            if (response.IsSuccessStatusCode)
            {
                string jsonInput = response.Content.ReadAsStringAsync().Result;
                responseData = JsonConvert.DeserializeObject<Responses>(jsonInput) ?? new Responses();
                return responseData;
            }
            else
            {
                responseData.Message = mensajeError;
                responseData.Error = true;
                return responseData;
            }
        }

        private HttpClient ConfigurarHttpClient()
        {
            string? token = HttpContext.Session.GetString("token");
            var httpClient = GetHttpClient();

            if (!string.IsNullOrEmpty(token))
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            return httpClient;
        }

        private object ObtenerObjetoData(FishQuota fishQuota)
        {
            var req = new
            {
                Code = fishQuota.Code,
                Type = fishQuota.Type,
                NumberResolution = fishQuota.NumberResolution,
                ResolutionDate = fishQuota.ResolutionDate,
                ValidityDate = fishQuota.ValidityDate,
                ValidityYear = fishQuota.ValidityYear,
                ResolutionObject = fishQuota.ResolutionObject,
                Document = fishQuota.Document,
                InitialValidityDate = fishQuota.InitialValidityDate,
                FinalValidityDate = fishQuota.FinalValidityDate,
                CodeFishQuotaAmount = fishQuota.CodeFishQuotaAmount,
                Group = fishQuota.Group,
                GroupName = fishQuota.GroupName,
                speciesNameComun = fishQuota.speciesNameComun,
                SpeciesCode = fishQuota.SpeciesCode,
                Quota = fishQuota.Quota,
                Total = fishQuota.Total,
                Region = fishQuota.Region,
                SpeciesName = fishQuota.SpeciesName,
                FishQuotaAmounts = fishQuota.FishQuotaAmounts?.Select(a => new
                {
                    CodeFishQuotaAmount = a.CodeFishQuotaAmount,
                    Group = a.Group,
                    SpeciesCode = a.SpeciesCode,
                    SpeciesName = a.SpeciesName,
                    speciesNameComun = a.speciesNameComun,
                    Quota = a.Quota,
                    Total = a.Total,
                    NameRegion = a.NameRegion,
                    Region = a.Region,
                    ActionTemp = a.ActionTemp
                }).ToList(),
                FishQuotaAmountsRemoved = fishQuota.FishQuotaAmountsRemoved?.Select(a => new
                {
                    CodeFishQuotaAmount = a.CodeFishQuotaAmount,
                    Group = a.Group,
                    SpeciesCode = a.SpeciesCode,
                    SpeciesName = a.SpeciesName,
                    speciesNameComun = a.speciesNameComun,
                    Quota = a.Quota,
                    Total = a.Total,
                    NameRegion = a.NameRegion,
                    Region = a.Region,
                    ActionTemp = a.ActionTemp
                }).ToList(),
                SupportDocuments = fishQuota.SupportDocuments?.Select(a => new
                {
                    code = a.codigo,
                    base64Attachment = a.adjuntoBase64,
                    attachmentName = a.nombreAdjunto,
                    attachmentType = a.tipoAdjunto,
                    tempAction = a.ActionTemp
                }).ToList(),
                SupportDocumentsRemoved = fishQuota.SupportDocumentsRemoved?.Select(a => new
                {
                    code = a.codigo,
                    base64Attachment = a.adjuntoBase64,
                    attachmentName = a.nombreAdjunto,
                    attachmentType = a.tipoAdjunto
                }).ToList()
            };

            return req;
        }

        private Responses ManejarExcepcionResponses(Exception ex)
        {
            _logger.LogError(ex, "An error occurred in the method.");
            Responses responseData = new Responses();
            responseData.Error = true;
            responseData.Message = ex.Message;
            return responseData;

        }
    }
}
