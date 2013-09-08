using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dictionary {
    public class BinarySearchTree<Key, Value> : IDictionary<Key, Value> where Key : IComparable<Key> {
        public class Node {
            public KeyValuePair<Key, Value> pair;
            public Node(Key key, Value value) {
                this.pair = new KeyValuePair<Key, Value>(key, value);
            }
            public Key key {
                get { return this.pair.Key; }
            }
            public Value value {
                get { return this.pair.Value; }
            }
            public override String ToString() {
                return String.Format("{0} => {1}", this.key, this.value);
            }
            public int CompareTo(Node other) {
                return this.key.CompareTo(other.key);
            }
            public Node parent = null;
            public Node left = null;
            public Node right = null;
            public int height = 0;
        }
        protected int size = 0;
        protected Node root = null;
        protected Node Nil = null;

        public int Count {
            get { return this.size; }
        }

        public bool Empty {
            get { return this.size == 0; }
        }

        public Value this[Key key] {
            get {
                try {
                    return this.Search(key).value;
                } catch(NullReferenceException e) {
                    Console.WriteLine("Search For An Invalid Key: " + e.Message);
                    return default(Value);
                }
            }
        }

        public virtual bool Insert(Key key, Value value) {
            Node insert = new Node(key, value);
            if(this.IterativeInsert(insert) != null) {
                this.size++;
                return true;
            } else {
                return false;
            }
        }

        public virtual KeyValuePair<Key, Value>? Find(Key key) {
            Node find = this.Search(key);
            if(find != this.Nil) {
                return find.pair;
            } else {
                return null;
            }
        }

        public bool Contains(Key key) {
            return this.Search(key) != null;
        }

        public virtual KeyValuePair<Key, Value>? Delete(Key key) {
            Node rem = this.Search(key);
            this.Delete(this.root, rem);
            this.size--;
            return rem.pair;
        }

        public virtual bool Clear() {
            this.root = this.Nil;
            this.size = 0;
            return true;
        }

        public virtual void Display() {
            this.InorderTraverse(this.root);
        }

        protected virtual bool InorderTraverse(Node x) {
            if(x != this.Nil) {
                InorderTraverse(x.left);
                Console.WriteLine(x);
                InorderTraverse(x.right);
                return true;
            }
            return false;
        }

        protected Node RecursiveInsert(ref Node x, Key key, Value value) {
            if(x == this.Nil) {
                x = new Node(key, value);
                return x;
            } else if(key.CompareTo(x.key) < 0) {
                return RecursiveInsert(ref x.left, key, value);
            } else if(key.CompareTo(x.key) > 0) {
                return RecursiveInsert(ref x.right, key, value);
            }
            return null;
        }

        protected virtual Node IterativeInsert(Node insert) {
            Node y = this.Nil, z = this.root;
            Key key = insert.key;
            while(z != this.Nil) {
                y = z;
                if(key.CompareTo(z.key) < 0) {
                    z = z.left;
                } else if(key.CompareTo(z.key) > 0) {
                    z = z.right;
                }
            }
            insert.parent = y;
            if(y == this.Nil) {
                this.root = insert;
            } else if(key.CompareTo(y.key) < 0) {
                y.left = insert;
            } else if(key.CompareTo(y.key) > 0) {
                y.right = insert;
            }
            return insert;
        }

        protected Node Search(Key key) {
            return this.IterativeTreeSearch(this.root, key);
        }

        protected Node RecursiveTreeSearch(Node x, Key key) {
            if(x == this.Nil || key.Equals(x.key)) {
                return x;
            } else if(key.CompareTo(x.key) < 0) {
                return RecursiveTreeSearch(x.left, key);
            } else {
                return RecursiveTreeSearch(x.right, key);
            }
        }

        protected Node IterativeTreeSearch(Node x, Key key) {
            while(x != this.Nil && !key.Equals(x.key)) {
                if(key.CompareTo(x.key) < 0) {
                    x = x.left;
                } else {
                    x = x.right;
                }
            }
            return x;
        }

        protected void Delete(Node x, Node z) {
            if(z.left == this.Nil) {
                this.Transplant(z, z.right);
            } else if(z.right == this.Nil) {
                this.Transplant(z, z.left);
            } else {
                Node y = this.Minimum(z.right);
                if(y.parent != z) {
                    this.Transplant(y, y.right);
                    y.right = z.right;
                    y.right.parent = y;
                }
                this.Transplant(z, y);
                y.left = z.left;
                y.left.parent = y;
            }
        }

        protected void Transplant(Node u, Node v) {
            if(u.parent == this.Nil) {
                this.root = v;
            } else if(u == u.parent.left) {
                u.parent.left = v;
            } else {
                u.parent.right = v;
            }
            if(v != null) {
                v.parent = u.parent;
            }
        }

        protected Node Minimum(Node x) {
            while(x.left != this.Nil) {
                x = x.left;
            }
            return x;
        }

        protected Node Maximum(Node x) {
            while(x.right != this.Nil) {
                x = x.right;
            }
            return x;
        }

        protected Node LeftRotate(Node x) {
            Node y = x.right;
            x.right = y.left;
            if(y.left != this.Nil) {
                y.left.parent = x;
            }
            y.parent = x.parent;
            if(x.parent == this.Nil) {
                this.root = y;
            } else if(x == x.parent.left) {
                x.parent.left = y;
            } else {
                x.parent.right = y;
            }
            y.left = x;
            x.parent = y;
            // Additions for Height Balance
            x.height = Math.Max(this.GetHeight(x.left), this.GetHeight(x.right)) + 1;
            y.height = Math.Max(this.GetHeight(y.right), x.height) + 1;
            return y;
        }

        protected Node RightRotate(Node x) {
            Node y = x.left;
            x.left = y.right;
            if(y.right != this.Nil) {
                y.right.parent = x;
            }
            y.parent = x.parent;
            if(x.parent == this.Nil) {
                this.root = y;
            } else if(x == x.parent.right) {
                x.parent.right = y;
            } else {
                x.parent.left = y;
            }
            y.right = x;
            x.parent = y;
            // Additions for Height Balance
            x.height = Math.Max(this.GetHeight(x.left), this.GetHeight(x.right)) + 1;
            y.height = Math.Max(this.GetHeight(y.left), x.height) + 1;
            return y;
        }

        protected int GetHeight(Node x) {
            if(x == this.Nil) {
                return -1;
            } else if(x.left == this.Nil && x.right == this.Nil) {
                return 0;
            } else if(x.left == this.Nil) {
                return 1 + x.right.height;
            } else if(x.right == this.Nil) {
                return 1 + x.left.height;
            } else {
                return 1 + Math.Max(x.left.height, x.right.height);
            }
        }
    }
}
