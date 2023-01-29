using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RPInventarios.Data;
using RPInventarios.Models;

namespace RPInventarios.Pages.Departamentos
{
    public class CreateModel : PageModel
    {
        private readonly InventariosContext _context;
        private readonly INotyfService _servicioNotificacion;

        public CreateModel(InventariosContext context, INotyfService servicioNotificacion)
        {
            _context = context;
            _servicioNotificacion = servicioNotificacion;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Departamento Departamento { get; set; }
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid)
            {
                _servicioNotificacion.Error($"Es necesario corregir los problemas para poder crear el departamento {Departamento.Nombre}");
                return Page();
            }

            var existeDepartamentoBd = _context.Departamento.Any(u => u.Nombre.ToLower().Trim() == Departamento.Nombre.ToLower().Trim());
            if (existeDepartamentoBd)
            {
                _servicioNotificacion.Warning($"Ya existe un departamento con el nombre {Departamento.Nombre}");
                return Page();
            }

            _context.Departamento.Add(Departamento);
            await _context.SaveChangesAsync();
            _servicioNotificacion.Success($"Éxito al crear el departamento {Departamento.Nombre}");

            return RedirectToPage("./Index");
        }
    }
}
