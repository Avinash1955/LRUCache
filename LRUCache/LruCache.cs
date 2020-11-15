using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LRUCache
{
    public class LruCache<TKey, TValue>
    {
        private class cacheItem
        {
            public cacheItem(TKey k, TValue v)
            {
                this.Key = k;
                this.Value = v;
            }

            public TKey Key { get; }

            public TValue Value { get; set; }
        }

        public int Capacity { get; }
        private readonly Dictionary<TKey, LinkedListNode<cacheItem>> elements = new Dictionary<TKey, LinkedListNode<cacheItem>>();
        private readonly LinkedList<cacheItem> lruList = new LinkedList<cacheItem>();
        private readonly Action<TKey, TValue> itemEvictHandler;

        public LruCache(int capacity, Action<TKey,TValue> evictHandler = null)
        {
            Capacity = capacity;
            itemEvictHandler = evictHandler;
        }


        /// <summary>Gets the value associated with the specified key.</summary>
        /// <param name="key">
        /// The key of the value to get.
        /// </param>
        /// <param name="value">
        /// When this method returns, contains the value associated with the specified
        /// key, if the key is found; otherwise, the default value for the type of the 
        /// </param>
        /// <returns>
        /// true if the cache contains an element with the specified key; otherwise, false.
        /// </returns>
        
        public bool TryGetValue(TKey key, out TValue value)
        {
            lock(elements)
            {
                LinkedListNode<cacheItem> itemNode;
                if (elements.TryGetValue(key, out itemNode))
                {
                    value = itemNode.Value.Value;
                    lruList.Remove(itemNode);
                    lruList.AddFirst(itemNode);
                    return true;
                }

                value = default(TValue);
                return false;
            }
        }


        /// <summary>
        /// Add the given key and value to LRU Cache.
        /// If the element with given key is already present then value is updated.
        /// </summary>
        /// <param name="key">Key of element to be added.</param>
        /// <param name="value">Value of element to be added.</param>
        public void Add(TKey key, TValue value)
        {
            lock (elements)
            {
                LinkedListNode<cacheItem> itemNode;
                if (elements.TryGetValue(key, out itemNode))
                {
                    itemNode.Value.Value = value;
                    lruList.Remove(itemNode);
                    lruList.AddFirst(itemNode);
                }
                else
                {
                    if (elements.Count >= this.Capacity)
                    {
                        Remove(lruList.Last.Value.Key);
                       
                    }

                    cacheItem item = new cacheItem(key, value);
                    LinkedListNode<cacheItem> node = new LinkedListNode<cacheItem>(item);
                    lruList.AddFirst(node);
                    elements.Add(key, node);
                    
                }
            }

        }


        /// <summary>
        /// Remove element for given Key from Cache
        /// </summary>
        /// <param name="key"></param>
        public void Remove(TKey key)
        {
            lock (elements)
            {
                LinkedListNode<cacheItem> elementNode;
                if (elements.TryGetValue(key, out elementNode))
                {
                    lruList.Remove(elementNode);
                    elements.Remove(key);
                    this.itemEvictHandler?.Invoke(key, elementNode.Value.Value);

                    // If the object is disposable then invoke Dispose

                    if (elementNode.Value.Value is IDisposable d)
                    {
                        d.Dispose();
                    }
                }
            }

        }


    }
}
