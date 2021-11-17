using System;
using System.Collections.Generic;
using MenuSystem;

namespace ConsoleApp
{
    public class Misc
    {
        public static string MiscMenu()
        {
            var menu = new Menu("Which binary operation would you like to do?", EMenuLevel.First);
            menu.AddMenuItems(new List<MenuItem>()
            {
                new MenuItem("1", "Set color of background", BackGroundSettings),
                new MenuItem("2", "Set color of foreground", ForeGroundSettings),
                new MenuItem("3", "Console title name", FontSizeSettings)
            });
            var res = menu.Run();
            return res;
        }
        
        public static string BackGroundSettings()
        {
            Console.WriteLine("=====================");
            Console.WriteLine("Colors available:\n1)White\n2)Red\n3)Green\n4)Blue\n5)Yellow\n6)Black\n7)Magenta");
            Console.WriteLine("Please input your number:");
            var n = Console.ReadLine()?.Trim();
            double.TryParse(n, out var converted);
            
            switch (converted)
            {
                case 1:
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.Clear();
                    break;
                case 2:
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.Clear();
                    break;
                case 3:
                    Console.BackgroundColor = ConsoleColor.Green;
                    Console.Clear();
                    break;
                case 4:
                    Console.BackgroundColor = ConsoleColor.Blue;
                    Console.Clear();
                    break;
                case 5:
                    Console.BackgroundColor = ConsoleColor.Yellow;
                    Console.Clear();
                    break;
                case 6:
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.Clear();
                    break;
                case 7:
                    Console.BackgroundColor = ConsoleColor.Magenta;
                    Console.Clear();
                    break;
            }

            return "";
        }
        
        public static string ForeGroundSettings()
        {
            Console.WriteLine("=====================");
            Console.WriteLine("Colors available:\n1)White\n2)Red\n3)Green\n4)Blue\n5)Yellow\n6)Black\n7)Magenta");
            Console.WriteLine("Please input your number:");
            var n = Console.ReadLine()?.Trim();
            double.TryParse(n, out var converted);
            
            switch (converted)
            {
                case 1:
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Clear();
                    break;
                case 2:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Clear();
                    break;
                case 3:
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Clear();
                    break;
                case 4:
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Clear();
                    break;
                case 5:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Clear();
                    break;
                case 6:
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.Clear();
                    break;
                case 7:
                    Console.BackgroundColor = ConsoleColor.Magenta;
                    Console.Clear();
                    break;
            }

            return "";
        }

        public static string FontSizeSettings()
        {
            Console.WriteLine("=====================");
            Console.WriteLine("Please input your console name:");
            string titileName = Console.ReadLine();
            Console.Title = titileName;
            return "";
        }
    }
}