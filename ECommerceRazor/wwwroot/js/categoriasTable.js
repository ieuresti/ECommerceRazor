// Inicialización de DataTables con configuración en español cuando el documento esté listo
$(document).ready(function () {
    // Inicializa DataTables en la tabla con ID 'categoriasTable'
    $('#categoriasTable').DataTable({
        // Configuración de idioma en español
        language: {
            url: "//cdn.datatables.net/plug-ins/1.13.6/i18n/es-ES.json"
        },
        pageLength: 10, // Número de filas por página
        ordering: true, // Habilitar ordenamiento
        searching: true // Habilitar búsqueda
    });
});