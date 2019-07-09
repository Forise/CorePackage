namespace Core.Rewards
{
    [System.Serializable]
    public class ConsumeableReward : AConsumeableReward
    {
        public override void Consume()
        {
            base.Consume();
            switch(type)
            {
                case RewardType.Coins:
                    UserDataControl.Instance.UserData.Coins += (uint)reward;
                    State = ConsumeableRewardState.Consumed;
                    break;
            }            
            UserDataControl.Instance.SaveData();
        }
    }
}