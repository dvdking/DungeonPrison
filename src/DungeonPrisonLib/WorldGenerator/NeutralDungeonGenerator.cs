using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace DungeonPrisonLib.WorldGenerator
{
    class NeutralDungeonGenerator
    {
        private class Miner
        {
            public int X, Y;
            public int size = 1;
            TileMap _tileMap;
            public Miner(TileMap tileMap)
            {
                X = RandomTool.NextInt(tileMap.Width);
                Y = RandomTool.NextInt(tileMap.Height);

                _tileMap = tileMap;
            }

            public void Dig()
            {
                for (int i = 0; i < size; i++)
                {
                    for (int j = 0; j < size; j++)
                    {
                        if (_tileMap.InBounds(X + i, Y + j))
                        {
                            _tileMap.SetTile(X + i, Y + j, new Tile { Type = TileType.Floor });
                        }
                    }
                }
            }

            public void Update()
            {
                Point nextDirection = GetNextDirection();

                X += nextDirection.X;
                Y += nextDirection.Y;

                Dig();
            }

            Point GetNextDirection()
            {
                return RandomTool.NextChoice<Point>(new Point(-1, 0),
                                                    new Point(1, 0),
                                                    new Point(0, 1),
                                                    new Point(0, -1));
            }


            public bool IsAlive(TileMap tileMap) 
            {
                return tileMap.InBounds(X, Y); 
            }
        }

        public NeutralDungeonGenerator()
        {
        }

        public TileMap Generate(int width, int height)
        {
            TileMap tileMap = new TileMap(width, height);

            tileMap.FeelWith(new Tile{ Type = TileType.Wall });

            List<Miner> miners = new List<Miner>();
            miners.Add(new Miner(tileMap));
            miners.Add(new Miner(tileMap));
            miners.Add(new Miner(tileMap) { size = 2});

            while (miners.Count > 0)
            {
                foreach (var miner in miners)
                {
                    miner.Update();
                }
                miners.RemoveAll(p => !p.IsAlive(tileMap));
            }

            return tileMap;
        }

    }
}
