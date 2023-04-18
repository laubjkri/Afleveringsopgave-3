using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace KL_Utils
{

    /// <summary>
    /// This collection class provides a way to get the value and the index equally fast
    /// by utilizing both a Dictionary and a List.
    /// </summary>
    public class DictionaryList<T> : IEnumerable<T> where T : notnull
    {
        // T in dictionary must be notnull
        private Dictionary<T, int> _dictionary = new();
        private List<T> _list = new();

        public DictionaryList() { }
        public DictionaryList(IEnumerable<T> list) 
        {
            // Support initializer list
            foreach (T item in list)
            {
                Add(item);
            }            
        }

        public void Add(T item)
        {
            _list.Add(item);
            _dictionary.Add(item, _dictionary.Count);
        }

        public void Remove(T item)
        {
            _list.Remove(item);

            // Expensive:
            _dictionary.Clear();
            _list.ForEach(item => _dictionary.Add(item, _dictionary.Count));
        }

        public void Clear()
        {
            _list.Clear();
            _dictionary.Clear();
        }

        public int GetIndex(T item)
        {
            if (_dictionary.TryGetValue(item, out int value))
            {
                // Value in dictionary is index of list
                return value;
            }
            else
            {
                throw new KeyNotFoundException($"Item \"{item}\" was not found.");
            }
        }

        public T GetItem(int index)
        {
            if(_list.Count == 0)
            {
                throw new InvalidOperationException($"Collection does not contain any items.");
            }

            if(index < 0 || index >= _list.Count)
            {
                throw new ArgumentOutOfRangeException($"{nameof(index)} is out of range.");
            }

            return _list[index];
        }

        public ReadOnlyCollection<T> AsReadOnly()
        {
            return _list.AsReadOnly();
        }


        // Required for IEnumerable to support initializer list
        public IEnumerator<T> GetEnumerator()
        {            
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

    }
}
