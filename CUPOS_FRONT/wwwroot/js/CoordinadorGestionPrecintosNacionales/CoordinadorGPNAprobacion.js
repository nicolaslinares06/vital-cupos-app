(() => {

    const formularioAprobacion = document.getElementById("formAprobacionSolicitud");
    let btnAprobarsSolicitud = document.getElementById('btnAprobarsSolicitud');
    let btnConfirmarGuardar = document.getElementById('btnConfirmarGuardar');
    let btnAceptarModal = document.getElementById('btnAceptarModal');


    if (btnAprobarsSolicitud) {
        btnAprobarsSolicitud.addEventListener('click', function () {
            $('#modalFormAprobacion').modal('show');
            formularioAprobacion.reset();
        });

    }

    if (formularioAprobacion) {
        formularioAprobacion.addEventListener("submit", (e) => {
            e.preventDefault();
            $('#modalConfirmacionAprobacion').modal('show');
            let valorTipoEstado = parseInt(document.querySelector('#EstadoSolicitud').value);
            let divMensajeParrafo = document.querySelector('#DivModalParrafoMensaje');
            if (valorTipoEstado === 1) {
                divMensajeParrafo.textContent = "¿Está seguro de devolver al analista?";
            }
            if (valorTipoEstado === 2) {
                divMensajeParrafo.textContent = "¿Desea aprobar esta solicitud?";
            }


        });

    }

    if (btnConfirmarGuardar) {
        btnConfirmarGuardar.addEventListener("click", (e) => {
            e.preventDefault();
            $('#modalConfirmacionAprobacion').modal('hide');
            $('#modalFormAprobacion').modal('hide');
            EnviarDatosAprobacion(formularioAprobacion);
        });

    }

    btnAceptarModal.addEventListener('click', function () {
        let urlBase = window.location.origin;
        window.location = `${urlBase}/CoordinadorGestionPrecintosNacionales/Index`;
    });



    async function EnviarDatosAprobacion(form) {

        visualizarFondoProcesando();
        let formData = new FormData(form);
        const entries = formData.entries();
        const data = Object.fromEntries(entries);
        let urlBase = window.location.origin;
        let url = `${urlBase}/CoordinadorGestionPrecintosNacionales/EnviarDatosAprobacionSolicitud`;
        let resp = await Get(url, { datosActualizacion: data });
        let PmensajeProcesado = document.getElementById('PmensajeProcesado');
        PmensajeProcesado.textContent = resp.mensaje;
        ocultarFondoProcesando();
        $('.mensajes-modal').html('');
        if (data.EstadoSolicitud === "1") {
            $('.mensajes-modal').html('La solicitud ha sido devuelta al analista');
        }

        $('#modalMensaje').modal('show');

    }


})();