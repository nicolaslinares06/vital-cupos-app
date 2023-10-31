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
                _logger.LogInformation("method called");
                List<SaleDocumentModel>? saleDocumentsList = new List<SaleDocumentModel>();
                string? token = HttpContext.Session.GetString("token");
                string uri = "";

                uri = String.Format("{0}/SaleDocument/GetSaleDocumentId?code=" + code, urlApi);

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
                    Responses? responseData = JsonConvert.DeserializeObject<Responses>(jsonInput ?? "") ?? new Responses();
                    if (responseData.Response != null)
                    {
                        saleDocumentsList = JsonConvert.DeserializeObject<List<SaleDocumentModel>>(responseData.Response.ToString() ?? "");
                        HttpContext.Session.SetString("token", responseData.Token);
                    }
                }

                if (saleDocumentsList is null)
                    saleDocumentsList = new List<SaleDocumentModel>();

                return saleDocumentsList;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                List<SaleDocumentModel>? saleDocumentsList = new List<SaleDocumentModel>();
                return saleDocumentsList;
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
                _logger.LogInformation("method called");
                List<SaleDocumentModel>? saleDocumentsList = new List<SaleDocumentModel>();
                string? token = HttpContext.Session.GetString("token");
                string uri = "";
                if (typeDocument == null && documentNumber == null && numberCartaVenta == null)
                {
                    uri = String.Format("{0}/SaleDocument/GetSaleDocuments", urlApi);
                }
                else
                {
                    uri = String.Format("{0}/SaleDocument/GetSaleDocuments?typeDocument=" + typeDocument + "&documentNumber=" + documentNumber + "&numberCartaVenta=" + numberCartaVenta, urlApi);
                }

                var httpClient = GetHttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = httpClient.GetAsync(uri).Result;

                if (response.IsSuccessStatusCode)
                {
                    string jsonInput = response.Content.ReadAsStringAsync().Result;
                    Responses? responseData = JsonConvert.DeserializeObject<Responses>(jsonInput ?? "") ?? new Responses();
                    if (responseData.Response != null)
                    {
                        saleDocumentsList = JsonConvert.DeserializeObject<List<SaleDocumentModel>>(responseData.Response.ToString() ?? "");
                        HttpContext.Session.SetString("token", responseData.Token);
                    }
                }

                if (saleDocumentsList is null)
                    saleDocumentsList = new List<SaleDocumentModel>();

                return saleDocumentsList;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                List<SaleDocumentModel>? saleDocumentsList = new List<SaleDocumentModel>();
                return saleDocumentsList;
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
                _logger.LogInformation("method called");
                List<Quota>? quotaList = new List<Quota>();

                string? token = HttpContext.Session.GetString("token");
                string uri = String.Format("{0}/SaleDocument/GetQuotas?documentNumber={1}", urlApi, documentNumber);
                var httpClient = GetHttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = httpClient.GetAsync(uri).Result;

                if (response.IsSuccessStatusCode)
                {
                    string jsonInput = response.Content.ReadAsStringAsync().Result;
                    Responses? responseData = JsonConvert.DeserializeObject<Responses>(jsonInput ?? "") ?? new Responses();
                    if (responseData.Response != null)
                    {
                        quotaList = JsonConvert.DeserializeObject<List<Quota>>(responseData.Response.ToString() ?? "");
                        HttpContext.Session.SetString("token", responseData.Token);
                    }
                }

                return quotaList ?? new object { };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                List<Quota>? quotaList = new List<Quota>();
                return quotaList;
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
                _logger.LogInformation("method called");
                List<numbersSeals>? ListNumbers = new List<numbersSeals>();

                string? token = HttpContext.Session.GetString("token");
                string uri = String.Format("{0}/SaleDocument/GetQuotasNumeraciones?codigoCupo={1}&documentNumber={2}", urlApi, codigoCupo, documentNumber);
                var httpClient = GetHttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = httpClient.GetAsync(uri).Result;

                if (response.IsSuccessStatusCode)
                {
                    string jsonInput = response.Content.ReadAsStringAsync().Result;
                    Responses? responseData = JsonConvert.DeserializeObject<Responses>(jsonInput ?? "") ?? new Responses();
                    if (responseData.Response != null)
                    {
                        ListNumbers = JsonConvert.DeserializeObject<List<numbersSeals>>(responseData.Response.ToString() ?? "");
                        HttpContext.Session.SetString("token", responseData.Token);
                    }
                }

                return ListNumbers ?? new object { };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                List<numbersSeals>? ListNumbers = new List<numbersSeals>();
                return ListNumbers;
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
                _logger.LogInformation("method called");
                string? token = HttpContext.Session.GetString("token");
                string uri = String.Format("{0}/SaleDocument/ValidateNumbers", urlApi);
                var httpClient = GetHttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var req = new
                {
                    initialNumber = numbers.numeracionInicial,
                    finalNumber = numbers.numeracionFinal,
                    quotaCode = numbers.codigoCupo
                };

                var response = httpClient.PostAsJsonAsync(uri, req).Result;

                if (response.IsSuccessStatusCode)
                {
                    string jsonInput = response.Content.ReadAsStringAsync().Result;
                    Responses? responseData = JsonConvert.DeserializeObject<Responses>(jsonInput ?? "") ?? new Responses();
                    if (responseData.Response != null)
                    {
                        HttpContext.Session.SetString("token", responseData.Token);
                        return responseData.Response;
                    }
                }

                return new object { };
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
                _logger.LogInformation("method called");
                List<Inventory>? inventoryList = new List<Inventory>();

                string? token = HttpContext.Session.GetString("token");
                string uri = String.Format("{0}/SaleDocument/GetInventory?documentNumber={1}&code={2}", urlApi, documentNumber, code);
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
                    Responses? responseData = JsonConvert.DeserializeObject<Responses>(jsonInput ?? "") ?? new Responses();
                    if (responseData.Response != null)
                    {
                        inventoryList = JsonConvert.DeserializeObject<List<Inventory>>(responseData.Response.ToString() ?? "");
                        HttpContext.Session.SetString("token", responseData.Token);
                    }
                }

                return inventoryList ?? new object { };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                List<Inventory>? inventoryList = new List<Inventory>();
                return inventoryList;
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
                List<Species>? speciesList = new List<Species>();

                string? token = HttpContext.Session.GetString("token");
                string uri = String.Format("{0}/SaleDocument/GetSpecies", urlApi);
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
                    Responses? responseData = JsonConvert.DeserializeObject<Responses>(jsonInput ?? "") ?? new Responses();
                    if (responseData.Response != null)
                    {
                        speciesList = JsonConvert.DeserializeObject<List<Species>>(responseData.Response.ToString() ?? "");
                        HttpContext.Session.SetString("token", responseData.Token);
                    }
                }

                return speciesList ?? new object { };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                List<Species>? speciesList = new List<Species>();
                return speciesList;
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
                _logger.LogInformation("method called");
                string? token = HttpContext.Session.GetString("token");
                string uri = String.Format("{0}/SaleDocument/SaveSaleDocument", urlApi);
                var httpClient = GetHttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

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

                var response = httpClient.PostAsJsonAsync(uri, req).Result;

                if (response.IsSuccessStatusCode)
                {
                    string responseString = response.Content.ReadAsStringAsync().Result;
                    Responses? responseData = JsonConvert.DeserializeObject<Responses>(responseString ?? "") ?? new Responses();
                    HttpContext.Session.SetString("token", responseData.Token);
                    return true;
                }
                else
                {
                    return false;
                }
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
                _logger.LogInformation("method called");
                List<Company>? business = new List<Company>();
                string? token = HttpContext.Session.GetString("token");
                string uri = String.Format("{0}/SaleDocument/ValidateCompany?company=" + company + "&typeDocument=" + typeDocument + "&documentNumber=" + documentNumber, urlApi);

                var httpClient = GetHttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = httpClient.GetAsync(uri).Result;
                if (response.IsSuccessStatusCode)
                {
                    string responseString = response.Content.ReadAsStringAsync().Result;
                    Responses? responseData = JsonConvert.DeserializeObject<Responses>(responseString ?? "") ?? new Responses();
                    if (responseData.Response != null)
                    {
                        business = JsonConvert.DeserializeObject<List<Company>>(responseData.Response.ToString() ?? "");
                        HttpContext.Session.SetString("token", responseData.Token);
                    }
                }
                return business ?? new object { };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                List<Company>? business = new List<Company>();
                return business;
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
                _logger.LogInformation("method called");
                List<Company>? company = new List<Company>();
                string? token = HttpContext.Session.GetString("token");
                string uri = String.Format("{0}/SaleDocument/SearchCompany?number={1}&typeDocument={2}&companyCode={3}", urlApi, number, typeDocument, companyCode);

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
                    string responseString = response.Content.ReadAsStringAsync().Result;
                    Responses? responseData = JsonConvert.DeserializeObject<Responses>(responseString ?? "") ?? new Responses();
                    if (responseData.Response != null)
                    {
                        company = JsonConvert.DeserializeObject<List<Company>>(responseData.Response.ToString() ?? "");
                        HttpContext.Session.SetString("token", responseData.Token);
                    }
                }
                return company ?? new object { };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                throw;
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
            var httpClient = GetHttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = httpClient.GetAsync(URI).Result;

            if (response.IsSuccessStatusCode)
            {
                string responseString = response.Content.ReadAsStringAsync().Result;
                result = JsonConvert.DeserializeObject<InfoSalvoConducto>(responseString ?? "") ?? new InfoSalvoConducto();

            }

            return result;
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
                _logger.LogInformation("method called");
                string? token = HttpContext.Session.GetString("token");
                string uri = String.Format("{0}/SaleDocument/DeleteSaleDocument?id=" + id, urlApi);

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
                    return true;
                }
                else
                {
                    return false;
                }
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
                _logger.LogInformation("method called");
                string? token = HttpContext.Session.GetString("token");
                string uri = String.Format("{0}/SaleDocument/UpdateSaleDocument", urlApi);
                var httpClient = GetHttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

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

                var response = httpClient.PutAsJsonAsync(uri, req).Result;

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    return false;
                }
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
                _logger.LogInformation("method called");
                List<SupportDocuments>? supportDocumentsList = new List<SupportDocuments>();
                string? token = HttpContext.Session.GetString("token");
                string uri = "";

                uri = String.Format("{0}/SaleDocument/GetSupportDocument?code=" + code, urlApi);

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
                    Responses? responseData = JsonConvert.DeserializeObject<Responses>(jsonInput ?? "") ?? new Responses();
                    if (responseData.Response != null)
                    {
                        supportDocumentsList = JsonConvert.DeserializeObject<List<SupportDocuments>>(responseData.Response.ToString() ?? "");
                        HttpContext.Session.SetString("token", responseData.Token);
                    }
                }

                return supportDocumentsList ?? new object { };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                List<SupportDocuments>? supportDocumentsList = new List<SupportDocuments>();
                return supportDocumentsList;
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
                _logger.LogInformation("method called");
                List<Quota>? quotaList = new List<Quota>();

                string? token = HttpContext.Session.GetString("token");
                string uri = String.Format("{0}/SaleDocument/GetQuotasByCode?code={1}", urlApi, code);
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
                    Responses? responseData = JsonConvert.DeserializeObject<Responses>(jsonInput ?? "") ?? new Responses();
                    if (responseData.Response != null)
                    {
                        quotaList = JsonConvert.DeserializeObject<List<Quota>>(responseData.Response.ToString() ?? "");
                        HttpContext.Session.SetString("token", responseData.Token);
                    }
                }

                return quotaList ?? new object { };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                List<Quota>? quotaList = new List<Quota>();
                return quotaList;
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
                _logger.LogInformation("method called");
                Seal? numeracion = new Seal();

                string? token = HttpContext.Session.GetString("token");
                string uri = String.Format("{0}/SaleDocument/SearchSeals", urlApi);
                var httpClient = GetHttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = httpClient.PostAsJsonAsync(uri, data).Result;

                if (response.IsSuccessStatusCode)
                {
                    string jsonInput = response.Content.ReadAsStringAsync().Result;
                    Responses? responseData = JsonConvert.DeserializeObject<Responses>(jsonInput ?? "") ?? new Responses();
                    if (responseData.Response != null)
                    {
                        numeracion = JsonConvert.DeserializeObject<Seal>(responseData.Response.ToString() ?? "");
                        HttpContext.Session.SetString("token", responseData.Token);
                    }
                }

                return numeracion ?? new object { };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                Seal? numeracion = new Seal();
                return numeracion;
            }
        }
    }

}
