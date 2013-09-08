using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using System.IO;
using System.Diagnostics;

namespace Dictionary {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        enum KeyType {
            INTEGER, CHARACTER, STRING
        }
        enum ValueType {
            INTEGER, FLOAT, STRING
        }
        enum DictionaryStrcuture {
            BST, AVL, RBT, SPLAY, HASH
        }
        IDictionary<int, int> dictionary = null;
        Stopwatch watch = new Stopwatch();

        public MainWindow() {
            InitializeComponent();
        }

        private void BenchmarkButton_Click(object sender, RoutedEventArgs e) {
            int size = 100 * (int)Math.Pow(10, this.DictionarySize.SelectedIndex);
            this.ResultsBlock.Text = "";
            switch((DictionaryStrcuture)this.DictionaryType.SelectedIndex) {
                case DictionaryStrcuture.BST:
                    this.ResultsBlock.Text += "## Binary Search Tree ##\n";
                    dictionary = new BinarySearchTree<int, int>();
                    break;
                case DictionaryStrcuture.AVL:
                    this.ResultsBlock.Text += "## AVL Tree ##\n";
                    dictionary = new AVLTree<int, int>();
                    break;
                case DictionaryStrcuture.RBT:
                    this.ResultsBlock.Text += "## Red Black Tree ##\n";
                    dictionary = new RedBlackTree<int, int>();
                    break;
                case DictionaryStrcuture.SPLAY:
                    this.ResultsBlock.Text += "## Splay Tree ##\n";
                    dictionary = new SplayTree<int, int>();
                    break;
                case DictionaryStrcuture.HASH:
                    this.ResultsBlock.Text += "## Hash Table ##\n";
                    dictionary = new HashTable<int, int>();
                    break;
                default:
                    this.ResultsBlock.Text += "## Hash Table ##\n";
                    dictionary = new HashTable<int, int>();
                    break;
            }
            this.benchmark(ref dictionary, size);
        }

        private void benchmark(ref IDictionary<int, int> dictionary, int size) {
            if(this.BenchmarkType.SelectedIndex == 0) {
                this.benchmark_sorted(ref dictionary, size);
            } else {
                this.benchmark_random(ref dictionary, size);
            }
            dictionary.Clear();
            return;
        }

        private void benchmark_sorted(ref IDictionary<int, int> dictionary, int size) {
            this.insert_sorted(ref dictionary, size);
            this.find_sorted(ref dictionary, size);
            this.find_reverse_sorted(ref dictionary, size);
            return;
        }

        private void benchmark_random(ref IDictionary<int, int> dictionary, int size) {
            int[] nodes = this.load_nodes_from_file();
            // Start Benchmark after File read
            this.insert_random(ref dictionary, nodes);
            this.find_random(ref dictionary, nodes);
            this.find_sorted(ref dictionary, size);
            this.find_reverse_sorted(ref dictionary, size);
            return;
        }

        private int[] load_nodes_from_file() {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Text documents (.txt)|*.txt";
            if(dialog.ShowDialog() == true) {
                string file = dialog.FileName;
                if(file == String.Empty) {
                    return null;
                }
                List<int> NodeList = new List<int>();
                string line = "";
                using(StreamReader reader = new StreamReader(file)) {
                    while((line = reader.ReadLine()) != null) {
                        int key = Convert.ToInt32(line);
                        NodeList.Add(key);
                    }
                }
                int[] nodes = NodeList.ToArray();
                return nodes;
            }
            return null;
        }

        private void insert_sorted(ref IDictionary<int, int> dictionary, int size) {
            this.ResultsBlock.Text += String.Format("Sorted Insert of {0} Nodes\n", size);
            this.watch.Reset();
            this.ResultsBlock.Text += String.Format("Insertion Begins: {0} entries\n", dictionary.Count);
            long memory = System.GC.GetTotalMemory(true);
            this.watch.Start();
            for(int key = 1; key <= size; key++) {
                dictionary.Insert(key, key);
            }
            this.watch.Stop();
            memory = System.GC.GetTotalMemory(true) - memory;
            this.ResultsBlock.Text += String.Format("Memory Used: {0} bytes\n", memory);
            this.ResultsBlock.Text += String.Format("Insertion Ends: {0}ms\n", this.watch.Elapsed.TotalMilliseconds);
        }

        private void find_sorted(ref IDictionary<int, int> dictionary, int size) {
            this.ResultsBlock.Text += String.Format("Sorted Find of {0} Nodes\n", size);
            this.watch.Reset();
            this.ResultsBlock.Text += String.Format("Find Begins: {0} entries\n", dictionary.Count);
            this.watch.Start();
            for(int key = 1; key <= size; key++) {
                dictionary.Find(key);
            }
            this.watch.Stop();
            this.ResultsBlock.Text += String.Format("Find Ends: {0}ms\n", this.watch.Elapsed.TotalMilliseconds);
        }

        private void find_reverse_sorted(ref IDictionary<int, int> dictionary, int size) {
            this.ResultsBlock.Text += String.Format("Reverse Sorted Find of {0} Nodes\n", size);
            this.watch.Reset();
            this.ResultsBlock.Text += String.Format("Find Begins: {0} entries\n", dictionary.Count);
            this.watch.Start();
            for(int key = size; key > 0; key--) {
                dictionary.Find(key);
            }
            this.watch.Stop();
            this.ResultsBlock.Text += String.Format("Find Ends: {0}ms\n", this.watch.Elapsed.TotalMilliseconds);
        }

        private void insert_random(ref IDictionary<int, int> dictionary, int[] nodes) {
            this.ResultsBlock.Text += String.Format("Random Insert of Nodes\n");
            this.watch.Reset();
            this.ResultsBlock.Text += String.Format("Insertion Begins: {0} entries\n", dictionary.Count);
            long memory = System.GC.GetTotalMemory(true);
            this.watch.Start();
            foreach(int key in nodes) {
                dictionary.Insert(key, key);
            }
            this.watch.Stop();
            memory = System.GC.GetTotalMemory(true) - memory;
            this.ResultsBlock.Text += String.Format("Memory Used: {0} bytes\n", memory);
            this.ResultsBlock.Text += String.Format("Insertion Ends: {0}ms\n", this.watch.Elapsed.TotalMilliseconds);
        }

        private void find_random(ref IDictionary<int, int> dictionary, int[] nodes) {
            this.ResultsBlock.Text += String.Format("Random Find of Nodes\n");
            this.watch.Reset();
            this.ResultsBlock.Text += String.Format("Find Begins: {0} entries\n", dictionary.Count);
            this.watch.Start();
            foreach(int key in nodes) {
                dictionary.Find(key);
            }
            this.watch.Stop();
            this.ResultsBlock.Text += String.Format("Find Ends: {0}ms\n", this.watch.Elapsed.TotalMilliseconds);
        }
    }
}
