using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DungeonPrisonLib;
namespace DungeonPrisonConsoleRenderer
{
    class Program
    {
        static void Main(string[] args)
        {
            var renderer = new ConsoleRenderer(64,25);

            renderer.LogPositionX = 1;
            renderer.LogPositionY = 17;
            renderer.LogWidth = 30;
            renderer.LogHeight = 4;
            renderer.HealthBarPositionX = 32;
            renderer.HealthBarPositionY = 1;

            if (renderer.ReadGraphicsInfoData("Content//GraphicsInfo.xml"))
            {
                GameManager.CreateInstance(renderer, new ConsoleInput());
                GameManager.Instance.Run();
            }
            else
            {
                Console.WriteLine("Fatal error");
                Console.ReadKey();
            }
        }
    }
}
