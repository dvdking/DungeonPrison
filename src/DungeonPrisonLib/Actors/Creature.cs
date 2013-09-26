using DungeonPrisonLib.Actors.Behaviours;
using DungeonPrisonLib.Actors.CreaturesGroups;
using DungeonPrisonLib.Actors.Items;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;


namespace DungeonPrisonLib.Actors
{
    public enum CreatureRace
    {
        Human,
        Orc
    }


    public class Creature:Actor
    {
        public int MaxHealth;
        public int Health;

        protected enum Conclusion
        {
            Succes,
            Fail,
            AlreadyWielding
        }
        protected delegate void OnItemWielded(Conclusion conclusion, Item item, Item newItem);
        protected delegate void OnItemPickedUp(Item item);

        protected event OnItemWielded ItemWieldedEvent;
        protected event OnItemPickedUp ItemPickedUpEvent;

        public Inventory Inventory{get; private set;}

        public Item WieldedItem;

        public float UsedTime{get;private set;}

        public RelationManager RelationManager {get; private set;}
        public CreatureGroup CreatureGroup { get; private set; }

        Behaviour _behaviour;

        public Creature()
        {
            Inventory = new Inventory();
            RelationManager = new RelationManager();
            CreatureGroup = null;

            Depth = -1;
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

        public void AddToGroup(CreatureGroup group)
        {
            if (CreatureGroup != null)
            {
                CreatureGroup.RemoveCreatureFromGroup(this);
            }

            CreatureGroup = group;
            CreatureGroup.AddCreatureToGroup(this);
        }

        public void AddToGroup(string groupName)
        {
            AddToGroup(GameManager.Instance.GroupManager.GetGroup(groupName));
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

        public void Move(Point pos,TileMap tileMap)
        {
            Move(pos.X, pos.Y, tileMap);
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
                    int bonusDamage = 0;
                    if (WieldedItem != null)
                    {
                        if (WieldedItem is MeleeWeapon)
                        {
                            MeleeWeapon weapon = WieldedItem as MeleeWeapon;
                            bonusDamage += weapon.Damage;
                        }
                    }
                    Attack(creature, new AttackInfo { Damage = 1 + bonusDamage, Message = GameName + (GameName == "you" ? " hit " : " hits ") + actor.GameName });
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
                    var item = actor as Item;
                    Inventory.AddItem(item);

                    CallItemPickedUp(item);
                    UsedTime += 1.0f;
                }
	        }      
        }
        public void WieldItem(Item item)
        {
            Debug.Assert(Inventory.GetItems().Any(p => p == item), "item is not presented in inventory");



            if (WieldedItem == item)
            {
                CallItemWielded(Conclusion.AlreadyWielding);
                return;
            }

            if (WieldedItem == null)
            {
                UsedTime += 1.0f;
                CallItemWielded(Conclusion.Succes, null, item);
            }
            else
            {
                UsedTime += 2.0f;
                CallItemWielded(Conclusion.Succes, WieldedItem, item);
            }

            WieldedItem = item;
        }

        private void CallItemWielded(Conclusion conclusion, Item oldItem = null, Item newItem = null)
        {
            if(ItemWieldedEvent != null)
            {
                ItemWieldedEvent(conclusion, oldItem, newItem);
            }
        }

        private void CallItemPickedUp(Item item)
        {
            if (ItemPickedUpEvent != null)
            {
                ItemPickedUpEvent(item);
            }
        }
    }
}
