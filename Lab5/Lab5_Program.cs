using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

class TextCorrector {
  private Dictionary<string, string> errorDictionary;
  private string directoryPath;

  public TextCorrector() {
    errorDictionary = new Dictionary<string, string>();
    InitializeErrorDictionary();
  }

  private void InitializeErrorDictionary() {
    errorDictionary.Add("првиет", "привет");
    errorDictionary.Add("пирвет", "привет");
    errorDictionary.Add("преивет", "привет");
    errorDictionary.Add("привте", "привет");
    errorDictionary.Add("спасиоб", "спасибо");
    errorDictionary.Add("спсибо", "спасибо");
    errorDictionary.Add("сапсибо", "спасибо");
    errorDictionary.Add("пажалуйста", "пожалуйста");
    errorDictionary.Add("пожалйста", "пожалуйста");
    errorDictionary.Add("пожалуста", "пожалуйста");
    errorDictionary.Add("здраствуйте", "здравствуйте");
    errorDictionary.Add("здраствуте", "здравствуйте");
    errorDictionary.Add("здраствйте", "здравствуйте");
    errorDictionary.Add("сейчас", "сейчас");
    errorDictionary.Add("щас", "сейчас");
    errorDictionary.Add("сичас", "сейчас");
    errorDictionary.Add("извените", "извините");
    errorDictionary.Add("извенити", "извините");
    errorDictionary.Add("извиняюсь", "извините");
    errorDictionary.Add("большое", "большое");
    errorDictionary.Add("бальшое", "большое");
    errorDictionary.Add("болшое", "большое");
  }

  public void Run() {
    while (true) {
      Console.WriteLine("\n=== Text Correction Tool ===\n" +
                        "1. Set directory path\n" +
                        "2. View error dictionary\n" +
                        "3. Add word to dictionary\n" +
                        "4. Remove word from dictionary\n" +
                        "5. Process files (fix errors and phone numbers)\n" +
                        "6. Create test files\n" +
                        "7. Exit");
      Console.Write("Select action: ");

      if (!int.TryParse(Console.ReadLine(), out int choice)) {
        Console.WriteLine("Error: enter a number.");
        continue;
      }

      switch (choice) {
        case 1:
          SetDirectoryPath();
          break;
        case 2:
          ViewDictionary();
          break;
        case 3:
          AddWordToDictionary();
          break;
        case 4:
          RemoveWordFromDictionary();
          break;
        case 5:
          ProcessFiles();
          break;
        case 6:
          CreateTestFiles();
          break;
        case 7:
          Console.WriteLine("Exiting program.");
          return;
        default:
          Console.WriteLine("Invalid choice.");
          break;
      }
    }
  }

  private void SetDirectoryPath() {
    Console.Write("Enter directory path: ");
    string path = Console.ReadLine();
    if (Directory.Exists(path)) {
      directoryPath = path;
      Console.WriteLine($"Directory set to: {directoryPath}");
    } else {
      Console.WriteLine("Directory does not exist.");
    }
  }

  private void ViewDictionary() {
    if (errorDictionary.Count == 0) {
      Console.WriteLine("Dictionary is empty.");
      return;
    }
    Console.WriteLine("\n=== Error Dictionary ===");
    Console.WriteLine("Wrong word -> Correct word");
    Console.WriteLine("---------------------------");
    foreach (var pair in errorDictionary) {
      Console.WriteLine($"{pair.Key} -> {pair.Value}");
    }
  }

  private void AddWordToDictionary() {
    Console.Write("Enter wrong word: ");
    string wrongWord = Console.ReadLine()?.ToLower();
    if (string.IsNullOrWhiteSpace(wrongWord)) {
      Console.WriteLine("Invalid input.");
      return;
    }

    Console.Write("Enter correct word: ");
    string correctWord = Console.ReadLine()?.ToLower();
    if (string.IsNullOrWhiteSpace(correctWord)) {
      Console.WriteLine("Invalid input.");
      return;
    }

    if (errorDictionary.ContainsKey(wrongWord)) {
      Console.WriteLine($"Word '{wrongWord}' already exists. Updating...");
      errorDictionary[wrongWord] = correctWord;
    } else {
      errorDictionary.Add(wrongWord, correctWord);
    }
    Console.WriteLine($"Added: {wrongWord} -> {correctWord}");
  }

  private void RemoveWordFromDictionary() {
    Console.Write("Enter wrong word to remove: ");
    string wrongWord = Console.ReadLine()?.ToLower();
    if (string.IsNullOrWhiteSpace(wrongWord)) {
      Console.WriteLine("Invalid input.");
      return;
    }

    if (errorDictionary.Remove(wrongWord)) {
      Console.WriteLine($"Removed: {wrongWord}");
    } else {
      Console.WriteLine($"Word '{wrongWord}' not found in dictionary.");
    }
  }

  private void ProcessFiles() {
    if (string.IsNullOrEmpty(directoryPath)) {
      Console.WriteLine("Please set directory path first.");
      return;
    }

    if (!Directory.Exists(directoryPath)) {
      Console.WriteLine("Directory does not exist.");
      return;
    }

    string[] files = Directory.GetFiles(directoryPath, "*.txt");
    if (files.Length == 0) {
      Console.WriteLine("No text files found in directory.");
      return;
    }

    int totalFiles = 0;
    int totalWordsFixed = 0;
    int totalPhonesFixed = 0;

    foreach (string filePath in files) {
      try {
        string content = File.ReadAllText(filePath);
        string originalContent = content;
        int wordsFixed = 0;
        int phonesFixed = 0;

        foreach (var pair in errorDictionary) {
          string pattern = $@"\b{Regex.Escape(pair.Key)}\b";
          int matches = Regex.Matches(content, pattern, RegexOptions.IgnoreCase).Count;
          content = Regex.Replace(content, pattern, pair.Value, RegexOptions.IgnoreCase);
          wordsFixed += matches;
        }

        string phonePattern = @"\((\d{3})\)\s*(\d{3})-(\d{2})-(\d{2})";
        MatchCollection phoneMatches = Regex.Matches(content, phonePattern);
        phonesFixed = phoneMatches.Count;
        content = Regex.Replace(content, phonePattern, "+380 $1 $2 $3 $4");

        if (content != originalContent) {
          File.WriteAllText(filePath, content);
          ++totalFiles;
          totalWordsFixed += wordsFixed;
          totalPhonesFixed += phonesFixed;
          Console.WriteLine($"Processed: {Path.GetFileName(filePath)}\n" +
                            $"  Words fixed: {wordsFixed}\n" +
                            $"  Phone numbers fixed: {phonesFixed}");
        }
      } catch (Exception ex) {
        Console.WriteLine($"Error processing {Path.GetFileName(filePath)}: {ex.Message}");
      }
    }

    Console.WriteLine($"\n=== Summary ===\n" +
                      $"Files processed: {totalFiles}\n" +
                      $"Total words fixed: {totalWordsFixed}\n" +
                      $"Total phone numbers fixed: {totalPhonesFixed}");
  }

  private void CreateTestFiles() {
    Console.Write("Enter directory path for test files: ");
    string path = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(path)) {
      Console.WriteLine("Invalid path.");
      return;
    }

    try {
      if (!Directory.Exists(path)) {
        Directory.CreateDirectory(path);
      }

      string file1 = Path.Combine(path, "test1.txt");
      string content1 = "Првиет! Спасиоб за помощь. Звоните по номеру (012) 345-67-89.\n" +
                        "Пажалуйста, извените за беспокойство. Телефон: (098) 765-43-21.";
      File.WriteAllText(file1, content1);

      string file2 = Path.Combine(path, "test2.txt");
      string content2 = "Здраствуйте! Щас перезвоню на (050) 123-45-67.\n" +
                        "Бальшое спсибо! Мой номер (063) 987-65-43.";
      File.WriteAllText(file2, content2);

      string file3 = Path.Combine(path, "test3.txt");
      string content3 = "Преивет, как дела? Звони (067) 111-22-33.\n" +
                        "Пирвет! Контакт: (093) 444-55-66. Спасиоб!";
      File.WriteAllText(file3, content3);

      Console.WriteLine($"Created 3 test files in: {path}\n" +
                        $"  - test1.txt\n" +
                        $"  - test2.txt\n" +
                        $"  - test3.txt");

      directoryPath = path;
      Console.WriteLine($"Directory path automatically set to: {directoryPath}");
    } catch (Exception ex) {
      Console.WriteLine($"Error creating test files: {ex.Message}");
    }
  }
}

class Program {
  static void Main() {
    TextCorrector corrector = new TextCorrector();
    corrector.Run();
  }
}
