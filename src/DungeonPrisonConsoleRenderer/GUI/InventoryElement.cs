using DungeonPrisonLib;
using DungeonPrisonLib.Actors.Items;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace DungeonPrisonConsoleRenderer.GUI
{
    class InventoryElement:GUIElement
    {
        public delegate void OnItemChosen(InventoryType type, Item chosenItem);

        public enum InventoryType
        {
            Browse,
            WieldWeapon,
            WearArmor
        }
        
        public int ChosenItem;
        public event OnItemChosen ItemChosen;

        private InventoryType _type;

        public InventoryElement(InventoryType type)
        {
            InteruptInput = true;
            _type = type;
        }

        public override void Update()
        {
            base.Update();

            var keys = GameManager.Instance.Input;
            foreach (var key in keys.GetPressedKeys())
            {
                switch (key)
                {
                    case InputKey.MoveUp:
                        ChosenItem--;
                        break;
                    case InputKey.MoveDown:
                        ChosenItem++;
                        break;
                    case InputKey.Cancel:
                        Close();
                        break;
                    case InputKey.Accept:
                        ChooseItem();
                        Close();
                        break;
                    default:
                        break;
                }
            }
            int count = GameManager.Instance.Player.Inventory.GetItems().Count;
            if (ChosenItem >= count)
            {
                ChosenItem = 0;
            }
            else if (ChosenItem < 0)
            {
                ChosenItem = count - 1;
            }
        }

        private void ChooseItem()
        {
            switch (_type)
            {
                case InventoryType.Browse:
                    throw new NotImplementedException();
                    break;
                case InventoryType.WieldWeapon:
                case InventoryType.WearArmor:
                    if (GameManager.Instance.Player.Inventory.GetItems().Count == 0)
                        break;
                    if (ItemChosen != null)
                    {
                        ItemChosen(_type, GameManager.Instance.Player.Inventory.GetItemWithIndex(ChosenItem));
                    }
                    break;
                default:
                    Debug.Fail("some strange enum");
                    break;
            }
        }

        public void Close()
        {
            GuiManager.RemoveElement(this);
            InteruptInput = false;
        }

        public override void Draw()
        {
            base.Draw();
            var renderer = GuiManager.Renderer;

            renderer.CleanBuffer();
            int row = 0;
            string topMessage = "";
            switch (_type)
            {
                case InventoryType.Browse:
                    break;
                case InventoryType.WieldWeapon:
                    topMessage = "Wield what?";
                    break;
                case InventoryType.WearArmor:
                    topMessage = "Wear what";
                    break;
                default:
                    break;
            }
            renderer.DrawString(Location.X, Location.Y + row, topMessage);
            row++;
            renderer.DrawString(Location.X, Location.Y + row, "______________________________________");
            row++;
            int curItem = 0;
            foreach (var item in GameManager.Instance.Player.Inventory.GetItems())
            {
                renderer.DrawString(Location.X, Location.Y + row, (curItem == ChosenItem ? " -> " : "") + item.GameName, item == GameManager.Instance.Player.WieldedItem ? ConsoleColor.Green : ConsoleColor.White);
                row++;
                curItem++;
            }
        }
    }
}
