using System;

class Program {
  static void Main() {
    int baseNumber, exponent;
    long powerResult, resultNumber, originalNumber;
    string numberAsString, withoutSecondDigit, rearrangedString;
    char secondDigit;
    Console.Write("Enter base number a: ");
    baseNumber = int.Parse(Console.ReadLine());

    Console.Write("Enter exponent n: ");
    exponent = int.Parse(Console.ReadLine());

    powerResult = 1;

    for (int counter = 0; counter < exponent; ++counter)
    {
      powerResult = powerResult * baseNumber;
    }

    Console.WriteLine(baseNumber + " to the power of " + exponent + " = " + powerResult);

    Console.WriteLine();
    Console.Write("Enter number x (>= 100): ");
    originalNumber = long.Parse(Console.ReadLine());

    numberAsString = originalNumber.ToString();
    secondDigit = numberAsString[1];
    withoutSecondDigit = numberAsString.Remove(1, 1);
    rearrangedString = withoutSecondDigit + secondDigit;
    resultNumber = long.Parse(rearrangedString);

    Console.WriteLine("Original number x = " + originalNumber);
    Console.WriteLine("Result n = " + resultNumber);
  }
}