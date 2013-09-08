using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dictionary {
    public class HashTable<Key, Value> : IDictionary<Key, Value> where Key : IComparable<Key> {
        private int size = 0;
        private Dictionary<Key, Value> table;

        public HashTable() {
            this.table = new Dictionary<Key, Value>();
        }

        public int Count {
            get { return this.size; }
        }

        public Value this[Key key] {
            get { return this.table[key]; }
        }

        public override string ToString() {
            StringBuilder display = new StringBuilder();
            foreach(KeyValuePair<Key, Value> item in this.table) {
                display.AppendLine(String.Format("{0} => {1}", item.Key, item.Value));
            }
            return display.ToString();
        }

        public bool Insert(Key key, Value value) {
            this.table.Add(key, value);
            this.size++;
            return true;
        }

        public KeyValuePair<Key, Value>? Find(Key key) {
            if(this.table.ContainsKey(key)) {
                Value value = this.table[key];
                return new KeyValuePair<Key, Value>(key, value);
            }
            return default(KeyValuePair<Key, Value>);
        }

        public KeyValuePair<Key, Value>? Delete(Key key) {
            if(this.table.ContainsKey(key)) {
                var value = this.table[key];
                this.table.Remove(key);
                this.size--;
                return new KeyValuePair<Key, Value>(key, value);
            }
            return default(KeyValuePair<Key, Value>);
        }

        public bool Contains(Key key) {
            return this.table.ContainsKey(key);
        }

        public bool Clear() {
            this.table.Clear();
            this.size = 0;
            return true;
        }
    }
}
