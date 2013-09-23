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
        IInput _input;

        TileMap _tileMap;

        List<Actor> _actors;

        Queue<Actor> _actorsToAdd;
        Queue<Actor> _actorsToRemove;

        Player _player;

        public GameManager(IRenderer renderer, IInput input)
        {
            _renderer = renderer;
            _input = input;

            _actors = new List<Actor>(128);
            _actorsToAdd = new Queue<Actor>(16);
            _actorsToRemove = new Queue<Actor>(16);

            _player = new Player(input);
            _player.Name = "Player";
            _player.X = 4;
            _player.Y = 2;
            _actors.Add(_player);

            var act = new Creature();
            act.Name = "SomeGuy";
            act.X = 3;
            act.Y = 2;
            _actors.Add(act);

            _tileMap = new TileMap(30, 30);
            _tileMap.ReadSimpleMap("map1.txt");
        }

        public void Run()
        {
            
            while (!_exit)
            {                   
                Draw();
                _input.Update();
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
        }
        private void Draw()
        {
            if (_renderer != null)
                _renderer.Draw(_player, _actors, _tileMap);
        }
    }
}
