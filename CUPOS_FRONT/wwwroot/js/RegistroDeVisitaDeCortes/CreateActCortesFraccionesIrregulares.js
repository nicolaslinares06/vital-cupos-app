
document.addEventListener("DOMContentLoaded", () => {

    let btnGuardarFormularioCorteIrregular = document.getElementById('btnGuardarFormularioCorteIrregular');
    let btnConfirmarGuardar = document.getElementById('btnConfirmarGuardar');
    let btnAceptarModal = document.getElementById('btnAceptarModal');
    const inputsTipoPielIrregular = document.querySelectorAll(`[name = "TipoPielIrregular"]`);

    const select = document.querySelector('#SelectTipoActaCorte');
    select.value = '2';



    btnGuardarFormularioCorteIrregular.addEventListener('click', function () {
        const DatosValidos = ValidarDatosFormularioIrregulares();
        if (DatosValidos)
            $('#modalGuardar').modal('show');
        else
            $('#modalAlerta').modal('show');

    });


    btnConfirmarGuardar.addEventListener('click', function () {
        $('#modalGuardar').modal('hide');
        GuardarDatosCortesFraccionesIrregulares();
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

    //const hijosTipoPiel = parent.querySelectorAll('#divSeccionesPrimerControlYSeguimiento');
    //hijosTipoPiel.classList.add('mb-3');

    //const hijosTipoParte = parent.querySelectorAll('#divSeccionesPrimerControlYSeguimiento');
    //hijosTipoParte.classList.add('mb-3');

    inputsTipoPielIrregular.forEach((input, index) => {
        habilitarEventoMultiplicar(index);
    });

    evitarPegadoInputs();

});







async function GuardarDatosCortesFraccionesIrregulares() {
    visualizarFondoProcesando();

    let datosFormulario = {};
    datosFormulario.VisitaNumero = ObtenerValorNumeroVisitaActa();
    datosFormulario.VisitaNumero1 = document.getElementById("radioVisita1").checked;
    datosFormulario.VisitaNumero2 = document.getElementById("radioVisita2").checked;
    datosFormulario.TipoEstablecimiento = document.getElementById('TipoEstablecimiento').value;
    datosFormulario.EstablecimientoID = document.getElementById('EstablecimientoID').value;
    datosFormulario.CantidadPielACortar = document.getElementById('CantidadPielACortar').value;
    datosFormulario.EstadoPiel = ObtenerValorEstadoPiel();    
    datosFormulario.RepresentanteEstablecimiento = document.getElementById('RepresentanteEstablecimiento').value;
    datosFormulario.DocumentoRepresentante = 0;
    datosFormulario.Ciudad = document.getElementById('Ciudad').value;
    datosFormulario.Fecha = document.getElementById('Fecha').value;
    datosFormulario.TipoCortesPiel = ObtenerDatosTipoPielPrimerControl();
    datosFormulario.TipoPartes = ObtenerDatosTipoParteSegundoCorte();
    datosFormulario.DocumentoOrigenPiel = ObtenerDatosInputDocumentos('DocumentoOrigenPiel');
    datosFormulario.ResolucionorigenPiel = ObtenerDatosInputDocumentos('ResolucionNroOrigenPiel');
    datosFormulario.SalvoCondcutoNumeroOrigenPiel = ObtenerDatosInputDocumentos('SalvoConductoNro');
    datosFormulario.Archivos = ObtenerArchivosBase64();
    datosFormulario.ArchivoExcelPrecinto = archivoExcel;
    let url = `/RegistroVisitaDeCortes/CreateActaVisitaCorteIrregular`;
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




