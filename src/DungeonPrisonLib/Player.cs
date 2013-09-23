using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DungeonPrisonLib
{
    public class Player:Actor
    {
        public float UsedTime { get; private set; }

        private IInput _input;

        public Player(IInput input)
        {
            _input = input;
        }

        public override void Update(float delta)
        {
            var pressedKeys = _input.GetPressedKeys();

            foreach (var key in pressedKeys)
            {
                switch (key)
                {
                    case InputKey.MoveLeft:
                        X -= 1;
                        break;
                    case InputKey.MoveRight:
                        X += 1;
                        break;
                    case InputKey.MoveUp:
                        Y += 1;
                        break;
                    case InputKey.MoveDown:
                        Y -= 1;
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
