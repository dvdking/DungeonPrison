using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DungeonPrisonLib
{
    public abstract class Actor
    {
        public string Name;

        public int X, Y;

        public abstract void Update(float delta);
    }
}
