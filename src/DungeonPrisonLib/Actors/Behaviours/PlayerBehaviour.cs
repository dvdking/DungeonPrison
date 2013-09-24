using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DungeonPrisonLib.Actors.Behaviours
{
    class PlayerBehaviour:Behaviour
    {
        public PlayerBehaviour(Actor actor)
            : base(actor)
        {
 
        }

        public override void Update(float delta, TileMap tileMap)
        {
            var pressedKeys = GameManager.Instance.Input.GetPressedKeys();
            
            foreach (var key in pressedKeys)
            {
                switch (key)
                {
                    case InputKey.MoveLeft:
                        Actor.Move(-1, 0, tileMap);
                        break;
                    case InputKey.MoveRight:
                        Actor.Move(1, 0, tileMap);
                        break;
                    case InputKey.MoveUp:
                        Actor.Move(0, -1, tileMap);
                        break;
                    case InputKey.MoveDown:
                        Actor.Move(0, 1, tileMap);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
