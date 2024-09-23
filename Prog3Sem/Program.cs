using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Prog3Sem
{
    public enum FormatInchEnum
    {
        Eight,
        FiveAndQuater,
        ThreeAndHalf,
        Three,
    }

    public enum SideEnum
    {
        Single,
        Double,
    }

    public class Floppy
    {
        private string _manufacture;
        private FormatInchEnum _format;
        private ushort _capacity;

        public Floppy()
        {
            _manufacture = "<Undefinde>";
            _format = 0;
            _capacity = 0;
        }

        [JsonConstructor]
        public Floppy(string manufacture, FormatInchEnum format, ushort capacity)
        {
            _manufacture = manufacture;
            _format = format;
            _capacity = capacity;
        }

        public virtual void WriteFloppy()
        {
            Console.WriteLine("Запись на гибкий диск... Пожалйста не выключайте компьютер!");
            Task.Delay(200).Wait();
        }
        public virtual void ReadFloppy()
        {
            Console.WriteLine("Чтение гибкого диска... ");
            Task.Delay(200).Wait();
        }

        public string Manufacture { get => _manufacture; }
        public FormatInchEnum Format { get => _format; }
        public ushort Capacity { get => _capacity; }
    }

    public class FloppyHD : Floppy
    {
        private SideEnum _side;

        public FloppyHD(string manufacture, FormatInchEnum format, ushort capacity, SideEnum side) : this(manufacture, format, capacity)
        {
            _side = side;
        }

        public FloppyHD(string manufacture, FormatInchEnum format, ushort capacity) : base(manufacture, format, capacity)
        {
            _side = SideEnum.Single;
        }

        public override void WriteFloppy()
        {
            if (_side == SideEnum.Double)
            {
                Random r = new();
                Console.WriteLine("Запись на гибкий диск... Пожалйста не выключайте компьютер!");
                Console.WriteLine("Записываем первую сторону...");
                Task.Delay(r.Next(50, 300)).Wait();
                Console.WriteLine("Записываем вторую сторону...");
                Task.Delay(r.Next(50, 300)).Wait();
            }
            else 
            {
                base.WriteFloppy();
            }
        }
        public override void ReadFloppy()
        {
            if (_side == SideEnum.Double)
            {
                Random r = new();
                Console.WriteLine("Чтение гибкого диска... Пожалйста не выключайте компьютер!");
                Console.WriteLine("Читаем первую сторону...");
                Task.Delay(r.Next(50, 300)).Wait();
                Console.WriteLine("Читаем вторую сторону...");
                Task.Delay(r.Next(50, 300)).Wait();
            }
            else
            {
                base.WriteFloppy();
            }
        }

        public SideEnum Side { get => _side; }
    }

    public class FloppyStore
    {
        private Floppy[] _store;
        
        public FloppyStore()
        {
            _store = Array.Empty<Floppy>();
        }
        public FloppyStore(int size)
        {
            _store = new Floppy[size];
        }
        [JsonConstructor]
        public FloppyStore(Floppy[] store)
        {
            _store = store;
        }

        public Floppy this[int index]
        {
            get => _store[index];
            set => _store[index] = value;
        }

        public void Shuffle()
        {
            Random r = new Random();
            r.Shuffle(_store);
        }

        public void SelectionSort()
        {
            if (_store.Length == 0 || _store.Length == 1) return;

            for (int i = 0; i < _store.Length - 1; i++)
            {
                int min= i;
                for (int j = i + 1; j < _store.Length; j++)
                {
                    if (_store[j].Capacity < _store[min].Capacity)
                    {
                        min = j;
                    }
                }
                (_store[min], _store[i]) = (_store[i], _store[min]);
            }
        }

        public void InsertionSort()
        {
            if (_store.Length == 0 || _store.Length == 1) return;

            for (int i = 1; i < _store.Length; i++)
            {
                var value = _store[i];
                var index = i;
                while (index > 0 && _store[index - 1].Capacity > value.Capacity)
                {
                    _store[index] = _store[index - 1];
                    index--;
                }
                _store[index] = value;
            }
        }

        public void BubbleSort()
        {
            for (int k = 0; k < _store.Length - 1; k++)
            {
                for (int i = 0; i < _store.Length - 1; i++)
                {
                    if (_store[i].Capacity > _store[i + 1].Capacity)
                    {
                        (_store[i], _store[i + 1]) = (_store[i + 1], _store[i]);
                    }
                }
            }
        }

        public void ShakerSort()
        {
            bool swap = true;
            int index = _store.Length;
            while (swap)
            {
                swap = false;
                for (int i = _store.Length - index; i < index - 1; i++)
                {
                    if (_store[i].Capacity > _store[i + 1].Capacity)
                    {
                        (_store[i], _store[i + 1]) = (_store[i + 1], _store[i]);
                        swap = true;
                    }
                }
                if (!swap) break;
                index--;
                swap = false;
                for (int i = index - 1; i >= _store.Length - index; i--)
                {
                    if (_store[i-1].Capacity > _store[i].Capacity)
                    {
                        (_store[i-1], _store[i]) = (_store[i], _store[i-1]);
                        swap = true;
                    }
                }
            }
        }

        public void ShellSort()
        {
            if (_store.Length == 0 || _store.Length == 1) return;

            int[] l = [1750, 701, 301, 132, 57, 23, 10, 4, 1];

            foreach (int k in l)
            {
                if (k >= _store.Length) continue;
                if (k == 1)
                {
                    InsertionSort();
                    return;
                }

                for (int i = 0; i < (_store.Length / k); i++)
                {
                    if (_store[i].Capacity > _store[i + k].Capacity)
                    {
                        (_store[i], _store[i + k]) = (_store[i + k], _store[i]);
                    }
                }
            }
        }

        public void ShellSort2()
        {
            if (_store.Length == 0 || _store.Length == 1) return;

            for (int s = _store.Length / 2; s > 0; s /= 2)
            {
                for (int i = s; i < _store.Length; ++i)
                {
                    for (int j = i - s; j >= 0; j -= s)
                    {
                        if (_store[j].Capacity > _store[j + s].Capacity)
                        {
                            (_store[j], _store[j + s]) = (_store[j + s], _store[j]);
                        }
                    }
                    
                }
            }
            
        }

        public Floppy[] Store { get => _store; }
        public int Lenght => _store.Length;
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            lab_6();
        }

        static void lab_1()
        {
            Console.WriteLine("Введите данные для гибкого диска:");
            Console.Write("Производитель: ");
            string mnfc = Console.ReadLine() ?? "";
            Console.Write("Формат (0: 8дюйм, 1: 5.25дюйм, 2: 3.5дюйм, 3: 3дюйм): ");
            FormatInchEnum format = (FormatInchEnum)Convert.ToUInt16(Console.ReadLine());
            Console.Write("Емкость в [КБ]: ");
            ushort capacity = Convert.ToUInt16(Console.ReadLine());
            Console.WriteLine();

            Floppy floppy = new(mnfc, format, capacity);

            Console.WriteLine("Ваш диск: ");
            Console.WriteLine(floppy.Manufacture);
            Console.WriteLine(floppy.Format);
            Console.WriteLine(floppy.Capacity);
        }

        static void lab_2()
        {
            Floppy floppy = new();

            Console.WriteLine("Диск с конструктором по умолчанию: ");
            Console.WriteLine(floppy.Manufacture);
            Console.WriteLine(floppy.Format);
            Console.WriteLine(floppy.Capacity);
            Console.WriteLine();

            Floppy floppy1 = new("IBM", FormatInchEnum.FiveAndQuater, 1440);

            Console.WriteLine("Диск с перегруженным конструктором: ");
            Console.WriteLine(floppy1.Manufacture);
            Console.WriteLine(floppy1.Format);
            Console.WriteLine(floppy1.Capacity);
            Console.WriteLine();
        }

        static void lab_3()
        {
            Floppy floppy1 = new("IBM", FormatInchEnum.FiveAndQuater, 1440);

            Console.WriteLine("Диск родитель: ");
            Console.WriteLine(floppy1.Manufacture);
            Console.WriteLine(floppy1.Format);
            Console.WriteLine(floppy1.Capacity);
            Console.WriteLine();
            floppy1.WriteFloppy();
            Console.WriteLine();

            FloppyHD floppy2 = new("Apple", FormatInchEnum.ThreeAndHalf, 720, SideEnum.Double);

            Console.WriteLine("Диск наследник: ");
            Console.WriteLine(floppy2.Manufacture);
            Console.WriteLine(floppy2.Format);
            Console.WriteLine(floppy2.Capacity);
            Console.WriteLine(floppy2.Side);
            Console.WriteLine();
            floppy2.WriteFloppy();
            Console.WriteLine();
            floppy2.ReadFloppy();
        }

        static void lab_4()
        {
            Random r = new Random();

            Console.Write("Введите желаемый размер хранилища в целых числах: ");
            int size = Convert.ToInt32(Console.ReadLine());
            FloppyStore store = new FloppyStore(size);
            Console.WriteLine("Создаем Хранилище Дисков со случайными параметрами...");
            for (int i= 0; i < store.Lenght; i++)
            {
                store[i] = new Floppy($"Floppy {i+1}", (FormatInchEnum)r.Next(0, 3), (ushort)r.Next(0, 2880));
            }
            Console.WriteLine("Готово! Хранилище создано");
            Console.WriteLine("Вывожу на экран все диски...");
            for (int i = 0; i < store.Lenght; i++)
            {
                Console.WriteLine($"Диск:\r\n" +
                    $"  |Название: {store[i].Manufacture}\r\n" +
                    $"  |Формат: {store[i].Format}\r\n" +
                    $"  |Емкость: {store[i].Capacity}\r\n");
            }
        }

        static void lab_5()
        {
            Random r = new Random();

            FloppyStore store = new FloppyStore(5);
            Console.WriteLine("Создаем Хранилище Дисков со случайными параметрами...");
            for (int i = 0; i < store.Lenght; i++)
            {
                store[i] = new Floppy($"Floppy {i + 1}", (FormatInchEnum)r.Next(0, 3), (ushort)r.Next(0, 2880));
            }

            FileStream fs = new("data.json", FileMode.Create, FileAccess.Write);
            StreamWriter streamWriter = new StreamWriter(fs); 
            Console.WriteLine("Записываем данные в файл...");
            streamWriter.Write(JsonSerializer.Serialize(store));
            streamWriter.Flush();
            fs.Close();
            Console.WriteLine("Файл записан!");

            Console.WriteLine("Читаем данные из файла...");
            fs = new("data.json", FileMode.Open, FileAccess.Read);
            StreamReader streamReader = new StreamReader(fs);
            FloppyStore? store1 = JsonSerializer.Deserialize<FloppyStore>(streamReader.ReadToEnd());
            if (store1 == null)
            { 
                Console.WriteLine("Неудалось считать/десериализовать файл."); return;
            } 
            Console.WriteLine("Вывожу на экран все диски...");
            for (int i = 0; i < store1.Lenght; i++)
            {
                Console.WriteLine($"Диск:\r\n" +
                    $"  |Название: {store1[i].Manufacture}\r\n" +
                    $"  |Формат: {store1[i].Format}\r\n" +
                    $"  |Емкость: {store1[i].Capacity}\r\n");
            }
        }

        static void lab_6()
        {
            Random r = new Random();
            int sizeStore = 3000;
            int cycle = 50;

            FloppyStore store = new FloppyStore(sizeStore);
            Console.WriteLine("Создаем Хранилище Дисков со случайными параметрами...");
            for (int i = 0; i < store.Lenght; i++)
            {
                store[i] = new Floppy($"Floppy {i + 1}", (FormatInchEnum)r.Next(0, 3), (ushort)r.Next(0, 28800));
                //store[i] = new Floppy($"Floppy {i + 1}", (FormatInchEnum)r.Next(0, 3), Convert.ToUInt16(i));
            }
            Console.WriteLine($"Размер хранилище: {sizeStore} объектов, {cycle} циклов сортировки.\n\r");
            store.Shuffle();

            //сортировка
            Console.WriteLine("Сортируем массивы...\n\r");
            Console.WriteLine("+---------------------+");
            Console.WriteLine("| Алгоритм сортировки | Среднее время выполнения");
            Console.WriteLine("+---------------------+");
            Console.WriteLine($"| SelectionSort:      | {MeasureSort(store.SelectionSort, cycle)} сек."); store.Shuffle();
            Console.WriteLine($"| InsertionSort:      | {MeasureSort(store.InsertionSort, cycle)} сек."); store.Shuffle();
            Console.WriteLine($"| BubbleSort:         | {MeasureSort(store.BubbleSort, cycle)} сек."); store.Shuffle();
            Console.WriteLine($"| ShakerSort:         | {MeasureSort(store.ShakerSort, cycle)} сек."); store.Shuffle();
            Console.WriteLine($"| ShellSort:          | {MeasureSort(store.ShellSort, cycle)} сек."); store.Shuffle();
            Console.WriteLine($"| ShellSort2:         | {MeasureSort(store.ShellSort2, cycle)} сек."); store.Shuffle();
            Console.WriteLine("+---------------------+");

            //store.Shuffle();
            //store.ShellSort();
            //Console.WriteLine("Вывожу на экран все диски...");
            //for (int i = 0; i < store.Lenght; i++)
            //{
            //    Console.WriteLine($"|Е: {store[i].Capacity}");
            //}
        }

        private static double MeasureSort(Action action, int iterations)
        {
            Stopwatch watch = new();
            long[] acc = new long[iterations];
            FloppyStore store = (FloppyStore)action.Target;
            for (int i = 0; i < iterations; i++)
            {
                watch.Restart();
                action();
                watch.Stop();
                acc[i] = watch.ElapsedMilliseconds;
                store.Shuffle();
            }

            return acc.Average() / 1000;
        }
    }
}
