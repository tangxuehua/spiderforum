using System;
using System.Collections;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Caching;

namespace System.Web.Core
{
    public sealed class CacheManager
    {
        private CacheManager() { }

        private static readonly Cache _cache;
        private static int Factor = 5;
        public static readonly int DayFactor = 17280;
        public static readonly int HourFactor = 720;
        public static readonly int MinuteFactor = 12;
        public static readonly double SecondFactor = 0.2;

        static CacheManager()
        {
            HttpContext context = HttpContext.Current;
            if (context != null)
            {
                _cache = context.Cache;
            }
            else
            {
                _cache = HttpRuntime.Cache;
            }
        }

        public static void ReSetFactor(int cacheFactor)
        {
            Factor = cacheFactor;
        }
        public static void Clear()
        {
            IDictionaryEnumerator CacheEnum = _cache.GetEnumerator();
            ArrayList al = new ArrayList();
            while (CacheEnum.MoveNext())
            {
                al.Add(CacheEnum.Key);
            }

            foreach (string key in al)
            {
                _cache.Remove(key);
            }

        }
        public static void RemoveByPattern(string pattern)
        {
            IDictionaryEnumerator CacheEnum = _cache.GetEnumerator();
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Compiled);
            while (CacheEnum.MoveNext())
            {
                if (regex.IsMatch(CacheEnum.Key.ToString()))
                    _cache.Remove(CacheEnum.Key.ToString());
            }
        }
        public static void Remove(string key)
        {
            _cache.Remove(key);
        }

        public static void Insert(string key, object obj)
        {
            Insert(key, obj, null, 1);
        }
        public static void Insert(string key, object obj, CacheDependency dep)
        {
            Insert(key, obj, dep, MinuteFactor * 3);
        }
        public static void Insert(string key, object obj, double seconds)
        {
            Insert(key, obj, null, seconds);
        }
        public static void Insert(string key, object obj, double seconds, CacheItemPriority priority)
        {
            Insert(key, obj, null, seconds, priority);
        }
        public static void Insert(string key, object obj, CacheDependency dep, double seconds)
        {
            Insert(key, obj, dep, seconds, CacheItemPriority.Normal);
        }
        public static void Insert(string key, object obj, CacheDependency dep, double seconds, CacheItemPriority priority)
        {
            if (obj != null)
            {
                _cache.Insert(key, obj, dep, DateTime.Now.AddSeconds(Factor * seconds), TimeSpan.Zero, priority, null);
            }

        }

        public static void Max(string key, object obj)
        {
            Max(key, obj, null);
        }
        public static void Max(string key, object obj, CacheDependency dep)
        {
            if (obj != null)
            {
                _cache.Insert(key, obj, dep, DateTime.MaxValue, TimeSpan.Zero, CacheItemPriority.NotRemovable, null);
            }
        }

        public static object Get(string key)
        {
            return _cache[key];
        }
        public static long SecondFactorCalculate(double seconds)
        {
            return Convert.ToInt64(Math.Round(seconds * SecondFactor));
        }
    }
}
