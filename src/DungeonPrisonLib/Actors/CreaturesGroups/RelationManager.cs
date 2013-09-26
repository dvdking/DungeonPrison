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

        public RelationManager(Creature creature)
        {
            _creature = creature;
            Debug.Assert(_creature != null, "creature is null");
        }

        public CreatureRelation AddRelation(Creature creature)
        {
            CreatureRelation relation = new CreatureRelation();
            relation.Creature = creature;
            relation.RelationAmount = 0;
            _relations.Add(relation);
            return relation;
        }

        public void ChangeRelation(Creature creature, int amount)
        {
            var relation = _relations.Find(p => p.Creature == creature);
            
            if (relation == null)
            {
                relation = AddRelation(creature);
            }

            relation.RelationAmount += amount;

            if (relation.RelationAmount < -100)
                relation.RelationAmount = -100;
            else if (relation.RelationAmount > 100)
                relation.RelationAmount = 100;
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
            var k1 = 0.3f;
            var k2 = 0.7f;

            return (int)(groupRelation*k2 + creatureRelation*k1);
        }

        public RelationsTypes GetReletionTypeToCreature(Creature creature)
        {
            var toCreature = GetRelationAmountToCreature(creature);

            return ReletionHelper.GetReletion(toCreature);
        }
    }
}
