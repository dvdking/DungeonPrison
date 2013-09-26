﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using DungeonPrisonLib.AStar;
using DungeonPrisonLib.Actors.CreaturesGroups;
namespace DungeonPrisonLib.Actors.Behaviours
{
    class IntelegentCreatureBehaviour:Behaviour
    {
        private Creature _lastTarget;
        private Point _lastTargetPosition;
        private bool _targetPositionChanged;
        private Queue<Point> _path;
       

        public IntelegentCreatureBehaviour(Creature actor)
            : base(actor)
        {
        }
        public override void Update(float delta, TileMap tileMap)
        {
            var visibleActors = GameManager.Instance.GetVisibleActors(Creature);

            foreach (var actor in visibleActors)
            {
                if (actor == Creature) continue;

                if (actor is Creature)
                {
                    UpdateMeetCreatureBehaviour(actor as Creature);
                }
            }
            if (_lastTarget != null)
            {
                MoveToTarget(tileMap);
            }
        }

        private void MoveToTarget(TileMap tileMap)
        {
            if (_targetPositionChanged)
            {
                _path = AStar.AStar.FindPath(Creature.Position, _lastTargetPosition, tileMap);
            }

            if (_path != null && _path.Count != 0)
            {
                Point nextPos = _path.Peek();
                Creature.Move(Math.Sign(-Creature.Position.X + nextPos.X),
                              Math.Sign(-Creature.Position.Y + nextPos.Y),
                              tileMap);

                if (nextPos == Creature.Position)
                {
                    _path.Dequeue();// remove element only if we actualy moved that way
                }

                _targetPositionChanged = false;
            }
        }

        private void UpdateMeetCreatureBehaviour(Creature creature)
        {
           
            var relationType = Creature.CreatureGroup == null || 
                               creature.CreatureGroup == null? RelationsTypes.Unknown
                                                             : Creature.CreatureGroup.GetRelationType(creature.CreatureGroup);

            switch (relationType)
            {
                case RelationsTypes.Unknown:
                    creature.RelationManager.AddRelation(creature);
                    break;
                case RelationsTypes.Enemies:
                    OnMeetEnemy(creature);
                    break;
                case RelationsTypes.Neutral:
                    break;
                case RelationsTypes.Freindly:
                    break;
                default:
                    break;
            }

        }

        private void OnMeetEnemy(Creature creature)
        {
            if (_lastTarget == null)
            {
                _lastTarget = creature;
            }
            if (_lastTarget == creature)
            {
                if (_lastTargetPosition != creature.Position)
                {
                    _lastTargetPosition = creature.Position;
                    _targetPositionChanged = true;
                }
            }
        }
    }
}
