// Services/UserService.cs
using MunicipalServicesApp.Models;
using System.Collections.Generic;
using System.Linq;

namespace MunicipalServicesApp.Services
{
    public class UserService
    {
        private List<User> _users;

        public UserService()
        {
            // Predefined users - in a real app, this would be in a database
            _users = new List<User>
            {
                new User {
                    Username = "admin",
                    Password = "admin123",
                    Role = UserRole.Admin,
                    DisplayName = "System Administrator"
                },
                new User {
                    Username = "employee1",
                    Password = "emp123",
                    Role = UserRole.Employee,
                    DisplayName = "John Smith (Employee)"
                },
                new User {
                    Username = "employee2",
                    Password = "emp456",
                    Role = UserRole.Employee,
                    DisplayName = "Sarah Johnson (Employee)"
                },
                new User {
                    Username = "resident",
                    Password = "res123",
                    Role = UserRole.Resident,
                    DisplayName = "Community Resident"
                }
            };
        }

        public User Authenticate(string username, string password)
        {
            return _users.FirstOrDefault(u => u.Username == username && u.Password == password);
        }

        public User GetUserByUsername(string username)
        {
            return _users.FirstOrDefault(u => u.Username == username);
        }
    }
}