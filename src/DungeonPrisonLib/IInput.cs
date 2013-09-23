using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DungeonPrisonLib
{
    public interface IInput
    {
        void Update();
        
        List<InputKey> GetPressedKeys();
        List<InputKey> GetReleasedKeys();
    }
}
