using DungeonPrisonLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DungeonPrisonConsoleRenderer
{
    class ConsoleInput:IInput
    {
        List<InputKey> PressedKeys = new List<InputKey>();

        public void Update()
        {
            PressedKeys.Clear();
            var key = Console.ReadKey(true);

            switch (key.Key)
            {
                case ConsoleKey.LeftArrow:
                    PressedKeys.Add(InputKey.MoveLeft);
                    break;
                case ConsoleKey.RightArrow:
                    PressedKeys.Add(InputKey.MoveRight);
                    break;
                case ConsoleKey.UpArrow:
                    PressedKeys.Add(InputKey.MoveUp);
                    break;
                case ConsoleKey.DownArrow:
                    PressedKeys.Add(InputKey.MoveDown);
                    break;
                case ConsoleKey.S:
                    PressedKeys.Add(InputKey.Wait);
                    break;
            }
        }

        public List<InputKey> GetPressedKeys()
        {
            return PressedKeys.ToList();
        }

        public List<InputKey> GetReleasedKeys()
        {
            throw new NotImplementedException();
        }
    }
}
