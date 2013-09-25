using DungeonPrisonConsoleRenderer.GUI;
using DungeonPrisonLib;
using DungeonPrisonLib.Actors.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DungeonPrisonConsoleRenderer
{
    class ConsoleInput:IInput
    {
        List<InputKey> PressedKeys = new List<InputKey>();

        private GUIManager _guiManager;

        public ConsoleInput(GUIManager guiManager)
        {
            _guiManager = guiManager;
        }

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
                        var inventory = new InventoryElement(InventoryElement.InventoryType.WieldWeapon);
                        inventory.ItemChosen += inventory_ItemChosen;
                        _guiManager.AddElement(inventory);
                        break;
                    default:
                        break;
                }
            }
        }

        private void inventory_ItemChosen(InventoryElement.InventoryType type, Item chosenItem)
        {
            switch (type)
            {
                case InventoryElement.InventoryType.Browse:
                    break;
                case InventoryElement.InventoryType.WieldWeapon:
                    GameManager.Instance.Player.WieldItem(chosenItem);
                    break;
                default:
                    break;
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
                case ConsoleKey.Escape:
                    PressedKeys.Add(InputKey.Cancel);
                    break;
                case ConsoleKey.W:
                    PressedKeys.Add(InputKey.WieldWeapon);
                    break;
                case ConsoleKey.I:
                    PressedKeys.Add(InputKey.OpenInventory);
                    break;
                case ConsoleKey.Enter:
                    PressedKeys.Add(InputKey.Accept);
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
