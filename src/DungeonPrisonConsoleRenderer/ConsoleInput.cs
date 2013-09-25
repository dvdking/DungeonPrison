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
            UpdateConsoleKeys();

            UpdateUserControl();

        }

        private void UpdateUserControl()
        {
            foreach (var key in PressedKeys)
            {
                switch (key)
                {
                    case InputKey.MoveLeft:
                        GameManager.Instance.Player.Move(-1, 0, GameManager.Instance.TileMap);
                        break;
                    case InputKey.MoveRight:
                        GameManager.Instance.Player.Move(1, 0, GameManager.Instance.TileMap);
                        break;
                    case InputKey.MoveUp:
                        GameManager.Instance.Player.Move(0, -1, GameManager.Instance.TileMap);
                        break;
                    case InputKey.MoveDown:
                        GameManager.Instance.Player.Move(0, 1, GameManager.Instance.TileMap);
                        break;
                    case InputKey.Wait:
                        GameManager.Instance.Player.Wait();
                        break;
                    case InputKey.PickUp:
                        GameManager.Instance.Player.PickUpItem();
                        break;
                    case InputKey.WieldWeapon:
                        //ChooseWeaponToWield();
                        break;
                    default:
                        break;
                }
            }
        }

        private void UpdateConsoleKeys()
        {
            PressedKeys.Clear();
            var consoleKey = Console.ReadKey(true);


            switch (consoleKey.Key)
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
                case ConsoleKey.G:
                    PressedKeys.Add(InputKey.PickUp);
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
