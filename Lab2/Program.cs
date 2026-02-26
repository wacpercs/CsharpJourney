using System;
using System.Collections.Generic;

abstract class Animal
{
    public string nickname { get; set; }
    public int age { get; set; }
    public string habitat { get; set; }
    public string dietType { get; set; }

    public Animal(string nickname, int age, string habitat, string dietType)
    {
        this.nickname = nickname;
        this.age = age;
        this.habitat = habitat;
        this.dietType = dietType;
    }

    public virtual string GetInfo()
    {
        return $"Кличка: {nickname}, Возраст: {age}, Среда: {habitat}, Питание: {dietType}";
    }
}

class Mammal : Animal
{
    public bool hasFur { get; set; }

    public Mammal(string nickname, int age, string habitat, string dietType, bool hasFur)
      : base(nickname, age, habitat, dietType)
    {
        this.hasFur = hasFur;
    }

    public override string GetInfo()
    {
        return base.GetInfo() + $", Тип: Млекопитающее, Шерсть: {(hasFur ? "есть" : "нет")}";
    }
}

class Bird : Animal
{
    public double wingSpan { get; set; }

    public Bird(string nickname, int age, string habitat, string dietType, double wingSpan)
      : base(nickname, age, habitat, dietType)
    {
        this.wingSpan = wingSpan;
    }

    public override string GetInfo()
    {
        return base.GetInfo() + $", Тип: Птица, Размах крыльев: {wingSpan} м";
    }
}

class Fish : Animal
{
    public string waterType { get; set; }

    public Fish(string nickname, int age, string habitat, string dietType, string waterType)
      : base(nickname, age, habitat, dietType)
    {
        this.waterType = waterType;
    }

    public override string GetInfo()
    {
        return base.GetInfo() + $", Тип: Рыба, Тип воды: {waterType}";
    }
}

class Reptile : Animal
{
    public bool isVenomous { get; set; }

    public Reptile(string nickname, int age, string habitat, string dietType, bool isVenomous)
      : base(nickname, age, habitat, dietType)
    {
        this.isVenomous = isVenomous;
    }

    public override string GetInfo()
    {
        return base.GetInfo() + $", Тип: Пресмыкающееся, Ядовитость: {(isVenomous ? "да" : "нет")}";
    }
}

class Amphibian : Animal
{
    public string skinMoisture { get; set; }

    public Amphibian(string nickname, int age, string habitat, string dietType, string skinMoisture)
      : base(nickname, age, habitat, dietType)
    {
        this.skinMoisture = skinMoisture;
    }

    public override string GetInfo()
    {
        return base.GetInfo() + $", Тип: Земноводное, Влажность кожи: {skinMoisture}";
    }
}

class AnimalManager
{
    private static AnimalManager instance;
    private List<Animal> animals;

    private AnimalManager()
    {
        animals = new List<Animal>();
    }

    public static AnimalManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new AnimalManager();
            }
            return instance;
        }
    }

    public void AddAnimal(Animal animal)
    {
        animals.Add(animal);
        Console.WriteLine($"Животное '{animal.nickname}' успешно добавлено!");
    }

    public void ShowAllAnimals()
    {
        if (animals.Count == 0)
        {
            Console.WriteLine("Список животных пуст.");
            return;
        }

        Console.WriteLine($"\n=== Всего животных: {animals.Count} ===");
        for (int i = 0; i < animals.Count; ++i)
        {
            Console.WriteLine($"{i + 1}. {animals[i].GetInfo()}");
        }
    }

    public void ShowAnimalByIndex(int index)
    {
        if (index < 0 || index >= animals.Count)
        {
            Console.WriteLine("Некорректный индекс.");
            return;
        }
        Console.WriteLine(animals[index].GetInfo());
    }

    public void ShowMenu()
    {
        while (true)
        {
            Console.WriteLine("\n=== Меню управления животными ===");
            Console.WriteLine("1. Показать всех животных");
            Console.WriteLine("2. Добавить животное");
            Console.WriteLine("3. Показать животное по индексу");
            Console.WriteLine("4. Выход");
            Console.Write("Выберите действие: ");

            if (!int.TryParse(Console.ReadLine(), out int choice))
            {
                Console.WriteLine("Ошибка: введите число.");
                continue;
            }

            switch (choice)
            {
                case 1:
                    ShowAllAnimals();
                    break;
                case 2:
                    AddAnimalMenu();
                    break;
                case 3:
                    ShowAnimalByIndexMenu();
                    break;
                case 4:
                    Console.WriteLine("Выход из программы.");
                    return;
                default:
                    Console.WriteLine("Некорректный выбор.");
                    break;
            }
        }
    }

    private void AddAnimalMenu()
    {
        Console.WriteLine("\n=== Добавление животного ===");
        Console.WriteLine("1. Млекопитающее");
        Console.WriteLine("2. Птица");
        Console.WriteLine("3. Рыба");
        Console.WriteLine("4. Пресмыкающееся");
        Console.WriteLine("5. Земноводное");
        Console.Write("Выберите тип животного: ");

        if (!int.TryParse(Console.ReadLine(), out int animalType) || animalType < 1 || animalType > 5)
        {
            Console.WriteLine("Некорректный тип животного.");
            return;
        }

        Console.Write("Введите кличку: ");
        string nickname = Console.ReadLine();

        Console.Write("Введите возраст: ");
        if (!int.TryParse(Console.ReadLine(), out int age))
        {
            Console.WriteLine("Некорректный возраст.");
            return;
        }

        Console.Write("Введите среду обитания: ");
        string habitat = Console.ReadLine();

        Console.Write("Введите тип питания: ");
        string dietType = Console.ReadLine();

        Animal newAnimal = null;

        switch (animalType)
        {
            case 1:
                Console.Write("Есть ли шерсть? (да/нет): ");
                bool hasFur = Console.ReadLine()?.ToLower() == "да";
                newAnimal = new Mammal(nickname, age, habitat, dietType, hasFur);
                break;
            case 2:
                Console.Write("Введите размах крыльев (м): ");
                if (double.TryParse(Console.ReadLine(), out double wingSpan))
                {
                    newAnimal = new Bird(nickname, age, habitat, dietType, wingSpan);
                }
                break;
            case 3:
                Console.Write("Введите тип воды (пресная/морская): ");
                string waterType = Console.ReadLine();
                newAnimal = new Fish(nickname, age, habitat, dietType, waterType);
                break;
            case 4:
                Console.Write("Ядовитое? (да/нет): ");
                bool isVenomous = Console.ReadLine()?.ToLower() == "да";
                newAnimal = new Reptile(nickname, age, habitat, dietType, isVenomous);
                break;
            case 5:
                Console.Write("Введите влажность кожи: ");
                string skinMoisture = Console.ReadLine();
                newAnimal = new Amphibian(nickname, age, habitat, dietType, skinMoisture);
                break;
        }

        if (newAnimal != null)
        {
            AddAnimal(newAnimal);
        }
        else
        {
            Console.WriteLine("Ошибка при создании животного.");
        }
    }

    private void ShowAnimalByIndexMenu()
    {
        Console.Write("Введите индекс животного (начиная с 1): ");
        if (int.TryParse(Console.ReadLine(), out int index))
        {
            ShowAnimalByIndex(index - 1);
        }
        else
        {
            Console.WriteLine("Некорректный индекс.");
        }
    }
}

class Program
{
    static void Main()
    {
        AnimalManager manager = AnimalManager.Instance;

        manager.AddAnimal(new Mammal("Кот", 5, "лес", "хищник", true));
        manager.AddAnimal(new Bird("Птичка", 3, "город", "всеядное", 0.5));
        manager.AddAnimal(new Fish("Рыба", 2, "водоём", "всеядное", "морская"));
        manager.AddAnimal(new Reptile("Змея", 7, "пустыня", "хищник", true));
        manager.AddAnimal(new Amphibian("Жаба", 4, "болото", "хищник", "высокая"));

        manager.ShowMenu();
    }
}