const miModulo = (() => {

    const inicializar = async () => {
        visualizarFondoProcesando();

        const tablaPrecintos = document.querySelector('#TablaEspeciesDatos');
        if (tablaPrecintos)
            await caragarTablaNUmeraciones();

        const tablaVisitaCortes = document.querySelector('#TablaVisitaCortes');
        if (tablaVisitaCortes)
            cargarTablaVisitaCortes();



        let btnSoporteConsignacion = document.querySelector('#btnSoporteConsignacion');
        let btnSoporteDesistimiento = document.getElementById('btnSoporteDesistimiento');
        let btnAnexo = document.querySelectorAll('.btnAnexo');
        let btnSoporteRespuesta = document.querySelectorAll('.btnSoporteRespuesta');

        if (btnAnexo !== null) {
            abrirPDFAnexo();
        }

        if (btnSoporteRespuesta !== null) {
            abrirPDFRespuestas();
        }

        if (btnSoporteDesistimiento !== null) {
            await ObtenerDocumentoSolicitud('btnSoporteDesistimiento', 10169);
        }

        if (btnSoporteConsignacion != null) {
            await ObtenerDocumentoSolicitud('btnSoporteConsignacion', 10109);
        }
        ocultarFondoProcesando();

    }


    const caragarTablaNUmeraciones = async () => {
        let idSolicitud = document.querySelector('#IdSolicitud').value;
        let urlBase = window.location.origin;
        let url = `${urlBase}/CoordinadorGestionPrecintosNacionales/ObtenerListaNumeracionesSolicitud`;
        let resp = await Get(url, { codigoSolicitud: idSolicitud });


       $(`#TablaEspeciesDatos`).DataTable({
            destroy: true,
            scrollX: true,
            lengthChange: true,
            lengthMenu: [5, 10, 20, 50, 100],
            paging: true,
            info: true,
            filter: false,
            dom: "<'row'<'col-md-12 text-end'<'d-flex align-items-center mt-1'<'col-md-10'l><'col-md-2'i>>>>" + "<'row'<'col-md-12'rt><'col-md-12 text-center'p>>",
            language: {
                lengthMenu: "Resultados pág. _MENU_",
                info: "_START_ al _TOTAL_ Resultados",
                search: '',
                searchPlaceholder: "Buscar",
                zeroRecords: "No se encontraron resultados",
                infoEmpty: "0 al 0 Resultados",
                paginate: {
                    previous: 'Anterior',
                    next: 'Siguiente'
                }
            },
            data: resp.listaNumeraciones,
            columns: [
                {
                    "data": "especie",
                },
                {
                    "data": "numeracionInicial",
                },
                {
                    "data": "numeracionFinal",
                }

            ]

        });
        agregarClasesDatatable(`#TablaEspeciesDatos`);
    }

    const abrirPDFAnexo = () => {
        let btnAnexo = document.querySelectorAll('.btnAnexo');
        btnAnexo.forEach(element => {
            element.addEventListener('click', (event) => {
                const Adjunto = document.querySelector(`#base64Anexo_${event.target.id}`).value;
                let newWindow = window.open();
                newWindow.document.write('<iframe src=' + Adjunto + ' style="height:100%; width:100%;"></iframe>');
            })
        })
    };

    const abrirPDFRespuestas = () => {
        let btnAnexo = document.querySelectorAll('.btnSoporteRespuesta');
        btnAnexo.forEach(element => {
            element.addEventListener('click', (event) => {
                const Adjunto = document.querySelector(`#base64Respues_${event.target.id}`).value;
                let newWindow = window.open();
                newWindow.document.write('<iframe src=' + Adjunto + ' style="height:100%; width:100%;"></iframe>');
            })
        })
    };

    async function ObtenerDocumentoSolicitud(boton, documentoTipo) {

        let idSolicitud = document.querySelector('#IdSolicitud').value;
        let url = `/CoordinadorGestionPrecintosNacionales/ObtenerDocumentoSolicitud`;
        let resp = await Get(url, { codigoSolicitud: idSolicitud, tipoDocumento: documentoTipo });

        if (resp !== null) {

            if (resp.documentoSolicitud.adjuntoBase64) {

                $(`#${boton}`).on('click', function () {
                    let newWindow = window.open();
                    newWindow.document.write('<iframe src=' + resp.documentoSolicitud.adjuntoBase64 + ' style="height:100%; width:100%;"></iframe>');
                });

            }

        }
    }


    const cargarTablaVisitaCortes = () => {
        $(`#TablaVisitaCortes`).DataTable({
            destroy: true,
            scrollX: true,
            lengthChange: true,
            lengthMenu: [5, 10, 20, 50, 100],
            paging: true,
            info: true,
            filter: false,
            dom: "<'row'<'col-md-12 text-end'<'d-flex align-items-center mt-1'<'col-md-10'l><'col-md-2'i>>>>" + "<'row'<'col-md-12'rt><'col-md-12 text-center'p>>",
            language: {
                lengthMenu: "Resultados pág. _MENU_",
                info: "_START_ al _TOTAL_ Resultados",
                search: '',
                searchPlaceholder: "Buscar",
                zeroRecords: "No se encontraron resultados",
                infoEmpty: "0 al 0 Resultados",
                paginate: {
                    previous: 'Anterior',
                    next: 'Siguiente'
                }
            },

        });
        agregarClasesDatatable(`#TablaVisitaCortes`);
    }


    return {
        inicializarProcesos: inicializar
    }

})();



document.addEventListener("DOMContentLoaded", () => {
    miModulo.inicializarProcesos();
});
