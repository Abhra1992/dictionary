using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dictionary {
    public sealed class SplayTree<Key, Value> : BinarySearchTree<Key, Value> where Key : IComparable<Key> {
        public sealed new class Node : BinarySearchTree<Key, Value>.Node {
            public Node(Key key, Value value) : base(key, value) { }
        }

        public override bool Insert(Key key, Value value) {
            if(this.root == null) {
                root = new Node(key, value);
                this.size++;
                return true;
            }
            this.Splay(key);
            int c = key.CompareTo(root.key);
            if(c == 0) {
                // Duplicate Item
                return false;
            }
            Node node = new Node(key, value);
            if(c < 0) {
                node.left = root.left;
                node.right = root;
                root.left = null;
            } else {
                node.right = root.right;
                node.left = root;
                root.right = null;
            }
            root = node;
            this.size++;
            return true;
        }

        public override KeyValuePair<Key, Value>? Find(Key key) {
            if(this.root == null) {
                //throw new KeyNotFoundException("Key could not be found");
                return null;
            }
            this.Splay(key);
            if(this.root.key.CompareTo(key) != 0) {
                //throw new KeyNotFoundException("Key could not be found");
                return null;
            } else {
                return root.pair;
            }
        }

        public override KeyValuePair<Key, Value>? Delete(Key key) {
            this.Splay(key);
            if(key.CompareTo(this.root.key) != 0) {
                //throw new KeyNotFoundException("Key to be deleted could not be found");
                return null;
            }
            Node remove = this.root as Node;
            if(this.root.left != null) {
                this.root = this.root.right;
            } else {
                Node x = this.root.right as Node;
                this.root = this.root.left;
                this.Splay(key);
                this.root.right = x;
            }
            return remove.pair;
        }

        private void Splay(Key key) {
            Node left, right, root, y, head;
            left = right = head = new Node(default(Key), default(Value));
            root = this.root as Node;
            while(true) {
                int c = key.CompareTo(root.key);
                if(c < 0) {
                    if(root.left == null)
                        break;
                    if(key.CompareTo(root.left.key) < 0) {
                        y = root.left as Node;
                        root.left = y.right;
                        y.right = root;
                        root = y;
                        if(root.left == null)
                            break;
                    }
                    right.left = root;
                    right = root;
                    root = root.left as Node;
                } else if(c > 0) {
                    if(root.right == null)
                        break;
                    if(key.CompareTo(root.right.key) > 0) {
                        y = root.right as Node;
                        root.right = y.left;
                        y.left = root;
                        root = y;
                        if(root.right == null)
                            break;
                    }
                    left.right = root;
                    left = root;
                    root = root.right as Node;
                } else {
                    break;
                }
            }
            left.right = root.left;
            right.left = root.right;
            root.left = head.right;
            root.right = head.left;
            this.root = root;
            return;
        }
    }
}
