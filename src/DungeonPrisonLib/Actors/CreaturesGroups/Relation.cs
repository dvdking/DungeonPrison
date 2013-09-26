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

    public static class ReletionHelper
    {
        public static RelationsTypes GetReletion(int amount)
        {
            if (amount < -35)
                return RelationsTypes.Enemies;
            if (amount >= -35 && amount < 40)
                return RelationsTypes.Neutral;
            return RelationsTypes.Freindly;
        }
    }
    
    public class CreatureRelation
    {

        public Creature Creature;
        public int RelationAmount;
        public int LastTimeInteracted;


    }
}
