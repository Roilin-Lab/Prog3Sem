# Let's start by translating the C# code to Python step by step.
# We'll first define the enums and the Floppy class, then proceed with the rest.

from enum import Enum
from time import sleep
import random

# Enums for format and side
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

# MeasureSort equivalent function for timing the sorting algorithms
import time

def measure_sort(sorting_method, store, iterations):
    times = []
    for _ in range(iterations):
        start_time = time.time()
        sorting_method()
        end_time = time.time()
        times.append(end_time - start_time)
        store.shuffle()  # Shuffle after each iteration for randomization
    return sum(times) / iterations

# The translation is now complete, and the sorting methods can be used and timed in Python.
