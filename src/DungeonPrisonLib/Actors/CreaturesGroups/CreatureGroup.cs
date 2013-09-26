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

            if (relationAmount < -35)
                return RelationsTypes.Enemies;
            if (relationAmount >= -35 && relationAmount < 40)
                return RelationsTypes.Neutral;        
        
            return RelationsTypes.Freindly;
            
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

        public List<Creature> Creatures;

        public CreatureGroup()
        {
            Relations = new List<GroupRelation>();
            Creatures = new List<Creature>();
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

            var relation = Relations.Find(p => p.OtherGroup == group);
            if (relation == null)
                return RelationsTypes.Unknown;

            return relation.GetRelationTypeToGroup();
        }

        public override string ToString()
        {
            return GroupName.Length == 0 ? "Unknown group" : GroupName;
        }
    }
}
