using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DungeonPrisonLib
{
    public interface IRenderer
    {
        void Draw(Player player, List<Actor> actors, TileMap tileMap);
    }
}
