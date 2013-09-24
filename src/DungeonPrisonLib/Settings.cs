using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace DungeonPrisonLib
{
    public static class Settings
    {
        static public Size PlayerView{get; private set;}

        static Settings()
        {
            //SET ALL SETTINGS HERE FOR NOW
            //TODO: LOAD FROM FILE
            PlayerView = new Size(32, 16);
        }
    }
}
