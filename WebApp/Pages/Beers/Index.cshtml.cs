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
        public string CurrentSort { get; set; } 

       public async Task OnGetAsync(string sortOrder, string searchString)
        {
            CurrentSort = sortOrder;
            IQueryable<Beer> beerQuery = _context.Beer;

            if (!string.IsNullOrEmpty(searchString))
            {
                beerQuery = beerQuery.Where(b => b.Title.Contains(searchString));
                SearchString = searchString;
            }

            switch (sortOrder)
            {
                case "title_desc":
                    beerQuery = beerQuery.OrderByDescending(b => b.Title);
                    break;
                case "releaseDate":
                    beerQuery = beerQuery.OrderBy(b => b.RelaseDate);
                    break;
                case "volume":
                    beerQuery = beerQuery.OrderBy(b => b.Volume);
                    break;
                case "voltage":
                    beerQuery = beerQuery.OrderBy(b => b.Voltage);
                    break;
                case "price":
                    beerQuery = beerQuery.OrderBy(b => b.Price);
                    break;
                default:
                    beerQuery = beerQuery.OrderBy(b => b.Title);
                    break;
            }

            Beer = await beerQuery.AsNoTracking().ToListAsync();
        }
    }
}
