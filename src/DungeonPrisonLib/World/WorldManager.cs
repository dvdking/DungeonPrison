using DungeonPrisonLib.WorldGenerator;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace DungeonPrisonLib.World
{

    /// <summary>
    /// simulates global events
    /// </summary>
    public class WorldManager
    {
        public int SizeX, SizeY, SizeZ;

        private WorldGen _worldGenerator;

        private WorldChunk[,,] _world;

        public WorldManager(int sizeX, int sizeY, int sizeZ)
        {
            SizeX = sizeX;
            SizeY = sizeY;
            SizeZ = sizeZ;

            _worldGenerator = new WorldGen();
            if(GameManager.Instance.GroupsManager.IsEmpty)
                _worldGenerator.GenerateGroups();

            _world = new WorldChunk[SizeX, SizeY, SizeZ];
        }

        public WorldChunk GetChunk(int x, int y, int z)
        {
            if (_world[x, y, z] == null)
                _world[x, y, z] = new WorldChunk(x, y, z);
            if (!_world[x, y, z].IsLoaded)
            {
                if (_world[x, y, z].TileMapExists())
                {
                    _world[x, y, z].LoadTileMap();
                }
                else
                {
                    _world[x, y, z] = _worldGenerator.GenerateBlock(x, y, z, _world);
                }
            }
            return _world[x, y, z];
        }



    }
}
