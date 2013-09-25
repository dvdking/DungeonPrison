using DungeonPrisonLib;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace DungeonPrisonConsoleRenderer.GUI
{
    class LogRenderer:GUIElement
    {
        public override void Update()
        {
            base.Update();
        }

        public override void Draw()
        {
            var messages = GameManager.Instance.Log.GetMessages(Math.Max(0, GameManager.Instance.Log.MessagesCount - Size.Height), Size.Height);
            if (messages == null)
                return;
            for (int i = 0; i < messages.Length; i++)
            {
                GuiManager.Renderer.DrawString(Location.X, Location.Y + i, messages[i]);
            }
        }
    }
}
