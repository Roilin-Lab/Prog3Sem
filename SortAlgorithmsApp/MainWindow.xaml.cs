using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SortAlgorithmsApp
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<Rectangle> array { get; set; } = new ObservableCollection<Rectangle>();

        Dictionary<string, Func<Task>> functionSortDict;
        public int arrayAcceses = 0;
        public int comparisions = 0;
        public Stopwatch time = new Stopwatch();
        public int delay = 1;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            functionSortDict = new Dictionary<string, Func<Task>>()
            {
                { "Insertion", InsertionSort },
                { "Selection", SelectionSort },
                { "Bubble", BubbleSort },
                { "Shaker", ShakerSort },
                { "Shell", ShellSort },
                { "Quick", QuickSort },
            };
        }

        private void Array_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            arrayAcceses++;
            comps.Text = $"{comboBoxAlgorithm.SelectedItem} sort | Comparisions - {comparisions} | Array acceses - {arrayAcceses} | Time - {time.ElapsedMilliseconds} ms";
        }

        private bool Compare(double obj1, double obj2)
        {
            comparisions++;
            return obj1 > obj2;
        }

        private Rectangle GetValue(int index)
        {
            arrayAcceses++;
            return array[index];
        }

        private void GenerateArray(int count)
        {
            array.Clear();
            var w = border.ActualWidth / count;
            var h = (border.ActualHeight - 3) / count;
            for (int i = 1; i <= count; i++)
            {
                var rect = new Rectangle()
                {
                    Width = w,
                    Height = h * i,
                    Fill = Brushes.White,
                    SnapsToDevicePixels = true,
                    VerticalAlignment = VerticalAlignment.Bottom,
                    HorizontalAlignment = HorizontalAlignment.Left,
                };
                array.Add(rect);
            }
        }

        private async Task Shuffle(ObservableCollection<Rectangle> collection)
        {
            var r = new Random();
            int n = collection.Count;
            for (int i = 0; i < n - 1; i++)
            {
                int j = r.Next(i, n);
                if (j != i)
                {
                    collection[i].Fill = Brushes.Red;
                    collection.Move(j, i);
                    await Task.Delay(1);
                    collection[i].Fill = Brushes.White;
                }
                collection[j].Fill = Brushes.White;
            }
        }

        private async Task InsertionSort()
        {
            if (array.Count == 0 || array.Count == 1) return;

            for (int i = 1; i < array.Count; i++)
            {
                var temp = GetValue(i);
                var index = i;
                while (index > 0 && Compare(GetValue(index - 1).Height, temp.Height))
                {
                    array[index - 1].Fill = Brushes.Red;
                    array.Move(index - 1, index);
                    await Task.Delay(delay);
                    array[index].Fill = Brushes.White;
                    index--;
                }
                array.Remove(temp);
                array.Insert(index, temp);
            }
        }

        private async Task BubbleSort()
        {
            for (int k = 0; k < array.Count - 1; k++)
            {
                for (int i = 0; i < array.Count - 1; i++)
                {
                    if (Compare(GetValue(i).Height, GetValue(i + 1).Height))
                    {
                        array[i].Fill = Brushes.Red;
                        array.Move(i, i+1);
                        await Task.Delay(delay);
                    }
                    array[i].Fill = Brushes.White;
                }
                array[k].Fill = Brushes.White;
            }
        }

        private async Task ShakerSort()
        {
            bool swap = true;
            int index = array.Count;
            while (swap)
            {
                swap = false;
                for (int i = array.Count - index; i < index - 1; i++)
                {
                    array[i].Fill = Brushes.Red;
                    if (Compare(GetValue(i).Height, GetValue(i + 1).Height))
                    {
                        array[i].Fill = Brushes.Red;
                        array.Move(i, i+1);
                        await Task.Delay(delay);
                        swap = true;
                        array[i + 1].Fill = Brushes.White;
                    }
                    array[i].Fill = Brushes.White;
                }
                if (!swap) break;
                index--;
                swap = false;

                for (int i = index - 1; i >= array.Count - index; i--)
                {
                    array[i].Fill = Brushes.Red;
                    if (Compare(GetValue(i - 1).Height, GetValue(i).Height))
                    {
                        array.Move(i-1, i);
                        await Task.Delay(delay);
                        swap = true;
                        array[i-1].Fill = Brushes.White;
                    }
                    array[i].Fill = Brushes.White;
                }
            }
        }

        private async Task ShellSort()
        {
            if (array.Count == 0 || array.Count == 1) return;

            for (int s = array.Count / 2; s > 0; s /= 2)
            {
                for (int i = s; i < array.Count; i += 1)
                {
                    var temp = GetValue(i);
                    int j;
                    for (j = i; j >= s && Compare(GetValue(j - s).Height, temp.Height); j -= s)
                    {
                        array[j - s].Fill = Brushes.Red;
                        array.Move(j - s, j);
                        await Task.Delay(delay);
                        array[j].Fill = Brushes.White;
                    }
                    array.Remove(temp);
                    array.Insert(j, temp);
                }
            }
        }

        private async Task SelectionSort()
        {
            if (array.Count == 0 || array.Count == 1) return;

            for (int i = 0; i < array.Count - 1; ++i)
            {
                int min = i;
                for (int j = i + 1; j < array.Count; ++j)
                {
                    if (Compare(GetValue(min).Height, GetValue(j).Height))
                    {
                        min = j;
                        await Task.Delay(delay);
                    }
                }
                array[i].Fill = Brushes.Red;
                if (i != min)
                {
                    array.Move(min, i);
                    await Task.Delay(delay);
                }
                array[min].Fill = Brushes.White;
            }
        }

        private async Task _quickSort(int low, int high)
        {
            var partition = async (int low, int high) =>
            {
                var pivot = GetValue(high);
                var i = low - 1;

                for (var j = low; j <= high - 1; j++)
                {
                    if (Compare(pivot.Height, GetValue(j).Height))
                    {
                        i++;
                        array[i].Fill = Brushes.Red;
                        var tmp1 = GetValue(i);
                        var tmp2 = GetValue(j);
                        array.Remove(tmp1);
                        array.Insert(j, tmp1);
                        array.Remove(tmp2);
                        array.Insert(i, tmp2);

                        await Task.Delay(delay);
                        array[j].Fill = Brushes.White;
                    }
                }
                array.Remove(pivot);
                array.Insert(i + 1, pivot);
                return i + 1;
            };

            if (low < high)
            {
                int pi = await partition(low, high);

                await _quickSort(low, pi - 1);
                await _quickSort(pi + 1, high);
            }
        }

        private async Task QuickSort()
        {
            await _quickSort(0, array.Count - 1);
        }

        private async void Shuffle_Click(object sender, RoutedEventArgs e)
        {
            await Shuffle(array);
        }

        private async void Sort_Click(object sender, RoutedEventArgs e)
        {
            array.CollectionChanged += Array_CollectionChanged;
            comparisions = 0;
            arrayAcceses = 0;
            delay = int.Parse(textBoxDelay.Text);
            time.Restart();
            await functionSortDict[(string)comboBoxAlgorithm.SelectedItem]();
            time.Stop();
            rtb.AppendText($"{comboBoxAlgorithm.SelectedItem} sort | Comparisions - {comparisions} | Array acceses - {arrayAcceses} | Time - {time.ElapsedMilliseconds} ms | Elements - {array.Count} | Dalay - {delay}\n");
            array.CollectionChanged -= Array_CollectionChanged;
        }

        private void Generate_Click(object sender, RoutedEventArgs e)
        {
            
            GenerateArray(int.Parse(textBoxCount.Text));
            con.ItemsSource = array;
        }

        private void comboBoxAlgorithm_Loaded(object sender, RoutedEventArgs e)
        {
            comboBoxAlgorithm.ItemsSource = functionSortDict.Keys;
            comboBoxAlgorithm.SelectedIndex = 0;
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void rtb_Loaded(object sender, RoutedEventArgs e)
        {
            rtb.Document.PageWidth = 1000;
        }
    }
}