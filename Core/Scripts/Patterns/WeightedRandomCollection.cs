//Developed by Pavel Kravtsov.
using System.Collections.Generic;
using UnityEngine;
namespace Core
{
    public class WeightedRandomCollection<T>
    {
        private struct Entry
        {
            public double accumulatedWeight;
            public T item;
        }

        private List<Entry> entries = new List<Entry>();
        private double accumulatedWeight;

        public void AddEntry(T item, double weight)
        {
            accumulatedWeight += weight;
            entries.Add(new Entry { item = item, accumulatedWeight = accumulatedWeight });
        }

        public T GetRandom()
        {
            var r = Random.Range(0f, 1f) * accumulatedWeight;

            foreach (Entry entry in entries)
            {
                if (entry.accumulatedWeight >= r)
                {
#if DEBUG
                    //Debug.Log(string.Format("entrie index = {0}; R = {1};", entries.IndexOf(entry), r));
#endif
                    return entry.item;
                }
            }
            return default(T); //should only happen when there are no entries
        }
    }
}