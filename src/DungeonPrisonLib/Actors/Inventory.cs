using DungeonPrisonLib.Actors.Items;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace DungeonPrisonLib.Actors
{
    public class Inventory
    {
        List<Item> _items;

        public Inventory()
        {
            _items = new List<Item>();
        }

        public void AddItem(Item item)
        {
            _items.Add(item);
        }

        public Item GetItemWithIndex(int i)
        {
            if (i >= _items.Count)
            {
                Debug.Fail("index outside the range");
                return null;
            }
            return _items[i];
        }

        public List<Item> GetItems()
        {
            return _items.ToList();
        }
    }
}
