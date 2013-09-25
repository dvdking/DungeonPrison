using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;

namespace DungeonPrisonConsoleRenderer.GUI
{
    class GUIElement
    {
        public Point Location;
        public Size Size;

        public bool InteruptInput{get; protected set;}

        protected GUIManager GuiManager { get; private set; }
        public void SetGuiManager(GUIManager guiManager)
        {
            Debug.Assert(GuiManager == null, "guiManager is already set");
            GuiManager = guiManager;
        }

        public virtual void Update() { }
        public virtual void Draw() { }
    }
}
