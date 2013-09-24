using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DungeonPrisonLib.Actors.Behaviours
{
    public abstract class Behaviour
    {
        protected Actor Actor;

        public Behaviour(Actor actor = null)
        {
            Actor = actor;
        }

        public abstract void Update(float delta, TileMap tileMap);
    }
}
