using DungeonPrisonLib.Actors.Behaviours;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DungeonPrisonLib.Actors.Items;

namespace DungeonPrisonLib.Actors
{
    public class Player:Creature
    {
        TileMap _memoryMap;

        public Player()
            :base()
        {
            ItemWieldedEvent += Player_ItemWieldedEvent;
            ItemPickedUpEvent += Player_ItemPickedUpEvent;
            ArmorWorn += Player_ArmorWorn;
            Depth = -2;
            _memoryMap = new TileMap();
        }

        public void UpdateMemory(LOS los, TileMap tileMap)
        {
            for (int i = 0; i < Settings.PlayerView.Width; i++)
            {
                for (int j = 0; j < Settings.PlayerView.Height; j++)
                {
                    if (los.IsVisible(i, j))
                    {
                        var posX = i + X - Settings.PlayerView.Width/2;
                        var posY = j + Y - Settings.PlayerView.Height/2;

                        if(_memoryMap.InBounds(posX, posY))
                        {
                            _memoryMap.SetTile(posX, posY, tileMap.GetTile(posX,posY));
                        }
                    }
                }
            }
        }

        public Tile GetMemoryTile(int x, int y)
        {
            return _memoryMap.GetTile(x, y);
        }

        public void OpenTile(int x, int y, Tile tile)
        {
            _memoryMap.SetTile(x, y, tile);
        }


        /// <summary>
        /// creates new clean memory depending on size of tile map
        /// </summary>
        /// <param name="tileMap"></param>
        public void CreateMemory(TileMap tileMap)
        {
            _memoryMap = new TileMap(tileMap.Width, tileMap.Height);

        }

        public void LoadMemoryMap(string file)
        {
            _memoryMap.LoadMap(file);
        }
        public void SaveMemoryMap(string file)
        {
            _memoryMap.SaveMap(file);
        }

        void Player_ArmorWorn(Conclusion conclusion, Armor armor, Armor newArmor)
        {
            switch (conclusion)
            {
                case Conclusion.Succes:
                    if (armor != null)
                    {
                        GameManager.Instance.Log.AddMessage("You remove your " + armor.GameName);
                    }
                    if (newArmor != null)
                    {
                        GameManager.Instance.Log.AddMessage("You putting on your " + newArmor.GameName);
                    }
                    break;
                case Conclusion.Fail:
                    GameManager.Instance.Log.AddMessage("Cannot wear that");
                    break;
                default:
                    break;
            }
            
        }

        void Player_ItemPickedUpEvent(Items.Item item)
        {
            if (item == null)
            {
                GameManager.Instance.Log.AddMessage("There is nothing to pick up");
            }
            else
            {
                GameManager.Instance.Log.AddMessage("You pick up " + item.GameName);
            }
        }

        void Player_ItemWieldedEvent(Creature.Conclusion conclusion, Items.Item oldItem, Items.Item newItem)
        {
            switch (conclusion)
            {
                case Conclusion.Succes:
                    if (oldItem != null)
                    {
                        GameManager.Instance.Log.AddMessage("You take off your " + oldItem.GameName);
                    }
                    GameManager.Instance.Log.AddMessage("You are now wielding " + newItem.GameName);
                    break;
                case Conclusion.Fail:
                    GameManager.Instance.Log.AddMessage("Can not wield that");
                    break;
                case Conclusion.AlreadyWielding:
                    GameManager.Instance.Log.AddMessage("You are alredy wielding that");
                    break;
                default:
                    break;
            }
        }

        public override void Update(float delta, TileMap tileMap)
        {
            base.Update(delta, tileMap);
        }



    }
}
