using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace MunicipalServicesApp.Models.CustomCollections
{
    public class CustomStack<T> : IEnumerable<T>
    {
        private List<T> _items;

        public int Count => _items.Count;
        public bool IsEmpty => _items.Count == 0;

        public CustomStack()
        {
            _items = new List<T>();
        }

        public void Push(T item)
        {
            _items.Add(item);
        }

        public T Pop()
        {
            if (_items.Count == 0)
                throw new InvalidOperationException("Stack is empty");

            var item = _items[_items.Count - 1];
            _items.RemoveAt(_items.Count - 1);
            return item;
        }

        public T Peek()
        {
            if (_items.Count == 0)
                throw new InvalidOperationException("Stack is empty");

            return _items[_items.Count - 1];
        }

        public List<T> ToList()
        {
            return new List<T>(_items);
        }

        public void FromList(List<T> list)
        {
            _items = list ?? new List<T>();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}