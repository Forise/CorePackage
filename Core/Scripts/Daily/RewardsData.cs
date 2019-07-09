namespace Core.Daily
{
    [System.Serializable]
    public class RewardsData
    {
        public byte currentDailyDay = 0;
        public byte lastClaimedDaily = 255;
        public byte currentWheelDay = 0;
        public byte lastClaimedWheel = 255;
        public byte currentScratchDay = 0;
        public byte lastClaimedScratch = 255;
        public byte currentSlotDay = 0;
        public byte lastClaimedSlot = 255;
        public byte currentRewardMechanic = 0;
        public string nextDate;
        public string nextSurprizeDate;

        public RewardsData()
        {
            currentDailyDay = 0;
            currentWheelDay = 0;
            currentScratchDay = 0;
            currentSlotDay = 0;
            lastClaimedDaily = 255;
            lastClaimedWheel = 255;
            lastClaimedScratch = 255;
            lastClaimedSlot = 255;
            currentRewardMechanic = 0;
        }
    }
}