using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace MunicipalServicesApp.Models.CustomCollections
{
    [JsonConverter(typeof(EventSetConverter<>))]
    public class EventSet<T> : IEnumerable<T>
    {
        private CustomDictionary<T, bool> _items;

        public EventSet()
        {
            _items = new CustomDictionary<T, bool>();
        }

        public int Count => _items.Count;

        public bool Add(T item)
        {
            if (_items.ContainsKey(item))
                return false;

            _items.Add(item, true);
            return true;
        }

        public bool Remove(T item)
        {
            return _items.ContainsKey(item);
        }

        public bool Contains(T item)
        {
            return _items.ContainsKey(item);
        }

        public EventSet<T> Union(EventSet<T> other)
        {
            var result = new EventSet<T>();
            foreach (var item in this)
            {
                result.Add(item);
            }
            foreach (var item in other)
            {
                result.Add(item);
            }
            return result;
        }

        public EventSet<T> Intersection(EventSet<T> other)
        {
            var result = new EventSet<T>();
            foreach (var item in this)
            {
                if (other.Contains(item))
                {
                    result.Add(item);
                }
            }
            return result;
        }

        public List<T> ToList()
        {
            var list = new List<T>();
            foreach (var item in this)
            {
                list.Add(item);
            }
            return list;
        }

        public void FromList(List<T> list)
        {
            _items = new CustomDictionary<T, bool>();
            foreach (var item in list)
            {
                Add(item);
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            foreach (var kvp in _items)
            {
                yield return kvp.Key;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class EventSetConverter<T> : JsonConverter<EventSet<T>>
    {
        public override void WriteJson(JsonWriter writer, EventSet<T> value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value?.ToList());
        }

        public override EventSet<T> ReadJson(JsonReader reader, Type objectType, EventSet<T> existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var list = serializer.Deserialize<List<T>>(reader);
            var set = new EventSet<T>();
            set.FromList(list);
            return set;
        }
    }
}