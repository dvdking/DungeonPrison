using DungeonPrisonLib.WorldGenerator;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace DungeonPrisonLib.World
{
    class WorldChunk
    {
        public int X, Y, Z;
        public TlleMapType Type;

        public TileMap GetTileMap()
        {
            if (TileMapExists())
            {
                return LoadTileMap();
            }
            else
            {
                return GenerateTileMap(Type);
            }
        }

        private bool TileMapExists()
        {
            return false;
        }

        private TileMap GenerateTileMap(TlleMapType mapType)
        {
            return null;
        }

        private TileMap LoadTileMap()
        {
            TileMap tileMap = new TileMap();



            return null;
        }
    }
}
