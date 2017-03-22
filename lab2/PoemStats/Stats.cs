using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Caching;

namespace PoemStats
{
    public class Stats
    {
        private class ThreadSafeStatsData
        {
            int _val = 0;
            private Object thisLock = new Object();

            public void Store(int val)
            {
                lock (thisLock)
                {
                    _val = val;
                }
            }

            public int Get()
            {
                return _val;
            }
        }

        private static readonly ThreadSafeStatsData _sourceLinesCount = new ThreadSafeStatsData();
        private static readonly ThreadSafeStatsData _goodLinesCount = new ThreadSafeStatsData();

        CacheItemPolicy _cachePolicy;
        static readonly TimeSpan _cacheSlidingExpiration = TimeSpan.FromMinutes(5);

        public Stats()
        {
            _cachePolicy = new CacheItemPolicy();
            _cachePolicy.SlidingExpiration = _cacheSlidingExpiration;
        }

        public double GetGoodLinesPercent()
        {
            if (_sourceLinesCount.Get() != 0)
            {
                return (double)_goodLinesCount.Get() / (double)_sourceLinesCount.Get();
            }
            else
            {
                return 0;
            }
        }

        public void SaveSourceLinesCount(int linesCount, string id)
        {
            MemoryCache.Default.AddOrGetExisting(id, linesCount, _cachePolicy);
        }

        public void SaveGoodLinesCount(int linesCount, string id)
        {
            var value = MemoryCache.Default.Get(id);
            if (value != null)
            {
                int sourceLinesCount = _sourceLinesCount.Get();
                int goodLinesCount = _goodLinesCount.Get();
                _sourceLinesCount.Store(sourceLinesCount + (int)value);
                _goodLinesCount.Store(goodLinesCount + linesCount);
                MemoryCache.Default.Remove(id);
            }
        }
    }
}
