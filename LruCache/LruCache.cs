using System.Collections.Generic;
using System.Net.Http.Headers;

namespace LruCache
{
    public class LruCache<K, V> where K : notnull
    {
        private readonly int capacity;

        private readonly Dictionary<K, LruNode<K, V>> cache;
        private readonly LruNode<K, V> head;
        private readonly LruNode<K, V> tail;

        public LruCache(int capacity)
        {
            this.capacity = capacity;

            cache = new Dictionary<K, LruNode<K, V>>(capacity);
            head = new(default!, default!);
            tail = new(default!, default!);

            head.Next = tail;
            tail.Previous = head;
        }

        // This implementation uses HEAD as the most CURRENT node and TAIL as the OLDEST node.
        // HEAD <-> (CURRENT) <-> (OLDEST) <-> TAIL

        public V? Get(K key)
        {
            if(cache.TryGetValue(key, out var value))
            {
                MoveToHead(value);
                return value.Value;
            }

            return default;
        }

        public void Put(K key, V value)
        {
            // if already exists, refresh value and move to head
            if (cache.TryGetValue(key, out LruNode<K, V>? currentNode))
            {
                currentNode.Value = value;
                MoveToHead(currentNode);
            }
            else
            {
                // else add and add to head
                var node = new LruNode<K, V>(key, value);
                cache.Add(key, node);
                AddToHead(node);

                if (cache.Count > capacity)
                {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                    cache.Remove(tail.Previous.Key);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8604 // Possible null reference argument.
                    Remove(tail.Previous);
#pragma warning restore CS8604 // Possible null reference argument.
                }
            }
        }

        private void Remove(LruNode<K, V> node)
        {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            node.Previous.Next = node.Next;
            node.Next.Previous = node.Previous;
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        }

        private void AddToHead(LruNode<K, V> node)
        {
            node.Next = head.Next;
            node.Previous = head;

#pragma warning disable CS8602 // Dereference of a possibly null reference.
            head.Next.Previous = node;
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            head.Next = node;
        }

        private void MoveToHead(LruNode<K, V> node)
        {
            Remove(node);
            AddToHead(node);
        }
    }
}
