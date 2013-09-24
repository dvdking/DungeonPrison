using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DungeonPrisonLib.Actors.Behaviours
{
    class SomeGuyBehaviour:Behaviour
    {
        public SomeGuyBehaviour(Actor actor)
            : base(actor)
        {
 
        }

        public override void Update(float delta, TileMap tileMap)
        {
            int dirX = RandomTool.NextBool() ? RandomTool.NextSign() : 0;
            int dirY = dirX == 0?  RandomTool.NextSign(): 0;
            Actor.Move(dirX, dirY, tileMap);
        }
    }
}
