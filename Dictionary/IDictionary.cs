using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dictionary {
    public interface IDictionary<Key, Value> where Key : IComparable<Key> {
        int Count { get; }
        Value this[Key key] { get; }
        bool Insert(Key key, Value value);
        KeyValuePair<Key, Value>? Find(Key key);
        bool Contains(Key key);
        KeyValuePair<Key, Value>? Delete(Key key);
        bool Clear();
    }
}
