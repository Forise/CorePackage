//Developed by Pavel Kravtsov.
using UnityEngine;
using Core.Audio;
using Core.EventSystem;
using GoogleMobileAds.Api;
using System;

namespace Core.AD
{
    public enum InterstitialType
    {
        Default, Video, Reward
    }
    public class AdMobManager : MonoBehaviour
    {
        #region Fields
        private InterstitialAd interstitialReg;
        private InterstitialAd interstitialVideo;
        private RewardedAd interstitialReward;

        private System.Action OkReward;
        private System.Action CancelledReward;


        private InterstitialType currentInterstitial;
        [SerializeField]
        private AdData adData;
        [SerializeField]
        private TestDevices testDevices;
        #endregion Fields

        #region Properties
        #endregion Properties
        private void Start()
        {
            Init();
        }

        #region Methods
        private void Init()
        {
            MobileAds.Initialize(adData.AppID);

            interstitialReg = new InterstitialAd(adData.InterstitialRegID);
            interstitialVideo = new InterstitialAd(adData.InterstitialVideoID);
            interstitialReward = new RewardedAd(adData.InterstitialRewardID);

            SubscribeAds();

            EventManager.Subscribe(Events.ADEvents.SHOW_AD, ShowAD_Handler);
            EventManager.Subscribe(Events.GameEvents.GAME_STARTED, ResetCurrentInterstitial);
            EventManager.Subscribe(Events.GameEvents.GAME_ENDED, ResetCurrentInterstitial);

            LoadInterstitial(interstitialReg);
            LoadInterstitial(interstitialVideo);
            LoadInterstitial(interstitialReward);
        }

        private void OnDestroy()
        {
            EventManager.Unsubscribe(Events.ADEvents.SHOW_AD, ShowAD_Handler);
            EventManager.Unsubscribe(Events.GameEvents.GAME_STARTED, ResetCurrentInterstitial);
            EventManager.Unsubscribe(Events.GameEvents.GAME_ENDED, ResetCurrentInterstitial);
            UnsubscribeAds();
        }

        private void SubscribeAds()
        {
            interstitialReg.OnAdClosed += HandleOnAdClosed;
            interstitialReg.OnAdFailedToLoad += HandleOnAdFailedToLoad;
            interstitialReg.OnAdLeavingApplication += HandleOnAdLeavingApplication;
            interstitialReg.OnAdLoaded += HandleOnAdLoaded;
            interstitialReg.OnAdOpening += HandleOnAdOpened;

            interstitialVideo.OnAdClosed += HandleOnAdClosed;
            interstitialVideo.OnAdFailedToLoad += HandleOnAdFailedToLoad;
            interstitialVideo.OnAdLeavingApplication += HandleOnAdLeavingApplication;
            interstitialVideo.OnAdLoaded += HandleOnAdLoaded;
            interstitialVideo.OnAdOpening += HandleOnAdOpened;

            interstitialReward.OnAdClosed += HandleOnAdClosed;
            interstitialReward.OnAdFailedToLoad += HandleOnAdFailedToLoad;
            interstitialReward.OnUserEarnedReward += HandleOnEarned;
            interstitialReward.OnAdLoaded += HandleOnAdLoaded;
            interstitialReward.OnAdOpening += HandleOnAdOpened;
        }

        private void UnsubscribeAds()
        {
            interstitialReg.OnAdClosed -= HandleOnAdClosed;
            interstitialReg.OnAdFailedToLoad -= HandleOnAdFailedToLoad;
            interstitialReg.OnAdLeavingApplication -= HandleOnAdLeavingApplication;
            interstitialReg.OnAdLoaded -= HandleOnAdLoaded;
            interstitialReg.OnAdOpening -= HandleOnAdOpened;

            interstitialVideo.OnAdClosed -= HandleOnAdClosed;
            interstitialVideo.OnAdFailedToLoad -= HandleOnAdFailedToLoad;
            interstitialVideo.OnAdLeavingApplication -= HandleOnAdLeavingApplication;
            interstitialVideo.OnAdLoaded -= HandleOnAdLoaded;
            interstitialVideo.OnAdOpening -= HandleOnAdOpened;

            interstitialReward.OnAdClosed -= HandleOnAdClosed;
            interstitialReward.OnAdFailedToLoad -= HandleOnAdFailedToLoad;
            interstitialReward.OnUserEarnedReward -= HandleOnEarned;
            interstitialReward.OnAdLoaded -= HandleOnAdLoaded;
            interstitialReward.OnAdOpening -= HandleOnAdOpened;
        }

        private void ShowInterstitial()
        {
            //Debug.Log("current interstitial = " + currentInterstitial);
            switch (currentInterstitial)
            {
                case InterstitialType.Default:
                    if (interstitialReg.IsLoaded())
                        interstitialReg.Show();
                    break;
                case InterstitialType.Video:
                    if (interstitialVideo.IsLoaded())
                        interstitialVideo.Show();
                    break;
                case InterstitialType.Reward:
                    if (interstitialReward.IsLoaded())
                        interstitialReward.Show();
                    break;
            }
        }

        private void LoadInterstitial(InterstitialAd interstitial)
        {
            var builder = new AdRequest.Builder();
            foreach (var d in testDevices.Devices)
            {
                builder.AddTestDevice(d);
            }
            AdRequest request = builder.Build();
            interstitial.LoadAd(request);
        }
        private void LoadInterstitial(RewardedAd interstitial)
        {
            var builder = new AdRequest.Builder();
            foreach (var d in testDevices.Devices)
            {
                builder.AddTestDevice(d);
            }
            AdRequest request = builder.Build();
            interstitial.LoadAd(request);
        }
        #endregion Methods

        #region Handlers

        private void ShowAD_Handler(object sender, GameEventArgs args)
        {
            currentInterstitial = (InterstitialType)args.intParam.Value;
            //Debug.Log("start interstitial");
            OkReward = args.okAction;
            CancelledReward = args.cancelAction;
            ShowInterstitial();
            //Debug.Log("end interstitial");

            interstitialReg.Destroy();
            interstitialVideo.Destroy();

            interstitialReg = new InterstitialAd(adData.InterstitialRegID);
            interstitialVideo = new InterstitialAd(adData.InterstitialVideoID);
            interstitialReward = new RewardedAd(adData.InterstitialRewardID);

            LoadInterstitial(interstitialReg);
            LoadInterstitial(interstitialVideo);
            LoadInterstitial(interstitialReward);
        }

        private void ResetCurrentInterstitial(object sender, GameEventArgs args)
        {
            currentInterstitial = 0;
        }

        public void HandleOnAdLoaded(object sender, System.EventArgs args)
        {
            Debug.Log("HandleAdLoaded event received");
        }

        public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
        {
            Debug.Log("HandleFailedToReceiveAd event received with message: "
                                + args.Message);
        }

        public void HandleOnAdFailedToLoad(object sender, AdErrorEventArgs args)
        {
            Debug.Log("HandleFailedToReceiveAd event received with message: "
                                + args.Message);
        }

        public void HandleOnAdOpened(object sender, System.EventArgs args)
        {
            AudioControl.Instance.Mute();
            Debug.Log("HandleAdOpened event received");
        }

        public void HandleOnAdClosed(object sender, System.EventArgs args)
        {
            if(currentInterstitial == InterstitialType.Reward)
            {
                CancelledReward?.Invoke();
                CancelledReward = delegate { };
            }
            AudioControl.Instance.Unmute();
            Debug.Log("HandleAdClosed event received");
        }

        public void HandleOnAdLeavingApplication(object sender, System.EventArgs args)
        {
            AudioControl.Instance.Unmute();
            Debug.Log("HandleAdLeavingApplication event received");
        }

        private void HandleOnEarned(object sender, Reward e)
        {
            CancelledReward = delegate { };
            OkReward?.Invoke();
            OkReward = delegate { };
        }
        #endregion Handlers
    }
}