using MunicipalServicesApp.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace MunicipalServicesApp.Models.CustomCollections
{
    [JsonConverter(typeof(EventPriorityQueueConverter))]
    public class EventPriorityQueue : IEnumerable<Event>
    {
        private List<Event> _events;
        private bool _isMinHeap;

        public EventPriorityQueue(bool isMinHeap = false)
        {
            _events = new List<Event>();
            _isMinHeap = isMinHeap;
        }

        public int Count => _events.Count;

        public void Enqueue(Event eventItem)
        {
            _events.Add(eventItem);
            int currentIndex = _events.Count - 1;

            while (currentIndex > 0)
            {
                int parentIndex = (currentIndex - 1) / 2;
                if (CompareEvents(_events[currentIndex], _events[parentIndex]) >= 0)
                    break;

                Swap(currentIndex, parentIndex);
                currentIndex = parentIndex;
            }
        }

        public Event Dequeue()
        {
            if (_events.Count == 0)
                throw new InvalidOperationException("Queue is empty");

            Event firstItem = _events[0];
            int lastIndex = _events.Count - 1;
            _events[0] = _events[lastIndex];
            _events.RemoveAt(lastIndex);

            int currentIndex = 0;
            while (true)
            {
                int leftChildIndex = currentIndex * 2 + 1;
                int rightChildIndex = currentIndex * 2 + 2;

                if (leftChildIndex >= _events.Count) break;

                int minIndex = leftChildIndex;
                if (rightChildIndex < _events.Count &&
                    CompareEvents(_events[rightChildIndex], _events[leftChildIndex]) < 0)
                {
                    minIndex = rightChildIndex;
                }

                if (CompareEvents(_events[currentIndex], _events[minIndex]) <= 0)
                    break;

                Swap(currentIndex, minIndex);
                currentIndex = minIndex;
            }

            return firstItem;
        }

        public Event Peek()
        {
            if (_events.Count == 0)
                throw new InvalidOperationException("Queue is empty");
            return _events[0];
        }

        private int CompareEvents(Event a, Event b)
        {
            int dateComparison = a.EventDate.CompareTo(b.EventDate);
            if (dateComparison != 0)
                return _isMinHeap ? dateComparison : -dateComparison;

            if (a.IsFeatured && !b.IsFeatured) return -1;
            if (!a.IsFeatured && b.IsFeatured) return 1;

            return string.Compare(a.Title, b.Title, StringComparison.Ordinal);
        }

        private void Swap(int i, int j)
        {
            Event temp = _events[i];
            _events[i] = _events[j];
            _events[j] = temp;
        }

        public List<Event> ToList()
        {
            return new List<Event>(_events);
        }

        public void FromList(List<Event> events)
        {
            _events.Clear();
            foreach (var eventItem in events)
            {
                Enqueue(eventItem);
            }
        }

        public IEnumerator<Event> GetEnumerator()
        {
            return _events.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class EventPriorityQueueConverter : JsonConverter<EventPriorityQueue>
    {
        public override void WriteJson(JsonWriter writer, EventPriorityQueue value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value?.ToList());
        }

        public override EventPriorityQueue ReadJson(JsonReader reader, Type objectType, EventPriorityQueue existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var list = serializer.Deserialize<List<Event>>(reader);
            var queue = new EventPriorityQueue();
            queue.FromList(list);
            return queue;
        }
    }
}