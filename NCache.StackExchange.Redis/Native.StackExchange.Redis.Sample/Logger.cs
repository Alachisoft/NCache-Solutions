using System;

namespace Native.StackExchange.Redis.Sample
{
    internal static class Logger
    {
        internal static void PrintTestStartInformation(string message)
        {
            var color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"******{message}******");
            Console.ForegroundColor = color;
            Console.Write("\n\n");

        }

        internal static void PrintDataCacheException(Exception ex)
        {
            var color = Console.ForegroundColor;
            if (ex != null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"EXCEPTION");
                Console.WriteLine($"DataCache exception encountered:");
                Console.WriteLine($"Error Messge: {ex.Message}");
                Console.Write("\n\n");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"EXCEPTION");
                Console.WriteLine($"General exception encountered:");
                Console.WriteLine($"Exception Type:{ex.GetType().Name}");
                Console.WriteLine($"ErrorMessage:{ex.Message}");
                Console.Write("\n\n");
            }

            Console.ForegroundColor = color;
        }

        internal static void PrintSuccessfulOutcome(string message)
        {
            var color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"SUCCESS: {message}");
            Console.ForegroundColor = color;
            Console.Write("\n\n");
            Program.successfulTests++;
        }

        internal static void PrintFailureOutcome(string message)
        {
            var color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine($"FAILURE: {message}");
            Console.ForegroundColor = color;
            Console.Write("\n\n");
            Program.failedTests++;
        }

        internal static void PrintBreakLine()
        {
            var color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"*************************");
            Console.ForegroundColor = color;
            Console.Write("\n\n");
        }

        internal static void WriteHeaderLine(string message)
        {
            var color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write($"++{message}++");
            Console.ForegroundColor = color;
            Console.Write("\n\n");
        }

        internal static void WriteFooterLine(string message)
        {
            var color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write($"--{message}--");
            Console.ForegroundColor = color;
            Console.Write("\n\n\n\n");
        }
    }
}