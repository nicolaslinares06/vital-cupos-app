using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Repository.Helpers;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Repository.Helpers.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Web.Models;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using static Web.Models.TrayForNationalSealsExternUsers;
using System.Security.Policy;

namespace Web.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class SaleDocumentController : Controller
    {
        private readonly string urlApi;
        private readonly string UrlSunL;
        private readonly ILogger<SaleDocumentController> _logger;
        readonly string RUTAAPI = Environment.GetEnvironmentVariable("RUTAAPI") ?? "";

        public SaleDocumentController(ILogger<SaleDocumentController> logger)
        {
            urlApi = string.IsNullOrEmpty(RUTAAPI) ? new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build().GetValue<string>("Variables:RutaApi") : RUTAAPI;
            UrlSunL = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build().GetValue<string>("Variables:RutaSunL");
            _logger = logger;
        }

        public HttpClient GetHttpClient()
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            return new HttpClient(clientHandler);
        }

        public IActionResult SaleDocuments()
        {
            try
            {
                var model = GetSaleDocuments();
                _logger.LogInformation("method called");
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                throw;
            }
        }




        public IActionResult SaleDocumentForm()
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

        /// <summary>
        /// Obtiene el documento de venta por codigo
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public object GetSaleDocumentId(int code)
        {
            try
            {                
                string uri = "";
                uri = String.Format("{0}/SaleDocument/GetSaleDocumentId?code=" + code, urlApi);
                var respuesta = ProcesarDataApiGet<List<SaleDocumentModel>>(uri);
                return respuesta;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");               
                return new List<SaleDocumentModel>();
            }
        }



        /// <summary>
        /// Obtiene los documentos de venta
        /// </summary>
        /// <param name="typeDocument"></param>
        /// <param name="documentNumber"></param>
        /// <returns></returns>
        public List<SaleDocumentModel> GetSaleDocuments(string? typeDocument = null, string? documentNumber = null, string? numberCartaVenta = null)
        {
            try
            {               
                string uri = "";
                if (typeDocument == null && documentNumber == null && numberCartaVenta == null)
                {
                    uri = String.Format("{0}/SaleDocument/GetSaleDocuments", urlApi);
                }
                else
                {
                    uri = String.Format("{0}/SaleDocument/GetSaleDocuments?typeDocument=" + typeDocument + "&documentNumber=" + documentNumber + "&numberCartaVenta=" + numberCartaVenta, urlApi);
                }

                var respuesta = (List<SaleDocumentModel>)ProcesarDataApiGet<List<SaleDocumentModel>>(uri);
                return respuesta;
             
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");               
                return new List<SaleDocumentModel>();
            }
        }



        public IActionResult Quotas(decimal company, decimal typeDocument, string documentNumber)
        {
            try
            {
                _logger.LogInformation("method called");
                ViewBag.documentNumber = documentNumber;
                ViewBag.typeDocument = typeDocument;
                ViewBag.company = company;
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                throw;
            }
        }

        /// <summary>
        /// Obtiene los cupos
        /// </summary>
        /// <returns></returns>
        public object GetQuotas(string documentNumber)
        {
            try
            {               
                string uri = String.Format("{0}/SaleDocument/GetQuotas?documentNumber={1}", urlApi, documentNumber);
                var respuesta = ProcesarDataApiGet<List<Quota>>(uri);
                return respuesta;              
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");               
                return new List<Quota>();
            }
        }

        /// <summary>
        /// Obtiene las numeraciones por numero documento de la empresa y codigo cupo
        /// </summary>
        /// <returns></returns>
        public object GetQuotasNumeraciones(int codigoCupo, string documentNumber)
        {
            try
            {               
                string uri = String.Format("{0}/SaleDocument/GetQuotasNumeraciones?codigoCupo={1}&documentNumber={2}", urlApi, codigoCupo, documentNumber);
                var respuesta = ProcesarDataApiGet<List<numbersSeals>>(uri);
                return respuesta;               
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                return new List<numbersSeals>();
            }
        }

        /// <summary>
        /// Valida si estan disponibvles las numeraciones
        /// </summary>
        /// <returns></returns>
        public object ValidateNumbers(Seal numbers)
        {
            try
            {
                string uri = String.Format("{0}/SaleDocument/ValidateNumbers", urlApi);
                var respuesta = ProcesarDataApiPost<object>(uri,numbers);
                return respuesta;              
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                return new object { };
            }
        }

        /// <summary>
        /// Obtiene el inventario
        /// </summary>
        /// <returns></returns>
        public object GetInventory(string documentNumber, string? code = null)
        {
            try
            {              
                string uri = String.Format("{0}/SaleDocument/GetInventory?documentNumber={1}&code={2}", urlApi, documentNumber, code);
                var respuesta = ProcesarDataApiGet<List<Inventory>>(uri);
                return respuesta;
               
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");               
                return new List<Inventory>(); 
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
                string uri = String.Format("{0}/SaleDocument/GetSpecies", urlApi);
                var respuesta = ProcesarDataApiGet<List<Species>>(uri);
                return respuesta;
               
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");             
                return new List<Species>();
            }
        }

        /// <summary>
        /// Guarda el documento de la venta
        /// </summary>
        /// <param name="saleDocument"></param>
        /// <returns></returns>
        public bool SaveSaleDocument(SaleDocumentModel saleDocument)
        {
            try
            {              
                string uri = String.Format("{0}/SaleDocument/SaveSaleDocument", urlApi);
                var req = ConversionDatoObject(saleDocument);
                var respuesta = (bool)ProcesarDataApiPostReturnBool(uri, req);
                return respuesta;
             
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                return false;
            }
        }

        public IActionResult ValidateCompanyForm()
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
        /// Valida si la empresa se encuentra registrada
        /// </summary>
        /// <param name="documentNumber"></param>
        /// <param name="business"></param>
        /// <param name="typeDocument"></param>
        /// <returns></returns>
        public object ValidateCompany(decimal company = 0, decimal typeDocument = 0, string documentNumber = "0")
        {
            try
            {                
                string uri = String.Format("{0}/SaleDocument/ValidateCompany?company=" + company + "&typeDocument=" + typeDocument + "&documentNumber=" + documentNumber, urlApi);
                var respuesta = ProcesarDataApiGet<List<Company>>(uri);
                return respuesta;                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                return new List<Company>();
            }
        }

        /// <summary>
        /// Filtra la empresa por numero de documento
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public object SearchCompany(string number, decimal typeDocument = 0, decimal companyCode = 0)
        {
            try
            {                
                string uri = String.Format("{0}/SaleDocument/SearchCompany?number={1}&typeDocument={2}&companyCode={3}", urlApi, number, typeDocument, companyCode);
                var respuesta = ProcesarDataApiGet<List<Company>>(uri);
                return respuesta;   
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                return new List<Company>();
            }
        }

        public IActionResult ExceptConduitMobilization()
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

        public IActionResult ConduitMobilizationForm(string numeroIdentificacion, string numeroSalvoConducto)
        {
            try
            {
                var data = ConsultarSalvoConducto(numeroIdentificacion, numeroSalvoConducto);
                ViewBag.data = data;
                ViewBag.numeroIdentificacion = numeroIdentificacion;
                ViewBag.numeroSalvoConducto = numeroSalvoConducto;

                _logger.LogInformation("method called");
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                throw;
            }
        }

        // <summary>
        // 
        // </summary>
        // <returns></returns>
        private string AccessSunl()
        {
            string responseString = "";

            AccessSUNL _usuario = new AccessSUNL
            {
                UserName = "juan",
                Password = "80245757"
            };

            if (_usuario.UserName != null && _usuario.Password != null)
            {
                string URI = UrlSunL + "/login/acceess-token";
                var httpClient = GetHttpClient();
                var response = httpClient.PostAsJsonAsync(URI, _usuario).Result;

                if (response.IsSuccessStatusCode)
                {
                    responseString = response.Content.ReadAsStringAsync().Result;
                    responseString = responseString.Trim('\"'); // Eliminar los caracteres "\" al inicio y al final
                    HttpContext.Session.SetString("tokenSunL", responseString);
                }

                return responseString;
            }
            else
            {
                return "Campos no válidos";
            }
        }

        public InfoSalvoConducto ConsultarSalvoConducto(string numeroIdentificacion, string numeroSalvoConducto)
        {
            InfoSalvoConducto result = new InfoSalvoConducto();

            string token = AccessSunl();

            if (token == null)
            {
                return result;
            }

            string URI = UrlSunL + "/GetDataSUNL?SunlNumber=" + numeroSalvoConducto + "&Identification=" + numeroIdentificacion;           
            var respuesta = (InfoSalvoConducto)ProcesarDataApiGet<InfoSalvoConducto>(URI);
            return respuesta;         
        }

        /// <summary>
        /// Elimina un documento de venta por id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public object DeleteSaleDocument(int id)
        {
            try
            {              
                string uri = String.Format("{0}/SaleDocument/DeleteSaleDocument?id=" + id, urlApi);
                var respuesta = ProcesarDataApiPostReturnBool(uri, id);
                return respuesta;               
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                return false;
            }
        }

        /// <summary>
        /// Actualiza el documento de venta
        /// </summary>
        /// <param name="saleDocument"></param>
        /// <returns></returns>
        public bool UpdateSaleDocument(SaleDocumentModel saleDocument)
        {
            try
            {              
                var req = ConversionDatoObject(saleDocument);
                string uri = String.Format("{0}/SaleDocument/UpdateSaleDocument", urlApi);
                var respuesta = (bool)ProcesarDataApiPutReturnBool(uri, req);
                return respuesta;
          
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                return false;
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
                string uri = "";
                uri = String.Format("{0}/SaleDocument/GetSupportDocument?code=" + code, urlApi);
                var respuesta = ProcesarDataApiGet<List<SupportDocuments>>(uri);
                return respuesta;               
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                return new List<SupportDocuments>();
            }
        }

        /// <summary>
        /// Obtiene los cupos por codigo documento de venta
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public object GetQuotasByCode(decimal code)
        {
            try
            {            
                string uri = String.Format("{0}/SaleDocument/GetQuotasByCode?code={1}", urlApi, code);
                var respuesta = ProcesarDataApiGet<List<Quota>>(uri);
                return respuesta;               
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");               
                return new List<Quota>();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public object SearchSeals(numbersSeals data)
        {
            try
            {             
                string uri = String.Format("{0}/SaleDocument/SearchSeals", urlApi);
                var respuesta = ProcesarDataApiGet<Seal>(uri);
                return respuesta;               
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");               
                return new Seal(); 
            }
        }

        private object ProcesarDataApiGet<T>(string URI) where T : new()
        {

            var httpClient = ConfigurarHttpClient();
            var response = httpClient.GetAsync(URI).Result;
            var data = ProcessHttpResponse<T>(response);
            return data ?? new object { };
        }

        private object ProcesarDataApiPost<T>(string URI, T req) where T : new()
        {

            var httpClient = ConfigurarHttpClient();
            var response = httpClient.PostAsJsonAsync(URI, req).Result;
            var data = ProcessHttpResponse<T>(response);
            return data ?? new object { };
        }

        private object ProcesarDataApiPostReturnBool<T>(string URI, T req) 
        {

            var httpClient = ConfigurarHttpClient();
            var response = httpClient.PostAsJsonAsync(URI, req).Result;
            var data = ProcesarPeticion(response);
            return data;
        }

        private object ProcesarDataApiPutReturnBool<T>(string URI, T req)
        {

            var httpClient = ConfigurarHttpClient();
            var response = httpClient.PutAsJsonAsync(URI, req).Result;
            var data = ProcesarPeticion(response);
            return data;
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

        private object ProcessHttpResponse<T>(HttpResponseMessage response) where T : new()
        {
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return new { volverInicio = true };
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

        private object ProcesarPeticion(HttpResponseMessage response)
        {
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return new
                {
                    volverInicio = true
                };
            }

            if (response.IsSuccessStatusCode)
            {
                string responseString = response.Content.ReadAsStringAsync().Result;
                Responses? respuesta = JsonConvert.DeserializeObject<Responses>(responseString) ?? new Responses();
                if (respuesta.Response != null)
                {
                    HttpContext.Session.SetString("token", respuesta.Token);
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        private object ConversionDatoObject(SaleDocumentModel saleDocument)
        {
            var req = new
            {
                Code = saleDocument.Code,
                Numeration = saleDocument.Numeration,
                CarteNumber = saleDocument.CarteNumber,
                SaleDate = saleDocument.SaleDate,
                NumberSold = saleDocument.NumberSold,
                BusinessSale = saleDocument.BusinessSale,
                TypeCarte = saleDocument.TypeCarte,
                TypeDocumentSeller = saleDocument.TypeDocumentSeller,
                DocumentNumberSeller = saleDocument.DocumentNumberSeller,
                ReasonSocial = saleDocument.ReasonSocial,
                InitialBalanceBusiness = saleDocument.InitialBalanceBusiness,
                FinalBalanceBusiness = saleDocument.FinalBalanceBusiness,
                Observations = saleDocument.Observations,
                BusinessShopper = saleDocument.BusinessShopper,
                InventoryAvailability = saleDocument.InventoryAvailability,
                TypeDocumentShopper = saleDocument.TypeDocumentShopper,
                DocumentNumberShopper = saleDocument.DocumentNumberShopper,
                ReasonSocialShopper = saleDocument.ReasonSocialShopper,
                InitialBalanceBusinessShopper = saleDocument.InitialBalanceBusinessShopper,
                FinalBalanceBusinessShopper = saleDocument.FinalBalanceBusinessShopper,
                ObservationsShopper = saleDocument.ObservationsShopper,
                Quota = saleDocument.Quota,
                Solds = saleDocument.Solds,
                QuotasSold = saleDocument.QuotasSold,
                NitCompanySeller = saleDocument.NitCompanySeller,
                NitCompanyShopper = saleDocument.NitCompanyShopper,
                CompanySellerCode = saleDocument.CompanySellerCode,
                CompanyShopperCode = saleDocument.CompanyShopperCode,
                RegistrationDateCarteSale = saleDocument.RegistrationDateCarteSale,
                SupportDocuments = saleDocument.SupportDocuments?.Select(a => new
                {
                    code = a.codigo,
                    base64Attachment = a.adjuntoBase64,
                    attachmentName = a.nombreAdjunto,
                    attachmentType = a.tipoAdjunto
                }).ToList(),
                SupportDocumentsRemoved = saleDocument.SupportDocumentsRemoved?.Select(a => new
                {
                    code = a.codigo,
                    base64Attachment = a.adjuntoBase64,
                    attachmentName = a.nombreAdjunto,
                    attachmentType = a.tipoAdjunto
                }).ToList(),
                Quotas = saleDocument.Quotas?.Select(a => new
                {
                    Code = a.Code,
                    NumberResolution = a.NumberResolution,
                    QuotasGrant = a.QuotasGrant,
                    QuotasAdvantageCommercialization = a.QuotasAdvantageCommercialization,
                    QuotasRePoblation = a.QuotasRePoblation,
                    QuotasAvailable = a.QuotasAvailable,
                    ProductionDate = a.ProductionDate,
                    YearProduction = a.YearProduction,
                    SpeciesCode = a.SpeciesCode,
                    SpeciesName = a.SpeciesName,
                    QuotasSold = a.QuotasSold,
                    InitialNumeration = a.InitialNumeration,
                    FinalNumeration = a.FinalNumeration,
                    CompanyCode = a.CompanyCode,
                    InitialNumerationRePoblation = a.InitialNumerationRePoblation,
                    FinalNumerationRePoblation = a.FinalNumerationRePoblation,
                    InitialNumerationSeal = a.InitialNumerationSeal,
                    FinalNumerationSeal = a.FinalNumerationSeal
                }).ToList(),
                QuotasInventory = saleDocument.QuotasInventory?.Select(a => new
                {
                    quotaCode = a.quotaCode,
                    Code = a.Code,
                    NumberSaleCarte = a.NumberSaleCarte,
                    ReasonSocial = a.ReasonSocial,
                    SaleDate = a.SaleDate,
                    AvailabilityInventory = a.AvailabilityInventory,
                    Year = a.Year,
                    AvailableInventory = a.AvailableInventory,
                    InitialNumeration = a.InitialNumeration,
                    FinalNumeration = a.FinalNumeration,
                    InitialNumerationRePoblation = a.InitialNumerationRePoblation,
                    FinalNumerationRePoblation = a.FinalNumerationRePoblation,
                    InitialNumerationSeal = a.InitialNumerationSeal,
                    FinalNumerationSeal = a.FinalNumerationSeal,
                    SpeciesCode = a.SpeciesCode,
                    SpeciesName = a.SpeciesName,
                    InventorySold = a.InventorySold
                }).ToList(),
                TypeSpecimenSeller = saleDocument.TypeSpecimenSeller,
                TypeSpecimenShopper = saleDocument.TypeSpecimenShopper,
            };

            return req;
        }
    }

}
