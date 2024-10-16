using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using System;

public class AdManager : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
{
    //public Score score;
    //  public GameStateController gameStateController;

    [SerializeField] public string androidGameId = "YOUR_ANDROID_GAME_ID";
    [SerializeField] public string iosGameId = "YOUR_IOS_GAME_ID";


    public string gameId;
    public string interstitialAdUnitId = "Interstitial_Ad";
    public string bannerAdUnitId = "Banner_Ad";
    public string rewardedAdUnitId = "Rewarded_Ad";

    private bool isInterstitialReady = false;
    private bool isRewardedReady = false;

    void Start()
    {
        InitializeAds();
    }

    public void InitializeAds()
    {
        gameId = (Application.platform == RuntimePlatform.IPhonePlayer) ? iosGameId : androidGameId;
        Advertisement.Initialize(gameId, false, this);
    }

    public void OnInitializationComplete()
    {
        Debug.Log("Unity Ads initialization complete.");
        LoadInterstitialAd();
        LoadBannerAd();
        LoadRewardedAd();
        ShowBannerAd();
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
    }

    public void LoadInterstitialAd()
    {
        Advertisement.Load(interstitialAdUnitId, this);
    }

    public void ShowInterstitialAd()
    {
        if (isInterstitialReady)
        {
            Advertisement.Show(interstitialAdUnitId, this);
        }
        else
        {
            Debug.Log("Interstitial ad not ready.");
        }
    }

    public void LoadBannerAd()
    {
        Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);
        Advertisement.Banner.Load(bannerAdUnitId, new BannerLoadOptions
        {
            loadCallback = OnBannerLoaded,
            errorCallback = OnBannerError
        });
    }

    public void ShowBannerAd()
    {
        Advertisement.Banner.Show(bannerAdUnitId);
    }

    public void HideBannerAd()
    {
        Advertisement.Banner.Hide();
    }

    private void OnBannerLoaded()
    {
        Debug.Log("Banner loaded");
    }

    private void OnBannerError(string message)
    {
        Debug.Log($"Banner Error: {message}");
    }

    public void LoadRewardedAd()
    {
        Advertisement.Load(rewardedAdUnitId, this);
    }

    public void ShowRewardedAd()
    {
        if (isRewardedReady)
        {
            Advertisement.Show(rewardedAdUnitId, this);
        }
        else
        {
            Debug.Log("Rewarded ad not ready.");
        }
    }

    public void OnUnityAdsAdLoaded(string adUnitId)
    {
        if (adUnitId.Equals(interstitialAdUnitId))
        {
            isInterstitialReady = true;
        }
        else if (adUnitId.Equals(rewardedAdUnitId))
        {
            isRewardedReady = true;
        }
        Debug.Log($"Ad Loaded: {adUnitId}");
    }

    public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
    {
        Debug.Log($"Error loading Ad Unit {adUnitId}: {error.ToString()} - {message}");
    }

    public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
    {
        Debug.Log($"Error showing Ad Unit {adUnitId}: {error.ToString()} - {message}");
    }

    public void OnUnityAdsShowStart(string adUnitId) { }
    public void OnUnityAdsShowClick(string adUnitId) { }
    public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState) { }
}


