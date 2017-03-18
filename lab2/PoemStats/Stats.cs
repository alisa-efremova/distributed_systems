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
        /*private class ThreadSafeStatsData
        {

            int _sourceLinesCount = 0;
            int _goodLinesCount = 0;

            public void Store(int val)
            {
                lock(){

                }
            }
        }

        private static readonly ThreadSafeStatsData _statsData = new ThreadSafeStatsData();*/

        private static readonly Stats instance = new Stats();

        int _sourceLinesCount = 0;
        int _goodLinesCount = 0;
        
        CacheItemPolicy _cachePolicy;
        static readonly TimeSpan _cacheSlidingExpiration = TimeSpan.FromMinutes(5);

        private Stats()
        {
            _cachePolicy = new CacheItemPolicy();
            _cachePolicy.SlidingExpiration = _cacheSlidingExpiration;
        }

        public double GetGoodLinesPercent()
        {
            if (_sourceLinesCount != 0)
            {
                return (double)_goodLinesCount / (double)_sourceLinesCount;
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
                _sourceLinesCount += (int)value;
                _goodLinesCount += linesCount;
                MemoryCache.Default.Remove(id);
            }
        }

        public static Stats GetInstance()
        {
            return instance;
        }
    }
}
