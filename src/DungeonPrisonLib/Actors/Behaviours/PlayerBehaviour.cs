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
