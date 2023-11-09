const moduloIndex = (() => {

    const inicializarPagina =  () => {
        let btnBuscarActasEstablecimiento = document.getElementById('btnBuscarActasEstablecimiento');
        btnBuscarActasEstablecimiento.addEventListener('click', () => {
            CargarTablaActasVisitas();
        });


        $(".btnModalConfirmacion").click(async function () {
            visualizarFondoProcesando();
            let idActaVisita = document.getElementById('HiddenIdActavisita').value;
            let url = `/RegistroVisitaDeCortes/InhabilitarActaVisita`;         
            let resp = await Get(url, { actaVisitaId: idActaVisita });
            let labelMensaje = document.getElementById('labelConfirmacionProceso');
            if (resp) {
                labelMensaje.textContent = "Su información ha sido inhabilitado con éxito";
                ocultarFondoProcesando();
                $('#ModalMensajeError').modal('show');
            }
            else {
                labelMensaje.textContent = "ha ocurrido un error al inhabilitar la informacion";
                ocultarFondoProcesando();
                $('#ModalMensajeError').modal('show');
            }
        });


        $("#btnExitoInhabilitado").click(function () {
            CargarTablaActasVisitas();
            $('#ModalConfirmacioneliminacion').modal('hide');
            $('#ModalMensajeError').modal('hide');
           
        });
    }


    // async function VisualizarInfoActaVisita(actaVisitaId) {
    //    let url = "@Url.Action("ConsultarDatosActaVisita","RegistroVisitaDeCortes")";
    //    let resp = await Get(url, { idActaVisita: actaVisitaId });
    //    if(resp !== null)
    //    {
    //          $("#ModalInfoActaVisita").find(".modal-body").html(resp);
    //          $("#ModalInfoActaVisita").modal({backdrop: "static"});
    //          $("#ModalInfoActaVisita").modal('show');
    //    }

    //}

    async function CargarTablaActasVisitas() {
        visualizarFondoProcesando();

        const tipoEstablecimiento = document.getElementById('IdTipoEstablecimiento').value;
        const establecimientoId = document.querySelector('#IdEstablecimiento').value;
        const fechaVisitaCorte = document.querySelector('#FechaVisita').value;


        const parametros = {
            IdTipoEstablecimiento: tipoEstablecimiento,
            IdEstablecimiento: establecimientoId,
            FechaVisita: fechaVisitaCorte
        }
        let url = `/RegistroVisitaDeCortes/ConsultarActas`;
        let resp = await Get(url, parametros);      


   
       $(`#TablaActas`).DataTable({
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
            data: resp,
            columns: [
                {
                    "data": "reportType",
                },
                {
                    "data": "numerosVisitas",
                },
                {
                    "data": "establishment",
                },
                {
                    "data": "establishmentType"
                },
                {
                    "data": "fechaFormat",
                

                },
                {
                    "data": "fechaRegistroFormat",
               

                },
                {
                    "data": "visitReportId",
                    render: function (data, type, row) {

                        let url = window.location.origin;
                        let opcionesEnlaces = "";

                        let nombreAction = `${url}/RegistroVisitaDeCortes/EditActaCortePartesIdentificables`;
                        if (row.reportTypeId == 2)
                            nombreAction = `${url}/RegistroVisitaDeCortes/EditActaCortesFraccionesIrregulares`;


                        if (row.registrationStatus === 72)
                            opcionesEnlaces = `/ <a href="${nombreAction}?idActaVisita=${data}">
                                    EDITAR</a>/<a href="javascript:moduloIndex.InactivarActavista('${data}');" class="btnConfirmacionInhabilitacion" > ELIMINAR</a>`;

                    

                        return `<a href="${url}/RegistroVisitaDeCortes/ConsultarActaVisita?idactavisita=${data}">
                                    VER</a>${opcionesEnlaces}`;
                    }
                }
            ],
            columnDefs: [{
                targets: 5
            }]

        });
        agregarClasesDatatable(`#TablaActas`);
        ocultarFondoProcesando();
    }

    const InhabilitarActaVisita = (id) => {      
        document.getElementById('HiddenIdActavisita').value = id;
        $("#ModalConfirmacioneliminacion").modal('show');
    }

    const selectTipoEstblecimiento = document.querySelector('#IdTipoEstablecimiento');
    selectTipoEstblecimiento.addEventListener('change', (e) => {
        let id = e.target.value;
        mostrarEstablecimientos(id);


    })

    const mostrarEstablecimientos = async (idTipoEstablecimiento) => {
      
        let url = `/RegistroVisitaDeCortes/ConsultarEstablecimientosPorTipo`;
        const parametro = { tipoEstablecimientoId: idTipoEstablecimiento };
        const resp = await Get(url, parametro);

        const opciones = resp.map(establecimiento => `<option value="${establecimiento.value}">${establecimiento.text}</option>`)
        $('#IdEstablecimiento').html(opciones);
    }


    return {
        inicioPagina: inicializarPagina,
        InactivarActavista: InhabilitarActaVisita,
        mostrarActasVisitas: CargarTablaActasVisitas
      
    };

})();


document.addEventListener("DOMContentLoaded", () => {
    moduloIndex.inicioPagina();
    moduloIndex.mostrarActasVisitas();
});