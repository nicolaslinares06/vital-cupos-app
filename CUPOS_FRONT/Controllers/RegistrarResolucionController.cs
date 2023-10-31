using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Repository.Helpers;
using Repository.Helpers.Models;
using System.Data;
using System.Net.Http.Headers;
using Web.Models;

namespace WebFront.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class RegistrarResolucionController : Controller
    {

        private readonly string UrlApi;
        private readonly ILogger<RegistrarResolucionController> _logger;
        readonly string RUTAAPI = Environment.GetEnvironmentVariable("RUTAAPI");

        public RegistrarResolucionController(ILogger<RegistrarResolucionController> logger)
        {
            UrlApi = string.IsNullOrEmpty(RUTAAPI) ? new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build().GetValue<string>("Variables:RutaApi") : RUTAAPI;
            _logger = logger;
        }
        /// <summary>
        /// htttp client
        /// </summary>
        /// <returns></returns>
        public HttpClient getHttpClient()
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            return new HttpClient(clientHandler);
        }
        /// <summary>
        /// retorna la vista 
        /// </summary>
        /// <param name="documentType"></param>
        /// <param name="nitBussines"></param>
        /// <returns></returns>
        public IActionResult RegistrarResolucion(decimal documentType, string nitBussines)
        {
            try
            {
                var entidades = ConsultEntityDates(documentType, nitBussines, null);
                var entidadViewModel = new Entidad();

                if (entidades.Any())
                {
                    entidadViewModel = entidades[0];
                }

                var model = entidadViewModel;
                _logger.LogInformation("method called");
                ViewBag.nitBussines = nitBussines;
                ViewBag.documentType = documentType;
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
        public IActionResult Ayuda(decimal documentType, string nitBussines)
        {
            try
            {
                _logger.LogInformation("method called");
                string? token = HttpContext.Session.GetString("token");

                if (token != null)
                {
                    ViewBag.nitBussines = nitBussines;
                    ViewBag.documentType = documentType;
                    return View("Views/RegistrarResolucion/Partials/Ayuda.cshtml");
                }

                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                throw;
            }
        }
        /// consultar datos de entidad
        /// </summary>
        /// <param name="documentType"></param>
        /// <param name="nitBussines"></param>
        /// <param name="entityType"></param>
        /// <returns></returns>
        public List<Entidad> ConsultEntityDates(decimal documentType, string nitBussines, decimal? entityType)
        {
            try
            {
                _logger.LogInformation("method called");
                List<Entidad>? datos = new List<Entidad>();
                string? token = HttpContext.Session.GetString("token");
                string URI = UrlApi + "/ResolutionRegister/ConsultEntityDates?documentType=" + documentType + "&nitBussines=" + nitBussines + (entityType == null ? "" : "&entityType=" + entityType);
                var httpClient = getHttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = httpClient.GetAsync(URI).Result;

                //if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                //{
                //    return new
                //    {
                //        volverInicio = true
                //    };
                //}

                if (response.IsSuccessStatusCode)
                {
                    string responseString = response.Content.ReadAsStringAsync().Result;
                    Responses? respuesta = JsonConvert.DeserializeObject<Responses>(responseString);
                   if (respuesta.Response != null)
                    {
                        datos = JsonConvert.DeserializeObject<List<Entidad>>(respuesta.Response.ToString() ?? "");
                        HttpContext.Session.SetString("token", respuesta.Token);
                    }
                }
                if (datos is null)
                    datos = new List<Entidad>();

                return datos;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                throw;
            }
        }

        /// <summary>
        /// consultar todas las resoluciones
        /// </summary>
        /// <param name="nitBussines"></param>
        /// <returns></returns>
        public object ConsultQuotas(string nitBussines)
        {
            try
            {
                _logger.LogInformation("method called");
                List<Cupos>? cupos = new List<Cupos>();
                string? token = HttpContext.Session.GetString("token");
                string URI = UrlApi + "/ResolutionRegister/ConsultQuotas?nitBussines=" + nitBussines;
                var httpClient = getHttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = httpClient.GetAsync(URI).Result;
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
                    Responses? respuesta = JsonConvert.DeserializeObject<Responses>(responseString);
                   if (respuesta.Response != null)
                    {
                        cupos = JsonConvert.DeserializeObject<List<Cupos>>(respuesta.Response.ToString() ?? "");
                        HttpContext.Session.SetString("token", respuesta.Token);
                    }
                    if (cupos != null && cupos.Count > 0)
                    {
                        cupos.ForEach(el =>
                        {
                            el.anioProduccion = el.fechaProduccion.Year;
                        });

                    }
                }
                return cupos ?? new object { };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                throw;
            }
        }

        /// <summary>
        /// consultar cupos por numero de resolucion
        /// </summary>
        /// <param name="ResolutionNumbre"></param>
        /// <returns></returns>
        public object SearchQuotas(decimal ResolutionNumbre)
        {
            try
            {
                _logger.LogInformation("method called");
                List<Cupos>? cupos = new List<Cupos>();
                string? token = HttpContext.Session.GetString("token");
                string URI = UrlApi + "/ResolutionRegister/SearchQuotas?resolutionNumber=" + ResolutionNumbre;
                var httpClient = getHttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = httpClient.GetAsync(URI).Result;
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
                    Responses? respuesta = JsonConvert.DeserializeObject<Responses>(responseString);
                   if (respuesta.Response != null)
                    {
                        cupos = JsonConvert.DeserializeObject<List<Cupos>>(respuesta.Response.ToString() ?? "");
                        HttpContext.Session.SetString("token", respuesta.Token);
                    }
                    if (cupos != null && cupos.Count > 0)
                    {
                        cupos.ForEach(el =>
                        {
                            el.anioProduccion = el.fechaProduccion.Year;
                        });

                    }
                }
                return cupos ?? new object { };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                throw;
            }
        }

        /// <summary>
        /// consultar invenatrio
        /// </summary>
        /// <returns></returns>
        public object ConsultInventory()
        {
            try
            {
                _logger.LogInformation("method called");
                List<InventarioCupos>? inventario = new List<InventarioCupos>();
                string? token = HttpContext.Session.GetString("token");
                string URI = UrlApi + "/ResolutionRegister/ConsultInventory";
                var httpClient = getHttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = httpClient.GetAsync(URI).Result;
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
                    Responses? respuesta = JsonConvert.DeserializeObject<Responses>(responseString);
                   if (respuesta.Response != null)
                    {
                        inventario = JsonConvert.DeserializeObject<List<InventarioCupos>>(respuesta.Response.ToString() ?? "");
                        HttpContext.Session.SetString("token", respuesta.Token);
                    }
                    if (inventario != null && inventario.Count > 0)
                    {
                        inventario.ForEach(el =>
                        {
                            el.anio = el.fechaProduccion.Year;
                        });

                    }
                }
                return inventario ?? new object { };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                throw;
            }
        }

        /// <summary>
        /// consultar tipos de marcaje
        /// </summary>
        /// <returns></returns>
        public object ConsultMarkingType()
        {
            try
            {
                _logger.LogInformation("method called");
                List<ElementTypes>? markingType = new List<ElementTypes>();
                string? token = HttpContext.Session.GetString("token");
                string URI = UrlApi + "/ResolutionRegister/ConsultMarkingType";
                var httpClient = getHttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = httpClient.GetAsync(URI).Result;
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
                    Responses? respuesta = JsonConvert.DeserializeObject<Responses>(responseString);
                   if (respuesta.Response != null)
                    {
                        markingType = JsonConvert.DeserializeObject<List<ElementTypes>>(respuesta.Response.ToString() ?? "");
                        HttpContext.Session.SetString("token", respuesta.Token);
                    }
                }
                return markingType ?? new object { };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                throw;
            }
        }

        /// <summary>
        /// consultar tipos de marcaje
        /// </summary>
        /// <returns></returns>
        public object ConsultEntityTypes()
        {
            try
            {
                _logger.LogInformation("method called");
                List<ElementTypes>? EntityTypes = new List<ElementTypes>();
                string? token = HttpContext.Session.GetString("token");
                string URI = UrlApi + "/ResolutionRegister/ConsultEntityTypes";
                var httpClient = getHttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = httpClient.GetAsync(URI).Result;
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
                    Responses? respuesta = JsonConvert.DeserializeObject<Responses>(responseString);
                   if (respuesta.Response != null)
                    {
                        EntityTypes = JsonConvert.DeserializeObject<List<ElementTypes>>(respuesta.Response.ToString() ?? "");
                        HttpContext.Session.SetString("token", respuesta.Token);
                    }
                }
                return EntityTypes ?? new object { };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                throw;
            }
        }

        /// <summary>
        /// consultar pagos de repoblacion
        /// </summary>
        /// <returns></returns>
        public object ConsultRepoblationPay()
        {
            try
            {
                _logger.LogInformation("method called");
                List<ElementTypes>? RepoblationPay = new List<ElementTypes>();
                string? token = HttpContext.Session.GetString("token");
                string URI = UrlApi + "/ResolutionRegister/ConsultRepoblationPay";
                var httpClient = getHttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = httpClient.GetAsync(URI).Result;
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
                    Responses? respuesta = JsonConvert.DeserializeObject<Responses>(responseString);
                   if (respuesta.Response != null)
                    {
                        RepoblationPay = JsonConvert.DeserializeObject<List<ElementTypes>>(respuesta.Response.ToString() ?? "");
                        HttpContext.Session.SetString("token", respuesta.Token);
                    }
                }
                return RepoblationPay ?? new object { };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                throw;
            }
        }

        /// <summary>
        /// consultar tipos de especimenes
        /// </summary>
        /// <returns></returns>
        public object ConsultEspecimensTypes()
        {
            try
            {
                _logger.LogInformation("method called");
                List<ElementTypesEspecies>? EspecimentsTypes = new List<ElementTypesEspecies>();
                string? token = HttpContext.Session.GetString("token");
                string URI = UrlApi + "/ResolutionRegister/ConsultEspecimensTypes";
                var httpClient = getHttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = httpClient.GetAsync(URI).Result;
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
                    Responses? respuesta = JsonConvert.DeserializeObject<Responses>(responseString);
                   if (respuesta.Response != null)
                    {
                        EspecimentsTypes = JsonConvert.DeserializeObject<List<ElementTypesEspecies>>(respuesta.Response.ToString() ?? "");
                        HttpContext.Session.SetString("token", respuesta.Token);
                    }
                }
                return EspecimentsTypes ?? new object { };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                throw;
            }
        }

        /// <summary>
        /// consultar una resolucion por codigo
        /// </summary>
        /// <param name="quotaCode"></param>
        /// <returns></returns>
        public object ConsultOneQuota(decimal quotaCode)
        {
            try
            {
                _logger.LogInformation("method called");
                resolutionQuota? resolutionQuota = new resolutionQuota();
                string? token = HttpContext.Session.GetString("token");
                string URI = UrlApi + "/ResolutionRegister/ConsultOneQuota?quotaCode=" + quotaCode;
                var httpClient = getHttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = httpClient.GetAsync(URI).Result;
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
                    Responses? respuesta = JsonConvert.DeserializeObject<Responses>(responseString);
                   if (respuesta.Response != null)
                    {
                        resolutionQuota = JsonConvert.DeserializeObject<resolutionQuota>(respuesta.Response.ToString() ?? "");
                        HttpContext.Session.SetString("token", respuesta.Token);
                    }
                }
                return resolutionQuota ?? new object { };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                throw;
            }
        }

        /// <summary>
        /// editar resolucion
        /// </summary>
        /// <param name="datos"></param>
        /// <returns></returns>
        public object EditaEliminarResolucionEspecieExportar(saveResolutionQuotas datos)
        {
            try
            {
                _logger.LogInformation("method called");
                string? token = HttpContext.Session.GetString("token");
                string URI = UrlApi + "/ResolutionRegister/EditDeleteResolutionQuota";
                var httpClient = getHttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var req = new
                {
                    dataToSave = new
                    {
                        quotaCode = datos.datoGuardar.codigoCupo,
                        issuingAuthority = datos.datoGuardar.autoridadEmiteResolucion,
                        zoocriaderoCode = datos.datoGuardar.codigoZoocriadero,
                        resolutionNumber = datos.datoGuardar.numeroResolucion,
                        resolutionDate = datos.datoGuardar.fechaResolucion,
                        resolutionRegistrationDate = datos.datoGuardar.fechaRegistroResolucion,
                        observations = datos.datoGuardar.observaciones,
                        companyNit = datos.datoGuardar.nitEmpresa
                    },
                    newExportSpeciesData = datos.datosEspecieExportarNuevo?.Select(especie => new
                    {
                        PkT005code = especie.PkT005codigo,
                        quotaCode = especie.codigoCupo,
                        year = especie.anio,
                        quotas = especie.cupos,
                        id = especie.id,
                        markingTypeParametricCode = especie.codigoParametricaTipoMarcaje,
                        speciesCode = especie.codigoEspecie,
                        repopulationQuotaPaymentParametricCode = especie.codigoParametricaPagoCuotaDerepoblacion,
                        filingDate = especie.fechaRadicado,
                        specimenType = especie.tipoEspecimen,
                        productionYear = especie.añoProduccion,
                        batchCode = especie.marcaLote,
                        markingConditions = especie.condicionesMarcaje,
                        parentalPopulationMale = especie.poblacionParentalMacho,
                        parentalPopulationFemale = especie.poblacionParentalHembra,
                        totalParentalPopulation = especie.poblacionParentalTotal,
                        populationFromIncubator = especie.poblacionSalioDeIncubadora,
                        populationAvailableForQuotas = especie.poblacionDisponibleParaCupos,
                        individualsForRepopulation = especie.individuosDestinadosARepoblacion,
                        grantedUtilizationQuotas = especie.cupoAprovechamientoOtorgados,
                        replacementRate = especie.tasaReposicion,
                        mortalityNumber = especie.numeroMortalidad,
                        repopulationQuotaForUtilization = especie.cuotaRepoblacionParaAprovechamiento,
                        grantedPaidUtilizationQuotas = especie.cupoAprovechamientoOtorgadosPagados,
                        observations = especie.observaciones,
                        repopulationQuota = especie.cuotaRepoblacion,
                        initialRepopulationQuotaInternalNumber = especie.numeroInternoInicialCuotaRepoblacion,
                        finalRepopulationQuotaInternalNumber = especie.numeroInternoFinalCuotaRepoblacion,
                        initialInternalNumber = especie.numeroInternoInicial,
                        finalInternalNumber = especie.numeroInternoFinal,
                        totalQuotas = especie.totalCupos,
                        availableQuotas = especie.cuposDisponibles,
                        repopulationQuotaPaid = especie.sePagoCuotaRepoblacion,
                        speciesRegisteredForCommercialization = especie.seRegistroEspecieComercializar,
                        supportingDocuments = especie.documentosSoporte?.Select(doc => new
                        {
                            code = doc.codigo,
                            base64Attachment = doc.adjuntoBase64,
                            attachmentName = doc.nombreAdjunto,
                            attachmentType = doc.tipoAdjunto
                        }).ToList(),
                        newSupportingDocuments = especie.documentosSoporteNuevos
                            ?.Select(doc => new
                            {
                                code = doc.codigo,
                                base64Attachment = doc.adjuntoBase64,
                                attachmentName = doc.nombreAdjunto,
                                attachmentType = doc.tipoAdjunto
                            }).ToList(),
                        deletedSupportingDocuments = especie.documentosSoporteEliminar?.Select(doc => new
                        {
                            code = doc.codigo,
                            base64Attachment = doc.adjuntoBase64,
                            attachmentName = doc.nombreAdjunto,
                            attachmentType = doc.tipoAdjunto
                        }).ToList()
                    }).ToList()
                };

                var response = httpClient.PostAsJsonAsync(URI, req).Result;
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
                    Responses? respuesta = JsonConvert.DeserializeObject<Responses>(responseString);
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                throw;
            }
        }

        /// <summary>
        /// gusrdar resolucion nueva
        /// </summary>
        /// <param name="datos"></param>
        /// <returns></returns>
        public object GuardarResolucionEspecieExportar(saveResolutionQuotas datos)
        {
            try
            {
                _logger.LogInformation("method called");
                string? token = HttpContext.Session.GetString("token");
                string URI = UrlApi + "/ResolutionRegister/saveResolutionQuota";
                var httpClient = getHttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var req = new
                {
                    dataToSave = new
                    {
                        quotaCode = datos.datoGuardar.codigoCupo,
                        issuingAuthority = datos.datoGuardar.autoridadEmiteResolucion,
                        zoocriaderoCode = datos.datoGuardar.codigoZoocriadero,
                        resolutionNumber = datos.datoGuardar.numeroResolucion,
                        resolutionDate = datos.datoGuardar.fechaResolucion,
                        resolutionRegistrationDate = datos.datoGuardar.fechaRegistroResolucion,
                        observations = datos.datoGuardar.observaciones,
                        companyNit = datos.datoGuardar.nitEmpresa
                    },
                    newExportSpeciesData = datos.datosEspecieExportarNuevo?.Select(especie => new
                    {
                        PkT005code = especie.PkT005codigo,
                        quotaCode = especie.codigoCupo,
                        year = especie.anio,
                        quotas = especie.cupos,
                        id = especie.id,
                        markingTypeParametricCode = especie.codigoParametricaTipoMarcaje,
                        speciesCode = especie.codigoEspecie,
                        repopulationQuotaPaymentParametricCode = especie.codigoParametricaPagoCuotaDerepoblacion,
                        filingDate = especie.fechaRadicado,
                        specimenType = especie.tipoEspecimen,
                        productionYear = especie.añoProduccion,
                        batchCode = especie.marcaLote,
                        markingConditions = especie.condicionesMarcaje,
                        parentalPopulationMale = especie.poblacionParentalMacho,
                        parentalPopulationFemale = especie.poblacionParentalHembra,
                        totalParentalPopulation = especie.poblacionParentalTotal,
                        populationFromIncubator = especie.poblacionSalioDeIncubadora,
                        populationAvailableForQuotas = especie.poblacionDisponibleParaCupos,
                        individualsForRepopulation = especie.individuosDestinadosARepoblacion,
                        grantedUtilizationQuotas = especie.cupoAprovechamientoOtorgados,
                        replacementRate = especie.tasaReposicion,
                        mortalityNumber = especie.numeroMortalidad,
                        repopulationQuotaForUtilization = especie.cuotaRepoblacionParaAprovechamiento,
                        grantedPaidUtilizationQuotas = especie.cupoAprovechamientoOtorgadosPagados,
                        observations = especie.observaciones,
                        repopulationQuota = especie.cuotaRepoblacion,
                        initialRepopulationQuotaInternalNumber = especie.numeroInternoInicialCuotaRepoblacion,
                        finalRepopulationQuotaInternalNumber = especie.numeroInternoFinalCuotaRepoblacion,
                        initialInternalNumber = especie.numeroInternoInicial,
                        finalInternalNumber = especie.numeroInternoFinal,
                        totalQuotas = especie.totalCupos,
                        availableQuotas = especie.cuposDisponibles,
                        repopulationQuotaPaid = especie.sePagoCuotaRepoblacion,
                        speciesRegisteredForCommercialization = especie.seRegistroEspecieComercializar,
                        supportingDocuments = especie.documentosSoporte?.Select(doc => new
                        {
                            code = doc.codigo,
                            base64Attachment = doc.adjuntoBase64,
                            attachmentName = doc.nombreAdjunto,
                            attachmentType = doc.tipoAdjunto
                        }).ToList(),
                        newSupportingDocuments = especie.documentosSoporteNuevos
                            ?.Select(doc => new
                            {
                                code = doc.codigo,
                                base64Attachment = doc.adjuntoBase64,
                                attachmentName = doc.nombreAdjunto,
                                attachmentType = doc.tipoAdjunto
                            }).ToList(),
                        deletedSupportingDocuments = especie.documentosSoporteEliminar?.Select(doc => new
                        {
                            code = doc.codigo,
                            base64Attachment = doc.adjuntoBase64,
                            attachmentName = doc.nombreAdjunto,
                            attachmentType = doc.tipoAdjunto
                        }).ToList()
                    }).ToList()
                };

                var response = httpClient.PostAsJsonAsync(URI, req).Result;

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
                    Responses? respuesta = JsonConvert.DeserializeObject<Responses>(responseString);
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                throw;
            }
        }

        /// <summary>
        /// deshabilitar resolucion
        /// </summary>
        /// <param name="quotaCode"></param>
        /// <returns></returns>
        public object DisableResolution(decimal quotaCode)
        {
            try
            {
                _logger.LogInformation("method called");
                string? token = HttpContext.Session.GetString("token");
                string URI = UrlApi + "/ResolutionRegister/DisableResolution?quotaCode=" + quotaCode;
                var httpClient = getHttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = httpClient.GetAsync(URI).Result;
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
                    Responses? respuesta = JsonConvert.DeserializeObject<Responses>(responseString);
                   if (respuesta.Response != null)
                    {
                        HttpContext.Session.SetString("token", respuesta.Token);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the method.");
                throw;
            }
        }
    }
}
