using System.Collections.Generic;

namespace CustomerReaderLib.Models
{
    // Simple thread safe List
    class ListEx<T> : List<T>
    {
        private object _obj = new object();

        // For now we only care about making Add() safe
        public void AddEx(T item)
        {
            lock (_obj)
            {
                Add(item);
            }
        }
    }
}
