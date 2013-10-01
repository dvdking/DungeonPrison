using DungeonPrisonLib.Actors;
using DungeonPrisonLib.Actors.Behaviours;
using DungeonPrisonLib.Actors.CreaturesGroups;
using DungeonPrisonLib.Actors.Items;
using DungeonPrisonLib.World;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace DungeonPrisonLib.WorldGenerator
{
    public class WorldGen
    {
        private TileMapGenerator _generator;

        public WorldGen()
        {
            _generator = new TileMapGenerator();
        }

        public WorldChunk GenerateBlock(int x, int y, int z, WorldChunk[,,] chunks)
        {
            var chunk = new WorldChunk(x, y, z);

            chunk.TileMap = _generator.GenerateTileMap(Settings.TileMapSize.X, Settings.TileMapSize.Y, TlleMapType.NeutralDungeon);

            SettleCreatures(x, y, z, chunk, chunks);
            SettleItems(chunk);

            return chunk;
        }

        public void GenerateGroups()
        {
            for (int i = 0; i < Settings.GroupsCount; i++)
            {
                GameManager.Instance.GroupsManager.AddGroup(RandomNameGenerator.NextName());
            }
        }

        private void SettleCreatures(int x, int y, int z, WorldChunk chunk, WorldChunk[, ,] chunks)
        {    
            int groupPopulation = RandomTool.NextInt(1, 10);


            
            List<Creature> creatures = new List<Creature>();
            for (int j = 0; j < Settings.GroupsCount; j++)
            {
                Point position = chunk.TileMap.GetRandomEmptyPlace();
                Creature baseCreature = new Creature();
                baseCreature.MaxHealth = RandomTool.NextInt(15, 30);
                baseCreature.Name = "Human";

                CreatureGroup group = GameManager.Instance.GroupsManager.GetGroup(j);
                creatures.Clear();

                for (int i = 0; i < groupPopulation; i++)
                {
                    Creature creature = new Creature();
                    creature.AddToGroup(group);

                    creature.MaxHealth = baseCreature.MaxHealth + RandomTool.NextInt(0,3);
                    creature.Health = creature.MaxHealth;

                    creature.Name = baseCreature.Name;
                    creature.GameName = RandomNameGenerator.NextName();

                    creature.SetBehaviour(new IntelegentCreatureBehaviour(creature));

                    creature.Position = position;

                    creature.LifeTarget = LifeTarget.CollectItems;

                    group.SetGroupLeader(creature);

                    creatures.Add(creature);
                    chunk.AddActor(creature);
                }
                ConnectCreatures(creatures);
            }

            


        }

        private void ConnectCreatures(List<Creature> creatures)
        {
            for (int i = 0; i < creatures.Count(); i++)
            {
                for (int j = 0; j < creatures.Count(); j++)
                {
                    if (i != j)
                    {
                        creatures[i].RelationManager.AddRelation(creatures[j]);
                        creatures[i].RelationManager.ChangeRelation(creatures[j], 60 + RandomTool.NextInt(0, 20));
                    }
                }
            }
        }

        private void SettleItems(WorldChunk chunk)
        {
            const int itemsCount = 20;

            for (int i = 0; i < itemsCount; i++)
            {
                var p = chunk.TileMap.GetRandomEmptyPlace();

                var item = new MeleeWeapon();
                item.Name = "Sword";
                item.Damage = 3 + RandomTool.NextInt(6);
                item.GameName = "Sword +" + item.Damage.ToString();
                item.Position = p;
                chunk.AddActor(item);
            }
        }
    }
}
