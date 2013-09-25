using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DungeonPrisonConsoleRenderer.GUI
{
    class GUIManager
    {
        List<GUIElement> _elements = new List<GUIElement>();

        public bool IsInputInterupted { get; private set; }

        public ConsoleRenderer Renderer { get; private set; }

        public GUIManager(ConsoleRenderer renderer)
        {
            Renderer = renderer;
        }

        public void AddElement(GUIElement guiElement)
        {
            guiElement.SetGuiManager(this);
            _elements.Add(guiElement);
        }

        public void Update()
        {
            foreach (var item in _elements)
            {
                item.Update();
            }
        }

        public void Draw()
        {
            foreach (var item in _elements)
            {
                item.Draw();
            }
        }
    }
}
