using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DungeonPrisonLib;
using System.Diagnostics;
using DungeonPrisonLib.Actors;
using System.IO;
using System.Xml;

namespace DungeonPrisonConsoleRenderer
{
    public class ConsoleRenderer:IRenderer
    {
        public struct GraphicsInfo
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

        public int LogPositionX;
        public int LogPositionY;

        public int LogWidth;
        public int LogHeight;

        public int HealthBarPositionX;
        public int HealthBarPositionY;
        
        public ConsoleRenderer(int screenWidth, int screenHeight)
        {
            _screenWidth = screenWidth;
            _screenHeight = screenHeight;

            _buffer = new GraphicsInfo[_screenWidth, _screenHeight];
            _oldBuffer = new GraphicsInfo[_screenWidth, _screenHeight];

            Console.CursorVisible = false;

            Debug.Assert(_screenWidth >= Settings.PlayerView.Width, "Too small screen size");
            Debug.Assert(_screenHeight >= Settings.PlayerView.Height, "Too small screen size");

            Debug.Assert(_buffer != null, "NULL");
            Debug.Assert(_oldBuffer != null, "NULL");
        }

        public bool ReadGraphicsInfoData(string path)
        {
            var file = new XmlDocument();
            try
            {
                file.Load(path);
            }
            catch(Exception e)
            {
                Console.WriteLine("could not open file: " + path);
                Console.WriteLine(e.ToString());
                return false;
            }


            charMap = new Dictionary<string, GraphicsInfo>();
            tileGraphicsMap = new Dictionary<TileType, GraphicsInfo>();

            XmlNode root = file.SelectSingleNode("GraphicsInfo");

            XmlNode main = root.SelectSingleNode("Actors");
            XmlNodeList actors = main.SelectNodes("Actor");
            foreach (XmlNode node in actors)
            {
                string name = node.Attributes["name"].Value;
                string strColor = node.Attributes["color"].Value;
                char ch = node.Attributes["ch"].Value.ToCharArray()[0];

                charMap[name] = new GraphicsInfo 
                {
                    Char = ch,
                    Color = GetColorFromString(strColor) 
                };
            }

            main = root.SelectSingleNode("Tiles");
            XmlNodeList tiles = main.SelectNodes("Tile");

            foreach (XmlNode node in tiles)
            {
                string name = node.Attributes["name"].Value;
                string strColor = node.Attributes["color"].Value;
                char ch = node.Attributes["ch"].Value.ToCharArray()[0];


                tileGraphicsMap[GetTileFromString(name)] = new GraphicsInfo
                {
                    Char = ch,
                    Color = GetColorFromString(strColor)
                };
            }

            return true;
        }

        private ConsoleColor GetColorFromString(string str)
        {
            switch (str.ToLower())
            {
                case "white":
                    return ConsoleColor.White;
                case "darkgreen":
                    return ConsoleColor.DarkGreen;
                default:
                    return ConsoleColor.Red;//it's set to red so it will be more noticable if there are errors
            }
        }

        private TileType GetTileFromString(string str)
        {
            switch (str.ToLower())
            {
                case "wall":
                    return TileType.Wall;
                case "floor":
                    return TileType.Floor;
                default:
                    return TileType.Nothing;
            }
        }

        public void Draw(Player player, List<Actor> actors, TileMap tileMap)
        {
            CleanBuffer();
            DrawTileMap(player, tileMap);
            DrawActors(player, actors);
        }

        private void DrawActors(Player player, List<Actor> actors)
        {
            int halfWidth = Settings.PlayerView.Width / 2;
            int halfHeight = Settings.PlayerView.Height / 2;

            foreach (var item in actors.Where(p => p.IsAlive).OrderByDescending(p => p.Depth))
            {
                int relativeX = player.X - item.X;
                int relativeY = player.Y - item.Y;

                int screenPosX = -relativeX + halfWidth;
                int screenPosY = -relativeY + halfHeight;

                if (!InView(screenPosX, screenPosY))
                    continue;
                if (!GameManager.Instance.LOS.IsVisible(screenPosX, screenPosY))
                    continue;


                DrawGraphicsInfoToBuffer(screenPosX, screenPosY, charMap[item.Name]);
            }
        }

        private void DrawTileMap(Player player, TileMap tileMap)
        {
            int startX = player.X - Settings.PlayerView.Width / 2;
            int startY = player.Y - Settings.PlayerView.Height / 2;

            int endX = startX + tileMap.Width + Settings.PlayerView.Width / 2;
            int endY = startY + tileMap.Height + Settings.PlayerView.Width / 2;

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
                    if (!GameManager.Instance.LOS.IsVisible(screenPosX, screenPosY))
                        continue;

                    DrawGraphicsInfoToBuffer(screenPosX, screenPosY, tileGraphicsMap[tileMap.GetTile(i, j).Type]);
                }
            }
        }

        public void DrawString(int x, int y, string str, ConsoleColor color = ConsoleColor.White)
        {
            if (y >= _screenHeight)
                return;

            for (int i = 0; i < str.Length; i++)
            {
                if (x + i >= _screenWidth)
                    break;
                _buffer[x + i, y] = new GraphicsInfo() { Char = str[i], Color = color };
            }
        }

        public void DrawGraphicsInfoToBuffer(int x, int y, GraphicsInfo graphicsInfo)
        {
            _buffer[x, y] = graphicsInfo;
        }

        private void DrawGraphicsInfo(int x, int y, GraphicsInfo graphicsInfo)
        {
            Console.SetCursorPosition(x, y);
            Console.ForegroundColor =  graphicsInfo.Color;
            Console.Write(graphicsInfo.Char);
        }

        public void CleanBuffer()
        {
            for (int i = 0; i < _screenWidth; i++)
            {
                for (int j = 0; j < _screenHeight; j++)
                {
                    _buffer[i, j] = new GraphicsInfo { Char = ' ', Color = ConsoleColor.White};
                }
            }
        }

        public void DrawBuffer()
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
            if (x >= Settings.PlayerView.Width)
                return false;
            if (x < 0)
                return false;
            if (y >= Settings.PlayerView.Height)
                return false;
            if (y < 0)
                return false;
            return true;
        }
    }
}
