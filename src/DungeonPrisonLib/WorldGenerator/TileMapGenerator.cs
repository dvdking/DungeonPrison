using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace DungeonPrisonLib.WorldGenerator
{
    public enum TlleMapType
    {
        NeutralDungeon
    }

    public class TileMapGenerator
    {
        public TileMap GenerateTileMap(int width, int height, TlleMapType mapType)
        {

            switch (mapType)
            {
                case TlleMapType.NeutralDungeon:
                    return GenerateNeutralDungeon(width, height);
                    break;
                default:
                    Debug.Fail("Not implemented map type generation");
                    break;
            }
            return null;
        }

        private TileMap GenerateNeutralDungeon(int width, int height)
        {
            NeutralDungeonGenerator dgen = new NeutralDungeonGenerator();
            return dgen.Generate(width, height);
        }
    }
}
