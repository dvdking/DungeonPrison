using DungeonPrisonLib.Actors.Behaviours;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DungeonPrisonLib.Actors
{
    public class Player:Creature
    {
        public Player()
            :base()
        {
            ItemWieldedEvent += Player_ItemWieldedEvent;
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
