namespace Core.Rewards
{
    public abstract class AReward
    {
        public RewardType type;
        public int reward;
        public abstract void Consume();
    }
}