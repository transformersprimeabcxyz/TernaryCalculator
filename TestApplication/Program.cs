using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TernaryCalculator.Framework;

namespace TestApplication
{
    class Program
    {
        public enum TestMethod
        {
            Add,
            Subtract,
            Multiply
        }

        static char GetOperatorChar(TestMethod method)
        {
            switch (method)
            {
                case TestMethod.Add:
                    return '+';
                case TestMethod.Subtract:
                    return '-';
                case TestMethod.Multiply:
                    return '*';
            }
            throw new ArgumentException("method");
        }

        static void Main(string[] args)
        {
            CheckAnswers(-300, 300, TestMethod.Add);
            CheckAnswers(-300, 300, TestMethod.Subtract);
            CheckAnswers(-300, 300, TestMethod.Multiply);

            Process.GetCurrentProcess().WaitForExit();
        }

        static void CheckAnswers(int start, int end, TestMethod method)
        {
            int errors = 0;
            for (var x = start; x <= end; x++)
            {
                for (var y = start; y <= end; y++)
                {
                    var ternaryX = BalancedTryte.FromInt32(x);
                    var ternaryY = BalancedTryte.FromInt32(y);

                    var binaryResult = 0;
                    var ternaryResult = default(BalancedTryte);

                    switch (method)
                    {
                        case TestMethod.Add:
                            binaryResult = x + y;
                            ternaryResult = ternaryX + ternaryY;
                            break;
                        case TestMethod.Subtract:
                            binaryResult = x - y;
                            ternaryResult = ternaryX - ternaryY;
                            break;
                        case TestMethod.Multiply:
                            binaryResult = x * y;
                            ternaryResult = ternaryX * ternaryY;
                            break;
                    }

                    if (binaryResult != ternaryResult.ToInt32())
                    {
                        Console.WriteLine("{0} {7} {1} ({2}) != {6}", x, y, binaryResult, ternaryX,
                            ternaryY, ternaryResult, ternaryResult.ToInt32(), GetOperatorChar(method));
                        errors++;
                    }
                }
            }

            Console.WriteLine("Finished {0} tests for {1} to {2} with {3} errors", method, start, end, errors);
        }
    }
}
