using System;
using EventaDors.Entities.Classes;
using EventaDors.WebApplication.Models;
using Microsoft.AspNetCore.Mvc;

namespace EventaDors.WebApplication.Controllers
{
    public class TemplatesController : Controller
    {
        // GET
        public IActionResult Index(Journey journey)
        {
            Console.WriteLine(journey.Surname);
            return View();
        }
    }
}