namespace Core.Rewards
{
    public abstract class ARewardMechanicManager<T> : MonoSingleton<T> where T:UnityEngine.MonoBehaviour
    {
        protected abstract byte GetRewardByDay { get; }
    }
}