using System;
using System.Collections.Generic;
using System.Text;

namespace LruCache
{
    internal class LruNode<K, V>(K key, V value)
    {
        public K Key { get; } = key;
        public V Value { get; set; } = value;
        public LruNode<K, V>? Previous { get; set; } = null;
        public LruNode<K, V>? Next { get; set; } = null;

        public override string ToString()
        {
            return $"{Key}: {Value}";
        }
    }
}
