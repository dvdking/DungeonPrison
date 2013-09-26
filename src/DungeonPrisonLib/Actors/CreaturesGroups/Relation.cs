using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DungeonPrisonLib.Actors.CreaturesGroups
{
    public enum RelationsTypes
    {
        Unknown,
        Enemies,
        Neutral,
        Freindly
    }
    
    public class CreatureRelation
    {
        public RelationsTypes Type
        {
            get
            {
                if (RelationAmount < -35)
                    return RelationsTypes.Enemies;
                if (RelationAmount >= -35 && RelationAmount < 40)
                    return RelationsTypes.Neutral;
                return RelationsTypes.Freindly;
            }
        }

        public Creature Creature;
        public int RelationAmount;
        public int LastTimeInteracted;
    }
}
