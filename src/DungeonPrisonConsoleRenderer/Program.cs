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
            var renderer = new ConsoleRenderer(24,15);
            renderer.LogPositionX = 1;
            renderer.LogPositionY = 11;
            renderer.LogWidth = 30;
            renderer.LogHeight = 4;

            GameManager.CreateInstance(renderer, new ConsoleInput());
            GameManager.Instance.Run();
        }
    }
}
