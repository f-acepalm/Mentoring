using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _10Ints
{
    class Program
    {
        private const int NumberCount = 10;
        private const string Separator = "---------------------------------------------------";
        private static Random _random = new Random();

        static void Main(string[] args)
        {
            Task.Run(() => CreateNumbers())
                .ContinueWith((data) => MultiplyNumbers(data.Result))
                .ContinueWith((data) => SortNumbers(data.Result))
                .ContinueWith((data) => CalculateAverage(data.Result))
                .Wait();
            Console.WriteLine("Done!");
        }

        private static int[] CreateNumbers()
        {
            var numbers = new int[NumberCount];
            for (int i = 0; i < NumberCount; i++)
            {
                numbers[i] = _random.Next(1000);
            }
            Console.WriteLine("Created");
            WriteResult(numbers);

            return numbers;
        }

        private static int[] MultiplyNumbers(int[] numbers)
        {
            var result = new int[NumberCount];
            for (int i = 0; i < NumberCount; i++)
            {
                result[i] = numbers[i] * _random.Next(1000);
            }
            Console.WriteLine("Multiplied");
            WriteResult(result);

            return result;
        }

        private static int[] SortNumbers(int[] numbers)
        {
            numbers = numbers.OrderByDescending(x => x).ToArray();
            Console.WriteLine("Sorted");
            WriteResult(numbers);

            return numbers;
        }

        private static void CalculateAverage(int[] numbers)
        {
            var average = numbers.Average();
            Console.WriteLine($"Average: {average}");
        }

        private static void WriteResult(int[] numbers)
        {
            Console.WriteLine(Separator);
            foreach (var item in numbers)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine(Separator);
        }
    }
}
