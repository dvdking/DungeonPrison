using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using DungeonPrisonLib.AStar;
using DungeonPrisonLib.Actors.CreaturesGroups;
using DungeonPrisonLib.Actors.Items;
namespace DungeonPrisonLib.Actors.Behaviours
{
    class IntelegentCreatureBehaviour:Behaviour
    {
        private Actor _lastTarget;
        private Point _lastTargetPosition;
        private bool _targetPositionChanged;
        private Queue<Point> _path;

        private Point _lastRandomPosition;
       

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

            switch (Creature.LifeTarget)
            {
                case LifeTarget.NoTarget:
                    //do something randomly
                    int dirX = RandomTool.NextBool() ? RandomTool.NextSign() : 0;
                    int dirY = dirX == 0 ? RandomTool.NextSign() : 0;

                    if (GameManager.Instance.IsPositionFree<Creature>(Creature.X + dirX, Creature.Y + dirY))
                        Creature.Move(dirX, dirY, tileMap);
                    break;
                case LifeTarget.CollectItems:
                    if (Creature.CreatureGroup == null)// search for items by himself
                    {
                        if (visibleActors.Any(p => p is Item))
                        {
                            _lastTarget = visibleActors.Find(p => p is Item);
                            _lastTargetPosition = _lastTarget.Position;
                            _targetPositionChanged = true;
                        }
                        else
                        {
                            SetNextRandomTarget(tileMap);
                        }
                    }
                    else// search for items with leader
                    {
                        if (visibleActors.Any(p => p is Item))
                        {
                            _lastTarget = visibleActors.Find(p => p is Item);
                            _lastTargetPosition = _lastTarget.Position;
                            _targetPositionChanged = true;
                        }
                        else
                        {
                            if (Creature.CreatureGroup.IsGroupLeader(Creature))
                            {
                                SetNextRandomTarget(tileMap);
                            }
                            else
                            {
                                _lastTarget = Creature.CreatureGroup.GroupLeader;
                                _targetPositionChanged = true;
                            }
                        }
                    }
                    break;
                case LifeTarget.KillThings:
                    break;
                default:
                    throw new NotImplementedException();
                    break;
            }

            if (_lastTarget != null)
            {
                if (MoveToTarget(tileMap, _lastTargetPosition))
                {
                    TargetReached();
                }
            }
            else
            {
                if (MoveToTarget(tileMap, _lastRandomPosition))
                {
                    _lastRandomPosition = new Point(-1, -1);

                    TargetReached();
                }
            }

        }

        private void TargetReached()
        {
            switch (Creature.LifeTarget)
            {
                case LifeTarget.NoTarget:

                    break;
                case LifeTarget.CollectItems:
                    Creature.PickUpItem();
                    _lastTarget = null;
                    break;
                case LifeTarget.KillThings:
                    _lastTarget = null;
                    break;
                default:
                    break;
            }
        }

        private void SetNextRandomTarget(TileMap tileMap)
        {
            if (_lastRandomPosition.X == -1 && _lastRandomPosition.Y == -1)
            {
                _lastRandomPosition = tileMap.GetRandomEmptyPlace();
            }
            _targetPositionChanged = true;
        }

        private bool MoveToTarget(TileMap tileMap, Point lastTargetPosition)
        {
            if (_targetPositionChanged)
            {
                _path = AStar.AStar.FindPath(Creature.Position, lastTargetPosition, tileMap);
            }

            if (_path != null && _path.Count != 0)
            {
                Point nextPos = _path.Peek();

                int dirX = Math.Sign(-Creature.Position.X + nextPos.X);
                int dirY = Math.Sign(-Creature.Position.Y + nextPos.Y);

                if (GameManager.Instance.IsPositionFree<Creature>(Creature.X + dirX, Creature.Y + dirY) ||
                    GameManager.Instance.GetActorsAtPosition(Creature.X + dirX, Creature.Y + dirY).Any(p => p == _lastTarget))
                {
                    Creature.Move(dirX, dirY, tileMap);
                }

                if (!_lastTarget.IsAlive)
                {
                    _lastTarget = null;
                    _path.Clear();
                    return true;
                }
                
                _targetPositionChanged = false;


                if (nextPos == Creature.Position)
                {
                    _path.Dequeue();// remove element only if we actualy moved that way
                }
            }
            if (_path == null)
                return false;

            return _path.Count == 0;
        }

        private void UpdateMeetCreatureBehaviour(Creature metCreature)
        {

            var relationType = Creature.RelationManager.GetReletionTypeToCreature(metCreature);

            switch (relationType)
            {
                case RelationsTypes.Unknown:
                    metCreature.RelationManager.AddRelation(metCreature);
                    break;
                case RelationsTypes.Enemies:
                    OnMeetEnemy(metCreature);
                    break;
                case RelationsTypes.Neutral:
                    break;
                case RelationsTypes.Freindly:
                    break;
                default:
                    break;
            }

        }

        private void OnMeetEnemy(Creature metCreature)
        {
            if (_lastTarget == null)
            {
                _lastTarget = metCreature;
            }
            if (_lastTarget == metCreature)
            {
                if (_lastTargetPosition != metCreature.Position)
                {
                    _lastTargetPosition = metCreature.Position;
                    _targetPositionChanged = true;
                }
            }
        }
    }
}
