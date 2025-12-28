using System;
using Runtime.Data.Enums;

namespace Runtime.Data.Structs
{
    // GDD 5.2 - NPC schedule entry for a specific day/phase
    [Serializable]
    public struct ScheduleEntry
    {
        public Weekday Day;
        public TimePhase Phase;
        public Zone Location;
        public NPCActivity Activity;

        public ScheduleEntry(Weekday day, TimePhase phase, Zone location, NPCActivity activity)
        {
            Day = day;
            Phase = phase;
            Location = location;
            Activity = activity;
        }
    }
}
