using System;
using System.Collections.Generic;

abstract class Animal {
  public string nickname { get; set; }
  public int age { get; set; }
  public string habitat { get; set; }
  public string dietType { get; set; }

  public Animal(string nickname, int age, string habitat, string dietType) {
    this.nickname = nickname;
    this.age = age;
    this.habitat = habitat;
    this.dietType = dietType;
  }

  public virtual string GetInfo() {
    return $"Nickname: {nickname}, Age: {age}, Habitat: {habitat}, Diet: {dietType}";
  }
}

class Mammal : Animal {
  public bool hasFur { get; set; }

  public Mammal(string nickname, int age, string habitat, string dietType, bool hasFur)
    : base(nickname, age, habitat, dietType) {
    this.hasFur = hasFur;
  }

  public override string GetInfo() {
    return base.GetInfo() + $", Type: Mammal, Fur: {(hasFur ? "yes" : "no")}";
  }
}

class Bird : Animal {
  public double wingSpan { get; set; }

  public Bird(string nickname, int age, string habitat, string dietType, double wingSpan)
    : base(nickname, age, habitat, dietType) {
    this.wingSpan = wingSpan;
  }

  public override string GetInfo() {
    return base.GetInfo() + $", Type: Bird, Wing span: {wingSpan} m";
  }
}

class Fish : Animal {
  public string waterType { get; set; }

  public Fish(string nickname, int age, string habitat, string dietType, string waterType)
    : base(nickname, age, habitat, dietType) {
    this.waterType = waterType;
  }

  public override string GetInfo() {
    return base.GetInfo() + $", Type: Fish, Water type: {waterType}";
  }
}

class Reptile : Animal {
  public bool isVenomous { get; set; }

  public Reptile(string nickname, int age, string habitat, string dietType, bool isVenomous)
    : base(nickname, age, habitat, dietType) {
    this.isVenomous = isVenomous;
  }

  public override string GetInfo() {
    return base.GetInfo() + $", Type: Reptile, Venomous: {(isVenomous ? "yes" : "no")}";
  }
}

class Amphibian : Animal {
  public string skinMoisture { get; set; }

  public Amphibian(string nickname, int age, string habitat, string dietType, string skinMoisture)
    : base(nickname, age, habitat, dietType) {
    this.skinMoisture = skinMoisture;
  }

  public override string GetInfo() {
    return base.GetInfo() + $", Type: Amphibian, Skin moisture: {skinMoisture}";
  }
}

class AnimalManager {
  private static AnimalManager instance;
  private List<Animal> animals;

  private AnimalManager() {
    animals = new List<Animal>();
  }

  public static AnimalManager Instance {
    get {
      if (instance == null) {
        instance = new AnimalManager();
      }
      return instance;
    }
  }

  public void AddAnimal(Animal animal) {
    animals.Add(animal);
    Console.WriteLine($"Animal '{animal.nickname}' successfully added!");
  }

  public void ShowAllAnimals() {
    if (animals.Count == 0) {
      Console.WriteLine("Animal list is empty.");
      return;
    }

    Console.WriteLine($"\n=== Total animals: {animals.Count} ===");
    for (int i = 0; i < animals.Count; ++i) {
      Console.WriteLine($"{i + 1}. {animals[i].GetInfo()}");
    }
  }

  public void ShowAnimalByIndex(int index) {
    if (index < 0 || index >= animals.Count) {
      Console.WriteLine("Invalid index.");
      return;
    }
    Console.WriteLine(animals[index].GetInfo());
  }

  public void ShowMenu() {
    while (true) {
      Console.WriteLine("\n=== Animal Management Menu ===");
      Console.WriteLine("1. Show all animals");
      Console.WriteLine("2. Add animal");
      Console.WriteLine("3. Show animal by index");
      Console.WriteLine("4. Exit");
      Console.Write("Select action: ");

      if (!int.TryParse(Console.ReadLine(), out int choice)) {
        Console.WriteLine("Error: enter a number.");
        continue;
      }

      switch (choice) {
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
          Console.WriteLine("Exiting program.");
          return;
        default:
          Console.WriteLine("Invalid choice.");
          break;
      }
    }
  }

  private void AddAnimalMenu() {
    Console.WriteLine("\n=== Adding animal ===");
    Console.WriteLine("1. Mammal");
    Console.WriteLine("2. Bird");
    Console.WriteLine("3. Fish");
    Console.WriteLine("4. Reptile");
    Console.WriteLine("5. Amphibian");
    Console.Write("Select animal type: ");

    if (!int.TryParse(Console.ReadLine(), out int animalType) || animalType < 1 || animalType > 5) {
      Console.WriteLine("Invalid animal type.");
      return;
    }

    Console.Write("Enter nickname: ");
    string nickname = Console.ReadLine();

    Console.Write("Enter age: ");
    if (!int.TryParse(Console.ReadLine(), out int age)) {
      Console.WriteLine("Invalid age.");
      return;
    }

    Console.Write("Enter habitat: ");
    string habitat = Console.ReadLine();

    Console.Write("Enter diet type: ");
    string dietType = Console.ReadLine();

    Animal newAnimal = null;

    switch (animalType) {
      case 1:
        Console.Write("Has fur? (yes/no): ");
        bool hasFur = Console.ReadLine()?.ToLower() == "yes";
        newAnimal = new Mammal(nickname, age, habitat, dietType, hasFur);
        break;
      case 2:
        Console.Write("Enter wing span (m): ");
        if (double.TryParse(Console.ReadLine(), out double wingSpan)) {
          newAnimal = new Bird(nickname, age, habitat, dietType, wingSpan);
        }
        break;
      case 3:
        Console.Write("Enter water type (fresh/salt): ");
        string waterType = Console.ReadLine();
        newAnimal = new Fish(nickname, age, habitat, dietType, waterType);
        break;
      case 4:
        Console.Write("Venomous? (yes/no): ");
        bool isVenomous = Console.ReadLine()?.ToLower() == "yes";
        newAnimal = new Reptile(nickname, age, habitat, dietType, isVenomous);
        break;
      case 5:
        Console.Write("Enter skin moisture: ");
        string skinMoisture = Console.ReadLine();
        newAnimal = new Amphibian(nickname, age, habitat, dietType, skinMoisture);
        break;
    }

    if (newAnimal != null) {
      AddAnimal(newAnimal);
    } else {
      Console.WriteLine("Error creating animal.");
    }
  }

  private void ShowAnimalByIndexMenu() {
    Console.Write("Enter animal index (starting from 1): ");
    if (int.TryParse(Console.ReadLine(), out int index)) {
      ShowAnimalByIndex(index - 1);
    } else {
      Console.WriteLine("Invalid index.");
    }
  }
}

class Program {
  static void Main() {
    AnimalManager manager = AnimalManager.Instance;

    manager.AddAnimal(new Mammal("Barsik", 5, "forest", "carnivore", true));
    manager.AddAnimal(new Bird("Kesha", 3, "city", "omnivore", 0.5));
    manager.AddAnimal(new Fish("Nemo", 2, "water", "omnivore", "salt"));
    manager.AddAnimal(new Reptile("Snake", 7, "desert", "carnivore", true));
    manager.AddAnimal(new Amphibian("Toad", 4, "swamp", "carnivore", "high"));

    manager.ShowMenu();
  }
}
