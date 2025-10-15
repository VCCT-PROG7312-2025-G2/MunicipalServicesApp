using MunicipalServicesApp.Models;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MunicipalServicesApp.Models.CustomCollections
{
    public class NotificationCollection : IEnumerable<Notification>
    {
        private List<Notification> _items;

        public NotificationCollection()
        {
            _items = new List<Notification>();
        }

        public int Count => _items.Count;

        public void Add(Notification notification)
        {
            _items.Add(notification);
        }

        public Notification GetAt(int index)
        {
            if (index < 0 || index >= _items.Count)
                return null;
            return _items[index];
        }

        public Notification FindById(string id)
        {
            return _items.FirstOrDefault(n => n.Id == id);
        }

        public NotificationCollection GetUnreadNotifications()
        {
            var result = new NotificationCollection();
            foreach (var notification in _items)
            {
                if (!notification.IsRead)
                {
                    result.Add(notification);
                }
            }
            return result;
        }

        public IEnumerator<Notification> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}