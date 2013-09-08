using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dictionary {
    public sealed class AVLTree<Key, Value> : BinarySearchTree<Key, Value> where Key : IComparable<Key> {
        public sealed new class Node : BinarySearchTree<Key, Value>.Node {
            public Node(Key key, Value value) : base(key, value) { }
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
            private int balance = 0;
            //public int height = 0;
            public int Balance {
                get { return this.balance; }
                set { this.balance = value; }
            }
            public override string ToString() {
                return String.Format("{0} {1}", base.ToString(), this.balance);
            }
        }

        public override bool Insert(Key key, Value value) {
            Node insert = new Node(key, value);
            insert = this.IterativeInsert(insert) as Node;
            if(insert != null) {
                this.size++;
                Node parent = insert.parent, current = insert;
                while(parent != null) {
                    parent.height = Math.Max(parent.height, current.height + 1);
                    int balance = this.GetBalance(parent);
                    if(Math.Abs(balance) == 2) {
                        this.BalanceAt(parent, balance);
                    }
                    current = parent;
                    parent = parent.parent;
                }
                return true;
            } else {
                return false;
            }
        }

        public override KeyValuePair<Key, Value>? Delete(Key key) {
            throw new NotImplementedException();
        }

        private void BalanceAt(Node x, int balance) {
            if(balance == 2) {
                int rightBalance = this.GetBalance(x.right);
                if(rightBalance == 1 || rightBalance == 0) {
                    LeftRotate(x);
                } else if(rightBalance == -1) {
                    RightRotate(x.right);
                    LeftRotate(x);
                }
            } else if(balance == -2) {
                int leftBalance = this.GetBalance(x.left);
                if(leftBalance == 1) {
                    LeftRotate(x.left);
                    RightRotate(x);
                } else if(leftBalance == -1 || leftBalance == 0) {
                    RightRotate(x);
                }
            }
        }

        private int GetBalance(Node x) {
            int balance = this.GetHeight(x.right) - this.GetHeight(x.left);
            x.Balance = balance;
            return balance;
        }
    }
}
