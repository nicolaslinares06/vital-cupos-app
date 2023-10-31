


document.addEventListener("DOMContentLoaded", () => {
    cargarTablaSolicitudesAnalistas(0);
});




async function cargarTablaSolicitudesAnalistas(tipoConsulta) {

    let urlBase = window.location.origin;
    let url = `${urlBase}/CoordinadorGestionPrecintosNacionales/ObtenerListado`;
    let resp = await Get(url, { tipoConsultaTabla: tipoConsulta });

    let columns = [

        {
            "data": "numeroRadicado",
        },
        {
            "data": "solicitudPrecintoNacional",
        },
        {
            "data": "nombreEntidadSolicitante",
        },
        {
            "data": "fechaSolicitud",
            render: function (data, type, row) {
                if (data) {
                    let fecha = data.split('T');
                    return fecha[0];
                }

            },
        },
        {
            "data": "fechaRadicacion",

        },
        {
            "data": "estado",

        },
        {
            "data": "numeroRadicacionSalida",
        },
        {
            "data": "fechaRadicacionSalida",
        },
        {
            "data": "accionVisual",
            render: function (data, type, row) {
                console.log(row);
                let url = window.location.origin;
                let nombreAction = `${url}/CoordinadorGestionPrecintosNacionales/${data}?codigoSolicitud=${row.codigoSolicitud}`;

                return `<a href="${nombreAction}">
                      VER</a>`;
            }


        }
    ];    

    const tablaSolicitudes = $('#TablaSolicitudesAnalistas').DataTable({
        destroy: true,
        scrollX: true,
        lengthChange: true,
        lengthMenu: [5, 10, 20, 50, 100],
        paging: true,
        info: true,
        order: [[3, "asc"]],
        dom:
            "<'row'<'col-md-12'f><'col-md-12 text-end'<'d-flex align-items-center mt-1'<'col-md-10'l><'col-md-2'i>>>>" +
            "<'row'<'col-md-12'rt><'col-md-12 text-center'p>>",
        language: {
            lengthMenu: "Resultados pág. _MENU_",
            info: "_START_ al _TOTAL_ Resultados",
            search: "",
            searchPlaceholder: "Buscar",
            zeroRecords: "No se encontraron resultados",
            infoEmpty: "0 al 0 Resultados",
            paginate: {
                previous: "Anterior",
                next: "Siguiente",
            },
        },
        data: resp.listado,
        columns: columns,
        columnDefs: [
            {
                targets: 8,
            },
        ],
    });
    agregarClasesDatatable('#TablaSolicitudesAnalistas');

    tablaSolicitudes.off('page.dt');

    tablaSolicitudes.on('page.dt', () => {
        const { page } = tablaSolicitudes.page.info();
        console.log(tablaSolicitudes.page.info())
        const currentPage = page + 1; // Ajustamos el índice para que empiece desde 1
        console.log(`La página ha cambiado a: ${currentPage}`);
    });

    tablaSolicitudes.on('length.dt', () => {
        const { length } = tablaSolicitudes.page.info();
        console.log(`Número de registros mostrados cambiado a: ${length}`);
    });
}





