//Developed by Pavel Kravtsov.
using UnityEngine;
using UnityEngine.Analytics;
using Core.GooglePlayServices;

namespace Core
{
    public class AnalyticsManager : MonoSingleton<AnalyticsManager>
    {
        [SerializeField]
        private TestAccounts testAccounts;
        [SerializeField]
        private TestDevices testDevices;
        private void Awake()
        {
            Analytics.enabled = false;
            Analytics.deviceStatsEnabled = false;
        }

        public void OnOffAnalytics()
        {
#if UNITY_ANDROID
            if (testAccounts.Accounts.Contains(GooglePlaySocialManager.Instance.CurrentEmail))
            {
                //Debug.Log("ANALYTICS[Email contains in test accs]");
                //Debug.Log("ANALYTICS[CurrentEmail]: " + GooglePlaySocialManager.Instance.CurrentEmail);
                foreach (var acc in testAccounts.Accounts)
                {
                    //Debug.Log("ANALYTICS[TestAcc]: " + acc);
                }
                if (GooglePlaySocialManager.Instance.CurrentEmail == testAccounts.Accounts[0])
                    Debug.Log("ANALYTICS[EQUAL]");
                else
                    Debug.Log("ANALYTICS[NOT EQUAL]");

                Analytics.enabled = false;
                Analytics.deviceStatsEnabled = false;
            }
            else
            {
                //Debug.Log("ANALYTICS[Email doesn't contains in test accs]");
                //Debug.Log("ANALYTICS[CurrentEmail]: " + GooglePlaySocialManager.Instance.CurrentEmail);
                foreach (var acc in testAccounts.Accounts)
                {
                    //Debug.Log("ANALYTICS[TestAcc]: " + acc);
                }
                if (GooglePlaySocialManager.Instance.CurrentEmail == testAccounts.Accounts[0])
                    Debug.Log("ANALYTICS[EQUAL]");
                else
                    Debug.Log("ANALYTICS[NOT EQUAL]");

                Analytics.enabled = true;
                Analytics.deviceStatsEnabled = true;
            }
            //Debug.Log("ANALYTICS[EMAIL]: " + GooglePlaySocialManager.Instance.CurrentEmail);
            //Debug.Log("ANALYTICS[ENABLED]: " + Analytics.enabled);
#elif UNITY_IOS
            //TODO: Add on/Off Analytics for ios
        //if(testAccounts.Accounts.Contains(GooglePlaySocialManager.Instance.GetUserEMail()))
        //{
        //}
#endif
        }
    }
}