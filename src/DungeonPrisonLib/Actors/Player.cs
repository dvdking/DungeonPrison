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
        }

        public override void Update(float delta, TileMap tileMap)
        {
            base.Update(delta, tileMap);
        }



    }
}
