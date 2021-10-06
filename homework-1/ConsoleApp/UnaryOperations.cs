using System;

namespace ConsoleApp
{
    public class UnaryOperations
    {
        public static string Negate()
        {
            if (ValueOperations.CalculatorCurrentDisplay == 0) return ValueOperations.CalculatorCurrentDisplay.ToString();
            ValueOperations.CalculatorCurrentDisplay = ValueOperations.CalculatorCurrentDisplay * -1;
            return ValueOperations.CalculatorCurrentDisplay.ToString();
        }
        public static string Sqrt()
        {
            if (ValueOperations.CalculatorCurrentDisplay < 0)
            {
                Console.WriteLine("You cannot find square root of negative number!");
                return ValueOperations.CalculatorCurrentDisplay.ToString();
            }
            ValueOperations.CalculatorCurrentDisplay = Math.Sqrt(ValueOperations.CalculatorCurrentDisplay);
            return ValueOperations.CalculatorCurrentDisplay.ToString();
        }
        public static string Root()
        {
            ValueOperations.CalculatorCurrentDisplay = ValueOperations.CalculatorCurrentDisplay*ValueOperations.CalculatorCurrentDisplay;
            return ValueOperations.CalculatorCurrentDisplay.ToString();
        }
        
        public static string Abs()
        {
            if (ValueOperations.CalculatorCurrentDisplay<0) ValueOperations.CalculatorCurrentDisplay = ValueOperations.CalculatorCurrentDisplay * -1; 

        return ValueOperations.CalculatorCurrentDisplay.ToString();
        }
    }
}