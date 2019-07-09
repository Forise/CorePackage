namespace Core.Rewards
{
    public abstract class AConsumeableReward : AReward
    {
        public event System.Action StateChanged;
        private ConsumeableRewardState state = ConsumeableRewardState.Inactive;
        public ConsumeableRewardState State
        {
            get => state;
            set
            {
                state = value;
                StateChanged?.Invoke();
            }
        }
        public override void Consume()
        {
            if (State != ConsumeableRewardState.Active)
                return;
        }
    }
}