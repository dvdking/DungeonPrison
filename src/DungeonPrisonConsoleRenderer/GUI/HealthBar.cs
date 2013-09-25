using DungeonPrisonLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DungeonPrisonConsoleRenderer.GUI
{
    class HealthBar:GUIElement
    {
        public override void Draw()
        {
            GuiManager.Renderer.DrawString(Location.X, Location.Y, GameManager.Instance.Player.Health + "/" + GameManager.Instance.Player.MaxHealth);
        }
    }
}
