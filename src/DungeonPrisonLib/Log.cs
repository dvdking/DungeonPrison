using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DungeonPrisonLib
{
    public class Log
    {
        protected List<string> Messages = new List<string>();

        public int MessagesCount { get { return Messages.Count; } }

        public void AddMessage(string message)
        {
            Messages.Add(message);
        }

        public string[] GetMessages(int position, int messagesCount)
        {
            if (position >= MessagesCount)
                return null;
            if (position < 0)
                return null;
            
            int countLeft = MessagesCount - position;
            int messagesToReturn = Math.Min(countLeft,messagesCount);

            if (messagesToReturn <= 0)
                return null;

            var messages = Messages.Skip(position).Take(messagesToReturn).ToArray();

            return messages;
        }
    }
}
