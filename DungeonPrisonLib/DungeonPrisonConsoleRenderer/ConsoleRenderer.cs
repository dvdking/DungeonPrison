using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DungeonPrisonLib;

namespace DungeonPrisonConsoleRenderer
{
    class ConsoleRenderer:IRenderer
    {
        private struct GraphicsInfo
        {
            public char Char;
            public ConsoleColor Color;
        }
        Dictionary<string, GraphicsInfo> charMap;

        private char[,] _buffer;
        private char[,] _oldBuffer;


        private int _screenWidth, _screenHeight;

        public ConsoleRenderer(int screenWidth, int screenHeight)
        {
            _screenWidth = screenWidth;
            _screenHeight = screenHeight;


            _buffer = new char[_screenWidth, _screenHeight];

            charMap = new Dictionary<string, GraphicsInfo>();
            charMap["Player"] = new GraphicsInfo() 
            {
                Char = '@', Color = ConsoleColor.White 
            };
            charMap["SomeGuy"] = new GraphicsInfo()
            {
                Char = '@',
                Color = ConsoleColor.DarkYellow
            };
            Console.CursorVisible = false;
        }

        public void Draw(Player player, List<Actor> actors)
        {
            int halfWidth = _screenWidth / 2;
            int halfHeight = _screenHeight / 2;
            Console.SetCursorPosition(halfWidth, halfHeight);
            DrawGraphicsInfo(charMap[player.Name]);

            foreach (var item in actors)
            {
                int relativeX = player.X - item.X;
                int relativeY = player.Y - item.Y;

                if (relativeX > player.X + halfWidth)
                    continue;
                if (relativeX <= player.X - halfWidth)
                    continue;
                if (relativeY > player.Y + halfHeight)
                    continue;
                if (relativeY <= player.Y - halfHeight)
                    continue;

                Console.SetCursorPosition(player.X - relativeX + halfWidth, player.Y - relativeY + halfHeight);
                DrawGraphicsInfo(charMap[item.Name]);
            }
        }

        private void DrawGraphicsInfo(GraphicsInfo graphicsInfo)
        {
            Console.ForegroundColor = graphicsInfo.Color;
            Console.Write(graphicsInfo.Char);
        }
    }
}
