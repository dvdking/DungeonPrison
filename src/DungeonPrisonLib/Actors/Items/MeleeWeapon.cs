using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DungeonPrisonLib.Actors.Items
{
    public class MeleeWeapon:Item
    {
        public int Damage;

        public MeleeWeapon()
            : base()
        {
 
        }

        public override void Update(float delta, TileMap tileMap)
        {
            base.Update(delta, tileMap);
        }
    }
}
