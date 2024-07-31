using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe.Checkout;
using StripeWebApp.Data;
using StripeWebApp.Models;
using System.Threading.Tasks;

namespace StripeWebApp.Controllers
{
    // This controller handles payment-related actions.
    public class PaymentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PaymentController(ApplicationDbContext context)
        {
            _context = context;
        }

        // This action method displays the payment page.
        public async Task<IActionResult> Index()
        {
            var products = await _context.Products.ToListAsync();
            return View(products);
        }

        // This action method creates a new Stripe Checkout session when the form is submitted.
        [HttpPost]
        public ActionResult CreateCheckout(int Id)
        {
            // Retrieve the product based on the provided ID
            var item = _context.Products.Find(Id);
            if (item == null)
            {
                return NotFound();
            }

            // The domain of the application, adjust to your systems localhost
            var domain = "http://localhost:7086";

            // Define the options for the Stripe Checkout session.
            var options = new SessionCreateOptions
            {
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        // Provide the exact Price ID of the product you want to sell.
                        Price = item.PriceId,
                        Quantity = 1,
                    },
                },
                Mode = "payment",
                // URLs to redirect to after a successful or canceled payment.
                SuccessUrl = domain + "/Payment/Success",
                CancelUrl = domain + "/Payment/Cancel",
                AutomaticTax = new SessionAutomaticTaxOptions { Enabled = true },
            };

            // Create the session using Stripe's SessionService.
            var service = new SessionService();
            Session session = service.Create(options);

            // Redirect the user to the Stripe Checkout page.
            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);
        }

        // This action method displays the success page after a successful payment.
        public IActionResult Success()
        {
            return View();
        }

        // This action method displays the cancel page after a canceled payment.
        public IActionResult Cancel()
        {
            return View();
        }
    }
}
