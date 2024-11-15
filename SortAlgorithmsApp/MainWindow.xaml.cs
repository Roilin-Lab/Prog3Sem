using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Security.AccessControl;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SortAlgorithmsApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<Rectangle> array { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        private ObservableCollection<Rectangle> GenerateArray(int count)
        {
            var result = new ObservableCollection<Rectangle>();
            var w = container.ActualWidth / count;
            var h = container.ActualHeight / count;
            for (int i = 1; i <= count; i++)
            {
                var rect = new Rectangle()
                {
                    Width = w,
                    Height = h * i,
                    Fill = Brushes.White,


                    VerticalAlignment = VerticalAlignment.Bottom,
                    HorizontalAlignment = HorizontalAlignment.Left,
                };
                result.Add(rect);
            }
            return result;
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

        private async Task InsertionSort(ObservableCollection<Rectangle> collection)
        {
            if (collection.Count == 0 || collection.Count == 1) return;

            for (int i = 1; i < collection.Count; i++)
            {
                var value = collection[i];
                var index = i;
                while (index > 0 && collection[index - 1].Height > value.Height)
                {
                    //collection[index] = collection[index - 1];
                    collection.Move(index, index - 1);

                    await Task.Delay(1);
                    index--;
                }
                //collection[index] = value;
                collection.Move(index, i);

            }
        }

        private async Task BubbleSort(ObservableCollection<Rectangle> collection)
        {
            for (int k = 0; k < collection.Count - 1; k++)
            {
                for (int i = 0; i < collection.Count - 1; i++)
                {
                    if (collection[i].Height > collection[i + 1].Height)
                    {
                        collection[i].Fill = Brushes.Red;
                        collection.Move(i, i+1);
                        await Task.Delay(1);
                    }
                    collection[i].Fill = Brushes.White;
                }
                collection[k].Fill = Brushes.White;
            }
        }

        private async Task ShakerSort(ObservableCollection<Rectangle> collection)
        {
            bool swap = true;
            int index = collection.Count;
            while (swap)
            {
                swap = false;
                for (int i = collection.Count - index; i < index - 1; i++)
                {
                    collection[i].Fill = Brushes.Red;
                    if (collection[i].Height > collection[i + 1].Height)
                    {
                        collection[i].Fill = Brushes.Red;
                        collection.Move(i, i+1);
                        swap = true;
                        await Task.Delay(1);
                        collection[i + 1].Fill = Brushes.White;
                    }
                    collection[i].Fill = Brushes.White;
                }
                if (!swap) break;
                index--;
                swap = false;
                for (int i = index - 1; i >= collection.Count - index; i--)
                {
                    collection[i].Fill = Brushes.Red;
                    if (collection[i - 1].Height > collection[i].Height)
                    {
                        
                        collection.Move(i-1, i);
                        swap = true;
                        await Task.Delay(1);
                        collection[i-1].Fill = Brushes.White;
                    }
                    collection[i].Fill = Brushes.White;
                }
            }
        }

        private async Task ShellSort2(ObservableCollection<Rectangle> collection)
        {
            if (collection.Count == 0 || collection.Count == 1) return;

            for (int s = collection.Count / 2; s > 0; s /= 2)
            {
                for (int i = s; i < collection.Count; ++i)
                {
                    for (int j = i - s; j >= 0; j -= s)
                    {
                        
                        if (collection[j].Height > collection[j + s].Height)
                        {
                            collection[j].Fill = Brushes.Red;
                            collection.Move(j, j + s);
                            await Task.Delay(1);
                        }
                        collection[j + s].Fill = Brushes.White;
                    }

                }
            }

        }

        private async Task ShellSort(ObservableCollection<Rectangle> collection)
        {
            if (collection.Count == 0 || collection.Count == 1) return;

            int[] l = [1750, 701, 301, 132, 57, 23, 10, 4, 1];

            foreach (int k in l)
            {
                if (k >= collection.Count) continue;
                if (k == 1)
                {
                    await ShellSort2(collection);
                    return;
                }

                for (int i = 0; i < (collection.Count / k); i++)
                {
                    if (collection[i].Height < collection[i + k].Height)
                    {
                        collection[i].Fill = Brushes.Red;
                        collection.Move(i, i + k);
                        await Task.Delay(1);
                    }
                    collection[i + k].Fill = Brushes.White;
                }
            }
        }

        private async Task SelectionSort(ObservableCollection<Rectangle> collection)
        {
            if (collection.Count == 0 || collection.Count == 1) return;

            for (int i = 0; i < collection.Count - 1; i++)
            {
                int min = i;
                for (int j = i + 1; j < collection.Count; j++)
                {
                    if (collection[j].Height > collection[min].Height)
                    {
                        min = j;
                    }
                }
                collection[min].Fill = Brushes.Red;
                collection.Move(i, min);
                await Task.Delay(1);
                collection[i].Fill = Brushes.White;
            }
        }



        private async void Shuffle_Click(object sender, RoutedEventArgs e)
        {
            await Shuffle(array);
        }

        private async void Bubble_Click(object sender, RoutedEventArgs e)
        {
            await BubbleSort(array);
        }

        private async void Shaker_Click(object sender, RoutedEventArgs e)
        {
            await ShakerSort(array);
        }

        private async void Shell2_Click(object sender, RoutedEventArgs e)
        {
            await ShellSort2(array);
        }

        private async void Selection_Click(object sender, RoutedEventArgs e)
        {
            await SelectionSort(array);
        }

        private async void Shell_Click(object sender, RoutedEventArgs e)
        {
            await ShellSort(array);
        }

        private void Generate_Click(object sender, RoutedEventArgs e)
        {
            array = GenerateArray(200);
            con.ItemsSource = array;
            array.CollectionChanged += (sender, e) => { con.ItemsSource = array; };

        }
    }
}