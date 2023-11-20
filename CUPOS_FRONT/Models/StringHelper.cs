using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Helpers
{
    [ExcludeFromCodeCoverage]
    public static class StringHelper
    {
        public const string estadoAceptada = "Aceptada";
        public const string estadoPendiente = "Pendiente";
        public const string estadoPendienteRadicar = "Pendiente por radicar";
        public const string estadoRadicada = "Radicada";
        public const string estadoEnRequerimiento = "En requerimiento";
        public const string estadoAprobado = "Aprobada";
        public const string estadoDesistimiento = "Desistida";
        public const string estadoRechazada = "Rechazada";
        public const decimal estadoActivo = 72;
        public const decimal estadoInactivo = 73;
        public const string estadoSinRol = "Pendiente por Rol";
        public const decimal estadoValidarCorreo = 10111;
        public const decimal tipoCetificadosFloraNoMaderable = 34;
        public const decimal tipoDocumentoNit = 95;
        public const decimal tipoUsuarioInterno = 103;
        public const decimal tipoUsuarioExterno = 104;
        public const decimal tipoDocumentoAdjuntoOtro = 25;
        public const decimal CodAdminRoles = 6;
        public const decimal CodAdminRolesFuncionalidades = 7;
        public const decimal CodAdminTablasParametricas = 8;
        public const decimal CodAdminTablasServicios = 36;
        public const decimal CodGestionAuditoria = 9;
        public const decimal CodGestionSolicitudesAsigRol = 10;
        public const decimal CodGenerarReportesInformes = 16;
        public const decimal CodAdminEstados = 17;
        public const decimal CodAdminTecnica = 20;
        public const decimal CodGestionDocumental = 21;
        public const decimal CodResolucionAsignacionCUPOS = 11;
        public const decimal CodRegistroDocumentosVenta = 12;
        public const decimal CodGestionarEntidad = 13;
        public const decimal CodResolucionPecesEntidad = 14;
        public const decimal CodFloraNoMaderable = 15;
        public const decimal CodHojaVidaEntidad = 22;
        public const decimal CodPresolucionPecesNivelNacional = 23;
        public const decimal CodPrecintosMarquillas = 24;
        public const decimal CodBandejaPrecintosNacional = 25;
        public const decimal CodBandejaTrabajoUsuarioInterno = 26;
        public const decimal CodBandejaTrabajoValidacionSolicitud = 27;
        public const decimal CodBandejaTrabajoFirmaRespuestaSolicitante = 28;
        public const decimal CodRegistroActaVisitaCortes = 29;
        public const decimal CodGestionarSolicitudesPermisosCITES = 30;
        public const decimal CodGestionPrecintosNacionalesAnalista = 31;
        public const decimal CodGestionPrecintosNacionalesDirector = 32;
        public const decimal CodCoordinadorGestionarPrecintos = 34;
        public const decimal CodGestionUsuario = 35;
        public const decimal codReportesEmpresasMarcajes = 10036;
        public const decimal codReportesPrecintos = 10037;
        public const decimal codReportesMarquillas = 10038;
        public const decimal codReportesCupos = 10039;


        public const decimal estadoCertificadoSolicitado = 10;
        public const decimal estadoCertificadoRadicada = 10166;
        public const decimal estadoCeritificadoEnRequerimiento = 90;
        public const decimal estadoCeritificadoAprobado = 15;
        public const decimal estadoCertificadoDesistimiento = 17;
        public const decimal tipoDocumentoAdjuntoAnexos = 10110;
        public const decimal tipoDocumentoAdjuntoFactura = 10109;

        public const string msgGuardadoExitoso = "Su información ha sido guardada con éxito";
        public const string msgUsuarioCreadoExitoso = "Su información ha sido guardada, y se ha enviado un mensaje al correo del usuario";
        public const string msgEliminadoExitoso = "Su información ha sido eliminada con éxito";
        public const string msgNoEncontradoEliminar = "No se encontro el valor para eliminar";
        public const string msgNoEncontradoEditar = "No se encontro el registro para editar";
        public const string msgNoEncontradoId = "No se encontro el id para realizar la consulta";
        public const string msgNoEncontradoRol = "No se encontro el rol para realizar la consulta";
        public const string msgNoAutorizado = "No Autorizado";
        public const string msgValidarUsuarioClave = "Usuario/Contraseña no valido";
        public const string msgValidarTerminos = "El usuario debe aceptar ambos terminos para poder continuar";
        public const string msgIntenteNuevamente = "Error, intente nuevamente";
        public const string msgArchivoNoEncontrado = "Error, Archivo no Encontrado";
        public const string msgAdministrador = "Por favor contacte con el administrador del sistema, hubo un error";
        public const string msgEmailEnviado = "Se ha enviado un enlace de recuperación al correo electrónico asociado al usuario";
        public const string msgUsuarioNoExiste = "Verifique si el usuario esta escrito correctamente";
        public const string msgInsuficientesPermisos = "Insuficiente permisos de Rol";
        public const string msgRolObligatorio = "Rol es obligatorio para la consulta";
        public const string msgEstadoObligatorio = "Estado es obligatorio para la consulta";
        public const string msgYaExisteElRol = "Ya existe el rol con el nombre: ";
        public const string msgCamposIncompletos = "Campos incompletos";
        public const string msgNoPermisoActualizar = "El usuario no cuenta con los permisos para Actualizar";
        public const string msgNoPermisoEliminar = "El usuario no cuenta con los permisos para Eliminar información";
        public const string msgCrearRoles = "El usuario no cuenta con los permisos para Crear Roles";
        public const string msgCrearUsuarios = "El usuario no cuenta con los permisos para Crear Usuarios";

        public const string msgCodigoEstadoNoEncontrado = "Código de estado no encontrado";

        public static bool esEmailValido(string emailaddress) { 
            try 
            {
                if (emailaddress == null) return false;
                MailAddress m = new MailAddress(emailaddress); return true;
            } 
            catch (FormatException)
            { 
                return false; 
            } 
        }    

        public static string generaContrasenaAleatoria()
        {
            Random rdn = new Random();
            string caracteres = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890%$#@";
            int longitud = caracteres.Length;
            char letra;
            int longitudContrasena = 6;
            string contrasenaAleatoria = string.Empty;
            for (int i = 0; i < longitudContrasena; i++)
            {
                letra = caracteres[rdn.Next(longitud)];
                contrasenaAleatoria += letra.ToString();
            }
            return contrasenaAleatoria;
        }

    }
}
