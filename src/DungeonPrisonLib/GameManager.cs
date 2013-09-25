using DungeonPrisonLib.Actors;
using DungeonPrisonLib.Actors.Behaviours;
using System;
using System.Collections.Generic;
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

        public TileMap TileMap{get; private set;}

        List<Actor> _actors;

        Queue<Actor> _actorsToAdd;
        Queue<Actor> _actorsToRemove;

        public Player Player{get; private set;}

        public GameManager(IRenderer renderer, IInput input)
        {
            _renderer = renderer;
            Input = input;

            Log = new Log();
            LOS = new LOS();

            _actors = new List<Actor>(128);
            _actorsToAdd = new Queue<Actor>(16);
            _actorsToRemove = new Queue<Actor>(16);

            Player = new Player();
            Player.SetBehaviour(new PlayerBehaviour(Player));
            Player.Name = "you";
            Player.GameName = "you";
            Player.X = 4;
            Player.Y = 2;
            _actors.Add(Player);
            Player.MaxHealth = 15;
            Player.Health = 15;

            var act = new Creature();
            act.SetBehaviour(new SomeGuyBehaviour(act));
            act.Name = "BrainlessSlime";
            act.GameName = "brainless slime";
            act.X = 3;
            act.Y = 5;
            act.MaxHealth = 5;
            act.Health = 5;
            _actors.Add(act);

            TileMap = new TileMap(30, 30);
            TileMap.ReadSimpleMap("map1.txt");
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
            foreach (var actor in _actors)
            {
                if (actor.X == x && actor.Y == y)
                {
                    return actor;
                }
            }
            return null;
        }

        public List<Actor> GetActorsAtPosition(int x, int y)
        {
            List<Actor> actors = new List<Actor>();
            foreach (var actor in actors)
            {
                if (actor.X == x && actor.Y == y)
                {
                    actors.Add(actor);
                }
            }
            return actors;
        }

        public void DestroyObject(Actor actor)
        {
            _actorsToRemove.Enqueue(actor);
        }

        public void Update()
        {
            if (Player.UsedTime > 0.001f)
            {
                UpdateWorld(Player.UsedTime);
            }
            Player.Update(0.0f, TileMap);
            LOS.UpdateVisibleArea(Player, TileMap);
        }

        private void UpdateWorld(float delta)
        {
            foreach (var actor in _actorsToRemove)
            {
                _actors.Remove(actor);
            }
            _actorsToRemove.Clear();

            foreach (var actor in _actors)
            {
                if(actor.IsAlive && actor != Player)
                    actor.Update(delta, TileMap);
            }

            
        }
        public void Draw()
        {
            if (_renderer != null)
                _renderer.Draw(Player, _actors, TileMap);
        }
    }
}
