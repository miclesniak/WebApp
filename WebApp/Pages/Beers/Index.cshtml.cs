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
    public class IndexModel : PageModel
    {
        private readonly WebApp.Data.WebAppContext _context;

        public IndexModel(WebApp.Data.WebAppContext context)
        {
            _context = context;
        }

        public IList<Beer> Beer { get;set; } = default!;
        public string SearchString { get; set; }

       public async Task OnGetAsync(string searchString)
        {
            IQueryable<Beer> beerQuery = _context.Beer;

            if (!string.IsNullOrEmpty(searchString))
            {
                beerQuery = beerQuery.Where(b => b.Title.Contains(searchString));
                SearchString = searchString;
            }

            Beer = await beerQuery.ToListAsync();
        }
    }
}
