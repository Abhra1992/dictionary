using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dictionary {
    public sealed class RedBlackTree<Key, Value> : BinarySearchTree<Key, Value> where Key : IComparable<Key> {
        public sealed new class Node : BinarySearchTree<Key, Value>.Node {
            public Node(Key key, Value value) : base(key, value) { }
            public Node(Key key, Value value, Color color)
                : base(key, value) {
                this.color = color;
            }
            public new Node left {
                get { return base.left as Node; }
                set { base.left = value; }
            }
            public new Node right {
                get { return base.right as Node; }
                set { base.right = value; }
            }
            public new Node parent {
                get { return base.parent as Node; }
                set { base.parent = value; }
            }
            public Color color = Color.RED;
            public override string ToString() {
                return String.Format("{0} {1}", base.ToString(), this.color);
            }
        }

        public enum Color { RED, BLACK }

        public RedBlackTree() {
            this.Nil = new Node(default(Key), default(Value), Color.BLACK);
            this.Nil.left = this.Nil.right = this.Nil.parent = this.Nil;
            this.root = this.Nil as Node;
        }

        public override void Display() {
            this.InorderTraverse(this.root);
        }

        public override bool Insert(Key key, Value value) {
            Node insert = new Node(key, value);
            insert.left = insert.right = this.Nil as Node;
            insert = this.IterativeInsert(insert) as Node;
            if(insert != null) {
                if(this.InsertFix(ref insert)) {
                    this.size++;
                    return true;
                }
            }
            return false;
        }

        private bool InsertFix(ref Node z) {
            while(z.parent.color == Color.RED) {
                if(z.parent == z.parent.parent.left) {
                    Node y = z.parent.parent.right as Node;
                    if(y.color == Color.RED) {
                        (z.parent as Node).color = Color.BLACK;
                        y.color = Color.BLACK;
                        (z.parent.parent as Node).color = Color.RED;
                        z = z.parent.parent as Node;
                    } else {
                        if(z == z.parent.right) {
                            z = z.parent as Node;
                            this.LeftRotate(z);
                        }
                        (z.parent as Node).color = Color.BLACK;
                        if(z.parent != this.root) {
                            (z.parent.parent as Node).color = Color.RED;
                            this.RightRotate(z.parent.parent);
                        }
                    }
                } else {
                    Node y = z.parent.parent.left as Node;
                    if(y.color == Color.RED) {
                        (z.parent as Node).color = Color.BLACK;
                        y.color = Color.BLACK;
                        (z.parent.parent as Node).color = Color.RED;
                        z = z.parent.parent as Node;
                    } else {
                        if(z == z.parent.left) {
                            z = z.parent as Node;
                            this.RightRotate(z);
                        }
                        (z.parent as Node).color = Color.BLACK;
                        if(z.parent != this.root) {
                            (z.parent.parent as Node).color = Color.RED;
                            this.LeftRotate(z.parent.parent);
                        }
                    }
                }
            }
            (this.root as Node).color = Color.BLACK;
            return true;
        }
    }
}
