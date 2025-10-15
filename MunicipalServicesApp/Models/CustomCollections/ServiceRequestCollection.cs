using MunicipalServicesApp.Models;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MunicipalServicesApp.Models.CustomCollections
{
    public class ServiceRequestCollection : IEnumerable<ServiceRequest>
    {
        private List<ServiceRequest> _items;

        public ServiceRequestCollection()
        {
            _items = new List<ServiceRequest>();
        }

        public int Count => _items.Count;

        public void Add(ServiceRequest serviceRequest)
        {
            _items.Add(serviceRequest);
        }

        public ServiceRequest GetAt(int index)
        {
            if (index < 0 || index >= _items.Count)
                return null;
            return _items[index];
        }

        public ServiceRequest FindById(string id)
        {
            return _items.FirstOrDefault(r => r.Id == id);
        }

        public ServiceRequestCollection GetRequestsByStatus(RequestStatus status)
        {
            var result = new ServiceRequestCollection();
            foreach (var request in _items)
            {
                if (request.Status == status)
                {
                    result.Add(request);
                }
            }
            return result;
        }

        public IEnumerator<ServiceRequest> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}