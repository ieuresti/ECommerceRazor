using ECommerce.DataAccess.Repository.IRepository;
using ECommerce.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ECommerceRazor.Pages.Admin.Ordenes
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public IndexModel(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }
        // Con en default! le indicamos al compilador que esta propiedad se inicializa en otro lugar
        public IEnumerable<Orden> Ordenes { get; set; } = default!;

        public void OnGet()
        {
            Ordenes = _unitOfWork.Orden.GetAll(filter: null, "ApplicationUser");
        }
    }
}
