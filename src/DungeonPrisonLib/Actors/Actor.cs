using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace DungeonPrisonLib.Actors
{
    public abstract class Actor
    {
        public bool IsAlive { get; private set; }

        public string Name;
        public string GameName;

        public int X, Y;

        public Point Position 
        {
            get 
            {
                return new Point(X, Y); 
            }
            set 
            { 
                X = value.X;
                Y = value.Y; 
            }
        }

        public int Depth;

        public abstract void Update(float delta, TileMap tileMap);

        public Actor()
        {
            IsAlive = true;
        }

        internal void Destroy()
        {
            GameManager.Instance.DestroyObject(this);
            GameManager.Instance.Log.AddMessage(GameName == "you" ? "you are dead" : GameName + " is dead");
            IsAlive = false;
        }
    }
}
