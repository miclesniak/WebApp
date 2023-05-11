using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.Models;

namespace WebApp.Pages.Beers
{
    public class DetailsModel : PageModel
    {
        private readonly WebApp.Data.WebAppContext _context;

        public DetailsModel(WebApp.Data.WebAppContext context)
        {
            _context = context;
        }

      public Beer Beer { get; set; } = default!; 

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Beer == null)
            {
                return NotFound();
            }

            var beer = await _context.Beer.FirstOrDefaultAsync(m => m.ID == id);
            if (beer == null)
            {
                return NotFound();
            }
            else 
            {
                Beer = beer;
            }
            return Page();
        }
    }
}
