using DungeonPrisonLib.Actors;
using DungeonPrisonLib.Actors.Behaviours;
using DungeonPrisonLib.Actors.CreaturesGroups;
using DungeonPrisonLib.Actors.Items;
using DungeonPrisonLib.World;
using DungeonPrisonLib.WorldGenerator;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace DungeonPrisonLib
{
    public class GameManager
    {
        static public GameManager Instance
        {
            get;
            private set;
        }

        static public void CreateInstance(IRenderer renderer, IInput input)
        {
            Instance = new GameManager(renderer, input);
        }

        public bool Exit;

        IRenderer _renderer;
        public IInput Input{get; private set;}
        public Log Log{get; private set;}
        public LOS LOS { get; private set; }
        public CreatureGroupsManager GroupsManager { get; private set; }


        public WorldManager WorldManager { get; private set; }
        public WorldChunk CurrentChunk { get; private set; }
        public TileMap TileMap { get { return CurrentChunk.TileMap; } }

        public Player Player{get; private set;}

        public GameManager(IRenderer renderer, IInput input)
        {
            _renderer = renderer;
            Input = input;

            //TileMap = gen.GenerateTileMap(256, 256, TlleMapType.NeutralDungeon);
           // TileMap.PrintMapToFile("map.bmp");
        }

        public void InitGame()
        {
            Log = new Log();
            LOS = new LOS();
            RandomNameGenerator.LoadDB(Settings.RandomNamesDB);
            GroupsManager = new CreatureGroupsManager();

            WorldManager = new WorldManager(Settings.WorldSizeX, Settings.WorldSizeY, Settings.WorldSizeZ);
            TileMapGenerator gen = new TileMapGenerator();


            CurrentChunk = WorldManager.GetChunk(5, 5, 5);

            


            CreateTestItems();

            CreateTestGroups();
            CreatePlayer();
            CreateTestCreatures();
        }

        private void CreateTestItems()
        {
            var weapon = new MeleeWeapon();
            weapon.Name = "Sword";
            weapon.GameName = "short sword";
            weapon.X = 4;
            weapon.Y = 5;
            weapon.Damage = 5;
            CurrentChunk.AddActor(weapon);
        }

        private void CreatePlayer()
        {
            Player = new Player();
            Player.SetBehaviour(new PlayerBehaviour(Player));
            Player.Name = "you";
            Player.GameName = "you";
        //    Player.AddToGroup("Klarks");
            Player.Position = TileMap.GetRandomEmptyPlace();
            Player.MaxHealth = 15;
            Player.Health = 15;
            Player.CreateMemory(TileMap);
            CurrentChunk.AddActor(Player);
        }

        private void CreateTestGroups()
        {
            //var bohels = GroupsManager.AddGroup("Bohels");
            //var klarks = GroupsManager.AddGroup("Klarks");
            //var animal = GroupsManager.AddGroup("Animal");

            //bohels.AddGroupRelation(klarks);
            //klarks.AddGroupRelation(bohels);
        }

        private void CreateTestCreatures()
        {
            //var act = new Creature();
            //act.SetBehaviour(new BrainlessSlimeBehaviour(act));
            //act.Name = "BrainlessSlime";
            //act.GameName = "brainless slime";
            //act.AddToGroup("Animal");
            //act.X = 3;
            //act.Y = 5;
            //act.MaxHealth = 5;
            //act.Health = 5;
            //CurrentChunk.AddActor(act);

            //var act1 = new Creature();
            //act1.SetBehaviour(new IntelegentCreatureBehaviour(act1));
            //act1.Name = "Human";
            //act1.GameName = "David";
            //act1.AddToGroup("Bohels");
            //act1.X = 4;
            //act1.Y = 4;
            //act1.MaxHealth = 10;
            //act1.Health = 10;
            //CurrentChunk.AddActor(act1);

            //var act2 = new Creature();
            //act2.SetBehaviour(new IntelegentCreatureBehaviour(act2));
            //act2.Name = "Human";
            //act2.GameName = "John";
            //act2.AddToGroup("Klarks");
            //act2.X = 5;
            //act2.Y = 5;
            //act2.MaxHealth = 15;
            //act2.Health = 15;
            //CurrentChunk.AddActor(act2);

            //act1.RelationManager.AddRelation(act2);
            //act1.RelationManager.ChangeRelation(act2, -80);

            //act2.RelationManager.AddRelation(act1);
            //act2.RelationManager.ChangeRelation(act1, -80);
        }

        public void Run()
        {
            UpdateWorld(0.0f);
            Draw();
            while (!Exit)
            {
                Draw();
                Input.Update();
                Update();
            }
        }

        public Actor GetActorAtPosition(int x, int y)
        {
            foreach (var actor in CurrentChunk.Actors)
            {
                if (actor.X == x && actor.Y == y)
                {
                    return actor;
                }
            }
            return null;
        }


        public bool IsPositionFree<T>(int x, int y) where T: Actor
        {
            List<Actor> actors = GetActorsAtPosition(x, y);
            foreach (var item in actors)
            {
                if (item is T)
                    return false;
            }
            return true;
        }

        public List<Actor> GetActorsAtPosition(int x, int y)
        {
            List<Actor> actors = new List<Actor>();
            foreach (var a in CurrentChunk.Actors)
            {
                if (a.X == x && a.Y == y)
                {
                    actors.Add(a);
                }
            }
            return actors;
        }

        public List<Actor> GetVisibleActors(Actor actor)
        {
            List<Actor> actors = new List<Actor>();
            foreach (var a in CurrentChunk.Actors)
            {
                if(LOS.InteserectsWall(a, TileMap, actor.X, actor.Y))
                    actors.Add(a);
            }
            return actors;
        }

        public void DestroyObject(Actor actor)
        {
            //_actorsToRemove.Enqueue(actor);
            CurrentChunk.RemoveActor(actor);
        }

        public void Update()
        {
            if (Player.UsedTime > 0.001f)
            {
                UpdateWorld(Player.UsedTime);
            }
            Player.Update(0.0f, TileMap);
            LOS.UpdateVisibleArea(Player, TileMap);
            Player.UpdateMemory(LOS, TileMap);
        }



        private void UpdateWorld(float delta)
        {
            //foreach (var actor in _actorsToRemove)
            //{
            //    _actors.Remove(actor);
            //}
            //_actorsToRemove.Clear();

            foreach (var actor in CurrentChunk.Actors.ToList())
            {
                if(actor.IsAlive && actor != Player)
                    actor.Update(delta, TileMap);
            }            
        }
        public void Draw()
        {
            if (_renderer != null)
                _renderer.Draw(Player, CurrentChunk.Actors, TileMap);
        }
    }
}
