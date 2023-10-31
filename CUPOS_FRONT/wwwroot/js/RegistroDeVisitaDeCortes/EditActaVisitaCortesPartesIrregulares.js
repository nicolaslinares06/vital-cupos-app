
document.addEventListener("DOMContentLoaded", async () => {
    visualizarFondoProcesando();
    let btnEditarFormularioCorteIrregular = document.getElementById('btnEditarFormularioCorteIrregular');
    let btnConfirmarGuardar = document.getElementById('btnConfirmarGuardar');
    let btnAceptarModal = document.getElementById('btnAceptarModal');

    let hiddenRadioEstadoPiel = parseInt(document.getElementById('HiddenEstadoPiel').value);
    let radioButtonEstadoPiel = document.getElementById(`EstadoPiel${hiddenRadioEstadoPiel}`);
    radioButtonEstadoPiel.checked = true;

    const inputsTipoPielIrregular = document.querySelectorAll(`[name = "TipoPielIrregular"]`);


    inputsTipoPielIrregular.forEach((input, index) => {
        habilitarEventoMultiplicar(index);
    });


    //let hiddenRadioNumeroVisita = parseInt(document.getElementById('HiddenVisitaNro').value);
    //let radioButtonNumeroVisita = document.getElementById(`radioVisita${hiddenRadioNumeroVisita}`);
    //radioButtonNumeroVisita.checked = true;



    btnEditarFormularioCorteIrregular.addEventListener('click', function () {
        const DatosValidos = ValidarDatosFormularioIrregulares();
        if (DatosValidos)
            $('#modalGuardar').modal('show');
        else
            $('#modalAlerta').modal('show');

    });


    btnConfirmarGuardar.addEventListener('click', function () {
        $('#modalGuardar').modal('hide');
        EditarDatosCortesPielIrregulares();
    });

    btnAceptarModal.addEventListener('click', function () {
        let valorResultado = parseInt(document.getElementById('HiddenResultadoProceso').value);
        if (valorResultado > 0) {
            let urlBase = window.location.origin;
            window.location = `${urlBase}/RegistroVisitaDeCortes/Index`;
        }

        else {
            $('#modalMensaje').modal('hide');
        }
    });


    await ObtenerArchivosActaVisita();

    let btnAgregarTipoPielIrregular = document.getElementById("btnAgregarTipoPielIrregular");
    btnAgregarTipoPielIrregular.addEventListener('click', function () {
        const CntrlSeguimientoTipoPielpropertys = {
            divPadre1: "divSubSeccion1ControlYSeguimientoTipoPiel",
            divPadre2: "divSeccion2ControlYSeguimientoTipoPiel",
            divPadre3: "divSeccion3ControlYSeguimientoTipoPiel",
            divPadre4: "divSeccion4ControlYSeguimientoTipoPiel",
            nameElementos1: "TipoPielIrregular",
            nameElementos2: "AreaPromedioTipoPiel",
            nameElementos3: "CantidadTipoPiel",
            nameElementos4: "AreaTotalTipoPiel",
            divTataraAbuelo: "divSeccionesPrimerControlYSeguimiento",
        };
        AgregarSeccionControlYSeguimientoTipoPiel(CntrlSeguimientoTipoPielpropertys);
        RefrescarEventosTiposIrregulares();
        evitarPegadoInputs();
    });

    let btnAgregarTipoParteIrregular = document.getElementById("btnAgregarTipoParteIrregular");
    btnAgregarTipoParteIrregular.addEventListener('click', function () {
        const CntrlSeguimientoTipoPartepropertys = {
            divPadre1: "divSubSeccion1CorteYSeguimientoTipoParte",
            divPadre2: "divSubSeccion2TipoParteCantidad",
            divPadre3: "divSubSeccion3TipoParteTotal",
            nameElementos1: "TipoParteCtrlYseguimiento",
            nameElementos2: "TipoParteCandidad",
            nameElementos3: "TipoParteTotal",
            divTataraAbuelo: "divContenedorTipoParteCntrlSeguimiento",
        };
        AgregarSeccionControlYSeguimientoTipoParte(CntrlSeguimientoTipoPartepropertys);
        RefrescarEventosTiposIrregulares();
    });

    RefrescarEventosTiposIrregulares();
    await ObtenerArchivoExcelPrecinto();
    evitarPegadoInputs();
    ocultarFondoProcesando();

});



async function EditarDatosCortesPielIrregulares() {
    visualizarFondoProcesando();
    let datosFormulario = {};
    datosFormulario.ActaVisitaCortes = ObtenerDatosActaVisita();
    datosFormulario.TipoCortesPiel = ObtenerDatosTipoPielPrimerControl();
    datosFormulario.TipoPartes = ObtenerDatosTipoParteSegundoCorte();
    datosFormulario.DocumentoOrigenPiel = ObtenerDatosInputDocumentos('DocumentoOrigenPiel');
    datosFormulario.ResolucionorigenPiel = ObtenerDatosInputDocumentos('ResolucionNroOrigenPiel');
    datosFormulario.SalvoCondcutoNumeroOrigenPiel = ObtenerDatosInputDocumentos('SalvoConductoNro');
    datosFormulario.Archivos = ObtenerArchivosBase64();
    let url = `/RegistroVisitaDeCortes/EditActaVisitaCortesIrregulares`;
    let resp = await Get(url, datosFormulario);
    if (resp !== null) {
        if (!resp.Error)
            resultadoProceso = 1;
        document.getElementById('PmensajeProcesado').innerHTML = resp.mensaje;

    }
    document.getElementById('HiddenResultadoProceso').value = resultadoProceso;
    ocultarFondoProcesando();
    $('#modalMensaje').modal('show');
}

function ObtenerDatosActaVisita() {
    let datosActaVisita = {};

    datosActaVisita.ActaVisitaId = document.getElementById("ActaVisitaId").value;
    datosActaVisita.VisitaNumero = ObtenerValorNumeroVisitaActa();
    datosActaVisita.VisitaNumero1 = document.getElementById("radioVisita1").checked;
    datosActaVisita.VisitaNumero2 = document.getElementById("radioVisita2").checked;
    datosActaVisita.TipoEstablecimiento = document.getElementById('TipoEstablecimiento').value;
    datosActaVisita.EstablecimientoID = document.getElementById('EstablecimientoID').value;
    datosActaVisita.CantidadPielACortar = document.getElementById('CantidadPielACortar').value;
    /* datosActaVisita.PrecintoIdentificacion = document.getElementById('PrecintoIdentificacion').value;*/
    datosActaVisita.EstadoPiel = ObtenerValorEstadoPiel();
    datosActaVisita.RepresentanteEstablecimiento = document.getElementById('RepresentanteEstablecimiento').value;
    datosActaVisita.DocumentoRepresentante = 0;
    datosActaVisita.Ciudad = document.getElementById('Ciudad').value;
    datosActaVisita.Departamento = document.getElementById('Departamento').value;
    datosActaVisita.Fecha = document.getElementById('Fecha').value;
    datosActaVisita.ArchivoExcelPrecinto = archivoExcel;

    return datosActaVisita;
}

