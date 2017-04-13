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
            SortedDictionary<string, int> _map = new SortedDictionary<string, int> { };
            private Object thisLock = new Object();

            public void Store(int val, string corrId)
            {
                lock (thisLock)
                {
                    _map[corrId] = val;
                }
            }

            public int GetCount()
            {
                int sum = 0;
                foreach (var val in _map.Values)
                {
                    sum += val;
                }
                return sum;
            }

            public bool Remove(string corrId)
            {
                return _map.Remove(corrId);
            }
        }

        private static readonly ThreadSafeStatsData _sourceLines = new ThreadSafeStatsData();
        private static readonly ThreadSafeStatsData _goodLines = new ThreadSafeStatsData();

        public double GetGoodLinesPercent()
        {
            int sourceLineCount = _sourceLines.GetCount();
            if (sourceLineCount != 0)
            {
                return (double)_goodLines.GetCount() / (double)sourceLineCount;
            }
            else
            {
                return 0;
            }
        }

        public void SaveSourceLinesCount(int linesCount, string id)
        {
            _sourceLines.Store(linesCount, id);
        }

        public void SaveGoodLinesCount(int linesCount, string id)
        {
            _goodLines.Store(linesCount, id);
        }

        public void RemoveStats(string id)
        {
            _sourceLines.Remove(id);
            _goodLines.Remove(id);
        }
    }
}
