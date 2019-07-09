//Developed by Pavel Kravtsov.
using UnityEngine;

namespace Core.AD
{
    [CreateAssetMenu(fileName = "AdData", menuName = "Core/ScriptableObjects/AdData", order = 3)]
    public class AdData : ScriptableObject
    {
        [SerializeField]
        private string appID;
        [SerializeField]
        private string interstitialRegID;
        [SerializeField]
        private string interstitialVideoID;
        [SerializeField]
        private string interstitialRewardID;

        public string AppID { get => appID; }
        public string InterstitialRegID { get => interstitialRegID; }
        public string InterstitialVideoID { get => interstitialVideoID; }
        public string InterstitialRewardID { get => interstitialRewardID; }
    }
}
