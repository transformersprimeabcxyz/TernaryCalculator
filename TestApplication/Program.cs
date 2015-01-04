using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TernaryCalculator.Framework;

namespace TestApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
            BalancedTryte x = 25;
            BalancedTryte y = 2;
            y <<= 9;
            Console.WriteLine(x);
            Console.WriteLine(new string('-', 20));
            for (int i = 0; i < BalancedTryte.TryteSize-2; i++)
            {
                Console.WriteLine(string.Format("{0} x<y={1},x>y={2}", y, x < y, x > y));
                y >>= 1;
            }
            Console.ReadKey();
            
             */
            Console.Write("Enter X: ");
            var tryte1 = BalancedTryte.FromInt32(int.Parse(Console.ReadLine()));
            Console.Write("Enter Y: ");
            var tryte2 = BalancedTryte.FromInt32(int.Parse(Console.ReadLine()));
            Console.WriteLine();

            WriteBenchmarkHeader();
            while (true)
            {
                Benchmark(tryte1, tryte2, (x, y) => x & y, "&");
                Benchmark(tryte1, tryte2, (x, y) => x | y, "|");
                Benchmark(tryte1, tryte2, (x, y) => x ^ y, "^");
                Benchmark(tryte1, tryte2, (x, y) => x + y, "+");
                Benchmark(tryte1, tryte2, (x, y) => x - y, "-");
                Benchmark(tryte1, tryte2, (x, y) => x * y, "*");
                Benchmark(tryte1, tryte2, (x, y) => x / y, "/");

                Console.ReadKey();
            }

        }


        static void WriteBenchmarkHeader()
        {
            Console.WriteLine("{0, -39}  █  {1, -21}  █  {2}", "Ternary", "Decimal", "Time");
            Console.WriteLine(new string('█', 79));
        }

        static void Benchmark(BalancedTryte tryte1, BalancedTryte tryte2, Func<BalancedTryte, BalancedTryte, BalancedTryte> operation, string @operator)
        {
            var watch = new Stopwatch();
            watch.Start();
            var result = operation(tryte1, tryte2);
            watch.Stop();

            Console.WriteLine("{0} {1} {2} = {3}  █  {4:00000} {1} {5:00000} = {6, -5}  █  {7:0.0000}ms",
                tryte1, 
                @operator,
                tryte2, 
                result, 
                tryte1.ToInt32(),
                tryte2.ToInt32(),
                result.ToInt32(), 
                watch.Elapsed.TotalMilliseconds);

        }

    }
}
