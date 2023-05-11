using System.Net.Http;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.Models;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace WebApp.Pages.Beers
{
    public class IndexModel : PageModel
    {
        private readonly WebAppContext _context;
        private readonly IHttpClientFactory _httpClientFactory;

        public IndexModel(WebAppContext context, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
        }

        public IList<Beer> Beer { get; set; } = default!;
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

        public async Task<IActionResult> OnPostAddRandomBeerAsync()
        {
            var randomBeerUrl = "https://api.punkapi.com/v2/beers/random";

            try
            {
                if (Request.Form.ContainsKey("addRandomBeer"))
                {
                    using (var httpClient = _httpClientFactory.CreateClient())
                    {
                        var response = await httpClient.GetAsync(randomBeerUrl);
                        response.EnsureSuccessStatusCode();

                        var responseContent = await response.Content.ReadAsStringAsync();
                        var randomBeer = JsonSerializer.Deserialize<Beer>(responseContent);

                        if (randomBeer != null)
                        {
                            _context.Beer.Add(randomBeer);
                            await _context.SaveChangesAsync();
                        }
                    }
                }

                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                // Obsługa błędów - można dodać odpowiednie logowanie lub komunikat dla użytkownika
                return RedirectToPage("./Index");
            }
        }
    }
}
