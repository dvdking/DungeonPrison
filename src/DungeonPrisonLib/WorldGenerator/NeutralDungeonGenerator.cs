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
            bool _createMiners;
            Point[] _choises;


            public Miner(TileMap tileMap, bool createMiners = false, Point[] choises = null)
            {
                X = RandomTool.NextInt(tileMap.Width);
                Y = RandomTool.NextInt(tileMap.Height);

                _tileMap = tileMap;
                if (choises == null)
                {
                    _choises = new Point[]{new Point(-1, 0),
                                            new Point(1, 0),
                                            new Point(0, 1),
                                            new Point(0, -1)};
                }
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

            public Miner Update()
            {
                Point nextDirection = GetNextDirection();

                X += nextDirection.X;
                Y += nextDirection.Y;

                Dig();

                if (_createMiners)
                {
                    if (RandomTool.NextBool(0.01f))
                    {
                        if(_choises.Length == 0)
                            return null;

                        var ch = new Point[_choises.Length - 1];                        

                        for (int i = 0; i < ch.Length; i++)
			            {
                            ch[i] = _choises[RandomTool.NextInt(_choises.Length)];
			            }

                        return new Miner(_tileMap, RandomTool.NextBool(0.05f), _choises) { X = X, Y = Y};
                    }
                }
                return null;
            }

            Point GetNextDirection()
            {
                return RandomTool.NextChoice<Point>(_choises);
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
            miners.Add(new Miner(tileMap, true) );
            miners.Add(new Miner(tileMap, true));
            miners.Add(new Miner(tileMap, true) { size = 2});


            Queue<Miner> newMiners = new Queue<Miner>(4);
            while (miners.Count > 0)
            {
                foreach (var miner in miners)
                {
                    var newMiner = miner.Update();
                    if(newMiner != null)
                        newMiners.Enqueue(newMiner);
                }

                while (newMiners.Count != 0)
                {
                    miners.Add(newMiners.Dequeue());
                }

                miners.RemoveAll(p => !p.IsAlive(tileMap));
            }

            return tileMap;
        }

    }
}
