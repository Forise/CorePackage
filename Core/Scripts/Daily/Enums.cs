using System.Collections;
using System.Collections.Generic;

namespace Core.Rewards
{
    public enum RewardType : byte
    {
        Coins
    }

    public enum ConsumeableRewardState : byte
    {
        Inactive,
        Active,
        Consumed
    }

    public enum RewardMechanic : byte
    {
        Wheel = 0,
        Slot = 1,
        Scratch = 2,
        Error = 255
    }
}