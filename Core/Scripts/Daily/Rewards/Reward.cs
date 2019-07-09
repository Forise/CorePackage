namespace Core.Rewards
{
    [System.Serializable]
    public class Reward : AReward
    {
        public override void Consume()
        {
            switch (type)
            {
                case RewardType.Coins:
                    UserDataControl.Instance.UserData.Coins += (uint)reward;
                    break;
            }
            UserDataControl.Instance.SaveData();
        }
    }
}