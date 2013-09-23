using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DungeonPrisonLib
{
    public class GameManager
    {
        private bool _exit;

        IRenderer _renderer;
        IInput _input;

        TileMap _tileMap;

        List<Actor> _actors;
        Player _player;
        public GameManager(IRenderer renderer, IInput input)
        {
            _renderer = renderer;
            _input = input;

            _actors = new List<Actor>(128);
            _player = new Player(input);
            _player.Name = "Player";

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
                Update();
                Draw();

                _input.Update();
            }
        }

        private void Update()
        {
            _player.Update(0.0f);
            foreach (var actor in _actors)
            {
                actor.Update(0.0f);
            }
        }
        private void Draw()
        {
            if (_renderer != null)
                _renderer.Draw(_player, _actors, _tileMap);
        }
    }
}
