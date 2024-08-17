using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TaskManagementFrontend.Models;

namespace TaskManagementFrontend.Controllers
{
    public class TasksController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configuration;

        public TasksController(IHttpClientFactory clientFactory, IConfiguration configuration)
        {
            _clientFactory = clientFactory;
            _configuration = configuration;
        }

        // List all tasks
        public async Task<IActionResult> Index()
        {
            var client = _clientFactory.CreateClient();
            var apiBaseUrl = _configuration["ApiBaseUrl"];
            var response = await client.GetAsync($"{apiBaseUrl}/api/tasks");

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var tasks = JsonSerializer.Deserialize<List<TaskItem>>(jsonResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return View(tasks);
            }

            return View(new List<TaskItem>());
        }

        // Get details of a specific task
        public async Task<IActionResult> Details(string id)
        {
            var client = _clientFactory.CreateClient();
            var apiBaseUrl = _configuration["ApiBaseUrl"];
            var response = await client.GetAsync($"{apiBaseUrl}/api/tasks/{id}");

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var task = JsonSerializer.Deserialize<TaskItem>(jsonResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return View(task);
            }

            return NotFound();
        }

        // Show the create task form
        public IActionResult Create()
        {
            return View();
        }

        // Handle the task creation form submission
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TaskItem task)
        {
            if (ModelState.IsValid)
            {
                var client = _clientFactory.CreateClient();
                var apiBaseUrl = _configuration["ApiBaseUrl"];
                var jsonTask = JsonSerializer.Serialize(task);
                var content = new StringContent(jsonTask, Encoding.UTF8, "application/json");
                var response = await client.PostAsync($"{apiBaseUrl}/api/tasks", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(task);
        }

        // Show the edit task form
        public async Task<IActionResult> Edit(string id)
        {
            var client = _clientFactory.CreateClient();
            var apiBaseUrl = _configuration["ApiBaseUrl"];
            var response = await client.GetAsync($"{apiBaseUrl}/api/tasks/{id}");

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var task = JsonSerializer.Deserialize<TaskItem>(jsonResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return View(task);
            }

            return NotFound();
        }

        // Handle the task edit form submission
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, TaskItem task)
        {
            if (id != task.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                var client = _clientFactory.CreateClient();
                var apiBaseUrl = _configuration["ApiBaseUrl"];
                var jsonTask = JsonSerializer.Serialize(task);
                var content = new StringContent(jsonTask, Encoding.UTF8, "application/json");
                var response = await client.PutAsync($"{apiBaseUrl}/api/tasks/{id}", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(task);
        }

        // Show the delete task confirmation page
        public async Task<IActionResult> Delete(string id)
        {
            var client = _clientFactory.CreateClient();
            var apiBaseUrl = _configuration["ApiBaseUrl"];
            var response = await client.GetAsync($"{apiBaseUrl}/api/tasks/{id}");

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var task = JsonSerializer.Deserialize<TaskItem>(jsonResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return View(task);
            }

            return NotFound();
        }

        // Handle the task deletion
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var client = _clientFactory.CreateClient();
            var apiBaseUrl = _configuration["ApiBaseUrl"];
            var response = await client.DeleteAsync($"{apiBaseUrl}/api/tasks/{id}");

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }

            return Problem("There was an error deleting the task.");
        }
    }
}
