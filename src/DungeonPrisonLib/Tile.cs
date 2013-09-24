using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DungeonPrisonLib
{
    public struct Tile
    {
        public TileType Type;
    }

    public enum TileType
    {
        Nothing,
        Wall,
        Floor
    }
}
