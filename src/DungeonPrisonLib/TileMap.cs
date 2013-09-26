using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
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

        public TileMap()
        {
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
        public void SetTile(int x, int y, Tile tile)
        {
            if (!InBounds(x, y))
            {
                Debug.Fail("Error occured call a dev", "Index was outside the range");
                return;
            }

            Map[x, y] =  tile;
        }

        public void FeelWith(Tile tile)
        {
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    Map[i, j] = tile;
                }
            }
        }

        public bool IsSolid(Point p)
        {
            return IsSolid(p.X, p.Y);
        }
        public bool IsSolid(int x, int y)
        {
            if (!InBounds(x, y))
                return false;

            var tile = GetTile(x, y).Type;

            return tile == TileType.Wall;
        }

        public bool InBounds(Point p)
        {
            return InBounds(p.X, p.Y);
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
                                Map[i, j] = new Tile { Type = TileType.Floor };
                                break;
                        }
                    }
                }
            }
            catch(IOException e)
            {
                Debug.Fail("File could not be loaded: \n" + e.ToString());
                return;
            }
        }

        public Point GetRandomEmptyPlace()
        {
            Point p;
            int tries = 200;
            do
            {
                p = new Point(RandomTool.NextInt(Width), RandomTool.NextInt(Height));
                if (tries-- < 0)
                {
                    Debug.Fail("NO EMPTY PLACE FOR YOU MOTHER FUCKER");
                    return new Point(-1,-1);
                }
            } while (IsSolid(p.X, p.Y));

            return p;
        }

        public void SaveMap(string path)
        {
            FileStream fs = new FileStream(path, FileMode.OpenOrCreate);
            // Create the writer for data.
            BinaryWriter w = new BinaryWriter(fs);
            // Write data to Test.data.

            w.Write(Width);
            w.Write(Height);

            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    w.Write((int)Map[i, j].Type);
                }
            }
            w.Close();
            fs.Close();
        }

        public void LoadMap(string path)
        {
            var fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            BinaryReader r = new BinaryReader(fs);

            Width = r.ReadInt32();
            Height = r.ReadInt32();

            Map = new Tile[Width, Height];

            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    Map[i, j].Type = (TileType)r.ReadInt32();// check 64 bit
                }
            }
            r.Close();
            fs.Close(); 
        }

        public unsafe void PrintMapToFile(string file)
        {
            Bitmap b = new Bitmap(Width, Height);

            var data = b.LockBits(new Rectangle(0, 0, Width, Height), ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);

            byte* ptr = (byte*)data.Scan0.ToPointer();
            int t = 0;

            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    Color color = GetTileColor(Map[i,j]);
                    
                    ptr[t++] = color.R;
                    ptr[t++] = color.G;
                    ptr[t++] = color.B;
                }
            }

            b.UnlockBits(data);

            b.Save(file);
        }

        private Color GetTileColor(Tile tile)
        {
            switch (tile.Type)
            {
                case TileType.Nothing:
                    return Color.Red;
                case TileType.Wall:
                    return Color.Black;
                case TileType.Floor:
                    return Color.Wheat;
                default:
                    break;
            }

            return Color.White;
        }
    }
}
