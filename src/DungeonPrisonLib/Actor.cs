using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DungeonPrisonLib
{
    public abstract class Actor
    {
        public bool IsAlive { get; private set; }

        public string Name;
        public string GameName;

        public int X, Y;

        public abstract void Update(float delta, TileMap tileMap);

        public Actor()
        {
            IsAlive = true;
        }

        internal void Attack(Actor actor, AttackInfo attackInfo)
        {
            GameManager.Instance.Log.AddMessage(attackInfo.Message);
            actor.Destroy();
        }

        internal void Destroy()
        {
            GameManager.Instance.DestroyObject(this);
            IsAlive = false;
        }
    }
}
