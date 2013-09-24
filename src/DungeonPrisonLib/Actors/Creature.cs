using DungeonPrisonLib.Actors.Behaviours;
using DungeonPrisonLib.Actors.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace DungeonPrisonLib.Actors
{
    public class Creature:Actor
    {
        public int MaxHealth;
        public int Health;

        public Inventory Inventory{get; private set;}


        public float UsedTime{get;private set;}

        Behaviour _behaviour;

        public Creature()
        {
            Inventory = new Inventory();
        }

        public void SetBehaviour(Behaviour behaviour)
        {
            _behaviour = behaviour;
        }

        public override void Update(float delta, TileMap tileMap)
        {
            UsedTime = 0f;
            if(_behaviour != null)
                _behaviour.Update(delta, tileMap);
        }

        internal void Attack(Creature creature, AttackInfo attackInfo)
        {
            GameManager.Instance.Log.AddMessage(attackInfo.Message);
            creature.Health -= attackInfo.Damage;
            creature.CheckDeath();
            UsedTime += 1.0f;
        }

        public void CheckDeath()
        {
            if(Health <= 0)
                Destroy();
        }

        public void Move(int x, int y, TileMap tileMap)
        {
            if (tileMap.InBounds(X + x, Y + y))
               if (tileMap.IsSolid(X + x, Y + y))
                return;

            var actor = GameManager.Instance.GetActorAtPosition(X + x, Y + y);
           
            if (actor != null)
            {
                var creature = actor as Creature;
                if(creature != null)
                {
                    Attack(creature, new AttackInfo { Damage = 1, Message = GameName + (GameName == "you" ? " hit " : " hits ") + actor.GameName });
                    return;
                }
            }

            X += x;
            Y += y;

            UsedTime += 1.0f;
        }

        public void Wait()
        {
            UsedTime += 1.0f;
        }

        public void PickUpItem()
        {
            var actors = GameManager.Instance.GetActorsAtPosition(X, Y);

            foreach (var actor in actors)
	        {
                if(actor is Item)
                {
                    GameManager.Instance.DestroyObject(actor);
                    Inventory.AddItem(actor as Item);
                }
	        }
        }
    }
}
