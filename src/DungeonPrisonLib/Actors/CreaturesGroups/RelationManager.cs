using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace DungeonPrisonLib.Actors.CreaturesGroups
{
    public class RelationManager
    {
        private List<CreatureRelation> _relations = new List<CreatureRelation>();
        private Creature _creature;
        private CreatureGroupsManager _groupsManager;

        public RelationManager(Creature creature, CreatureGroupsManager groupsManager)
        {
            _creature = creature;
            _groupsManager = groupsManager;
            Debug.Assert(_creature != null, "creature is null");
        }

        public CreatureRelation AddRelation(Creature creature)
        {
            CreatureRelation relation = new CreatureRelation();
            relation.Creature = creature;
            relation.RelationAmount = 0;
            _relations.Add(relation);

            UpdateGroupsRelations(creature);

            return relation;
        }

        public void ChangeRelation(Creature creature, int amount)
        {
            var relation = _relations.Find(p => p.Creature == creature);
            
            if (relation == null)
            {
                relation = AddRelation(creature);
            }

            UpdateGroupsRelations(creature);

            relation.RelationAmount += amount;

            if (relation.RelationAmount < -100)
                relation.RelationAmount = -100;
            else if (relation.RelationAmount > 100)
                relation.RelationAmount = 100;
        }

        private void UpdateGroupsRelations(Creature creature)
        {
            if (_creature.CreatureGroup != null && creature.CreatureGroup != null)
            {
                if (!_creature.CreatureGroup.HasRelation(creature.CreatureGroup))
                {
                    _groupsManager.CreateGroupRelation(_creature.CreatureGroup, _creature.CreatureGroup);
                }
            }
        }



        public int GetRelationToGroup(CreatureGroup group)
        {
            float amount = 0.0f;

            foreach (var relation in _relations)
            {
                if(relation.Creature.CreatureGroup == group)
                amount += relation.RelationAmount; // todo add more factors
            }

            amount /= _relations.Count() == 0? 1 : _relations.Count();
            return (int)amount;
        }

        public int GetRelationAmountToCreature(Creature creature)
        {
            var groupRelation = _creature.CreatureGroup == null ||
                               creature.CreatureGroup == null ? 0
                                                 :  _creature.CreatureGroup.GetRelationAmount(creature.CreatureGroup);

            var relatedCreature = _relations.Find(p => p.Creature == creature);
            int creatureRelation = 0;

            if(relatedCreature != null)
            {
                creatureRelation = relatedCreature.RelationAmount;
            }

            //todo change dynamicaly these values according to creature's feelings
            var k1 = 0.9f;
            var k2 = 0.1f;


            if (_creature.CreatureGroup == null || creature.CreatureGroup == null)
            {
                k2 = 0;
                k1 = 1;
            }

            return (int)(groupRelation*k2 + creatureRelation*k1);
        }

        public RelationsTypes GetReletionTypeToCreature(Creature creature)
        {
            var toCreature = GetRelationAmountToCreature(creature);

            return ReletionHelper.GetReletion(toCreature);
        }
    }
}
