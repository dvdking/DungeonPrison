using DungeonPrisonLib.Actors;
using DungeonPrisonLib.WorldGenerator;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace DungeonPrisonLib.World
{
    public class WorldChunk
    {
        public int X, Y, Z;

        public Point3 Position
        {
            get
            {
                return new Point3(X,Y,Z);
            }
        }

        public TlleMapType Type;

        public List<Actor> Actors{get; private set;}
        public TileMap TileMap{get; set;}

        private string DirPath { get { return Settings.SaveDirectory + "\\" + "m" + X.ToString() + Y.ToString() + Z.ToString(); } }

        public WorldChunk(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;

            Actors = new List<Actor>();
        }

        public bool IsLoaded { get { return TileMap != null && Actors != null; } }

        public void AddActor(Actor actor)
        {
            actor.LastGlobalMapLocation = Position;
            Actors.Add(actor);
        }

        public void RemoveActor(Actor actor)
        {
            actor.LastGlobalMapLocation = new Point3(-1, -1, -1);
            Actors.Remove(actor);
        }

        public bool TileMapExists()
        {
            return File.Exists(DirPath);
        }

        public void UnloadMap()
        {
            TileMap = null;
        }

        public TileMap LoadTileMap()
        {
            TileMap = new TileMap();
            if (TileMapExists())
                TileMap.LoadMap(DirPath);
            else
                Debug.Fail("tileMap file does not exsist: " + DirPath);

            return TileMap;
        }
    }
}
