using System.Drawing;
using System;

namespace DungeonPrisonLib
{
    public static class RandomTool
    {
        public static Random random = new Random((int)DateTime.Now.Ticks);

        public static int NextInt(int min, int max)
        {
            return random.Next(min, max);
        }

        public static int NextInt(int max)
        {
            if (max >= 0)
                return random.Next(max);
            throw new ArgumentOutOfRangeException("max", "must be greater or equal than 0");
        }

        public static int NextInt()
        {
            return random.Next();
        }


        public static char NextChoice(params char[] objects)
        {
            return objects[NextInt(objects.Length)];
        }

        public static string NextChoice(params string[] objects)
        {
            return objects[NextInt(objects.Length)];
        }

        public static int NextChoice(params int[] objects)
        {
            return objects[NextInt(objects.Length)];
        }

        public static byte NextByte()
        {
            return (byte)random.Next();
        }
        public static byte NextByte(byte max)
        {
            return (byte)random.Next(max);
        }
        public static byte NextByte(byte min, byte max)
        {
            return (byte)random.Next(min, max);
        }

        public static double NextDouble()
        {
            return random.NextDouble();
        }

        public static float NextSingle()
        {
            return (float)random.NextDouble();
        }

        public static float NextSingle(float min, float max)
        {
            return (max - min) * NextSingle() + min;
        }

        public static bool NextBool(float ratio)
        {
            return random.NextDouble() <= ratio;
        }

        public static bool NextBool(double ratio)
        {
            return random.NextDouble() <= ratio;
        }

        public static bool NextBool()
        {
            return random.NextDouble() <= 0.5;
        }

        public static sbyte NextSign()
        {
            return random.NextDouble() <= 0.5 ? (sbyte)1 : (sbyte)-1;
        }
    }
}