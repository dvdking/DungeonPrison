using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DungeonPrisonLib;
using System.Diagnostics;
using DungeonPrisonLib.Actors;

namespace DungeonPrisonConsoleRenderer
{
    class ConsoleRenderer:IRenderer
    {
        private struct GraphicsInfo
        {
            public char Char;
            public ConsoleColor Color;

            public static bool operator ==(GraphicsInfo x, GraphicsInfo y)
            {
                return x.Char == y.Char &&
                       x.Color == y.Color;
            }

            public static bool operator !=(GraphicsInfo x, GraphicsInfo y)
            {
                return x.Char != y.Char ||
                       x.Color != y.Color;
            }
        }
        Dictionary<string, GraphicsInfo> charMap;
        Dictionary<TileType, GraphicsInfo> tileGraphicsMap;



        private GraphicsInfo[,] _buffer;
        private GraphicsInfo[,] _oldBuffer;


        private int _screenWidth, _screenHeight;
        private int _viewPositionX, _viewPositionY;

        public int LogPositionX;
        public int LogPositionY;

        public int LogWidth;
        public int LogHeight;

        
        public ConsoleRenderer(int screenWidth, int screenHeight)
        {
            _screenWidth = screenWidth;
            _screenHeight = screenHeight;

            _buffer = new GraphicsInfo[_screenWidth, _screenHeight];
            _oldBuffer = new GraphicsInfo[_screenWidth, _screenHeight];

            InitCharMap();
            InitTileGraphicsMap();


            Console.CursorVisible = false;

            Debug.Assert(_screenWidth >= Settings.PlayerView.Width, "Too small screen size");
            Debug.Assert(_screenHeight >= Settings.PlayerView.Height, "Too small screen size");

            Debug.Assert(_buffer != null, "NULL");
            Debug.Assert(_oldBuffer != null, "NULL");
            Debug.Assert(charMap != null, "NULL");
            Debug.Assert(tileGraphicsMap != null, "NULL");
        }

        private void InitCharMap()
        {
            charMap = new Dictionary<string, GraphicsInfo>();
            charMap["Player"] = new GraphicsInfo()
            {
                Char = '@',
                Color = ConsoleColor.White
            };
            charMap["SomeGuy"] = new GraphicsInfo()
            {
                Char = '@',
                Color = ConsoleColor.DarkYellow
            };
        }

        private void InitTileGraphicsMap()
        {
            tileGraphicsMap = new Dictionary<TileType, GraphicsInfo>();
            tileGraphicsMap[TileType.Wall] = new GraphicsInfo
            {
                Char = '#',
                Color = ConsoleColor.White
            };
            tileGraphicsMap[TileType.Ground] = new GraphicsInfo
            {
                Char = '.',
                Color = ConsoleColor.Gray
            };

            tileGraphicsMap[TileType.Nothing] = new GraphicsInfo
            {
                Char = ' ',
                Color = ConsoleColor.White
            };
        }

        public void Draw(Player player, List<Actor> actors, TileMap tileMap)
        {
            CleanBuffer();

            DrawTileMap(player, tileMap);
            DrawActors(player, actors);
            DrawPlayer(player);
            DrawLog();

            DrawBuffer();
        }

        private void DrawPlayer(Player player)
        {
            int halfWidth = Settings.PlayerView.Width / 2;
            int halfHeight = Settings.PlayerView.Height / 2;

            DrawGraphicsInfoToBuffer(_viewPositionX + halfWidth, _viewPositionY + halfHeight, charMap[player.Name]);
        }

        private void DrawActors(Player player, List<Actor> actors)
        {
            int halfWidth = Settings.PlayerView.Width / 2;
            int halfHeight = Settings.PlayerView.Height / 2;

            foreach (var item in actors.Where(p => p.IsAlive))
            {
                int relativeX = player.X - item.X;
                int relativeY = player.Y - item.Y;

                int screenPosX = -relativeX + halfWidth;
                int screenPosY = -relativeY + halfHeight;

                if (!InView(screenPosX, screenPosY))
                    continue;


                DrawGraphicsInfoToBuffer(screenPosX, screenPosY, charMap[item.Name]);
            }
        }

        private void DrawTileMap(Player player, TileMap tileMap)
        {
            int startX = player.X - Settings.PlayerView.Width / 2;
            int startY = player.Y - Settings.PlayerView.Height / 2;

            int endX = startX + tileMap.Width;
            int endY = startY + tileMap.Height;

            int halfWidth = Settings.PlayerView.Width / 2;
            int halfHeight = Settings.PlayerView.Height / 2;

            for (int i = startX; i < endX; i++)
            {
                for (int j = startY; j < endY; j++)
                {
                    int relativeX = player.X - i;
                    int relativeY = player.Y - j;

                    int screenPosX = -relativeX + halfWidth;
                    int screenPosY = -relativeY + halfHeight;

                    if (!InView(screenPosX, screenPosY))
                        continue;
                    if (!tileMap.InBounds(i, j))
                        continue;

                    DrawGraphicsInfoToBuffer(screenPosX, screenPosY, tileGraphicsMap[tileMap.GetTile(i, j).Type]);
                }
            }
        }

        private void DrawLog()
        {
            var messages = GameManager.Instance.Log.GetMessages(Math.Max(0, GameManager.Instance.Log.MessagesCount - LogHeight), LogHeight);
            if (messages == null)
                return;
            for (int i = 0; i < messages.Length; i++)
            {
                DrawString(LogPositionX, LogPositionY + i, messages[i]);
            }
        }

        private void DrawString(int x, int y, string str)
        {
            if (y >= _screenHeight)
                return;

            for (int i = 0; i < str.Length; i++)
            {
                if (x + i >= _screenWidth)
                    break;
                _buffer[x + i, y] = new GraphicsInfo() { Char = str[i], Color = ConsoleColor.White };
            }
        }

        private void DrawGraphicsInfoToBuffer(int x, int y, GraphicsInfo graphicsInfo)
        {
            _buffer[x, y] = graphicsInfo;
        }

        private void DrawGraphicsInfo(int x, int y, GraphicsInfo graphicsInfo)
        {
            Console.SetCursorPosition(x, y);
            Console.ForegroundColor = graphicsInfo.Color;
            Console.Write(graphicsInfo.Char);
        }

        private void CleanBuffer()
        {
            for (int i = 0; i < _screenWidth; i++)
            {
                for (int j = 0; j < _screenHeight; j++)
                {
                    _buffer[i, j] = new GraphicsInfo { Char = ' ', Color = ConsoleColor.White};
                }
            }
        }

        private void DrawBuffer()
        {
            for (int i = 0; i < _screenWidth; i++)
            {
                for (int j = 0; j < _screenHeight; j++)
                {
                    if (_buffer[i, j] != _oldBuffer[i, j])
                    {
                        DrawGraphicsInfo(i, j, _buffer[i, j]);
                        _oldBuffer[i, j] = _buffer[i, j];
                    }
                }
            }
        }

        private bool InView(int x, int y)
        {
            if (x > Settings.PlayerView.Width)
                return false;
            if (x < 0)
                return false;
            if (y > Settings.PlayerView.Height)
                return false;
            if (y < 0)
                return false;
            return true;
        }
    }
}
