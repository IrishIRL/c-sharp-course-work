using System;

namespace ConsoleApp
{
    public class BinaryOperations
    {
        public static string Add()
        {
            Console.WriteLine("Your Current value is: {0}.\nPlease, input a new number:", ValueOperations.CalculatorCurrentDisplay);
            var n = Console.ReadLine()?.Trim();
            double.TryParse(n, out var converted);

            ValueOperations.CalculatorCurrentDisplay += converted;
            return ValueOperations.CalculatorCurrentDisplay.ToString();
        }
        public static string Substract()
        {
            Console.WriteLine("Your Current value is: {0}.\nPlease, input a new number:", ValueOperations.CalculatorCurrentDisplay);
            var n = Console.ReadLine()?.Trim();
            double.TryParse(n, out var converted);
            
            ValueOperations.CalculatorCurrentDisplay -= converted;
            return ValueOperations.CalculatorCurrentDisplay.ToString();
        }
        public static string Divide()
        {
            Console.WriteLine("Your Current value is: {0}.\nPlease, input a new number:", ValueOperations.CalculatorCurrentDisplay);
            var n = Console.ReadLine()?.Trim();
            double.TryParse(n, out var converted);

            if (converted == 0)
            {
                Console.WriteLine("You cannot divide on 0");
                return ValueOperations.CalculatorCurrentDisplay.ToString();
            }

            ValueOperations.CalculatorCurrentDisplay /= converted;
            return ValueOperations.CalculatorCurrentDisplay.ToString();
        }
        public static string Multiply()
        {
            Console.WriteLine("Your Current value is: {0}.\nPlease, input a new number:", ValueOperations.CalculatorCurrentDisplay);
            var n = Console.ReadLine()?.Trim();
            double.TryParse(n, out var converted);

            ValueOperations.CalculatorCurrentDisplay *= converted;
            return ValueOperations.CalculatorCurrentDisplay.ToString();
        }

        public static string Power()
        {
            Console.WriteLine("Your Current value is: {0}.\nPlease, input a new number:", ValueOperations.CalculatorCurrentDisplay);
            var n = Console.ReadLine()?.Trim();
            double.TryParse(n, out var converted);

            ValueOperations.CalculatorCurrentDisplay = Math.Pow(ValueOperations.CalculatorCurrentDisplay, converted);
            return ValueOperations.CalculatorCurrentDisplay.ToString();
        }
    }
}