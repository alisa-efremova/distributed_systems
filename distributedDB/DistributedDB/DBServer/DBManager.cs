using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBServer
{
    public class DBManager
    {
        private static ConcurrentDictionary<string, DBValue> _data = new ConcurrentDictionary<string, DBValue>();

        public void Set(string key, DBValue value)
        {
            _data.AddOrUpdate(key, value, (currentKey, existingVal) =>
            {
                return ChooseActualValue(value, existingVal);
            });
        }

        public string Get(string key)
        {
            if (_data.ContainsKey(key))
            {
                // TODO: Check if removed
                return _data[key].Value;
            }
            else
            {
                return null;
            }
        }

        public int VersionForKey(string key)
        {
            return (_data.ContainsKey(key)) ? _data[key].Version : 0;
        }

        public void Remove(string key, DBValue value)
        {
            // make sure deleted value is valid
            value.IsDeleted = true;
            value.Value = null;

            Set(key, value);
        }

        public bool ContainsKey(string key)
        {
            return _data.ContainsKey(key);
        }

        private DBValue ChooseActualValue(DBValue val1, DBValue val2)
        {
            if (val1.Version > val2.Version)
            {
                return val1;
            }
            else if (val1.Version == val2.Version && val1.ServerId < val2.ServerId)
            {
                return val1;
            }
            else
            {
                return val2;
            }
        }
    }
}
