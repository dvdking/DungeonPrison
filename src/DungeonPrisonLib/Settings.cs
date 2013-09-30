using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace DungeonPrisonLib
{
    public static class Settings
    {
        static public Size PlayerView{get; private set;}

        public const string SaveFolder ="Saves";
        public const string CurrentPlayer = "TestPlayer";

        static public string SaveDirectory { get { return SaveFolder + "\\" + CurrentPlayer; } }

        public const int WorldSizeX = 10, WorldSizeY = 10, WorldSizeZ = 10;
        public readonly static  Point TileMapSize = new Point(128,128);

        static Settings()
        {
            //SET ALL SETTINGS HERE FOR NOW
            //TODO: LOAD FROM FILE
            PlayerView = new Size(32, 16);
        }
    }
}
