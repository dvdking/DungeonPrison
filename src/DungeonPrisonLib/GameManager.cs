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

        private bool _exit;

        IRenderer _renderer;
        public IInput Input{get; private set;}
        public Log Log{get; private set;}
        public LOS LOS { get; private set; }

        TileMap _tileMap;

        List<Actor> _actors;

        Queue<Actor> _actorsToAdd;
        Queue<Actor> _actorsToRemove;

        Player _player;

        public GameManager(IRenderer renderer, IInput input)
        {
            _renderer = renderer;
            Input = input;

            Log = new Log();
            LOS = new LOS();

            _actors = new List<Actor>(128);
            _actorsToAdd = new Queue<Actor>(16);
            _actorsToRemove = new Queue<Actor>(16);

            _player = new Player();
            _player.SetBehaviour(new PlayerBehaviour(_player));
            _player.Name = "you";
            _player.GameName = "you";
            _player.X = 4;
            _player.Y = 2;
            _actors.Add(_player);
            _player.MaxHealth = 15;
            _player.Health = 15;

            var act = new Creature();
            act.SetBehaviour(new SomeGuyBehaviour(act));
            act.Name = "SomeGuy";
            act.GameName = "Some guy 1";
            act.X = 3;
            act.Y = 6;
            act.MaxHealth = 5;
            act.Health = 5;
            _actors.Add(act);

            _tileMap = new TileMap(30, 30);
            _tileMap.ReadSimpleMap("map1.txt");
        }

        public void Run()
        {            
            while (!_exit)
            {
                Update();
                Draw();
                Input.Update();  
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

        public void DestroyObject(Actor actor)
        {
            _actorsToRemove.Enqueue(actor);
        }

        private void Update()
        {
            foreach (var actor in _actorsToRemove)
            {
                _actors.Remove(actor);
            }
            _actorsToRemove.Clear();

            foreach (var actor in _actors)
            {
                if(actor.IsAlive)
                    actor.Update(0.0f, _tileMap);
            }

            LOS.UpdateVisibleArea(_player, _tileMap);
        }
        private void Draw()
        {
            if (_renderer != null)
                _renderer.Draw(_player, _actors, _tileMap);
        }
    }
}
