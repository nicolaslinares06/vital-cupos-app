using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Repository.Helpers;
using Repository.Helpers.Models;
using System.Net.Http.Headers;
using Web.Models;

namespace Web.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class FishQuotaController : Controller
    {
        private readonly ILogger<FishQuotaController> _logger;
        private readonly string urlApi;
        readonly string RUTAAPI = Environment.GetEnvironmentVariable("RUTAAPI");

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
                _logger.LogError(ex, "An error occurred in the method.");
                throw;
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
                _logger.LogError(ex, "An error occurred in the method.");
                throw;
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
                _logger.LogInformation("method called");
                List<FishQuota> fishesQuotasList = new List<FishQuota>();
                string token = HttpContext.Session.GetString("token");
                string uri = "";
                if (numberResolution == 0 && initialValidityDate == null && finalValidityDate == null)
                {
                    uri = String.Format("{0}/FishQuota/GetFishesQuotas", urlApi);
                }
                else
                {
                    uri = String.Format("{0}/FishQuota/GetFishesQuotas?initialValidityDate={1}&finalValidityDate={2}&numberResolution={3}", urlApi, initialValidityDate, finalValidityDate, numberResolution);

                }

                var httpClient = GetHttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = httpClient.GetAsync(uri).Result;

                if (response.IsSuccessStatusCode)
                {
                    string jsonInput = response.Content.ReadAsStringAsync().Result;
                    Responses responseData = JsonConvert.DeserializeObject<Responses>(jsonInput);
                    if (responseData.Response != null)
                    {
                        fishesQuotasList = JsonConvert.DeserializeObject<List<FishQuota>>(responseData.Response.ToString());
                        HttpContext.Session.SetString("token", responseData.Token);
                    }
                }

                return fishesQuotasList;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                throw;
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
                _logger.LogInformation("method called");
                List<FishQuota> fishesQuotasList = new List<FishQuota>();
                string token = HttpContext.Session.GetString("token");
                string uri = "";

                uri = String.Format("{0}/FishQuota/GetFishQuotaByCode?code=" + code, urlApi);

                var httpClient = GetHttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = httpClient.GetAsync(uri).Result;

                if (response.IsSuccessStatusCode)
                {
                    string jsonInput = response.Content.ReadAsStringAsync().Result;
                    Responses responseData = JsonConvert.DeserializeObject<Responses>(jsonInput);
                    if (responseData.Response != null)
                    {
                        fishesQuotasList = JsonConvert.DeserializeObject<List<FishQuota>>(responseData.Response.ToString());
                        HttpContext.Session.SetString("token", responseData.Token);
                    }
                }

                foreach (var l in fishesQuotasList)
                {
                    if (l.Quota == 0)
                    {
                        l.SpeciesCode = 0;
                        l.speciesNameComun = null;
                        l.SpeciesName = null;
                    }
                }

                return fishesQuotasList;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                throw;
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
                _logger.LogInformation("method called");
                string token = HttpContext.Session.GetString("token");
                string uri = String.Format("{0}/FishQuota/SaveFishQuota", urlApi);
                var httpClient = GetHttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

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
                        attachmentType = a.tipoAdjunto
                    }).ToList(),
                    SupportDocumentsRemoved = fishQuota.SupportDocumentsRemoved?.Select(a => new
                    {
                        code = a.codigo,
                        base64Attachment = a.adjuntoBase64,
                        attachmentName = a.nombreAdjunto,
                        attachmentType = a.tipoAdjunto
                    }).ToList()
                };

                var response = httpClient.PostAsJsonAsync(uri, req).Result;

                Responses responseData = new Responses();

                if (response.IsSuccessStatusCode)
                {
                    string jsonInput = response.Content.ReadAsStringAsync().Result;
                    responseData = JsonConvert.DeserializeObject<Responses>(jsonInput);
                    return responseData;
                }
                else
                {
                    responseData.Message = "No se pudo consumir save servicio API CUPOS";
                    responseData.Error = true;
                    return responseData;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                throw;
            }
        }

        /// <summary>
        /// Elimina cuota para peces por code
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public object DeleteFishQuota(int code)
        {
            try
            {
                _logger.LogInformation("method called");
                string token = HttpContext.Session.GetString("token");
                string uri = String.Format("{0}/FishQuota/DeleteFishQuota?code=" + code, urlApi);

                var httpClient = GetHttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = httpClient.GetAsync(uri).Result;

                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return new
                    {
                        volverInicio = true
                    };
                }

                Responses responseData = new Responses();

                if (response.IsSuccessStatusCode)
                {
                    string jsonInput = response.Content.ReadAsStringAsync().Result;
                    responseData = JsonConvert.DeserializeObject<Responses>(jsonInput);
                    return responseData;
                }
                else
                {
                    responseData.Message = "No se pudo consumir delete servicio API CUPOS";
                    responseData.Error = true;
                    return responseData;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                throw;
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
                _logger.LogInformation("method called");
                string token = HttpContext.Session.GetString("token");
                string uri = String.Format("{0}/FishQuota/UpdateFishQuota", urlApi);
                var httpClient = GetHttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

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

                var response = httpClient.PutAsJsonAsync(uri, req).Result;

                Responses responseData = new Responses();

                if (response.IsSuccessStatusCode)
                {
                    string jsonInput = response.Content.ReadAsStringAsync().Result;
                    responseData = JsonConvert.DeserializeObject<Responses>(jsonInput);
                    return responseData;
                }
                else
                {
                    responseData.Message = "No se pudo consumir save servicio API CUPOS";
                    responseData.Error = true;
                    return responseData;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                throw;
            }
        }

        /// <summary>
        /// Obtiene especies
        /// </summary>
        /// <returns></returns>
        public object GetSpecies()
        {
            try
            {
                _logger.LogInformation("method called");
                List<ElementTypesEspecies>? speciesList = new List<ElementTypesEspecies>();

                string? token = HttpContext.Session.GetString("token");
                string uri = String.Format("{0}/FishQuota/GetSpecies", urlApi);
                var httpClient = GetHttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = httpClient.GetAsync(uri).Result;
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return new
                    {
                        volverInicio = true
                    };
                }

                if (response.IsSuccessStatusCode)
                {
                    string jsonInput = response.Content.ReadAsStringAsync().Result;
                    Responses? responseData = JsonConvert.DeserializeObject<Responses>(jsonInput);
                    if (responseData.Response != null)
                    {
                        speciesList = JsonConvert.DeserializeObject<List<ElementTypesEspecies>>(responseData.Response.ToString() ?? "");
                        HttpContext.Session.SetString("token", responseData.Token);
                    }
                }


                return speciesList ?? new object { };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                throw;
            }
        }

        /// <summary>
        /// Obtiene los documentos soporte por codigo
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public object GetSupportDocument(decimal code)
        {
            try
            {
                _logger.LogInformation("method called");
                List<SupportDocuments> supportDocumentsList = new List<SupportDocuments>();
                string token = HttpContext.Session.GetString("token");
                string uri = "";

                uri = String.Format("{0}/FishQuota/GetSupportDocument?code=" + code, urlApi);

                var httpClient = GetHttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = httpClient.GetAsync(uri).Result;
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return new
                    {
                        volverInicio = true
                    };
                }

                if (response.IsSuccessStatusCode)
                {
                    string jsonInput = response.Content.ReadAsStringAsync().Result;
                    Responses responseData = JsonConvert.DeserializeObject<Responses>(jsonInput);
                    if (responseData.Response != null)
                    {
                        supportDocumentsList = JsonConvert.DeserializeObject<List<SupportDocuments>>(responseData.Response.ToString());
                        HttpContext.Session.SetString("token", responseData.Token);
                    }
                }

                return supportDocumentsList;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                throw;
            }
        }
    }
}
