using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DungeonPrisonLib;
using DungeonPrisonConsoleRenderer.GUI;
using System.Drawing;
namespace DungeonPrisonConsoleRenderer
{
    class Program
    {
        static void Main(string[] args)
        {
            var renderer = new ConsoleRenderer(64,25);
            var guiManager = new GUIManager(renderer);

            var healthBar = new HealthBar() 
            {
                Location = new Point(32, 1) 
            };
            guiManager.AddElement(healthBar);

            renderer.LogPositionX = 1;
            renderer.LogPositionY = 17;
            renderer.LogWidth = 30;
            renderer.LogHeight = 4;
            renderer.HealthBarPositionX = 32;
            renderer.HealthBarPositionY = 1;

            if (renderer.ReadGraphicsInfoData("Content//GraphicsInfo.xml"))
            {
                GameManager.CreateInstance(renderer, new ConsoleInput());

                GameManager.Instance.Update();
                GameManager.Instance.Draw();

                while (!GameManager.Instance.Exit)
                {
                    GameManager.Instance.Draw();
                    guiManager.Draw();
                    renderer.DrawBuffer();

                    GameManager.Instance.Input.Update();
                    GameManager.Instance.Update();
                }
            }
            else
            {
                Console.WriteLine("Fatal error");
                Console.ReadKey();
            }
        }
    }
}
