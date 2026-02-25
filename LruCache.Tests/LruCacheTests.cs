using Xunit;
using LruCache;

namespace LruCache.Tests
{
    public class LruCacheTests
    {
        [Fact]
        public void Get_ReturnsInsertedValue()
        {
            var cache = new LruCache<int, string>(2);
            cache.Put(1, "one");

            var value = cache.Get(1);

            Assert.Equal("one", value);
        }

        [Fact]
        public void ExceedCapacity_EvictsLeastRecentlyUsed()
        {
            var cache = new LruCache<int, string>(2);
            cache.Put(1, "one");
            cache.Put(2, "two");
            cache.Put(3, "three"); // should evict key 1

            Assert.Null(cache.Get(1));
            Assert.Equal("two", cache.Get(2));
            Assert.Equal("three", cache.Get(3));
        }

        [Fact]
        public void UpdatingExistingKey_MovesItToHead_PreventsItsEviction()
        {
            var cache = new LruCache<int, string>(2);
            cache.Put(1, "one");
            cache.Put(2, "two");
            cache.Put(1, "uno"); // refresh key 1
            cache.Put(3, "three"); // should evict key 2

            Assert.Null(cache.Get(2));
            Assert.Equal("uno", cache.Get(1));
            Assert.Equal("three", cache.Get(3));
        }
    }
}