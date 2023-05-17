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
using System.Globalization;

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

        public List<Beer> Beer { get; set; } = new List<Beer>();
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
            Random rand = new Random();

            try
            {
                if (Request.Form.ContainsKey("addRandomBeer"))
                {
                    using (var httpClient = _httpClientFactory.CreateClient())
                    {
                        var response = await httpClient.GetAsync(randomBeerUrl);
                        response.EnsureSuccessStatusCode();

                        var responseContent = await response.Content.ReadAsStringAsync();
                        var apiBeers = JsonSerializer.Deserialize<List<ApiBeer>>(responseContent);
                        var apiBeer = apiBeers?.FirstOrDefault();
                        Console.WriteLine(apiBeers);

                        if (apiBeer != null)
                        {
                            Beer beer = new Beer
                            {
                                Title = apiBeer.name,
                                RelaseDate = DateTime.ParseExact(apiBeer.first_brewed, "MM/yyyy", CultureInfo.InvariantCulture), // Assuming the date is in "MM/yyyy" format
                                Volume = 500,
                                Voltage = apiBeer.abv,
                                Price = rand.Next(3, 11) // Random price between 3 and 10
                            };

                            _context.Beer.Add(beer);
                            await _context.SaveChangesAsync();
                            Beer = _context.Beer.ToList();
                        }
                    }
                }

                // Call OnGetAsync after adding the beer to populate the Beer list.
                await OnGetAsync(CurrentSort, SearchString);

                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                // Handle errors - you can add appropriate logging or user message
                return RedirectToPage("./Index");
            }
        }
    }
}
