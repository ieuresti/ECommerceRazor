// Manejo del evento de eliminación con SweetAlert2

// Selecciona todas las clases de HTML con la clase .btn-delete
document.querySelectorAll('.btn-delete').forEach(button => {
    // Agrega un listener para el evento 'click' en cada botón
    button.addEventListener('click', function () {
        // Obtiene el ID y el nombre de la categoría desde los atributos data-id y data-name del botón
        const categoriaId = this.getAttribute('data-id');
        const categoriaName = this.getAttribute('data-name');

        // Llamada a SweetAlert2 para mostrar el diálogo de confirmación
        Swal.fire({
            title: `¿Estás seguro de eliminar la categoría "${categoriaName}"?`,
            text: "Esta acción no se puede deshacer.",
            icon: "warning",
            showCancelButton: true,
            confirmButtonColor: "#d33",
            cancelButtonColor: "#3085d6",
            confirmButtonText: "Sí, eliminar",
            cancelButtonText: "Cancelar"
        }).then((result) => {
            // Si el usuario confirma, result.isConfirmed será true
            if (result.isConfirmed) {
                // Realiza la solicitud fetch para eliminar la categoría
                fetch("?handler=Delete", {
                    method: "POST",
                    headers: {
                        "Content-Type": "application/json",
                        // Se obtiene el token antifalsificación del formulario oculto generado por Razor
                        "RequestVerificationToken": document.querySelector('input[name="__RequestVerificationToken"]').value
                    },
                    // El cuerpo de la solicitud contiene el ID de la categoría a eliminar
                    body: JSON.stringify(categoriaId)

                })
                // Se espera que el handler devuelva JSON
                .then(response => response.json())
                .then(data => {
                    // Si el borrado fue exitoso mostramos un modal de éxito y recargamos la página
                    if (data.success) {
                        Swal.fire(
                            "Eliminado",
                            `La categoría "${categoriaName}" ha sido eliminada.`,
                            "success"
                        ).then(() => {
                            window.location.reload();
                        });
                    } else {
                        // Si el servidor indica error, lo mostramos con un modal de error
                        Swal.fire(
                            "Error",
                            data.message || "Ocurrió un problema al eliminar la categoría.",
                            "error"
                        );
                    }
                });
            }
        });
    });
});