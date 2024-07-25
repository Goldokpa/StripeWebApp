using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SpanishCuisine.Data;
using SpanishCuisine.Models;
using System.Diagnostics;
using System.Threading.Tasks;

namespace SpanishCuisine.Controllers
{
    public class HomeController : Controller
    {
        // Logger for logging messages and errors
        private readonly ILogger<HomeController> _logger;

        // Database context for accessing the SpanishCuisine database
        private readonly SpanishCuisineContext _context;

        // Constructor to initialize the logger and database context
        public HomeController(ILogger<HomeController> logger, SpanishCuisineContext context)
        {
            _logger = logger;
            _context = context;
        }

        // Action method to display the list of dishes on the home page
        public async Task<IActionResult> Index(string searchString)
        {
            // Query to fetch all dishes
            var dishes = from d in _context.Dishes
                         select d;

            // If a search string is provided, filter the dishes based on the search string
            if (!string.IsNullOrEmpty(searchString))
            {
                dishes = dishes.Where(d => d.Name.Contains(searchString));
            }

            // Fetch the list of dishes asynchronously
            var dishList = await dishes.ToListAsync();

            // Pass the dishes list to the view
            return View(dishList);
        }

        // Action method to display the privacy policy page
        public IActionResult Privacy()
        {
            return View();
        }

        // Action method to display the details of a specific dish
        public async Task<IActionResult> Details(int? id)
        {
            // Check if the dish ID is not provided
            if (id == null)
            {
                return NotFound();
            }

            // Fetch the dish with the provided ID, including its ingredients
            var dish = await _context.Dishes
                .Include(d => d.DishIngredient)
                .ThenInclude(di => di.Ingredient)
                .FirstOrDefaultAsync(d => d.Id == id);

            // Check if the dish was not found
            if (dish == null)
            {
                return NotFound();
            }

            // Pass the dish details to the view
            return View(dish);
        }

        // Action method to display the error page
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            // Create and return an error view model with the current request ID
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
