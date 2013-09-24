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

        internal void Attack(Actor actor, AttackInfo attackInfo)
        {
            GameManager.Instance.Log.AddMessage(attackInfo.Message);
            actor.Destroy();
        }

        public void Move(int x, int y, TileMap tileMap)
        {
            if (tileMap.InBounds(X + x, Y + y))
               if (tileMap.IsSolid(X + x, Y + y))
                return;

            var actor = GameManager.Instance.GetActorAtPosition(X + x, Y + y);

            if (actor != null)
            {
                Attack(actor, new AttackInfo { Damage = 1, Message = GameName + (GameName == "you" ? " hit " : " hits ") + actor.GameName });
                return;
            }

            X += x;
            Y += y;
        }
    }
}
