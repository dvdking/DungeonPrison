using DungeonPrisonLib.Actors.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DungeonPrisonLib.Actors.Behaviours
{
    class PlayerBehaviour:Behaviour
    {
        public PlayerBehaviour(Creature actor)
            : base(actor)
        {
 
        }

        public override void Update(float delta, TileMap tileMap)
        {
            var pressedKeys = GameManager.Instance.Input.GetPressedKeys();
            
            foreach (var key in pressedKeys)
            {
                switch (key)
                {
                    case InputKey.MoveLeft:
                        Creature.Move(-1, 0, tileMap);
                        break;
                    case InputKey.MoveRight:
                        Creature.Move(1, 0, tileMap);
                        break;
                    case InputKey.MoveUp:
                        Creature.Move(0, -1, tileMap);
                        break;
                    case InputKey.MoveDown:
                        Creature.Move(0, 1, tileMap);
                        break;
                    case InputKey.Wait:
                        Creature.Wait();
                        break;
                    case InputKey.PickUp:
                        Creature.PickUpItem();
                        break;
                    case InputKey.WieldWeapon:
                        ChooseWeaponToWield();
                        break;
                    default:
                        break;
                }
            }
        }

        private void ChooseWeaponToWield()
        {
 
        }

        private void WieldWeapon(Item item)
        {
            if (item != null)
            {
                GameManager.Instance.Log.AddMessage("Nothing to wield");
            }
            else
            {
                Creature.WieldItem(item);
            }
        }
    }
}
