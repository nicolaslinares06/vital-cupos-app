using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Web.Models
{
    public class RegistroVisitaDeCortesModels : RegistroActasBusqueda
    {
        public IEnumerable<SelectListItem> Establecimientos { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> TipoEstablecimientos { get; set; } = new List<SelectListItem>();
        public List<ActaRegistro> ActasRegistros { get; set; } = new List<ActaRegistro>();

    }

    public class PropsSelecListItems
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
    }

    public class ActaRegistro
    {
        public decimal VisitReportId { get; set; }
        public string? ReportType { get; set; }
        public int VisitNumber { get; set; }
        public string? Establishment { get; set; }
        public string? EstablishmentType { get; set; }
        public DateTime Date { get; set; }
        public decimal ReportTypeId { get; set; }
        public string? FechaFormat { get; set; } = "";
        public string? NumerosVisitas { get; set; } = "";
        public bool VisitNumberOne { get; set; }
        public bool VisitNumberTwo { get; set; }
        public decimal? RegistrationStatus { get; set; } = 0;
        public decimal CreationDateDecimal { get; set; }
        public string? FechaRegistroFormat { get; set; }
    }

    public class RegistroActasBusqueda
    {
        public int IdEstablecimiento { get; set; }
        public int IdTipoEstablecimiento { get; set; }

        [DataType(DataType.Date)]
        public DateTime? FechaVisita { get; set; }
        public int TipoBusqueda { get; set; } = 0;

    }

    public class IdRegistrosDocumentos
    {
        public long NumeroDocumento { get; set; }
    }

    public class IdRegistrosDocumentosString
    {
        public string NumeroDocumento { get; set; } = "";
    }

    public class RegistroVisitaCortes
    {
        public decimal VisitaNumero { get; set; }
        public bool VisitaNumero1 { get; set; } = false;
        public bool VisitaNumero2 { get; set; } = false;
        public int TipoEstablecimiento { get; set; }
        public decimal EstablecimientoID { get; set; } = 0;
        public IEnumerable<IdRegistrosDocumentosString> DocumentoOrigenPiel { get; set; } = new List<IdRegistrosDocumentosString>();
        public IEnumerable<IdRegistrosDocumentos> ResolucionorigenPiel { get; set; } = new List<IdRegistrosDocumentos>();
        public IEnumerable<IdRegistrosDocumentos> SalvoCondcutoNumeroOrigenPiel { get; set; } = new List<IdRegistrosDocumentos>();
        public int CantidadPielACortar { get; set; }
        public decimal PrecintoIdentificacion { get; set; } = 0;
        public string? EstadoPiel { get; set; }
        public decimal FuncionarioAutoridadCites { get; set; } = 0;
        public decimal DocumentoRepresentante { get; set; }
        public string? RepresentanteEstablecimiento { get; set; }
        public decimal Ciudad { get; set; }
        public DateTime Fecha { get; set; }
        public ActaVisitasPropArchivos ArchivoExcelPrecinto { get; set; } = new ActaVisitasPropArchivos();



    }

    public class EditVisitaActaCortes
    {
        public decimal ActaVisitaId { get; set; } = 0;
        public decimal VisitaNumero { get; set; } = 0;
        public bool VisitaNumero1 { get; set; } = false;
        public bool VisitaNumero2 { get; set; } = false;
        public decimal TipoEstablecimiento { get; set; } = 0;
        public string? TipoEstablecimientoNombre { get; set; } = "";
        public decimal NitEstablecimiento { get; set; } = 0;
        public decimal EstablecimientoID { get; set; } = 0;
        public string? NombreEstablecimiento { get; set; } = "";
        public decimal CantidadPielACortar { get; set; } = 0;
        public decimal PrecintoIdentificacion { get; set; } = 0;
        public string? EstadoPiel { get; set; } = "";
        public decimal FuncionarioAutoridadCites { get; set; } = 0;
        public decimal DocumentoRepresentante { get; set; } = 0;
        public string? RepresentanteEstablecimiento { get; set; } = "";
        public decimal Departamento { get; set; } = 0;
        public decimal Ciudad { get; set; } = 0;
        public DateTime Fecha { get; set; } = DateTime.Today;
        public string? FechaFormat { get; set; } = "";
        public decimal TipoActaVisita { get; set; } = 0;
        public int EstadoPielInt { get; set; } = 0;
        public string? NombreCiudadDepartamento { get; set; } = "";
        public List<SelectListItem>? Departamentos { get; set; }
        public List<SelectListItem>? Ciudades { get; set; }
        public string FuncionarioAutoridadCitesNombre { get; set; } = "";
        public ActaVisitasPropArchivos ArchivoExcelPrecinto { get; set; } = new ActaVisitasPropArchivos();



    }

    public class EditActaVisitaIdentificableModelView
    {
        public EditVisitaActaCortes ActaVisitaCortes { get; set; } = new EditVisitaActaCortes();
        public List<IdRegistrosDocumentosString> DocumentoOrigenPiel { get; set; } = new List<IdRegistrosDocumentosString>();
        public List<IdRegistrosDocumentos> ResolucionorigenPiel { get; set; } = new List<IdRegistrosDocumentos>();
        public List<IdRegistrosDocumentos> SalvoCondcutoNumeroOrigenPiel { get; set; } = new List<IdRegistrosDocumentos>();
        public IEnumerable<SelectListItem> TiposEstablecimientos { get; set; } = new List<SelectListItem>();
        public List<TipoCortesPielIdentificables> TipoCortesPiel { get; set; } = new List<TipoCortesPielIdentificables>();
        public List<TipoPartesPielIdentificables> TipoPartes { get; set; } = new List<TipoPartesPielIdentificables>();
        public List<ActaVisitasPropArchivos> Archivos { get; set; } = new List<ActaVisitasPropArchivos>();
        public IEnumerable<SelectListItem> TiposPartesPiel { get; set; } = new List<SelectListItem>();


    }

    public class EditActaVisitaIrregularesModelView
    {
        public EditVisitaActaCortes ActaVisitaCortes { get; set; } = new EditVisitaActaCortes();
        public List<IdRegistrosDocumentosString> DocumentoOrigenPiel { get; set; } = new List<IdRegistrosDocumentosString>();
        public List<IdRegistrosDocumentos> ResolucionorigenPiel { get; set; } = new List<IdRegistrosDocumentos>();
        public List<IdRegistrosDocumentos> SalvoCondcutoNumeroOrigenPiel { get; set; } = new List<IdRegistrosDocumentos>();
        public IEnumerable<SelectListItem> TiposEstablecimientos { get; set; } = new List<SelectListItem>();
        public List<TiposPielIrregulares> TipoCortesPiel { get; set; } = new List<TiposPielIrregulares>();
        public List<TiposParteIrregulares> TipoPartes { get; set; } = new List<TiposParteIrregulares>();
        public List<ActaVisitasPropArchivos> Archivos { get; set; } = new List<ActaVisitasPropArchivos>();
        public IEnumerable<SelectListItem> TiposPartesPiel { get; set; } = new List<SelectListItem>();
    }


    public class ConsultActaVisitaModelView
    {
        public EditVisitaActaCortes DatosActaVisita { get; set; } = new EditVisitaActaCortes();
        public List<IdRegistrosDocumentosString> DocumentoOrigenPiel { get; set; } = new List<IdRegistrosDocumentosString>();
        public List<IdRegistrosDocumentos> ResolucionorigenPiel { get; set; } = new List<IdRegistrosDocumentos>();
        public List<IdRegistrosDocumentos> SalvoCondcutoNumeroOrigenPiel { get; set; } = new List<IdRegistrosDocumentos>();
        public List<TiposPielIrregulares> TipoCortesPielIrregulares { get; set; } = new List<TiposPielIrregulares>();
        public List<TiposParteIrregulares> TipoPartesIrregulares { get; set; } = new List<TiposParteIrregulares>();
        public List<TipoCortesPielIdentificables> TipoCortesPielIdentificable { get; set; } = new List<TipoCortesPielIdentificables>();
        public List<TipoPartesPielIdentificables> TipoPartesIdentificables { get; set; } = new List<TipoPartesPielIdentificables>();
        public List<ActaVisitasPropArchivos> Archivos { get; set; } = new List<ActaVisitasPropArchivos>();

    }



    public class CreateRegistroVisitaCortesIdentificablesModelView
    {
        public IEnumerable<SelectListItem> TiposEstablecimientos { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> Departamentos { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> Ciudades { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> TiposPartesPiel { get; set; } = new List<SelectListItem>();
        public EditVisitaActaCortes ActaVisitaCortes { get; set; } = new EditVisitaActaCortes();
    }

    public class CreateRegistroVisitaCortesFraccionesIrregulares
    {
        public IEnumerable<SelectListItem> TiposEstablecimientos { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> Departamentos { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> Ciudades { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> TiposPartesPiel { get; set; } = new List<SelectListItem>();
    }


    public class RegistroVisitaCortesIdentificables : RegistroVisitaCortes
    {
        public List<TipoCortesPielIdentificables> TipoCortesPiel { get; set; } = new List<TipoCortesPielIdentificables>();
        public List<TipoPartesPielIdentificables> TipoPartes { get; set; } = new List<TipoPartesPielIdentificables>();
        public List<ActaVisitasPropArchivos> Archivos { get; set; } = new List<ActaVisitasPropArchivos>();

    }


    public class RegistroVisitaCortesIrregulares : RegistroVisitaCortes
    {
        public List<TiposPielIrregulares> TipoCortesPiel { get; set; } = new List<TiposPielIrregulares>();
        public List<TiposParteIrregulares> TipoPartes { get; set; } = new List<TiposParteIrregulares>();
        public List<ActaVisitasPropArchivos> Archivos { get; set; } = new List<ActaVisitasPropArchivos>();
    }


    public class ActaVisitaDocumentoOrigenPiel
    {
        public decimal A015CodigoActaVisita { get; set; }
        public string A015DocumentoOrigenPielNumero { get; set; } = "";
    }


    public class ActaVisitaResolucionNumero
    {
        public decimal A016CodigoActaVisita { get; set; }
        public decimal A016ResolucionNumero { get; set; }
    }


    public class ActaVisitaSalvoConducto
    {
        public decimal A017CodigoActaVisita { get; set; }
        public decimal A017SalvoConductoNumero { get; set; }
    }
    public class TipoCortesPielIdentificables
    {
        public string? TipoPiel { get; set; } = "";
        public int Cantidad { get; set; }
        public int CodigoActaVisita { get; set; } = 0;
    }

    public class TipoPartesPielIdentificables
    {
        public string? TipoParte { get; set; } = "";
        public int Cantidad { get; set; }
        public int CodigoActaVisita { get; set; } = 0;
    }

    public class TiposPielIrregulares
    {
        public string? TipoPielIrregular { get; set; } = "";
        public string? AreaPromedioTipoPiel { get; set; } = "";
        public decimal CantidadTipoPiel { get; set; }
        public decimal AreaTotalTipoPiel { get; set; }
        public decimal CodigoActaVisita { get; set; }
    }

    public class TiposParteIrregulares
    {
        public string? TipoParte { get; set; } = "";
        public decimal CantidadTipoParte { get; set; }
        public decimal AreaTotalTipoParte { get; set; }
        public decimal CodigoActaVisita { get; set; }
    }

    public class EmpresasEstablecimientos
    {
        public decimal PkT001codigo { get; set; }
        public decimal A001codigoUsuarioCreacion { get; set; }
        public decimal? A001codigoUsuarioModificacion { get; set; }
        public decimal A001codigoParametricaTipoEntidad { get; set; }
        public decimal A001codigoPersonaRepresentantelegal { get; set; }
        public decimal A001codigoCiudad { get; set; }
        public string A001estadoRegistro { get; set; } = null!;
        public DateTime A001fechaCreacion { get; set; }
        public DateTime? A001fechaModificacion { get; set; }
        public decimal A001nit { get; set; }
        public decimal A001telefono { get; set; }
        public string A001nombre { get; set; } = null!;
        public string A001correo { get; set; } = null!;
        public string A001direccion { get; set; } = null!;
        public string A001matriculaMercantil { get; set; } = string.Empty;
    }

    public class ActaVisitasPropArchivos
    {
        public decimal Code { get; set; }
        public string? FileName { get; set; }
        public string? Base64String { get; set; }
        public string? FileType { get; set; }
    }

    public class Departamentos
    {
        public decimal PkT003codigo { get; set; }
        public string? A003nombre { get; set; }
    }

    public class Ciudades
    {
        public decimal PkT004codigo { get; set; }
        public string? A004nombre { get; set; }
    }

    public class EstablecimientoProps
    {
        public decimal TipoEstablecimiento { get; set; }
        public string? TipoEstablecimientoNombre { get; set; } = "";
        public decimal EstablecimientoID { get; set; }
        public string? NombreEstablecimiento { get; set; } = "";
        public decimal NITEstablecimiento { get; set; }

    }

    public class ArchivoExcelPrecintos
    {
        public string base64Excel { get; set; } = "";
        public decimal NIT { get; set; }
    }

}
