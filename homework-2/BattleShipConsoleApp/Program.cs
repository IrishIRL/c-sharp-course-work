using System;
using BattleShipBrain;
using BattleShipConsoleUI;

namespace BattleShipConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello Battleship!");
            Console.WriteLine("Input X size:");
            var x = Console.ReadLine()?.Trim();
            int.TryParse(x, out var xConverted);
            Console.WriteLine("Input Y size:");
            var y = Console.ReadLine()?.Trim();
            int.TryParse(y, out var yConverted);
            
            var brain1 = new BSBrain(xConverted,yConverted);
            //BSConsoleUI.DrawBoard(brain1.GetBoard(0));
            //BSConsoleUI.DrawBoard(brain1.GetBoard(0));
            
            int player = 0;
            do
            {
                switch (player)
                {
                    case 0:
                        ChoosePlaceToPlantBomb(0, xConverted, yConverted);
                        BSConsoleUI.DrawBoard(brain1.GetBoard(0));
                        player = 1;
                        break;
                    case 1:
                        ChoosePlaceToPlantBomb(1, xConverted, yConverted);
                        BSConsoleUI.DrawBoard(brain1.GetBoard(1));
                        player = 0;
                        break;
                }
            } while (true); // Will be changed when we have win conditions.
        }

        public static void ChoosePlaceToPlantBomb(int person, int x, int y)
        {
            int xLocationConverted = LocationX(person);
            int yLocationConverted = LocationY(person);

            do
            {
                if (xLocationConverted > x - 1)
                {
                    Console.WriteLine("Please input number less than {0}", x);
                    xLocationConverted = LocationX(person);
                }
            } while (xLocationConverted > x-1);
            
            do
            {
                if (yLocationConverted > y - 1)
                {
                    Console.WriteLine("Please input number less than {0}", y);
                    yLocationConverted = LocationY(person);
                }
            } while (yLocationConverted > y-1);
            
            BSBrain.PlantBomb(xLocationConverted, yLocationConverted, person);
        }

        public static int LocationX(int person)
        {
            Console.WriteLine("Current Player: {0}", person+1);
            Console.WriteLine("Input bomb location X:");
            var xLocation = Console.ReadLine()?.Trim();
            
            int.TryParse(xLocation, out var xLocationConverted);
            return xLocationConverted;
        }
        
        public static int LocationY(int person)
        {
            Console.WriteLine("Current Player: {0}", person+1);
            Console.WriteLine("Input bomb location Y:");
            var yLocation = Console.ReadLine()?.Trim();
            int.TryParse(yLocation, out var yLocationConverted);
            return yLocationConverted;
        }
    }
}