using System;

class MatrixException : Exception {
  public MatrixException(string message) : base(message) { }
}

class SingularMatrixException : MatrixException {
  public SingularMatrixException() : base("Matrix is singular, cannot find inverse") { }
}

class IncompatibleMatrixException : MatrixException {
  public IncompatibleMatrixException(string operation) 
    : base($"Matrices are incompatible for {operation}") { }
}

class SquareMatrix : ICloneable, IComparable<SquareMatrix>, IEquatable<SquareMatrix> {
  private double[,] data;
  private int size;

  public int Size {
    get { return size; }
  }

  public double this[int row, int col] {
    get {
      if (row < 0 || row >= size || col < 0 || col >= size)
        throw new IndexOutOfRangeException("Matrix index out of range");
      return data[row, col];
    }
    set {
      if (row < 0 || row >= size || col < 0 || col >= size)
        throw new IndexOutOfRangeException("Matrix index out of range");
      data[row, col] = value;
    }
  }

  public SquareMatrix(int size) {
    if (size <= 0)
      throw new ArgumentException("Matrix size must be positive");
    this.size = size;
    data = new double[size, size];
  }

  public SquareMatrix(int size, bool randomize) : this(size) {
    if (randomize) {
      Random random = new Random();
      for (int i = 0; i < size; ++i) {
        for (int j = 0; j < size; ++j) {
          data[i, j] = random.Next(-10, 11);
        }
      }
    }
  }

  public SquareMatrix(double[,] array) {
    if (array.GetLength(0) != array.GetLength(1))
      throw new ArgumentException("Array must be square");
    size = array.GetLength(0);
    data = new double[size, size];
    for (int i = 0; i < size; ++i) {
      for (int j = 0; j < size; ++j) {
        data[i, j] = array[i, j];
      }
    }
  }

  private SquareMatrix(int size, double[,] sourceData) {
    this.size = size;
    data = new double[size, size];
    for (int i = 0; i < size; ++i) {
      for (int j = 0; j < size; ++j) {
        data[i, j] = sourceData[i, j];
      }
    }
  }

  public static SquareMatrix operator +(SquareMatrix a, SquareMatrix b) {
    if (a.size != b.size)
      throw new IncompatibleMatrixException("addition");
    SquareMatrix result = new SquareMatrix(a.size);
    for (int i = 0; i < a.size; ++i) {
      for (int j = 0; j < a.size; ++j) {
        result.data[i, j] = a.data[i, j] + b.data[i, j];
      }
    }
    return result;
  }

  public static SquareMatrix operator *(SquareMatrix a, SquareMatrix b) {
    if (a.size != b.size)
      throw new IncompatibleMatrixException("multiplication");
    SquareMatrix result = new SquareMatrix(a.size);
    for (int i = 0; i < a.size; ++i) {
      for (int j = 0; j < a.size; ++j) {
        double sum = 0;
        for (int k = 0; k < a.size; ++k) {
          sum += a.data[i, k] * b.data[k, j];
        }
        result.data[i, j] = sum;
      }
    }
    return result;
  }

  public static bool operator >(SquareMatrix a, SquareMatrix b) {
    return a.Determinant() > b.Determinant();
  }

  public static bool operator <(SquareMatrix a, SquareMatrix b) {
    return a.Determinant() < b.Determinant();
  }

  public static bool operator >=(SquareMatrix a, SquareMatrix b) {
    return a.Determinant() >= b.Determinant();
  }

  public static bool operator <=(SquareMatrix a, SquareMatrix b) {
    return a.Determinant() <= b.Determinant();
  }

  public static bool operator ==(SquareMatrix a, SquareMatrix b) {
    if (ReferenceEquals(a, b)) return true;
    if (ReferenceEquals(a, null) || ReferenceEquals(b, null)) return false;
    if (a.size != b.size) return false;
    for (int i = 0; i < a.size; ++i) {
      for (int j = 0; j < a.size; ++j) {
        if (Math.Abs(a.data[i, j] - b.data[i, j]) > 1e-10)
          return false;
      }
    }
    return true;
  }

  public static bool operator !=(SquareMatrix a, SquareMatrix b) {
    return !(a == b);
  }

  public static bool operator true(SquareMatrix m) {
    return Math.Abs(m.Determinant()) > 1e-10;
  }

  public static bool operator false(SquareMatrix m) {
    return Math.Abs(m.Determinant()) < 1e-10;
  }

  public static explicit operator double(SquareMatrix m) {
    return m.Determinant();
  }

  public static explicit operator int(SquareMatrix m) {
    return (int)m.Determinant();
  }

  public double Determinant() {
    return CalculateDeterminant(data, size);
  }

  private double CalculateDeterminant(double[,] matrix, int n) {
    if (n == 1)
      return matrix[0, 0];
    if (n == 2)
      return matrix[0, 0] * matrix[1, 1] - matrix[0, 1] * matrix[1, 0];

    double det = 0;
    for (int col = 0; col < n; ++col) {
      double[,] minor = GetMinor(matrix, 0, col, n);
      det += matrix[0, col] * CalculateDeterminant(minor, n - 1) * (col % 2 == 0 ? 1 : -1);
    }
    return det;
  }

  private double[,] GetMinor(double[,] matrix, int row, int col, int n) {
    double[,] minor = new double[n - 1, n - 1];
    int minorRow = 0;
    for (int i = 0; i < n; ++i) {
      if (i == row) continue;
      int minorCol = 0;
      for (int j = 0; j < n; ++j) {
        if (j == col) continue;
        minor[minorRow, minorCol] = matrix[i, j];
        ++minorCol;
      }
      ++minorRow;
    }
    return minor;
  }

  public SquareMatrix Inverse() {
    double det = Determinant();
    if (Math.Abs(det) < 1e-10)
      throw new SingularMatrixException();

    if (size == 1) {
      return new SquareMatrix(1) { [0, 0] = 1.0 / data[0, 0] };
    }

    double[,] adjugate = new double[size, size];
    for (int i = 0; i < size; ++i) {
      for (int j = 0; j < size; ++j) {
        double[,] minor = GetMinor(data, i, j, size);
        double cofactor = CalculateDeterminant(minor, size - 1);
        adjugate[j, i] = cofactor * ((i + j) % 2 == 0 ? 1 : -1);
      }
    }

    SquareMatrix result = new SquareMatrix(size);
    for (int i = 0; i < size; ++i) {
      for (int j = 0; j < size; ++j) {
        result.data[i, j] = adjugate[i, j] / det;
      }
    }
    return result;
  }

  public object Clone() {
    return new SquareMatrix(size, data);
  }

  public override string ToString() {
    string result = "";
    for (int i = 0; i < size; ++i) {
      for (int j = 0; j < size; ++j) {
        result += $"{data[i, j],8:F2}";
      }
      result += "\n";
    }
    return result;
  }

  public int CompareTo(SquareMatrix other) {
    if (other == null) return 1;
    return Determinant().CompareTo(other.Determinant());
  }

  public override bool Equals(object obj) {
    if (obj == null || GetType() != obj.GetType())
      return false;
    return this == (SquareMatrix)obj;
  }

  public bool Equals(SquareMatrix other) {
    if (other == null)
      return false;
    return this == other;
  }

  public override int GetHashCode() {
    int hash = size.GetHashCode();
    for (int i = 0; i < size; ++i) {
      for (int j = 0; j < size; ++j) {
        hash ^= data[i, j].GetHashCode();
      }
    }
    return hash;
  }
}

class MatrixCalculator {
  private SquareMatrix matrixA;
  private SquareMatrix matrixB;

  public void Run() {
    while (true) {
      try {
        Console.WriteLine("\n=== Matrix Calculator ===\n" +
                          "1. Generate random matrices\n" +
                          "2. Input matrices manually\n" +
                          "3. Add matrices (A + B)\n" +
                          "4. Multiply matrices (A * B)\n" +
                          "5. Calculate determinant of A\n" +
                          "6. Calculate determinant of B\n" +
                          "7. Find inverse of A\n" +
                          "8. Find inverse of B\n" +
                          "9. Compare matrices (A > B, A < B, A == B)\n" +
                          "10. Test true/false operator\n" +
                          "11. Clone matrix A (Prototype pattern)\n" +
                          "12. Display matrices\n" +
                          "13. Exit");
        Console.Write("Select action: ");

        if (!int.TryParse(Console.ReadLine(), out int choice)) {
          Console.WriteLine("Error: enter a number.");
          continue;
        }

        switch (choice) {
          case 1:
            GenerateRandomMatrices();
            break;
          case 2:
            InputMatricesManually();
            break;
          case 3:
            AddMatrices();
            break;
          case 4:
            MultiplyMatrices();
            break;
          case 5:
            CalculateDeterminant(matrixA, "A");
            break;
          case 6:
            CalculateDeterminant(matrixB, "B");
            break;
          case 7:
            FindInverse(matrixA, "A");
            break;
          case 8:
            FindInverse(matrixB, "B");
            break;
          case 9:
            CompareMatrices();
            break;
          case 10:
            TestTrueFalseOperator();
            break;
          case 11:
            CloneMatrix();
            break;
          case 12:
            DisplayMatrices();
            break;
          case 13:
            Console.WriteLine("Exiting calculator.");
            return;
          default:
            Console.WriteLine("Invalid choice.");
            break;
        }
      } catch (MatrixException ex) {
        Console.WriteLine($"Matrix Error: {ex.Message}");
      } catch (Exception ex) {
        Console.WriteLine($"Error: {ex.Message}");
      }
    }
  }

  private void GenerateRandomMatrices() {
    Console.Write("Enter matrix size: ");
    if (!int.TryParse(Console.ReadLine(), out int size) || size <= 0) {
      Console.WriteLine("Invalid size.");
      return;
    }
    matrixA = new SquareMatrix(size, true);
    matrixB = new SquareMatrix(size, true);
    Console.WriteLine("Random matrices generated successfully!");
    DisplayMatrices();
  }

  private void InputMatricesManually() {
    Console.Write("Enter matrix size: ");
    if (!int.TryParse(Console.ReadLine(), out int size) || size <= 0) {
      Console.WriteLine("Invalid size.");
      return;
    }

    Console.WriteLine("Enter matrix A:");
    matrixA = InputMatrix(size);

    Console.WriteLine("Enter matrix B:");
    matrixB = InputMatrix(size);

    Console.WriteLine("Matrices input successfully!");
  }

  private SquareMatrix InputMatrix(int size) {
    SquareMatrix matrix = new SquareMatrix(size);
    for (int i = 0; i < size; ++i) {
      for (int j = 0; j < size; ++j) {
        Console.Write($"Enter element [{i},{j}]: ");
        if (!double.TryParse(Console.ReadLine(), out double value)) {
          Console.WriteLine("Invalid input, using 0.");
          value = 0;
        }
        matrix[i, j] = value;
      }
    }
    return matrix;
  }

  private void AddMatrices() {
    if (matrixA == null || matrixB == null) {
      Console.WriteLine("Please generate or input matrices first.");
      return;
    }
    SquareMatrix result = matrixA + matrixB;
    Console.WriteLine("\nResult of A + B:");
    Console.WriteLine(result);
  }

  private void MultiplyMatrices() {
    if (matrixA == null || matrixB == null) {
      Console.WriteLine("Please generate or input matrices first.");
      return;
    }
    SquareMatrix result = matrixA * matrixB;
    Console.WriteLine("\nResult of A * B:");
    Console.WriteLine(result);
  }

  private void CalculateDeterminant(SquareMatrix matrix, string name) {
    if (matrix == null) {
      Console.WriteLine($"Matrix {name} is not initialized.");
      return;
    }
    double det = matrix.Determinant();
    Console.WriteLine($"Determinant of matrix {name}: {det:F4}\n" +
                      $"Determinant as int: {(int)matrix}");
  }

  private void FindInverse(SquareMatrix matrix, string name) {
    if (matrix == null) {
      Console.WriteLine($"Matrix {name} is not initialized.");
      return;
    }
    SquareMatrix inverse = matrix.Inverse();
    Console.WriteLine($"\nInverse of matrix {name}:");
    Console.WriteLine(inverse);
  }

  private void CompareMatrices() {
    if (matrixA == null || matrixB == null) {
      Console.WriteLine("Please generate or input matrices first.");
      return;
    }
    Console.WriteLine($"\nA > B: {matrixA > matrixB}\n" +
                      $"A < B: {matrixA < matrixB}\n" +
                      $"A >= B: {matrixA >= matrixB}\n" +
                      $"A <= B: {matrixA <= matrixB}\n" +
                      $"A == B: {matrixA == matrixB}\n" +
                      $"A != B: {matrixA != matrixB}\n" +
                      $"\n" +
                      $"CompareTo result: {matrixA.CompareTo(matrixB)}\n" +
                      $"Equals result: {matrixA.Equals(matrixB)}");
  }

  private void TestTrueFalseOperator() {
    if (matrixA == null) {
      Console.WriteLine("Please generate or input matrix A first.");
      return;
    }
    if (matrixA) {
      Console.WriteLine("Matrix A is non-singular (determinant != 0)");
    } else {
      Console.WriteLine("Matrix A is singular (determinant == 0)");
    }
  }

  private void CloneMatrix() {
    if (matrixA == null) {
      Console.WriteLine("Please generate or input matrix A first.");
      return;
    }
    SquareMatrix clone = (SquareMatrix)matrixA.Clone();
    Console.WriteLine($"\nCloned matrix A (deep copy):\n" +
                      $"{clone}" +
                      $"Are they equal? {matrixA.Equals(clone)}\n" +
                      $"Same reference? {ReferenceEquals(matrixA, clone)}\n" +
                      $"HashCode A: {matrixA.GetHashCode()}\n" +
                      $"HashCode Clone: {clone.GetHashCode()}");
  }

  private void DisplayMatrices() {
    if (matrixA == null && matrixB == null) {
      Console.WriteLine("No matrices to display.");
      return;
    }
    if (matrixA != null) {
      Console.WriteLine("\nMatrix A:");
      Console.WriteLine(matrixA);
    }
    if (matrixB != null) {
      Console.WriteLine("Matrix B:");
      Console.WriteLine(matrixB);
    }
  }
}

class Program {
  static void Main() {
    MatrixCalculator calculator = new MatrixCalculator();
    calculator.Run();
  }
}
