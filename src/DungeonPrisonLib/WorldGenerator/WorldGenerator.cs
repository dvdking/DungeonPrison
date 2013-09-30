using DungeonPrisonLib.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DungeonPrisonLib.WorldGenerator
{
    public class WorldGen
    {
        private TileMapGenerator _generator;

        public WorldGen()
        {
            _generator = new TileMapGenerator();
        }

        public WorldChunk GenerateBlock(int x, int y, int z, WorldChunk[,,] chunks)
        {
            var chunk = new WorldChunk(x, y, z);

            chunk.TileMap = _generator.GenerateTileMap(Settings.TileMapSize.X, Settings.TileMapSize.Y, TlleMapType.NeutralDungeon);

            return chunk;
        }
    }
}
