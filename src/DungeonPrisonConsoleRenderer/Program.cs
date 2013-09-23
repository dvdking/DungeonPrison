﻿using System;
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
            GameManager gameManager = new GameManager(new ConsoleRenderer(14,10), new ConsoleInput());
            gameManager.Run();
        }
    }
}
