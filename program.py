from enum import Enum
from time import sleep
import json
import time
import random

class FormatInchEnum(Enum):
    Eight = 0
    FiveAndQuater = 1
    ThreeAndHalf = 2
    Three = 3

class SideEnum(Enum):
    Single = 0
    Double = 1

# Floppy base class
class Floppy:
    def __init__(self, manufacture="<Undefined>", format=FormatInchEnum.Eight, capacity=0):
        self._manufacture = manufacture
        self._format = format
        self._capacity = capacity

    def write_floppy(self):
        print("Запись на гибкий диск... Пожалуйста не выключайте компьютер!")
        sleep(0.2)

    def read_floppy(self):
        print("Чтение гибкого диска...")
        sleep(0.2)

    @property
    def manufacture(self):
        return self._manufacture

    @property
    def format(self):
        return self._format

    @property
    def capacity(self):
        return self._capacity

# FloppyHD class with additional functionality for double-sided disks
class FloppyHD(Floppy):
    def __init__(self, manufacture, format, capacity, side=SideEnum.Single):
        super().__init__(manufacture, format, capacity)
        self._side = side

    def write_floppy(self):
        if self._side == SideEnum.Double:
            if random.random() < 0.5:
                print("Ошибка записи на вторую сторону диска. Отмена операции!")
                return
        super().write_floppy()

# The FloppyStore class and sorting functions would come next

# FloppyStore class to manage a collection of Floppy disks and implement sorting algorithms
class FloppyStore:
    def __init__(self, floppies):
        self.floppies = floppies

    def shuffle(self):
        random.shuffle(self.floppies)

    @property
    def length(self):
        return len(self.floppies)

    def __getitem__(self, index):
        return self.floppies[index]

    def selection_sort(self):
        for i in range(len(self.floppies)):
            min_index = i
            for j in range(i+1, len(self.floppies)):
                if self.floppies[j].capacity < self.floppies[min_index].capacity:
                    min_index = j
            self.floppies[i], self.floppies[min_index] = self.floppies[min_index], self.floppies[i]

    def insertion_sort(self):
        for i in range(1, len(self.floppies)):
            key = self.floppies[i]
            j = i - 1
            while j >= 0 and key.capacity < self.floppies[j].capacity:
                self.floppies[j + 1] = self.floppies[j]
                j -= 1
            self.floppies[j + 1] = key

    def bubble_sort(self):
        for i in range(len(self.floppies)):
            for j in range(0, len(self.floppies) - i - 1):
                if self.floppies[j].capacity > self.floppies[j + 1].capacity:
                    self.floppies[j], self.floppies[j + 1] = self.floppies[j + 1], self.floppies[j]

    def shaker_sort(self):
        left = 0
        right = len(self.floppies) - 1
        while left < right:
            for i in range(left, right):
                if self.floppies[i].capacity > self.floppies[i + 1].capacity:
                    self.floppies[i], self.floppies[i + 1] = self.floppies[i + 1], self.floppies[i]
            right -= 1
            for i in range(right, left, -1):
                if self.floppies[i].capacity < self.floppies[i - 1].capacity:
                    self.floppies[i], self.floppies[i - 1] = self.floppies[i - 1], self.floppies[i]
            left += 1

    def shell_sort(self):
        gap = len(self.floppies) // 2
        while gap > 0:
            for i in range(gap, len(self.floppies)):
                temp = self.floppies[i]
                j = i
                while j >= gap and self.floppies[j - gap].capacity > temp.capacity:
                    self.floppies[j] = self.floppies[j - gap]
                    j -= gap
                self.floppies[j] = temp
            gap //= 2

    def shell_sort_for_str(self):
        """Сортировка Шелла по строковому значению производителя дисков."""
        n = len(self.floppies)
        if n < 2:
            return

        gap = n // 2
        while gap > 0:
            for i in range(gap, n):
                j = i
                while j >= gap and self.floppies[j - gap].manufacture > self.floppies[j].manufacture:
                    self.floppies[j], self.floppies[j - gap] = self.floppies[j - gap], self.floppies[j]
                    j -= gap
            gap //= 2


def measure_sort(sorting_method, store, iterations):
    times = []
    for _ in range(iterations):
        start_time = time.time()
        sorting_method()
        end_time = time.time()
        times.append(end_time - start_time)
        store.shuffle()  # Shuffle after each iteration for randomization
    return sum(times) / iterations

def lab_1():
    print("Введите данные для гибкого диска:")
    mnfc = input("Производитель: ")
    format = FormatInchEnum(int(input("Формат (0: 8дюйм, 1: 5.25дюйм, 2: 3.5дюйм, 3: 3дюйм): ")))
    capacity = int(input("Емкость в [КБ]: "))
    print()

    floppy = Floppy(mnfc, format, capacity)

    print("Ваш диск: ")
    print(floppy.manufacture)
    print(floppy.format)
    print(floppy.capacity)

def lab_2():
    floppy = Floppy()

    print("Диск с конструктором по умолчанию: ")
    print(floppy.manufacture)
    print(floppy.format)
    print(floppy.capacity)
    print()

    floppy1 = Floppy("IBM", FormatInchEnum.FiveAndQuater, 1440)

    print("Диск с перегруженным конструктором: ")
    print(floppy1.manufacture)
    print(floppy1.format)
    print(floppy1.capacity)
    print()

def lab_3():
    floppy1 = Floppy("IBM", FormatInchEnum.FiveAndQuater, 1440)

    print("Диск родитель: ")
    print(floppy1.manufacture)
    print(floppy1.format)
    print(floppy1.capacity)
    print()
    floppy1.write_floppy()
    print()

    floppy2 = FloppyHD("Apple", FormatInchEnum.ThreeAndHalf, 720, SideEnum.Double)

    print("Диск наследник: ")
    print(floppy2.manufacture)
    print(floppy2.format)
    print(floppy2.capacity)
    print(floppy2._side)
    print()
    floppy2.write_floppy()
    print()
    floppy2.read_floppy()

def lab_4():
    size = int(input("Введите желаемый размер хранилища в целых числах: "))
    store = FloppyStore([Floppy(f"Floppy {i + 1}", FormatInchEnum(random.randint(0, 3)), random.randint(0, 2880)) for i in range(size)])
    
    print("Готово! Хранилище создано")
    print("Вывожу на экран все диски...")
    for floppy in store.floppies:
        print(f"Диск:\n  |Название: {floppy.manufacture}\n  |Формат: {floppy.format}\n  |Емкость: {floppy.capacity}\n")

def lab_5():
    store = FloppyStore([Floppy(f"Floppy {i + 1}", FormatInchEnum(random.randint(0, 3)), random.randint(0, 2880)) for i in range(5)])

    print("Создаем Хранилище Дисков со случайными параметрами и записываем данные в файл...")
    with open("data.json", "w") as file:
        json.dump([{"manufacture": floppy.manufacture, "format": floppy.format.name, "capacity": floppy.capacity} for floppy in store.floppies], file)
    print("Файл записан!")

    print("Читаем данные из файла...")
    try:
        with open("data.json", "r") as file:
            store_data = json.load(file)
            store1 = FloppyStore([Floppy(d["manufacture"], FormatInchEnum[d["format"]], d["capacity"]) for d in store_data])
            print("Вывожу на экран все диски...")
            for floppy in store1.floppies:
                print(f"Диск:\n  |Название: {floppy.manufacture}\n  |Формат: {floppy.format}\n  |Емкость: {floppy.capacity}\n")
    except (FileNotFoundError, json.JSONDecodeError):
        print("Не удалось считать/десериализовать файл.")

import random
import string

def get_random_string(length):
    """Возвращает случайную строку из букв заданной длины."""
    letters = string.ascii_uppercase
    return ''.join(random.choice(letters) for _ in range(length))

def lab_6():
    size_store = 10
    cycle = 50

    # Создаем хранилище с случайными параметрами
    store = FloppyStore([Floppy(get_random_string(5), FormatInchEnum(random.randint(0, 3)), random.randint(0, 28800)) for _ in range(size_store)])
    
    print(f"Размер хранилища: {size_store} объектов, {cycle} циклов сортировки.\n")
    store.shuffle()

    # Сортировка и измерение времени для разных алгоритмов
    print("+---------------------+")
    print("| Алгоритм сортировки | Среднее время выполнения (сек)")
    print("+---------------------+")
    
    for sort_method in ["selection_sort", "insertion_sort", "bubble_sort", "shaker_sort", "shell_sort"]:
        store.shuffle()
        avg_time = measure_sort(getattr(store, sort_method), store, cycle)
        print(f"| {sort_method.capitalize():<20} | {avg_time:.4f} сек.")

    store.shuffle()
    store.shell_sort_for_str()  # Здесь предполагается, что ShellSort2 аналогичен shell_sort в Python

    print("Вывожу на экран все диски...")
    for floppy in store.floppies:
        print("--------------------------")
        print(f"    |M: {floppy.manufacture}")
        print(f"    |C: {floppy.capacity}")


lab_6()