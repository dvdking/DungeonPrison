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

        public RelationManager()
        {
 
        }

        public void AddRelation(Creature creature)
        {
            CreatureRelation relation = new CreatureRelation();
            relation.Creature = creature;
            relation.RelationAmount = 0;
            _relations.Add(relation);
        }

        public void ChangeRelation(Creature creature, int amount)
        {
            var relation = _relations.Find(p => p.Creature == creature);

            if (relation == null)
            {
                Debug.Fail("cannot change relation, creature is null");
                return;
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

            amount /= _relations.Count();
            return (int)amount;
        }

    }
}
