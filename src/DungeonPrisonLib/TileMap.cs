using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace DungeonPrisonLib
{
    public class TileMap
    {
        public Tile[,] Map { get; private set; }

        public int Width { get; private set; }
        public int Height { get; private set; }

        public TileMap(int sizeX, int sizeY)
        {
            Width = sizeX;
            Height = sizeY;

            Map = new Tile[Width, Height];
        }

        public Tile GetTile(int x, int y)
        {
            if (!InBounds(x, y))
            {
                Debug.Fail("Error occured call a dev", "Index was outside the range");
                return new Tile { Type = TileType.Nothing};
            }

            return Map[x, y];
        }

        public bool InBounds(int x, int y)
        {
            return x >= 0 && y >= 0 && x < Width && y < Height;
        }

        public void ReadSimpleMap(string path)
        {
            try
            {
                string[] lines = File.ReadAllLines(path);

            Map = new Tile[lines.Length, lines[0].Length];

            Width = lines.Length;
            Height = lines[0].Length;

            for (int i = 0; i < lines.Length; i++)
            {
                for (int j = 0; j < lines[i].Length; j++)
                {
                    switch (lines[i][j])
                    {
                        case '#':
                            Map[i, j] = new Tile { Type = TileType.Wall };
                            break;
                        default:
                            Map[i, j] = new Tile { Type = TileType.Ground };
                            break;
                    }
                }
            }
            }
            catch(IOException e)
            {
                return;
            }
        }

    }
}
