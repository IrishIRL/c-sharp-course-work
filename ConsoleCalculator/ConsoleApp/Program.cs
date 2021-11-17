using System.Collections.Generic;
using MenuSystem;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var mainMenu = new Menu("Calculator Main", EMenuLevel.Root);
            mainMenu.AddMenuItems(new List<MenuItem>()
            {
                new MenuItem("V", "Value settings", ValueOperations.ValueMenu),
                new MenuItem("B", "Binary operations", SubmenuBinary),
                new MenuItem("U", "Unary operations", SubmenuUnary),
                new MenuItem("M", "Misc", Misc.MiscMenu)
            });
            mainMenu.Run();
        }
        private static string SubmenuBinary()
        {
            var menu = new Menu("Which binary operation would you like to do?", EMenuLevel.First);
            menu.AddMenuItems(new List<MenuItem>()
            {
                new MenuItem("+", "+", BinaryOperations.Add),
                new MenuItem("-", "-", BinaryOperations.Substract),
                new MenuItem("/", "/", BinaryOperations.Divide),
                new MenuItem("*", "*", BinaryOperations.Multiply),
                new MenuItem("p", "x Power y", BinaryOperations.Power)
            });
            var res = menu.Run();
            return res;
        }
        private static string SubmenuUnary()
        {
            var menu = new Menu("Which unary operation would you like to do?", EMenuLevel.First);
            menu.AddMenuItems(new List<MenuItem>()
            {
                new MenuItem("1", "Negate", UnaryOperations.Negate),
                new MenuItem("2", "Sqrt", UnaryOperations.Sqrt),
                new MenuItem("3", "Root", UnaryOperations.Root),
                new MenuItem("4", "Abs Value", UnaryOperations.Abs)
            });
            var res = menu.Run();
            return res;
        }
    }
}