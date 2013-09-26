using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DungeonPrisonLib.Actors.CreaturesGroups
{
    public class CreatureGroupsManager
    {
        public List<CreatureGroup> GameGroups;

        public CreatureGroupsManager()
        {
            GameGroups = new List<CreatureGroup>();
        }

        public CreatureGroup AddGroup(string groupName)
        {
            var group = new CreatureGroup();
            group.GroupName = groupName;
            GameGroups.Add(group);
            return group;
        }

        public CreatureGroup GetGroup(string groupName)
        {
            return GameGroups.Find(p => p.GroupName == groupName);
        }
    }
}
