using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace DungeonPrisonLib.Actors.CreaturesGroups
{    

    public class GroupRelation
    {
        public CreatureGroup Group;
        public CreatureGroup OtherGroup;

        public RelationsTypes GetRelationTypeToGroup()
        {
            int relationAmount = GetRelationAmountToGroup();

            return ReletionHelper.GetReletion(relationAmount);
            
        }
        public int GetRelationAmountToGroup()
        {
            if (Group == null)
            {
                Debug.Fail("Group is null");
                return 0;
            }

            float amount  = 0.0f;

            foreach (var creature in Group.Creatures)
            {
                amount += creature.RelationManager.GetRelationToGroup(OtherGroup);
            }

            return (int)amount;
        }
    }

    public class CreatureGroup
    {
        public string GroupName;

        private List<GroupRelation> Relations;

        public Creature GroupLeader{get; private set;}
        public LifeTarget CurrentTarget;

        public List<Creature> Creatures;

        public CreatureGroup()
        {
            Relations = new List<GroupRelation>();
            Creatures = new List<Creature>();


            // small hack
            // need to get relation to creature even if a creature in this group
            Relations.Add(new GroupRelation()
            {
                Group = this,
                OtherGroup = this,
            });
        }

        public void SetGroupLeader(Creature creature)
        {
            GroupLeader = creature;
            CurrentTarget = creature.LifeTarget;
        }

        public void AddGroupRelation(CreatureGroup group)
        {
            if (group == null)
            {
                Debug.Fail("group is null");
            }

            GroupRelation relation = new GroupRelation();

            relation.Group = this;
            relation.OtherGroup = group;

            Relations.Add(relation);
        }

        public void AddCreatureToGroup(Creature creature)
        {
            if (creature == null)
            {
                Debug.Fail("creature is null");
                return;
            }

            Creatures.Add(creature);
        }

        public void RemoveCreatureFromGroup(Creature creature)
        {
            if (creature == null)
            {
                Debug.Fail("creature is null");
                return;
            }

            Creatures.Remove(creature);
        }

        public RelationsTypes GetRelationType(CreatureGroup group)
        {
            if (group == null)
            {
                Debug.Fail("group is null");
                return RelationsTypes.Unknown;
            }
            if (group == this)
                return RelationsTypes.Freindly;

            var relation = Relations.Find(p => p.OtherGroup == group);
            if (relation == null)
                return RelationsTypes.Unknown;

            return relation.GetRelationTypeToGroup();
        }

        public int GetRelationAmount(CreatureGroup group)
        {
            if (group == null)
            {
                Debug.Fail("group is null");
                return 0;
            }

            var relation = Relations.Find(p => p.OtherGroup == group);
            if (relation == null)
                return 0;

            return relation.GetRelationAmountToGroup();
        }

        
        internal bool HasRelation(CreatureGroup gr2)
        {
            return Relations.Find(p => p.OtherGroup == gr2) != null;
        }

        public override string ToString()
        {
            return GroupName.Length == 0 ? "Unknown group" : GroupName;
        }

        internal bool IsGroupLeader(Creature creature)
        {
            return GroupLeader == creature;
        }
    }
}
