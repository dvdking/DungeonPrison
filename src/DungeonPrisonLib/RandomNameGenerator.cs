using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DungeonPrisonLib
{
    public static class RandomNameGenerator
    {
        private static List<string> _words;

        static public void LoadDB(string path)
        {
            _words = new List<string>();
            _words = File.ReadAllLines(path).ToList();
        }

        static public string NextName()
        {
             return _words[RandomTool.NextInt(_words.Count - 1)] + 
                    _words[RandomTool.NextInt(_words.Count - 1)];
        }
    }
}
