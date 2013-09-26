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
            Console.WriteLine("Loading...");

            var renderer = new ConsoleRenderer(64,25);
            var guiManager = new GUIManager(renderer);

            InitGUI(guiManager);

            if (renderer.ReadGraphicsInfoData("Content//GraphicsInfo.xml"))
            {
                GameManager.CreateInstance(renderer, new ConsoleInput(guiManager));
                GameManager.Instance.InitGame();
                GameManager.Instance.Update();
                GameManager.Instance.Draw();

                while (!GameManager.Instance.Exit)
                {
                    GameManager.Instance.Draw();
                    guiManager.Draw();
                    renderer.DrawBuffer();

                    GameManager.Instance.Input.Update();
                    guiManager.Update();
                    if (!guiManager.IsInputInterupted)
                    {
                        GameManager.Instance.Update();
                    }
                }
            }
            else
            {
                Console.WriteLine("Fatal error");
                Console.ReadKey();
            }
        }

        private static void InitGUI(GUIManager guiManager)
        {
            var healthBar = new HealthBar()
            {
                Location = new Point(32, 1)
            };
            guiManager.AddElement(healthBar);

            var logRenderer = new LogRenderer()
            {
                Location = new Point(1, 17),
                Size = new Size(30, 4)
            };
            guiManager.AddElement(logRenderer);
        }
    }
}
