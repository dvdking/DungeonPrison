using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DungeonPrisonLib.Actors.Behaviours
{
    public abstract class Behaviour
    {
        protected Creature Creature;

        public Behaviour(Creature actor = null)
        {
            Creature = actor;
        }

        public abstract void Update(float delta, TileMap tileMap);
    }
}
