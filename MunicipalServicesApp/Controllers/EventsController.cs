using Microsoft.AspNetCore.Mvc;
using MunicipalServicesApp.Models;
using MunicipalServicesApp.Services;
using System;
using System.Linq;

namespace MunicipalServicesApp.Controllers
{
    public class EventsController : Controller
    {
        private readonly EventService _eventService;

        public EventsController()
        {
            _eventService = new EventService();
        }

        public IActionResult Index()
        {
            var model = new EventSearchViewModel
            {
                Events = _eventService.GetUpcomingEvents(),
                Recommendations = _eventService.GetRecommendedEvents(),
                Categories = _eventService.GetCategories()
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Search(EventSearchViewModel model)
        {
            var events = _eventService.SearchEvents(model.SearchTerm, model.Category, model.SearchDate);
            var recommendations = _eventService.GetRecommendedEvents();

            model.Events = events;
            model.Recommendations = recommendations;
            model.Categories = _eventService.GetCategories();

            return View("Index", model);
        }

        public IActionResult Details(string id)
        {
            // For demo purposes, get from upcoming events
            var events = _eventService.GetUpcomingEvents(50);
            var eventItem = events.FirstOrDefault(e => e.Id == id);

            if (eventItem == null)
            {
                return NotFound();
            }

            return View(eventItem);
        }
    }
}