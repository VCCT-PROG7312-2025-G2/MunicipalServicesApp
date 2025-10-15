using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MunicipalServicesApp.Models;
using MunicipalServicesApp.Models.CustomCollections;
using MunicipalServicesApp.Models.ViewModels;
using MunicipalServicesApp.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MunicipalServicesApp.Controllers
{
    public class HomeController : Controller
    {
        private const string NotificationsKey = "UserNotifications";
        private const string ServiceRequestsKey = "UserServiceRequests";
        private const string UserSessionKey = "CurrentUser";
        private readonly IWebHostEnvironment _environment;
        private readonly UserService _userService;

        public HomeController(IWebHostEnvironment environment)
        {
            _environment = environment;
            _userService = new UserService();
        }

        // User session management methods
        private User GetCurrentUser()
        {
            var userJson = HttpContext.Session.GetString(UserSessionKey);
            if (string.IsNullOrEmpty(userJson))
                return null;

            return System.Text.Json.JsonSerializer.Deserialize<User>(userJson);
        }

        private void SetCurrentUser(User user)
        {
            var userJson = System.Text.Json.JsonSerializer.Serialize(user);
            HttpContext.Session.SetString(UserSessionKey, userJson);
        }

        private void ClearCurrentUser()
        {
            HttpContext.Session.Remove(UserSessionKey);
        }

        // Authentication check method
        private IActionResult CheckAuthentication()
        {
            var currentUser = GetCurrentUser();
            if (currentUser == null)
            {
                return RedirectToAction("Login");
            }
            ViewBag.CurrentUser = currentUser;
            return null;
        }

        // Login actions
        public IActionResult Login()
        {
            var currentUser = GetCurrentUser();
            if (currentUser != null)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = _userService.Authenticate(model.Username, model.Password);
            if (user == null)
            {
                ModelState.AddModelError("", "Invalid username or password");
                return View(model);
            }

            SetCurrentUser(user);

            if (model.RememberMe)
            {
                var options = new CookieOptions
                {
                    Expires = DateTime.Now.AddDays(30),
                    IsEssential = true
                };
                Response.Cookies.Append("UserAuth", user.Username, options);
            }

            TempData["SuccessMessage"] = $"Welcome back, {user.DisplayName}!";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Logout()
        {
            ClearCurrentUser();
            Response.Cookies.Delete("UserAuth");
            TempData["SuccessMessage"] = "You have been logged out successfully.";
            return RedirectToAction("Login");
        }

        // Updated actions with authentication
        public IActionResult Index()
        {
            var authResult = CheckAuthentication();
            if (authResult != null) return authResult;

            var model = new DashboardViewModel
            {
                Notifications = GetNotifications(),
                ServiceRequests = GetServiceRequests()
            };
            return View(model);
        }

        public IActionResult ServiceRequests()
        {
            var authResult = CheckAuthentication();
            if (authResult != null) return authResult;

            var model = GetServiceRequests();
            return View(model);
        }

        public IActionResult Notifications()
        {
            var authResult = CheckAuthentication();
            if (authResult != null) return authResult;

            var model = GetNotifications();
            return View(model);
        }

        public IActionResult Settings()
        {
            var authResult = CheckAuthentication();
            if (authResult != null) return authResult;

            return View();
        }

        public IActionResult ReportIssue()
        {
            var authResult = CheckAuthentication();
            if (authResult != null) return authResult;

            var model = new ReportIssueViewModel();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> SubmitIssue(ReportIssueViewModel model)
        {
            var authResult = CheckAuthentication();
            if (authResult != null) return authResult;

            if (!ModelState.IsValid)
            {
                return View("ReportIssue", model);
            }

            var requests = GetServiceRequests();

            var newRequest = new ServiceRequest
            {
                Id = Guid.NewGuid().ToString(),
                Title = model.Title,
                Description = model.Description,
                Location = model.Location,
                Category = model.Category,
                CreatedDate = DateTime.Now,
                Status = RequestStatus.Submitted,
                UserId = "user123"
            };

            // Handle file uploads
            if (model.MediaFiles != null && model.MediaFiles.Count > 0)
            {
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                foreach (var file in model.MediaFiles)
                {
                    if (file.Length > 0)
                    {
                        var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        newRequest.MediaAttachments.Push(uniqueFileName);
                    }
                }
            }

            requests.Add(newRequest);
            SaveServiceRequests(requests);

            // Create notification
            var notifications = GetNotifications();
            var notification = new Notification
            {
                Id = Guid.NewGuid().ToString(),
                Title = "Service Request Submitted",
                Message = $"Your {model.Category} request '{model.Title}' has been submitted successfully.",
                Timestamp = DateTime.Now,
                Category = NotificationCategory.ServiceUpdate,
                IsRead = false,
                UserId = "user123"
            };

            notifications.Add(notification);
            SaveNotifications(notifications);

            TempData["SuccessMessage"] = "Your issue has been reported successfully!";
            return RedirectToAction("ServiceRequests");
        }

        [HttpPost]
        public IActionResult MarkAsRead(string id)
        {
            var authResult = CheckAuthentication();
            if (authResult != null) return authResult;

            var notifications = GetNotifications();
            var notification = notifications.FindById(id);
            if (notification != null)
            {
                notification.IsRead = true;
                SaveNotifications(notifications);
                TempData["SuccessMessage"] = "Notification marked as read.";
            }
            return RedirectToAction("Notifications");
        }

        [HttpPost]
        public IActionResult MarkAllAsRead()
        {
            var authResult = CheckAuthentication();
            if (authResult != null) return authResult;

            var notifications = GetNotifications();
            var unreadNotifications = notifications.GetUnreadNotifications();

            foreach (var notification in unreadNotifications)
            {
                notification.IsRead = true;
            }

            SaveNotifications(notifications);
            TempData["SuccessMessage"] = "All notifications marked as read.";
            return RedirectToAction("Notifications");
        }

        // Existing notification and service request methods 
        private NotificationCollection GetNotifications()
        {
            var notificationsJson = HttpContext.Session.GetString(NotificationsKey);
            if (string.IsNullOrEmpty(notificationsJson))
            {
                return CreateSampleNotifications();
            }

            return DeserializeNotifications(notificationsJson);
        }

        private NotificationCollection CreateSampleNotifications()
        {
            var notifications = new NotificationCollection();

            var sampleNotifications = new[]
            {
                new Notification {
                    Id = "1",
                    Title = "Water Maintenance Scheduled",
                    Message = "Scheduled water maintenance in your area on Friday from 10:00 to 14:00",
                    Timestamp = DateTime.Now.AddHours(-2),
                    Category = NotificationCategory.General,
                    IsRead = false,
                    UserId = "user123"
                },
                new Notification {
                    Id = "2",
                    Title = "Load Shedding Update",
                    Message = "Stage 2 load shedding implemented from 16:00 to 18:00",
                    Timestamp = DateTime.Now.AddHours(-5),
                    Category = NotificationCategory.Emergency,
                    IsRead = false,
                    UserId = "user123"
                },
                new Notification {
                    Id = "3",
                    Title = "Service Request Updated",
                    Message = "Your request 'Pothole Repair' has been updated: Assessment team dispatched",
                    Timestamp = DateTime.Now.AddDays(-1),
                    Category = NotificationCategory.ServiceUpdate,
                    IsRead = true,
                    UserId = "user123"
                }
            };

            foreach (var notification in sampleNotifications)
            {
                notifications.Add(notification);
            }

            SaveNotifications(notifications);
            return notifications;
        }

        private void SaveNotifications(NotificationCollection notifications)
        {
            var notificationList = new List<Notification>();
            for (int i = 0; i < notifications.Count; i++)
            {
                notificationList.Add(notifications.GetAt(i));
            }

            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                Formatting = Formatting.None,
                NullValueHandling = NullValueHandling.Ignore
            };

            var json = JsonConvert.SerializeObject(notificationList, settings);
            HttpContext.Session.SetString(NotificationsKey, json);
        }

        private NotificationCollection DeserializeNotifications(string json)
        {
            if (string.IsNullOrEmpty(json))
                return new NotificationCollection();

            try
            {
                var settings = new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore
                };

                var notificationList = JsonConvert.DeserializeObject<List<Notification>>(json, settings);
                var collection = new NotificationCollection();

                if (notificationList != null)
                {
                    foreach (var notification in notificationList)
                    {
                        collection.Add(notification);
                    }
                }
                return collection;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Deserialization error: {ex.Message}");
                return new NotificationCollection();
            }
        }

        private ServiceRequestCollection GetServiceRequests()
        {
            var requestsJson = HttpContext.Session.GetString(ServiceRequestsKey);
            if (string.IsNullOrEmpty(requestsJson))
            {
                return CreateSampleServiceRequests();
            }

            return DeserializeServiceRequests(requestsJson);
        }

        private ServiceRequestCollection CreateSampleServiceRequests()
        {
            var requests = new ServiceRequestCollection();

            var sampleRequests = new[]
            {
        new ServiceRequest {
            Id = "1",
            Title = "Pothole Repair",
            Description = "Large pothole on Main Street causing traffic issues",
            Location = "Main Street, City Center",
            Category = "Roads",
            CreatedDate = DateTime.Now.AddDays(-2),
            Status = RequestStatus.InProgress,
            UserId = "user123",
            MediaAttachments = new CustomStack<string>(),
            Updates = new CustomStack<StatusUpdate>()
        },
        new ServiceRequest {
            Id = "2",
            Title = "Street Light Outage",
            Description = "Street light not working on Oak Avenue",
            Location = "Oak Avenue, Suburbia",
            Category = "Utilities",
            CreatedDate = DateTime.Now.AddDays(-1),
            Status = RequestStatus.Submitted,
            UserId = "user123",
            MediaAttachments = new CustomStack<string>(),
            Updates = new CustomStack<StatusUpdate>()
        }
    };

            // Add status update to first request
            sampleRequests[0].Updates.Push(new StatusUpdate
            {
                Timestamp = DateTime.Now.AddDays(-1),
                Message = "Assessment team dispatched",
                UpdatedBy = "Public Works Dept",
                NewStatus = RequestStatus.InProgress
            });

            foreach (var request in sampleRequests)
            {
                requests.Add(request);
            }

            SaveServiceRequests(requests);
            return requests;
        }

        private void SaveServiceRequests(ServiceRequestCollection serviceRequests)
        {
            var requestList = new List<ServiceRequest>();
            for (int i = 0; i < serviceRequests.Count; i++)
            {
                requestList.Add(serviceRequests.GetAt(i));
            }

            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                Formatting = Formatting.None,
                NullValueHandling = NullValueHandling.Ignore
            };

            var json = JsonConvert.SerializeObject(requestList, settings);
            HttpContext.Session.SetString(ServiceRequestsKey, json);
        }

        private ServiceRequestCollection DeserializeServiceRequests(string json)
        {
            if (string.IsNullOrEmpty(json))
                return new ServiceRequestCollection();

            try
            {
                var settings = new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore
                };

                var requestList = JsonConvert.DeserializeObject<List<ServiceRequest>>(json, settings);
                var collection = new ServiceRequestCollection();

                if (requestList != null)
                {
                    foreach (var request in requestList)
                    {
                        // Ensure collections are initialized if null
                        request.MediaAttachments ??= new CustomStack<string>();
                        request.Updates ??= new CustomStack<StatusUpdate>();
                        collection.Add(request);
                    }
                }
                return collection;
            }
            catch (Exception ex)
            {
                // Log the error and return empty collection
                System.Diagnostics.Debug.WriteLine($"Deserialization error: {ex.Message}");
                return new ServiceRequestCollection();
            }
        }
    }
}