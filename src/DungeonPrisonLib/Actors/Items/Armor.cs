using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DungeonPrisonLib.Actors.Items
{
    public class Armor:Item
    {
        public int ArmorAmount {get; private set;}
        public ArmorPlace Place {get; private set;}
        public ArmorType Type {get; private set;}

        public Armor(int amount, ArmorPlace place, ArmorType type)
            :base()
        {
            ArmorAmount = amount;
            Place = place;
            Type = type;
        }

        public override void Update(float delta, TileMap tileMap)
        {

        }
    }
}
