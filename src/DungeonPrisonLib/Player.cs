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



        public Player(IInput input):base()
        {
            _input = input;
        }

        public override void Update(float delta, TileMap tileMap)
        {
            var pressedKeys = _input.GetPressedKeys();



            foreach (var key in pressedKeys)
            {
                switch (key)
                {
                    case InputKey.MoveLeft:
                        Move(-1, 0, tileMap);                  
                        break;
                    case InputKey.MoveRight:
                        Move(1, 0, tileMap);
                        break;
                    case InputKey.MoveUp:
                        Move(0, -1, tileMap);
                        break;
                    case InputKey.MoveDown:
                        Move(0, 1, tileMap);
                        break;
                    default:
                        break;
                }
            }
        }

        private void Move(int x, int y, TileMap tileMap)
        {
            if (tileMap.IsSolid(X + x, Y + y))
                return;

            var actor = GameManager.Instance.GetActorAtPosition(X + x, Y + y);

            if (actor != null)
            {
                Attack(actor, new AttackInfo {Damage = 1, Message = "You hit " + actor.GameName });
                return;
            }

            X += x;
            Y += y;
        }

    }
}
