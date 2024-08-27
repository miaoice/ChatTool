using System;

namespace ChatTool
{
    public class CustomGamemode
    {
        public virtual void OnVotingComplete(MeetingHud __instance, MeetingHud.VoterState[] states, NetworkedPlayerInfo exiled, bool tie)
        {
            
        }

        public static CustomGamemode Instance;
    }
}