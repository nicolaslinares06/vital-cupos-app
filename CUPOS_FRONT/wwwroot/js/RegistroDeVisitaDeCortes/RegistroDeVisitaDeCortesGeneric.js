
document.addEventListener("DOMContentLoaded", () => {

    let btnAddInputNrDocumento = document.getElementById("btnAgregarInputDocumento");
    let btnAgregarInputResolucionNro = document.getElementById('btnAgregarInputResolucionNro');
    let btnAgregarInputSalvoConductoNro = document.getElementById('btnAgregarInputSalvoConductoNro');
    const btnBuscarNitEmpresa = document.getElementById('btnBuscarNitEmpresa');

    const btnEliminarDocumentoOrigenPiel = document.querySelectorAll(`.btnEliminarDocumentoOrigenPiel`);
    const btnEliminarSalvoConductoNro = document.querySelectorAll(`.btnEliminarSalvoConductoNro`);
    const btnEliminarResolucionNroOrigenPiel = document.querySelectorAll(`.btnEliminarResolucionNroOrigenPiel`);
    const btnEliminarInputsTipoPiel = document.querySelectorAll(`.btnEliminarInputsTipoPiel`);
    const btnEliminarInputsTipoParteIdentificable = document.querySelectorAll(`.btnEliminarInputsTipoParteIdentificable`);
    const btnEliminarInputsTipoPielIrregular = document.querySelectorAll(`.btnEliminarInputsTipoPielIrregular`);
    const btnEliminarInputsTipoParteIrregular = document.querySelectorAll(`.btnEliminarInputsTipoParteIrregular`);

    if (btnEliminarDocumentoOrigenPiel.length)
        HabilitarEventoEliminar(`DocumentoOrigenPiel`);

    if (btnEliminarSalvoConductoNro.length)
        HabilitarEventoEliminar(`SalvoConductoNro`);

    if (btnEliminarResolucionNroOrigenPiel.length)
        HabilitarEventoEliminar(`ResolucionNroOrigenPiel`);


    if (btnEliminarInputsTipoPiel.length) {
        btnEliminarInputsTipoPiel.forEach((elemento, index) => {
            HabilitarEventoEliminarInputsTipoPiel(elemento.id);
        });
    }

    if (btnEliminarInputsTipoParteIdentificable.length) {
        btnEliminarInputsTipoParteIdentificable.forEach((elemento, index) => {
            EventEliminarInputsTipoParteIdentificable(elemento.id);
        });
    }


    if (btnEliminarInputsTipoPielIrregular.length) {
        btnEliminarInputsTipoPielIrregular.forEach((elemento, index) => {
            HabilitarEventoEliminarInputsTipoPielIrregulares(elemento.id);
        });
    }


    if (btnEliminarInputsTipoParteIrregular.length) {
        btnEliminarInputsTipoParteIrregular.forEach((elemento, index) => {
            HabilitarEventoEliminarInputsTipoParteIrregulares(elemento.id);
        });
    }





    btnAddInputNrDocumento.addEventListener('click', function () {
        const propertysElements = {
            inputName: "DocumentoOrigenPiel",
            placeholderInput: "Nro de Documento",
            idDivPadre: "divOrigenPielNroDoc",
        };
        AgregarInputOrigenPiel(propertysElements);        
        HabilitarEventoEliminar(propertysElements.inputName);
    });

    btnAgregarInputResolucionNro.addEventListener('click', function () {
        const propertysElements = {
            inputName: "ResolucionNroOrigenPiel",
            placeholderInput: "Nro de Resolucion",
            idDivPadre: "divOrigenPielNroResolucion",
        };
        AgregarInputOrigenPiel(propertysElements);
        soloNumeros(`[name = "${propertysElements.inputName}"]`);
        HabilitarEventoEliminar(propertysElements.inputName);
    });

    btnAgregarInputSalvoConductoNro.addEventListener('click', function () {
        const propertysElements = {
            inputName: "SalvoConductoNro",
            placeholderInput: "Nro de salvoConducto",
            idDivPadre: "divOrigenPielNroSalvoConducto",
        };
        AgregarInputOrigenPiel(propertysElements);
        soloNumeros(`[name = "${propertysElements.inputName}"]`);
        HabilitarEventoEliminar(propertysElements.inputName);

    });


    btnBuscarNitEmpresa.addEventListener('click', async function () {

        let empresaNit = document.getElementById('NitEstablecimiento').value;
        let url = `/RegistroVisitaDeCortes/ConsultarDatosEmpresaPorNit`;
        const parametro = { NitEmpresa: empresaNit };
        const resp = await Get(url, parametro);
        document.getElementById('TipoEstablecimiento').value = resp.tipoEstablecimiento;
        document.getElementById('TipoEstablecimientoNombre').value = resp.tipoEstablecimientoNombre;
        document.getElementById('EstablecimientoID').value = resp.establecimientoID;
        document.getElementById('NombreEstablecimiento').value = resp.nombreEstablecimiento;
    });

    $('#Departamento').change(async function () {
        const valorSeleccionado = $(this).val();
        let url = `/RegistroVisitaDeCortes/ConsultarCiudades`;
        const parametro = { departamentoId: valorSeleccionado };
        const resp = await Get(url, parametro);

        const opciones = resp.map(ciudad => `<option value="${ciudad.value}">${ciudad.text}</option>`)
        $('#Ciudad').html(opciones);

    })

    let ArchivoPrecintosMarquilla = document.getElementById('ArchivoPrecintosMarquilla');
    ArchivoPrecintosMarquilla.addEventListener('click', () => {

        ArchivoPrecintosMarquilla.onchange = (e) => {

            validarArchivoPrecintos();
        }


    });
    soloLetras('#RepresentanteEstablecimiento');
    soloNumeros('#NitEstablecimiento');  
    soloNumeros('[name = "ResolucionNroOrigenPiel"]');
    soloNumeros('[name = "SalvoConductoNro"]');
    soloNumeros('#CantidadPielACortar');  
});



function RefrescarEventosPartesIdentificables() {
    soloNumeros('[name = "CantidadTipoPielidntificable"]');
    soloNumeros('[name = "CantidadTipoParteidntificable"]');
    soloLetras('[name = "TipoPielidntificable"]');
    soloLetras('[name = "TipoParteidntificable"]');
}

function RefrescarEventosTiposIrregulares() {   
    soloNumeros('[name = "CantidadTipoPiel"]');
    soloNumeros('[name = "AreaTotalTipoPiel"]');
    soloLetras('[name = "TipoParteCtrlYseguimiento"]');
    soloNumeros('[name = "TipoParteCandidad"]');
    soloNumeros('[name = "TipoParteTotal"]');
    soloNumeros('.no-paste');
}


function AgregarInputOrigenPiel(propiedadesDeElementos) {
    let elementos = document.getElementsByName(propiedadesDeElementos.inputName);
    let ultimoDiv = elementos.length;

    let newDiv = document.createElement("div");
    newDiv.setAttribute('class', 'mb-3 col-12 row align-items-center');
    newDiv.innerHTML = `<div class="col-11"><input class="form-control inputValidation" name="${propiedadesDeElementos.inputName}" id="${propiedadesDeElementos.inputName}_${ultimoDiv}" placeholder="${propiedadesDeElementos.placeholderInput}"/>
      <span class="SPANValidation span-advertencia" id="SPAN${propiedadesDeElementos.inputName}_${ultimoDiv}"> </span></div><div style="color:red;" class="col-1 fa-sharp fa-solid fa-square-minus fa-lg btnEliminar${propiedadesDeElementos.inputName}"></div>`;

    const currentDiv = document.getElementById(`${propiedadesDeElementos.idDivPadre}_0`);
    currentDiv.appendChild(newDiv);
}


function AgregarSeccionControlYSeguimientoTipoPiel(propiedades) {
    let divPadre1 = document.getElementById(propiedades.divPadre1);
    let divPadre2 = document.getElementById(propiedades.divPadre2);
    let divPadre3 = document.getElementById(propiedades.divPadre3);
    let divPadre4 = document.getElementById(propiedades.divPadre4);
    let elementos = document.getElementsByName(propiedades.nameElementos1);
    let ultimoInput = elementos.length;

    let newDiv = document.createElement('div');
    let newDiv2 = document.createElement('div');
    let newDiv3 = document.createElement('div');
    let newDiv4 = document.createElement('div');

    newDiv.setAttribute('class', 'DivInput mb-3');
    newDiv.innerHTML = `<input class="form-control inputSeccionCortesPieles" name="${propiedades.nameElementos1}" id="${propiedades.nameElementos1}_${ultimoInput}" placeholder="Inserte Tipo de Piel" />`;

    newDiv2.setAttribute('class', 'DivInput mb-3');
    newDiv2.innerHTML = `<input class="form-control inputSeccionCortesPieles no-paste" name="${propiedades.nameElementos2}" id="${propiedades.nameElementos2}_${ultimoInput}" placeholder="Inserte Dato" />`;

    newDiv3.setAttribute('class', 'DivInput mb-3');
    newDiv3.innerHTML = `<input class="form-control inputSeccionCortesPieles no-paste" name="${propiedades.nameElementos3}" id="${propiedades.nameElementos3}_${ultimoInput}" placeholder="Inserte cantidad" />`;


    newDiv4.setAttribute('class', 'DivInput mb-3 row align-items-center');
    newDiv4.innerHTML = `<div class="col-11"><input class="form-control inputSeccionCortesPieles no-paste" name="${propiedades.nameElementos4}" id="${propiedades.nameElementos4}_${ultimoInput}" placeholder="Inserte Dato" disabled/></div><div style="color:red;" class="p-0  col-1 fa-sharp fa-solid fa-square-minus fa-lg btnEliminarInputsTipoPielIrregular" id="divEliminarInputsInputsTipoPielIrregular_${ultimoInput}"> </div>`;



    insertAfter(newDiv2, divPadre2.lastElementChild)
    divPadre1.insertBefore(newDiv, divPadre1.lastElementChild);
    insertAfter(newDiv3, divPadre3.lastElementChild);
    insertAfter(newDiv4, divPadre4.lastElementChild);

    HabilitarEventoEliminarInputsTipoPielIrregulares(`divEliminarInputsInputsTipoPielIrregular_${ultimoInput}`);
    habilitarEventoMultiplicar(ultimoInput);



}


async function AgregarSeccionControlYSeguimientoTipoParte(propiedades) {
    let divPadre1 = document.getElementById(propiedades.divPadre1);
    let divPadre2 = document.getElementById(propiedades.divPadre2);
    let divPadre3 = document.getElementById(propiedades.divPadre3);
    let elementos = document.getElementsByName(propiedades.nameElementos1);
    let ultimoInput = elementos.length;

    let newDiv = document.createElement('div');
    let newDiv2 = document.createElement('div');
    let newDiv3 = document.createElement('div');   

 

    newDiv.setAttribute('class', 'DivInput mb-3');
    newDiv.innerHTML = `<input class="form-control inputSeccionCortesPieles" name="${propiedades.nameElementos1}" id="${propiedades.nameElementos1}_${ultimoInput}" placeholder="Inserte un tipo de Parte" />`;

    newDiv2.setAttribute('class', 'DivInput mb-3');
    newDiv2.innerHTML = `<input class="form-control inputSeccionCortesPieles" name="${propiedades.nameElementos2}" id="${propiedades.nameElementos2}_${ultimoInput}" placeholder="Inserte cantidad" />`;

    newDiv3.setAttribute('class', 'DivInput mb-3 row align-items-center');
    newDiv3.innerHTML = `<div class="col-11"><input class="form-control inputSeccionCortesPieles" name="${propiedades.nameElementos3}" id="${propiedades.nameElementos3}_${ultimoInput}" placeholder="Inserte Total" /></div><div style="color:red;" class="p-0  col-1 fa-sharp fa-solid fa-square-minus fa-lg btnEliminarInputsTipoParteIrregular" id="divEliminarInputsInputsTipoParteIrregular_${ultimoInput}"></div>`;


    insertAfter(newDiv2, divPadre2.lastElementChild)
    divPadre1.insertBefore(newDiv, divPadre1.lastElementChild);
    insertAfter(newDiv3, divPadre3.lastElementChild);

    HabilitarEventoEliminarInputsTipoParteIrregulares(`divEliminarInputsInputsTipoParteIrregular_${ultimoInput}`);
}


function insertAfter(newNode, existingNode) {
    existingNode.parentNode.insertBefore(newNode, existingNode.nextSibling);
}


function ValidarDatosFormulario() {
    let camposNoValidos = [];

    let datosValidos = true;
    let SPANSValidation = document.querySelectorAll("input");
    SPANSValidation.forEach(span => {
        span.classList.remove("required-validate");
    });

    let selects = document.querySelectorAll("select");
    selects.forEach((el) => {
        el.classList.remove("required-validate");
    });



    $('.SPANValidation').text("");

    let elementoEstablecimientoTipo = document.getElementById('TipoEstablecimientoNombre');
    let establecimientoTipo = elementoEstablecimientoTipo.value;
    if (!validarCampoVacio(establecimientoTipo)) {
        MensajeErrorCampo(elementoEstablecimientoTipo, document.getElementById('SPANTipoEstablecimientoNombre'), "El campo es obligatorio");
        camposNoValidos.push('Tipo Establecimiento');
        datosValidos = false;
    }


    let elementoNombreEstablecimiento = document.getElementById('NombreEstablecimiento');
    let nombreEstablecimiento = elementoNombreEstablecimiento.value;
    if (!validarCampoVacio(nombreEstablecimiento)) {
        MensajeErrorCampo(elementoNombreEstablecimiento, document.getElementById('SPANNombreEstablecimiento'), "El campo es obligatorio");
        camposNoValidos.push('Nombre Establecimiento');
        datosValidos = false;
    }


    let elementoCantidadPielCorte = document.getElementById('CantidadPielACortar');
    let cantidadPielCorte = elementoCantidadPielCorte.value;
    if (!validarCampoVacio(cantidadPielCorte)) {
        MensajeErrorCampo(elementoCantidadPielCorte, document.getElementById('SPANCantidadPielCorte'), "El campo es obligatorio");
        camposNoValidos.push('Cantidad de Piel a Cortar');
        datosValidos = false;
    }
    if (!ValidarValorNumero(cantidadPielCorte) && (cantidadPielCorte.length > 0)) {
        ComportamientoCampoError(document.getElementById('SPANCantidadPielCorte'), elementoCantidadPielCorte, "El campo debe ser numerico");
        datosValidos = false;
    }

    let elementoNombreRepresentante = document.getElementById('RepresentanteEstablecimiento');
    let nombreRepresentante = elementoNombreRepresentante.value;
    if (!validarCampoVacio(nombreRepresentante)) {
        ComportamientoCampoError(document.getElementById('SPANNombreRepresentante'), elementoNombreRepresentante, "El campo es obligatorio");
        camposNoValidos.push('Nombre de Representante');
        datosValidos = false;
    }

    let elementoDepartamento = document.getElementById('Departamento');
    let Departamento = elementoDepartamento.value;
    if (!validarCampoVacio(Departamento)) {
        ComportamientoCampoError(document.getElementById('SPANDepartamento'), elementoDepartamento, "Selecciona departamento");
        camposNoValidos.push('Departamento');
        datosValidos = false;
    }

    let elementoCiudad = document.getElementById('Ciudad');
    let Ciudad = elementoCiudad.value;
    if (!validarCampoVacio(Ciudad)) {
        ComportamientoCampoError(document.getElementById('SPANCiudad'), elementoCiudad, "Selecciona ciudad");
        camposNoValidos.push('Ciudad');
        datosValidos = false;
    }

    let elementoFechaVisitaCorte = document.getElementById('Fecha');
    let fechaVisitaCorte = elementoFechaVisitaCorte.value;
    if (!validarCampoVacio(fechaVisitaCorte)) {
        ComportamientoCampoError(document.getElementById('SPANFechaVisitaCorte'), elementoFechaVisitaCorte, "El campo es obligatorio");
        camposNoValidos.push('Fecha Visita');
        datosValidos = false;
    }


    let elementoArchivoPrecintosMarquilla = document.getElementById('ArchivoPrecintosMarquilla');
   
    const archivoExcelisEmpty = Object.keys(archivoExcel).length === 0;
    if (archivoExcelisEmpty) {
        ComportamientoCampoError(document.getElementById('SPANArchivoPrecintosMarquilla'), elementoArchivoPrecintosMarquilla, "El campo es obligatorio");
        camposNoValidos.push('Archivo Precintos');
        datosValidos = false;
    }


    const validarDocumentosOrigenPiel = ValidarDatosInputsDocumentos('DocumentoOrigenPiel');
    if (!validarDocumentosOrigenPiel) {
        camposNoValidos.push('Documento de origen de las pieles');       
        datosValidos = false;
    }


    const validarResolucionNroOrigenPiel = ValidarDatosInputsDocumentos('ResolucionNroOrigenPiel');
    if (!validarResolucionNroOrigenPiel) {
        camposNoValidos.push('Resolucion Nro');
        datosValidos = false;
    }
        

    const validarSalvoConductoNro = ValidarDatosInputsDocumentos('SalvoConductoNro');
    if (!validarSalvoConductoNro) {
        camposNoValidos.push('Salvoconducto Nro');
        datosValidos = false;
    }

    if (adjuntosMultipleFile.length <= 0) {
        camposNoValidos.push('Soportes Anexos');
        datosValidos = false;
    }

    if (!datosValidos) {
        modalCamposObligatorios(document.querySelector('#ulCamposOblgatorios'), camposNoValidos);
    }

  

    return datosValidos;

}

function ComportamientoCampoError(SPANElemento, Campo, mensaje) {
    MensajeErrorCampo(Campo, SPANElemento, mensaje);
    Campo.focus();
}

function validarCampoVacio(valor) {
    let esValido = true;
    if (valor === null || valor.trim() === "") {
        esValido = false;
    }
    return esValido;
}

function ValidarValorNumero(valor) {
    let valorNumerico = parseInt(valor);
    let esValido = true;

    if (isNaN(valorNumerico))
        esValido = false;

    return esValido;
}

function MensajeErrorCampo(campo, span, mensaje) {
    DibujarBordeError(campo);
    span.textContent = mensaje;
}

function DibujarBordeError(elemento) {
    elemento.classList.add("required-validate");
}


function ValidarDatosInputsDocumentos(elementoName) {

    let conteoInputs = document.getElementsByName(elementoName);
    if (conteoInputs.length > 0) {

        for (let i = 0; i < conteoInputs.length; i++) {
            let valorINput = document.getElementById(elementoName + '_' + i).value;
            let inputElemento = document.getElementById(elementoName + '_' + i);

            if (!validarCampoVacio(valorINput)) {
                ComportamientoCampoError(document.getElementById(`SPAN${elementoName}_${i}`), inputElemento, "El campo es obligatorio");
                return false;
            }

            if (!ValidarValorNumero(valorINput) && elementoName !== 'DocumentoOrigenPiel') {
                ComportamientoCampoError(document.getElementById(`SPAN${elementoName}_${i}`), inputElemento, "El campo debe ser numerico");
                return false;
            }
        }

    }

    return true;
}


function ValidarDatosTipoPielString(elementoName) {

    let conteoInputs = document.getElementsByName(elementoName);
    if (conteoInputs.length > 0) {

        for (let i = 0; i < conteoInputs.length; i++) {
            let valorINput = document.getElementById(elementoName + '_' + i).value;
            let inputElemento = document.getElementById(elementoName + '_' + i);

            if (!validarCampoVacio(valorINput)) {
                inputElemento.classList.add("required-validate");
                inputElemento.focus();
                return false;
            }
        }

    }

    return true;
}


function ObtenerDatosInputDocumentos(elementName) {


    let arrayDatos = [];
    let conteoInputs = document.getElementsByName(elementName);
    if (conteoInputs.length > 0) {
        for (let i = 0; i < conteoInputs.length; i++) {
            arrayDatos[i] = {
                NumeroDocumento: document.getElementById(`${elementName}_${i}`).value
            };
        }
    }

    return arrayDatos;
}



function ObtenerDatosTipoPielPrimerControl() {


    let arrayDatosTipoPiel = [];

    let conteoRegistrosTipoPiel = document.getElementsByName('TipoPielIrregular');
    if (conteoRegistrosTipoPiel.length > 0) {

        for (let i = 0; i < conteoRegistrosTipoPiel.length; i++) {
            arrayDatosTipoPiel[i] = {
                TipoPielIrregular: document.getElementById('TipoPielIrregular_' + i).value,
                AreaPromedioTipoPiel: document.getElementById('AreaPromedioTipoPiel_' + i).value,
                CantidadTipoPiel: document.getElementById('CantidadTipoPiel_' + i).value,
                AreaTotalTipoPiel: document.getElementById('AreaTotalTipoPiel_' + i).value,
            };
        }

    }

    return arrayDatosTipoPiel;
}


function ObtenerDatosTipoParteSegundoCorte() {


    let arrayDatosTipoParte = [];

    let conteoRegistrosTipoParte = document.getElementsByName('TipoParteCtrlYseguimiento');
    if (conteoRegistrosTipoParte.length > 0) {
        for (let i = 0; i < conteoRegistrosTipoParte.length; i++) {
            let datosTipoParte = {}
            datosTipoParte.TipoParte = document.getElementById('TipoParteCtrlYseguimiento_' + i).value;
            datosTipoParte.CantidadTipoParte = document.getElementById('TipoParteCandidad_' + i).value;
            datosTipoParte.AreaTotalTipoParte = document.getElementById('TipoParteTotal_' + i).value;
            console.log(datosTipoParte);
            arrayDatosTipoParte.push(datosTipoParte);
        }


    }

    return arrayDatosTipoParte;
}

function AgregarSeccionINputTipoPiel() {
    let divPadre1 = document.getElementById('divSubSeccion1TipoPiel');
    let divPadre2 = document.getElementById('divSubSeccion1TipoPielCantidad');
    let elementos = document.getElementsByName('TipoPielidntificable');
    let ultimoInput = elementos.length;

    let newDiv = document.createElement('div');
    let newDiv2 = document.createElement('div');

    newDiv.setAttribute('class', 'DivInput mb-3');
    newDiv.innerHTML = `<input class="form-control inputSeccionCortesPieles inputValidation" name="TipoPielidntificable" id="TipoPielidntificable_${ultimoInput}" placeholder="Inserte un tipo de piel" /> <span class="SPANValidation span-advertencia" id="SPANTipoPielidntificable_${ultimoInput}"> </span>`;
    newDiv2.setAttribute('class', 'DivInput mb-3 row  align-items-center');
    newDiv2.innerHTML = `<div class="col-11"><input class="form-control inputSeccionCortesPieles inputValidation" name="CantidadTipoPielidntificable" id="CantidadTipoPielidntificable_${ultimoInput}" placeholder="Inserte cantidad" /></div>  <div style="color:red;" class="p-0 col-1 col-1 fa-sharp fa-solid fa-square-minus fa-lg btnEliminarInputsTipoPiel"   id="divEliminarTipoPielIdentificable_${ultimoInput}"> </div>`;

    insertAfter(newDiv2, divPadre2.lastElementChild);
    divPadre1.insertBefore(newDiv, divPadre1.lastElementChild);
    HabilitarEventoEliminarInputsTipoPiel(`divEliminarTipoPielIdentificable_${ultimoInput}`);


}

async function  AgregarSeccionInputTipoParte(propiedadesElementos) {
    let divPadre1 = document.getElementById(propiedadesElementos.divPadre1);
    let divPadre2 = document.getElementById(propiedadesElementos.divPadre2);
    let elementos = document.getElementsByName(propiedadesElementos.nameElementos1);
     let ultimoInput = elementos.length;

    let newDiv = document.createElement('div');
    let newDiv2 = document.createElement('div');

    const opciones = await opcionesSelectTiposPartes();

    newDiv.setAttribute('class', 'DivInput mb-3');
    newDiv.innerHTML = `<select class="form-control inputValidation inputSeccionCortesPieles" id="${propiedadesElementos.nameElementos1}_${ultimoInput}"  name="${propiedadesElementos.nameElementos1}"> ${opciones}</select><span class="SPANValidation span-advertencia" id="SPAN${propiedadesElementos.nameElementos1}_${ultimoInput}"> </span>`;
    newDiv2.setAttribute('class', 'DivInput mb-3 row  align-items-center');
    newDiv2.innerHTML = `<div class="col-11"><input class="form-control inputSeccionCortesPieles" name="${propiedadesElementos.nameElementos2}" id="${propiedadesElementos.nameElementos2}_${ultimoInput}" placeholder="Inserte cantidad" /></div> <div style="color:red;" class="p-0 col-1 col-1 fa-sharp fa-solid fa-square-minus fa-lg btnEliminarInputsTipoParteIdentificable" id="divEliminarInputsTipoParteIdentificable_${ultimoInput}"> </div>`;
    divPadre1.insertBefore(newDiv, divPadre1.lastElementChild);
    insertAfter(newDiv2, divPadre2.lastElementChild);
    EventEliminarInputsTipoParteIdentificable(`divEliminarInputsTipoParteIdentificable_${ultimoInput}`);

}




function ObtenerDatosTipoPiel() {


    let arrayDatosTipoPiel = [];

    let conteoRegistrosTipoPiel = document.getElementsByName('TipoPielidntificable');
    if (conteoRegistrosTipoPiel.length > 0) {   
        for (let i = 0; i < conteoRegistrosTipoPiel.length; i++) {
            arrayDatosTipoPiel[i] = {
                TipoPiel: document.getElementById('TipoPielidntificable_' + i).value,
                Cantidad: document.getElementById('CantidadTipoPielidntificable_' + i).value
            };
        }

    }

    return arrayDatosTipoPiel;
}

function ObtenerTipoParteIdentificable() {


    let arrayDatosTipoParte = [];

    let conteoRegistrosParte = document.getElementsByName('TipoParteidntificable');
    if (conteoRegistrosParte.length > 0) {
        for (let i = 0; i < conteoRegistrosParte.length; i++) {
            let datosTipoParte = {}
            datosTipoParte.TipoParte = document.getElementById('TipoParteidntificable_' + i).value;
            datosTipoParte.Cantidad = document.getElementById('CantidadTipoParteidntificable_' + i).value;
            console.log(datosTipoParte);
            arrayDatosTipoParte.push(datosTipoParte);
        }


      

    }

    return arrayDatosTipoParte;
}

function ObtenerValorNumeroVisitaActa() {

    let radioButtonGroup = document.getElementsByName("radioVisita");
    let checkedRadio = Array.from(radioButtonGroup).find((radio) => radio.checked);
    return checkedRadio.value;
}

function ObtenerValorEstadoPiel() {
    let radioButtonGroup = document.getElementsByName("radioEstadosPielCorte");
    let checkedRadio = Array.from(radioButtonGroup).find((radio) => radio.checked);
    return checkedRadio.value;
}

async function ObtenerArchivosActaVisita() {

    let idActaVisita = document.getElementById('ActaVisitaId').value;
    let url = `/RegistroVisitaDeCortes/ObtenerArchivosPost`;
    let resp = await Get(url, { actaVisitaId: idActaVisita });
    if (resp !== null) {
        if (resp.listaArchivos.length > 0) {
            for (let i = 0, codigoIndex = 1; i < resp.listaArchivos.length; i++, codigoIndex++) {

                adjuntosMultipleFile.push({
                    'codigo': codigoIndex,
                    'Base64': resp.listaArchivos[i].base64String,
                    'nombreAdjunto': resp.listaArchivos[i].fileName,
                    'tipoAdjunto': resp.listaArchivos[i].fileType
                });
                adjuntoHTML(contenedorMultipleFile, resp.listaArchivos[i].base64String, resp.listaArchivos[i].fileType, resp.listaArchivos[i].fileName, null, codigoIndex);
            }
        }
    }



}



let fileMultipleFile = document.getElementById('fileMultipleFile');
let contenedorMultipleFile = document.getElementById('contenedorMultipleFile');
let spanFileMultipleFile = document.getElementById('spanFileMultipleFile');
let fileContenedorMultipleFile = document.getElementById('fileContenedorMultipleFile');

let IndividualFileBase64 = '';
let nombreIndividualFile = '';
let tipoAdjuntoIndividualFile = '';
let inc = 0;
let adjuntosMultipleFile = [];


function adjuntoHTML(contenedor, Base64, tipoAdjunto, nombre, inputFile, codigo = null) {
    if (codigo != null) {
        contenedor.innerHTML +=
            `<div class="row w-100 mt-2 contenerAdjuntos" id="divAdjunto${codigo}">
                            <div class="col-11">
                                <a type="buttton" style="cursor: pointer;" class="btnPrevisualizar" id="btnPrevisualizar${codigo}">${nombre}</a>
                            </div>
                            <div class="text-end col-1">
                                <a type="button" class="btnEliminarAdjunto" id="btnEliminarAdjunto${codigo}"><span class="fas fa-times"></span></a>
                            <div>
                        <div>`;
        return;
    }
    contenedor.innerHTML =
        `<div class="row w-100 mt-2 contenerAdjuntos">
                        <div class="col-11">
                            <a type="buttton" id="btnPrevisualizar">${nombre}</a>
                        </div>
                        <div class="text-end col-1">
                            <a type="button" id="btnEliminarAdjunto"><span class="fas fa-times"></span></a>
                        <div>
                    <div>`;
    if (inputFile != null) {
        ocultarElemento(inputFile, true);
    }
    $('#btnPrevisualizar').on('click', function () {
        let newWindow = window.open();
        newWindow.document.write('<iframe src=' + Base64 + ' style="height:100%; width:100%;"></iframe>');
    });
    $('#btnEliminarAdjunto').on('click', function () {
        contenedor.innerHTML = '';
        ocultarElemento(inputFile, false);
    });
    let adjunto = {
        'Base64': Base64,
        'tipoAdjunto': tipoAdjunto,
        'nombre': nombre
    }
    return adjunto;
}

const extensionesValidos = [{ fileType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", extension: ".xlsx" },
{ fileType: "application/pdf", extension: ".pdf" },
{ fileType: "application/vnd.openxmlformats-officedocument.wordprocessingml.document", extension: ".docx" },
{ fileType: "application/vnd.openxmlformats-officedocument.presentationml.presentation", extension: ".pptx" }];

async function mostrarAdjunto(file, contenedor, span, extencionesPermitidas, strExtenciones, inputFile = null, adjuntoMultiple = []) {

    console.log(file)
    const sizeFile = Math.round((file.size / 1024));
    if (sizeFile > 2048) {
        span.innerText = 'Tamaño maximo permitido es de 2MB';
        ocultarElemento(span, false);
        file = '';
        return;
    }


    const archivoValido = extensionesValidos.find(element => {
        if (element.fileType === file.type) {
            return true;
        }

        return false;
    });

    if (archivoValido !== undefined) {
        const Base64 = await toBase64(file);
        if (inputFile == null) {
            inc++;
            let codigo = 0;
            if (adjuntosMultipleFile.length > 0) {
                adjuntosMultipleFile.forEach(el => {
                    if (codigo < el.codigo) {
                        codigo = el.codigo;
                    }
                });
            }
            adjuntoMultiple.push({
                'codigo': codigo == 0 ? inc : codigo + 1,
                'Base64': Base64,
                'nombreAdjunto': file.name,
                'tipoAdjunto': file.type
            });
            contenedor.innerHTML = "";
            adjuntoMultiple.forEach(el => {
                adjuntoHTML(contenedor, el.Base64, el.tipoAdjunto, el.nombreAdjunto, inputFile, el.codigo);
            });
            return;
        }
        return adjuntoHTML(contenedor, Base64, file.type, file.name, inputFile);
    }
    let extensiones = ``;

    extensionesValidos.forEach(archivoTipo => {
        extensiones += ` ${archivoTipo.extension}`;
    });


    span.innerText = 'Solo se admiten tipos de documentos ' + extensiones;
    ocultarElemento(span, false);
    file = '';
}


fileContenedorMultipleFile.addEventListener('dragover', (e) => {
    e.preventDefault();
    ocultarElemento(spanFileMultipleFile, true);
    $(fileContenedorMultipleFile).addClass('activeFile');
});

fileContenedorMultipleFile.addEventListener('dragleave', (e) => {
    e.preventDefault();
    ocultarElemento(spanFileMultipleFile, true);
    $(fileContenedorMultipleFile).removeClass('activeFile');
});

fileContenedorMultipleFile.addEventListener('drop', (e) => {
    e.preventDefault();
    ocultarElemento(spanFileMultipleFile, true);
    $(fileContenedorMultipleFile).removeClass('activeFile');
    let files = e.dataTransfer.files;
    let extencionesPermitidas = ["application/pdf"];
    let strExtenciones = ".pdf";
    console.log(files);
    for (const file of files) {
        mostrarAdjunto(file, contenedorMultipleFile, spanFileMultipleFile, extencionesPermitidas, strExtenciones, null, adjuntosMultipleFile);
    };
});

$('#fileMultipleFile').on('change', async function () {
    ocultarElemento(spanFileMultipleFile, true);
    let dato_archivo = $('#fileMultipleFile').prop("files")[0];
    let extencionesPermitidas = ["application/pdf"];
    let strExtenciones = ".pdf";
    await mostrarAdjunto(dato_archivo, contenedorMultipleFile, spanFileMultipleFile, extencionesPermitidas, strExtenciones, null, adjuntosMultipleFile);
    console.log(adjuntosMultipleFile);
});

$(contenedorMultipleFile).on('click', '.btnPrevisualizar', function (e) {
    let cadena = e.target.parentElement.firstElementChild.id;
    let id = cadena.replace(/btnPrevisualizar/i, "");
    let adjunto = adjuntosMultipleFile.find(p => p.codigo == parseInt(id));
    console.log(adjunto);
    let newWindow = window.open();
    newWindow.document.write('<iframe src=' + adjunto.Base64 + ' style="height:100%; width:100%;"></iframe>');
});

$(contenedorMultipleFile).on('click', '.btnEliminarAdjunto', function (e) {
    let cadena = e.target.parentElement.id;
    let id = cadena.replace(/btnEliminarAdjunto/i, "");
    let adjunto = adjuntosMultipleFile.find(p => p.codigo == parseInt(id));
    adjuntosMultipleFile = adjuntosMultipleFile.filter(p => p.codigo != adjunto.codigo);
    let hijo = document.getElementById(`divAdjunto${adjunto.codigo}`);
    contenedorMultipleFile.removeChild(hijo);
});

function ObtenerArchivosBase64() {

    let arrayArchivosPDF = [];
    if (adjuntosMultipleFile.length > 0) {
        adjuntosMultipleFile.forEach(adjunto => {
            let archivoPDF = {
                FileName: adjunto.nombreAdjunto,
                FileType: adjunto.tipoAdjunto,
                Base64String: adjunto.Base64
            };
            arrayArchivosPDF.push(archivoPDF);
        });
    }
    console.log(arrayArchivosPDF);
    return arrayArchivosPDF;
}


let archivoExcel = {};
async function validarArchivoPrecintos() {
    let input = document.getElementById('ArchivoPrecintosMarquilla');
    let spanValidation = document.getElementById('SPANArchivoPrecintosMarquilla');
    spanValidation.textContent = "";



    if (input.files.length > 0) {

        const fsize = input.files.item(0).size;
        const file = Math.round((fsize / 1024));
        if (file > 2048) {
            spanValidation.textContent = "el tamaño del archivo supera 2MB";
            return;
        }
        let allowedExtensions = /(\.xlsx)$/i;
        let filePath = input.value;

        if (!allowedExtensions.exec(filePath)) {
            spanValidation.textContent = "el archivo debe ser Excel";
            return;
        }

        const errorDatosCuposValido = await validarDatosExcelCupos(input.files.item(0));      
        if (errorDatosCuposValido.error) {
            input.value = null;
            spanValidation.textContent = errorDatosCuposValido.message;
            return;
        }

        archivoExcel.FileName = input.files.item(0).name;
        archivoExcel.FileType = input.files.item(0).type;
        await toBase64(input.files.item(0)).then((valorBase64) => archivoExcel.base64string = valorBase64);

        console.log(archivoExcel);
        let div = document.getElementById('DivMostrarArchivoExcel');
        MostrarAdjuntoExcel(div, archivoExcel.base64string, archivoExcel.FileType, archivoExcel.FileName, input);

    }
}

function MostrarAdjuntoExcel(contenedor, Base64, tipoArchivo, nombre, inputFile) {

    contenedor.innerHTML =
        `<div class="row w-100 mt-2 contenerAdjuntos">
                        <div class="col-11">
                            <a type="buttton" style="cursor: pointer;" id="btnPrevisualizar">${nombre}</a>
                        </div>
                        <div class="text-end col-1">
                            <a type="button" id="btnEliminarAdjunto"><span class="fas fa-times"></span></a>
                        <div>
                    <div>`;
    if (inputFile != null) {
        ocultarElemento(inputFile, true);
    }
    $('#btnPrevisualizar').on('click', function () {
        let newWindow = window.open();
        newWindow.document.write('<iframe src=' + Base64 + ' style="height:100%; width:100%;"></iframe>');
    });
    $('#btnEliminarAdjunto').on('click', function () {
        contenedor.innerHTML = '';
        ocultarElemento(inputFile, false);
        archivoExcel = {};
        inputFile.value = "";
        console.log(archivoExcel);
    });
}


async function ObtenerArchivoExcelPrecinto() {

    let idActaVisita = document.getElementById('ActaVisitaId').value;
    let url = `/RegistroVisitaDeCortes/ObtenerExcelPrecintoActaVisita`;
    let resp = await Get(url, { actaVisitaId: idActaVisita });
    let div = document.getElementById('DivMostrarArchivoExcel');
    let input = document.getElementById('ArchivoPrecintosMarquilla');

    if (resp !== null) {

        if (resp.archivoExcel.base64String) {
            archivoExcel = {
                'FileName': resp.archivoExcel.fileName,
                'FileType': resp.archivoExcel.fileType,
                'Base64String': resp.archivoExcel.base64String

            }

            MostrarAdjuntoExcel(div, resp.archivoExcel.base64String, resp.archivoExcel.fileType, resp.archivoExcel.fileName, input);
        }

    }
}


function soloNumeros(nombreElemento) {


    $(`${nombreElemento}`).on('keypress', function (evt) {
        const regex = /^[0-9]+$/;
        let key = String.fromCharCode(!evt.charCode ? evt.which : evt.charCode);
        if (!regex.test(key)) {
            evt.preventDefault();
            return false;
        }
    });

}

function soloLetras(elemento) {
    $(elemento).on('keypress', function (event) {
        const regex = /^[a-zA-Z\s]+$/;
        let key = String.fromCharCode(!event.charCode ? event.which : event.charCode);
        if (!regex.test(key)) {
            event.preventDefault();
            return false;
        }
    });
}




function HabilitarEventoEliminar(elementoNombre) {
    const btnEliminar = document.querySelectorAll(`.btnEliminar${elementoNombre}`);
    btnEliminar.forEach(box => {
        box.addEventListener('click', function handleClick(event) {
            event.preventDefault();
            event.target.parentElement.remove();
            OrdenarPosicionamientoInput(elementoNombre);
        });
    });
}

function OrdenarPosicionamientoInput(nombreLemento = "") {
    let inputs = document.getElementsByName(nombreLemento);

    inputs.forEach((input, index) => {
        input.id = `${nombreLemento}_${index}`;
        let nextSibling = input.nextElementSibling;
        if (nextSibling)
            nextSibling.id = `SPAN${nombreLemento}_${index}`;
    });
}



function HabilitarEventoEliminarInputsTipoPiel(elemento) {
    const btnEliminar = document.getElementById(elemento);
    btnEliminar.addEventListener('click', function handleClick(event) {
        event.preventDefault();
        let arrayIdElement = event.target.id.split('_');
        const id = arrayIdElement[1];
        const InputTipoPielIdentificable = document.getElementById(`TipoPielidntificable_${id}`);
        InputTipoPielIdentificable.parentElement.remove();
        event.target.parentElement.remove();
        OrdenarPosicionamientoInput(`TipoPielidntificable`);
        OrdenarPosicionamientoInput(`CantidadTipoPielidntificable`);
        OrdenarPosicionamientoElemento(`.btnEliminarInputsTipoPiel`, `divEliminarTipoPielIdentificable`);
    });
}

function EventEliminarInputsTipoParteIdentificable(elemento) {
    const btnEliminar = document.getElementById(elemento);
    btnEliminar.addEventListener('click', function handleClick(event) {
        event.preventDefault();
        let arrayIdElement = event.target.id.split('_');
        const id = arrayIdElement[1];
        const InputTipoPielIdentificable = document.getElementById(`TipoParteidntificable_${id}`);
        InputTipoPielIdentificable.parentElement.remove();
        event.target.parentElement.remove();
        OrdenarPosicionamientoInput(`TipoParteidntificable`);
        OrdenarPosicionamientoInput(`CantidadTipoParteidntificable`);
        OrdenarPosicionamientoElemento(`.btnEliminarInputsTipoParteIdentificable`, `divEliminarInputsTipoParteIdentificable`);
    });
}

function OrdenarPosicionamientoElemento(elemento, nombreLemento) {
    let elementos = document.querySelectorAll(elemento);
    elementos.forEach((elemento, index) => {
        elemento.id = `${nombreLemento}_${index + 1}`;
    });
}


function HabilitarEventoEliminarInputsTipoPielIrregulares(elemento) {
    const btnEliminar = document.getElementById(elemento);
    btnEliminar.addEventListener('click', function handleClick(event) {
        event.preventDefault();
        let arrayIdElement = event.target.id.split('_');
        const id = arrayIdElement[1];
        const TipoPielIrregular = document.getElementById(`TipoPielIrregular_${id}`);
        const AreaPromedioTipoPiel = document.getElementById(`AreaPromedioTipoPiel_${id}`);
        const CantidadTipoPiel = document.getElementById(`CantidadTipoPiel_${id}`);
        TipoPielIrregular.parentElement.remove();
        AreaPromedioTipoPiel.parentElement.remove();
        CantidadTipoPiel.parentElement.remove();
        event.target.parentElement.remove();
        OrdenarPosicionamientoInput(`TipoPielIrregular`);
        OrdenarPosicionamientoInput(`AreaPromedioTipoPiel`);
        OrdenarPosicionamientoInput(`CantidadTipoPiel`);
        OrdenarPosicionamientoInput(`AreaTotalTipoPiel`);
        OrdenarPosicionamientoElemento(`.btnEliminarInputsTipoPielIrregular`, `divEliminarInputsInputsTipoPielIrregular`);
    });
}


function HabilitarEventoEliminarInputsTipoParteIrregulares(elemento) {
    const btnEliminar = document.getElementById(elemento);
    btnEliminar.addEventListener('click', function handleClick(event) {
        event.preventDefault();
        let arrayIdElement = event.target.id.split('_');
        const id = arrayIdElement[1];
        const TipoParteCtrlYseguimiento = document.getElementById(`TipoParteCtrlYseguimiento_${id}`);
        const TipoParteCandidad = document.getElementById(`TipoParteCandidad_${id}`);
        TipoParteCtrlYseguimiento.parentElement.remove();
        TipoParteCandidad.parentElement.remove();
        event.target.parentElement.remove();
        OrdenarPosicionamientoInput(`TipoParteCtrlYseguimiento`);
        OrdenarPosicionamientoInput(`TipoParteCandidad`);
        OrdenarPosicionamientoInput(`TipoParteTotal`);
        OrdenarPosicionamientoElemento(`.btnEliminarInputsTipoParteIrregular`, `divEliminarInputsInputsTipoParteIrregular`);
    });
}


const modalCamposObligatorios = (ul, camposObligatorios) => {
    ul.innerHTML = "";
    camposObligatorios.forEach((el) => {
        ul.innerHTML += `<li class="list-group-item">${el}</li>`;
    });
 
}


const ValidarDatosFormularioIdentificables = () => {
    let camposNoValidos = [];
    let validoValidaciones = true;

    if (!ValidarDatosFormulario()) {
        return false;
    }

    const validarTipoPiel = ValidarDatosTipoPielString('TipoPielidntificable');
    if (!validarTipoPiel) {
        camposNoValidos.push('Tipo de Piel');
        validoValidaciones = false;
    };

    const validarTipoParte = ValidarDatosTipoPielString('TipoParteidntificable');
    if (!validarTipoParte) {
        camposNoValidos.push('Tipo de partes obtenidas');
        validoValidaciones = false;
    }

    if (!validoValidaciones)
        modalCamposObligatorios(document.querySelector('#ulCamposOblgatorios'), camposNoValidos);

    return validoValidaciones;
}


const ValidarDatosFormularioIrregulares = () => {
    let camposNoValidos = [];
    let validoValidaciones = true;

    if (!ValidarDatosFormulario()) {
        return false;
    }
     


    const validarTipoPiel = ValidarDatosTipoPielString('TipoParteCtrlYseguimiento');
    if (!validarTipoPiel) {
        camposNoValidos.push('Tipo de Piel');
        validoValidaciones = false;
    }      


    const validarTipoParte = ValidarDatosTipoPielString('TipoPielIrregular');
    if (!validarTipoParte) {
        camposNoValidos.push('Tipo de partes irregulares');
        validoValidaciones = false;
    }

    if (!validoValidaciones)
        modalCamposObligatorios(document.querySelector('#ulCamposOblgatorios'), camposNoValidos);

    return validoValidaciones;
}

const validarDatosExcelCupos = async (archivoExcel) => {

    let base64Excel = "";
    let nitEmpresa = document.getElementById('NitEstablecimiento').value;

    await toBase64(archivoExcel).then((valorBase64) => base64Excel = valorBase64);
    let url = `/RegistroVisitaDeCortes/ValidarDatoslArchivoPrecintos`;
    let resp = await Get(url, { base64ArchivoExcelPrecintos: base64Excel, nit: nitEmpresa });   
    return resp;

};


const opcionesSelectTiposPartes = async () => {
    let url = `/RegistroVisitaDeCortes/ObtenerTiposPartesLista`;
    const listaTiposPartes = await Get(url);
    const opciones = listaTiposPartes.map(opcion => `<option value="${opcion.value}">${opcion.text}</option>`);
    return opciones;
}


const evitarPegadoInputs = () => {
    const noPasteInputs = document.querySelectorAll(".no-paste");

    noPasteInputs.forEach((input) => {
        input.addEventListener("paste", (event) => {
            event.preventDefault();           
        });
    });
}


const calcularArea = (idInputs) => {
    const input1 = document.getElementById(`AreaPromedioTipoPiel_${idInputs}`);
    const input2 = document.getElementById(`CantidadTipoPiel_${idInputs}`);
    const resultInput = document.getElementById(`AreaTotalTipoPiel_${idInputs}`);

    const value1 = parseFloat(input1.value) || 0;
    const value2 = parseFloat(input2.value) || 0;

    const product = value1 * value2;
    resultInput.value = product;
}

const habilitarEventoMultiplicar = (idInputs) => {
    document.getElementById(`AreaPromedioTipoPiel_${idInputs}`).addEventListener("keyup", () => { calcularArea(idInputs) });
    document.getElementById(`CantidadTipoPiel_${idInputs}`).addEventListener("keyup", () => { calcularArea(idInputs) });
}