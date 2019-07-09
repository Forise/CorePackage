using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Rewards.Wheel
{
    public abstract class AWheelOfFortuneManager : ARewardMechanicManager<AWheelOfFortuneManager>
    {
        [SerializeField]
        public WheelOfFortune wheel;

        public abstract void Spin();
        public abstract void Stop();
    }
}