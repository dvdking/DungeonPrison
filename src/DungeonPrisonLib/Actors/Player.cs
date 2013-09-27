using DungeonPrisonLib.Actors.Behaviours;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DungeonPrisonLib.Actors.Items;

namespace DungeonPrisonLib.Actors
{
    public class Player:Creature
    {
        public Player()
            :base()
        {
            ItemWieldedEvent += Player_ItemWieldedEvent;
            ItemPickedUpEvent += Player_ItemPickedUpEvent;
            ArmorWorn += Player_ArmorWorn;
            Depth = -2;
        }

        void Player_ArmorWorn(Conclusion conclusion, Armor armor, Armor newArmor)
        {
            switch (conclusion)
            {
                case Conclusion.Succes:
                    if (armor != null)
                    {
                        GameManager.Instance.Log.AddMessage("You remove your " + armor.GameName);
                    }
                    if (newArmor != null)
                    {
                        GameManager.Instance.Log.AddMessage("You putting on your " + newArmor.GameName);
                    }
                    break;
                case Conclusion.Fail:
                    GameManager.Instance.Log.AddMessage("Cannot wear that");
                    break;
                default:
                    break;
            }
            
        }

        void Player_ItemPickedUpEvent(Items.Item item)
        {
            if (item == null)
            {
                GameManager.Instance.Log.AddMessage("There is nothing to pick up");
            }
            else
            {
                GameManager.Instance.Log.AddMessage("You pick up " + item.Name);
            }
        }

        void Player_ItemWieldedEvent(Creature.Conclusion conclusion, Items.Item oldItem, Items.Item newItem)
        {
            switch (conclusion)
            {
                case Conclusion.Succes:
                    if (oldItem != null)
                    {
                        GameManager.Instance.Log.AddMessage("You take off your " + oldItem.GameName);
                    }
                    GameManager.Instance.Log.AddMessage("You are now wielding " + newItem.GameName);
                    break;
                case Conclusion.Fail:
                    GameManager.Instance.Log.AddMessage("Can not wield that");
                    break;
                case Conclusion.AlreadyWielding:
                    GameManager.Instance.Log.AddMessage("You are alredy wielding that");
                    break;
                default:
                    break;
            }
        }

        public override void Update(float delta, TileMap tileMap)
        {
            base.Update(delta, tileMap);
        }



    }
}
