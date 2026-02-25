using System;
using System.Collections.Generic;
using System.Text;

namespace LruCache
{
    public class LruCacheV2<K, V>(int capacity) where K  : notnull
    {
        private readonly Dictionary<K, LruNode<K, V>> cache = [];
        private readonly LinkedList<LruNode<K, V>> nodes = new();

        public V? Get(K key)
        {
            if (cache.TryGetValue(key, out var currentNode))
            {
                nodes.Remove(currentNode);
                nodes.AddFirst(currentNode);
                return currentNode.Value;
            }

            return default;
        }

        public void Put(K key, V value)
        {
            if (cache.TryGetValue(key, out var currentNode))
            {
                currentNode.Value = value;
                nodes.Remove(currentNode);
                nodes.AddFirst(currentNode);
            }
            else
            {
                var node = new LruNode<K, V>(key, value);
                cache.Add(key, node);
                nodes.AddFirst(node);

                if (cache.Count > capacity)
                {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                    cache.Remove(nodes.Last.Value.Key);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                    nodes.RemoveLast();
                }
            }
        }
    }
}
