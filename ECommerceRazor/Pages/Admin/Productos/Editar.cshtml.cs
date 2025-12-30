using ECommerce.DataAccess.Repository.IRepository;
using ECommerce.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ECommerceRazor.Pages.Admin.Productos
{
    public class EditarModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;

        public EditarModel(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            this._unitOfWork = unitOfWork;
            this._hostEnvironment = hostEnvironment;
        }

        [BindProperty]
        // Esto es para enlazar el modelo Producto con el formulario de la página Razor
        public Producto Producto { get; set; } = default!;

        [BindProperty]
        public IFormFile? ImagenArchivo { get; set; } // Para manejar la carga de imágenes

        // Lista de categorías para el dropdown
        public IEnumerable<SelectListItem> CategoriasList { get; set; }

        public IActionResult OnGet(int id)
        {
            Producto = _unitOfWork.Producto.GetFirstOrDefault(p => p.Id == id);
            if (Producto == null)
            {
                return NotFound();
            }
            // Cargar las categorías desde la base de datos para el dropdown
            CategoriasList = _unitOfWork.Categoria.GetAll().Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Nombre
            });
            if (CategoriasList == null || !CategoriasList.Any())
            {
                ModelState.AddModelError(string.Empty, "No hay categorías disponibles. Por favor, cree una categoría antes de agregar y editar productos.");
            }
            return Page();
        }

        public IActionResult OnPost() 
        {
            // Validacion personalizada: comprobar si el nombre ya existe
            bool nombreExiste = _unitOfWork.Producto.Exists(c => c.Nombre == Producto.Nombre);
            if (nombreExiste)
            {
                ModelState.AddModelError("Producto.Nombre", "El nombre del producto ya existe. Por favor, elija otro nombre.");
                return Page();
            }
            // Validación del modelo de datos
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Manejar la carga de la imagen si se proporcionó un archivo
            if (Producto.ImagenArchivo != null && Producto.ImagenArchivo.Length > 0)
            {
                // Crear una carpeta "img" en wwwroot si no existe
                string uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "img");
                // Generar un nombre de archivo único para evitar colisiones
                string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(Producto.ImagenArchivo.FileName);
                // Crear la carpeta si no existe
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }
                string filePath = Path.Combine(uploadsFolder, uniqueFileName); // Ruta completa del archivo a guardar

                // Restriciones de tamaño
                if (Producto.ImagenArchivo.Length > 2 * 1024 * 1024) // 2 MB
                {
                    ModelState.AddModelError("ImagenArchivo", "El tamaño del archivo no debe exceder 2 MB.");
                    return Page();
                }

                // Restricciones de tipo de archivo
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                if (!allowedExtensions.Contains(Path.GetExtension(Producto.ImagenArchivo.FileName).ToLower()))
                {
                    ModelState.AddModelError("ImagenArchivo", "Solo se permiten archivos de imagen (.jpg, .jpeg, .png, .gif).");
                    return Page();
                }

                // Guardar el archivo en el servidor
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    Producto.ImagenArchivo.CopyTo(fileStream);
                }

                // Eliminar la imagen anterior si existe
                if (!string.IsNullOrEmpty(Producto.Imagen))
                {
                    string oldFilePath = Path.Combine(uploadsFolder, Producto.Imagen);
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                }

                // Asignar la ruta de la imagen al producto
                Producto.Imagen = uniqueFileName;
            }
            else
            {
                // Si no se carga una nueva imagen, mantener la imagen existente
                var productoDesdeDb = _unitOfWork.Producto.GetFirstOrDefault(p => p.Id == Producto.Id);
                if (productoDesdeDb != null)
                {
                    Producto.Imagen = productoDesdeDb.Imagen;
                }
            }

            // Actualizar el producto en la base de datos
            _unitOfWork.Producto.Update(Producto);
            _unitOfWork.Save();

            // Usar TempData para mostrar un mensaje de éxito después de la redirección
            TempData["Success"] = "Producto actualizado exitosamente.";

            return RedirectToPage("Index");
        }
    }
}
