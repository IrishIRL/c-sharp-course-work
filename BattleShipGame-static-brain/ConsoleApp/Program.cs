using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Xml;
using CalculatorBrain;
using MenuSystem;

namespace ConsoleApp
{
    class Program
    {

        private static readonly Brain Brain = new Brain();
        
        
        static void Main(string[] args)
        {
            Console.Clear();


            var mainMenu = new Menu(ReturnCurrentDisplayValue, "Calculator Main", EMenuLevel.Root);
            mainMenu.AddMenuItems(new List<MenuItem>()
            {
                new MenuItem("A", "Binary operations", SubmenuBinary),
                new MenuItem("S", "Unary operations", SubmenuUnary),
            });

            mainMenu.Run();
        }

        public static string ReturnCurrentDisplayValue()
        {
            return Brain.CurrentValue.ToString();
        }

        public static string MethodA()
        {
            Console.WriteLine("Method A!!!!!");
            return "";
        }

        public static string SubmenuBinary()
        {
            var menu = new Menu(ReturnCurrentDisplayValue, "Binary", EMenuLevel.First);
            menu.AddMenuItems(new List<MenuItem>()
            {
                new MenuItem("+", "+", Add),
                new MenuItem("-", "-", MethodA),
                new MenuItem("/", "/", MethodA),
                new MenuItem("*", "*", Multiplication),
            });
            var res = menu.Run();
            return res;
        }

        public static string Multiplication()
        {
            Console.WriteLine("Current value: " + Brain.CurrentValue);
            Console.WriteLine("multiply");
            Console.Write("number: ");
            var n = Console.ReadLine()?.Trim();
            double.TryParse(n, out var converted);

            //Brain.ApplyCustomFunction( CustomMultiply, converted);
            Brain.ApplyCustomFunction( (a,  b) => a * b, converted);
            return "";
        }

        public static double CustomMultiply(double a , double b)
        {
            return a * b;
        }

        public static string Add()
        {
            // CalculatorCurrentDisplay
            Console.WriteLine("Current value: " + Brain.CurrentValue);
            Console.WriteLine("plus");
            Console.Write("number: ");
            var n = Console.ReadLine()?.Trim();
            double.TryParse(n, out var converted);

            Brain.Add(converted);

            return "";
        }

        public static string SubmenuUnary()
        {
            var menu = new Menu(ReturnCurrentDisplayValue, "Unary", EMenuLevel.First);
            menu.AddMenuItems(new List<MenuItem>()
            {
                new MenuItem("Negate", "Negate", MethodA),
                new MenuItem("Sqrt", "Sqrt", MethodA),
                new MenuItem("Root", "Root", MethodA),
            });
            var res = menu.Run();
            return res;
        }
    }
}