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

        public bool IsEmpty { get { return GameGroups == null || GameGroups.Count == 0; } }

        public void CreateGroupRelation(CreatureGroup gr1, CreatureGroup gr2)
        {
            gr1.AddGroupRelation(gr2);
            gr2.AddGroupRelation(gr1);
        }

        public CreatureGroup AddGroup(string groupName)
        {
            var group = new CreatureGroup();
            group.GroupName = groupName;
            GameGroups.Add(group);
            return group;
        }

        public CreatureGroup GetGroup(int index)
        {
            return GameGroups[index];
        }

        public CreatureGroup GetGroup(string groupName)
        {
            return GameGroups.Find(p => p.GroupName == groupName);
        }

        internal CreatureGroup GetRandomGroup()
        {
            return GameGroups[RandomTool.NextInt(GameGroups.Count - 1)];
        }
    }
}
