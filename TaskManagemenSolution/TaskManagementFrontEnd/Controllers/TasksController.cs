﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<TasksController> _logger;

        public TasksController(IHttpClientFactory clientFactory, IConfiguration configuration, ILogger<TasksController> logger)
        {
            _clientFactory = clientFactory;
            _configuration = configuration;
            _logger = logger;
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

            _logger.LogError($"Failed to retrieve tasks. Status Code: {response.StatusCode}");
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

            _logger.LogError($"Failed to retrieve task with ID {id}. Status Code: {response.StatusCode}");
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

                _logger.LogInformation("Sending POST request to create a new task.");
                _logger.LogInformation($"Task Data: {jsonTask}");

                var response = await client.PostAsync($"{apiBaseUrl}/api/tasks", content);

                _logger.LogInformation($"Response Status: {response.StatusCode}");

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }

                var responseBody = await response.Content.ReadAsStringAsync();
                _logger.LogError($"Error while creating task: {responseBody}");
                ModelState.AddModelError(string.Empty, $"Error: {responseBody}");
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

            _logger.LogError($"Failed to retrieve task for editing with ID {id}. Status Code: {response.StatusCode}");
            return NotFound();
        }

        // Handle the task edit form submission
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, TaskItem task)
        {
            if (id != task.Id)
            {
                _logger.LogError($"Task ID mismatch. Provided ID: {id}, Task ID: {task.Id}");
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                var client = _clientFactory.CreateClient();
                var apiBaseUrl = _configuration["ApiBaseUrl"];
                var jsonTask = JsonSerializer.Serialize(task);
                var content = new StringContent(jsonTask, Encoding.UTF8, "application/json");

                _logger.LogInformation($"Sending PUT request to update task with ID {id}.");
                _logger.LogInformation($"Task Data: {jsonTask}");

                var response = await client.PutAsync($"{apiBaseUrl}/api/tasks/{id}", content);

                _logger.LogInformation($"Response Status: {response.StatusCode}");

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }

                var responseBody = await response.Content.ReadAsStringAsync();
                _logger.LogError($"Error while updating task: {responseBody}");
                ModelState.AddModelError(string.Empty, $"Error: {responseBody}");
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

            _logger.LogError($"Failed to retrieve task for deletion with ID {id}. Status Code: {response.StatusCode}");
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

            _logger.LogInformation($"Attempting to delete task with ID {id}. Response Status: {response.StatusCode}");

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }

            var responseBody = await response.Content.ReadAsStringAsync();
            _logger.LogError($"Error while deleting task: {responseBody}");
            return Problem("There was an error deleting the task.");
        }
    }
}
