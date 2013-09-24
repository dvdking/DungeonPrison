using DungeonPrisonLib.Actors.Behaviours;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace DungeonPrisonLib.Actors
{
    public class Creature:Actor
    {
        Behaviour _behaviour;

        public Creature()
        {
        }

        public void SetBehaviour(Behaviour behaviour)
        {
            _behaviour = behaviour;
        }

        public override void Update(float delta, TileMap tileMap)
        {
            if(_behaviour != null)
                _behaviour.Update(delta, tileMap);
        }


    }
}
