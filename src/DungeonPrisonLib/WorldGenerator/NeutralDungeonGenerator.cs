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
            NeutralDungeonGenerator _gen;


            public Miner(NeutralDungeonGenerator gen, TileMap tileMap, bool createMiners = false, Point[] choises = null)
            {
                X = RandomTool.NextInt(tileMap.Width);
                Y = RandomTool.NextInt(tileMap.Height);
                _gen = gen;
                _tileMap = tileMap;
                if (choises == null)
                {
                    _choises = new Point[]{new Point(-1, 0),
                                            new Point(1, 0),
                                            new Point(0, 1),
                                            new Point(0, -1)};
                }
                else
                {
                    _choises = choises;
                }
                _createMiners = createMiners;
            }

            public void Dig()
            {
                for (int i = 0; i < size; i++)
                {
                    for (int j = 0; j < size; j++)
                    {
                        if (_tileMap.InBounds(X + i, Y + j))
                        {
                            if (_tileMap.GetTile(X, Y).Type == TileType.Wall)
                                _gen.UsedTiles++;
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
                    if (RandomTool.NextBool(0.001f))
                    {
                        if(_choises.Length == 0)
                            return null;

                        var ch = new Point[_choises.Length - 1];                        

                        for (int i = 0; i < ch.Length; i++)
			            {
                            ch[i] = _choises[RandomTool.NextInt(_choises.Length)];
			            }

                        return new Miner(_gen, _tileMap, RandomTool.NextBool(0.005f), _choises) { X = X, Y = Y};
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

        public int UsedTiles;

        public const int NeedToMinUse = 50;

        public NeutralDungeonGenerator()
        {
        }

        public TileMap Generate(int width, int height)
        {
            TileMap tileMap = new TileMap(width, height);

            tileMap.FeelWith(new Tile{ Type = TileType.Wall });

            List<Miner> miners = new List<Miner>();
            for (int i = 0; i < 1; i++)
            {
                miners.Add(new Miner(this, tileMap, true));
            }
            



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
                if(miners.Count == 0)
                {
                    if (UsedTiles < width * height * NeedToMinUse / 100.0f)
                    {
                        miners.Add(new Miner(this, tileMap, false));
                    }
                }
            }

            return tileMap;
        }

    }
}
