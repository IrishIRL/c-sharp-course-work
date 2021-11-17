using System;
using System.Collections.Generic;
using MenuSystem;

namespace ConsoleApp
{
    public class ValueOperations
    {
        private static double SimpleCheck = 0; // This will be used to check if the menu was opened for the first time or not. If yes, some additional data will be written.
        public static double CalculatorCurrentDisplay = 0.0;
        
        public static string ValueMenu()
        {
            var menu = new Menu("Which binary operation would you like to do?", EMenuLevel.First);
            menu.AddMenuItems(new List<MenuItem>()
            {
                new MenuItem("S", "Set new value", SetValue),
                new MenuItem("C", "Clear value", ClearResult),

            });
            var res = menu.Run();
            return res;
        }
        
        public static string SetValue()
        {
            Console.WriteLine("=====================");
            Console.WriteLine("Please input your number:");
            var n = Console.ReadLine()?.Trim();
            double.TryParse(n, out var converted);
            CalculatorCurrentDisplay += converted;
            SimpleCheck++;
            return CalculatorCurrentDisplay.ToString();
        }
        
        public static string ClearResult()
        {
            CalculatorCurrentDisplay = 0.0;
            SimpleCheck = 0;
            return CalculatorCurrentDisplay.ToString();
        }
    }
}