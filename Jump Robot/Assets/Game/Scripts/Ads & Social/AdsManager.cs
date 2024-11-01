﻿using System;
using UnityEngine;
//using GoogleMobileAds;     //...........................................Uncomment this line after importing google admob sdk
//using GoogleMobileAds.Api;
using System.Collections;//...........................................Uncomment this line after importing google admob sdk

/// <summary>
/// Script that handle your banner , noraml and reward ads
/// </summary>

//this is for IAP ads
//check link for more details :- https://firebase.google.com/docs/admob/android/games#in-app_purchase_ads


// Example script showing how to invoke the Google Mobile Ads Unity plugin.
public class AdsManager : MonoBehaviour
{
    public static AdsManager instance;

    [HideInInspector]
    public ManageVariables vars;

    void OnEnable()
    {
        vars = Resources.Load("ManageVariablesContainer") as ManageVariables;
    }

    public bool isTesting = true;

    [Header("For Banner")]
    public string Android_Banner_ID = "";
    public string IOS_Banner_ID = "";
    [Header("For Interstitial")]
    public string Android_Interstitial_ID = "";
    public string IOS_Interstitial_ID = "";

    [Header("Dont Know ID try \"Device ID Finder for AdMob\" App")]
    public string Device_ID = "";

        //...........................................Uncomment this line after importing google admob sdk

   // private BannerView bannerView;
  //  private InterstitialAd interstitial;
    private float deltaTime = 0.0f;
    private static string outputMessage = "";

    public static string OutputMessage
    {
        set { outputMessage = value; }
    }

    void Awake()
    {
        MakeSingleton();
    }

    void MakeSingleton()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }


    void Start()
    {
        //here we request for all the ads we need
        RequestBanner();
        RequestInterstitial();
    }

    void Update()
    {
        // Calculate simple moving average for time to render screen. 0.1 factor used as smoothing
        // value.
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
    }

    //.............................................................Methods used to request for ads
    //we use this methode to get the banner ads
    
    private void RequestBanner()
    {
#if UNITY_EDITOR
        string adUnitId = "unused";
#endif


        if (vars.bannerAdPoisiton == 0)
            {
                // Create a 320x50 banner at the top of the screen.
               // bannerView = new BannerView(adUnitId, AdSize.SmartBanner, AdPosition.Bottom);
            }
            else if (vars.bannerAdPoisiton == 1)
            {
                // Create a 320x50 banner at the top of the screen.
              //  bannerView = new BannerView(adUnitId, AdSize.SmartBanner, AdPosition.BottomLeft);
            }
            else if (vars.bannerAdPoisiton == 2)
            {
                // Create a 320x50 banner at the top of the screen.
              //  bannerView = new BannerView(adUnitId, AdSize.SmartBanner, AdPosition.BottomRight);
            }
            else if (vars.bannerAdPoisiton == 3)
            {
                // Create a 320x50 banner at the top of the screen.
               // bannerView = new BannerView(adUnitId, AdSize.SmartBanner, AdPosition.Top);
            }
            else if (vars.bannerAdPoisiton == 4)
            {
                // Create a 320x50 banner at the top of the screen.
               // bannerView = new BannerView(adUnitId, AdSize.SmartBanner, AdPosition.TopLeft);
            }
            else if (vars.bannerAdPoisiton == 5)
            {
                // Create a 320x50 banner at the top of the screen.
                //bannerView = new BannerView(adUnitId, AdSize.SmartBanner, AdPosition.TopRight);
            }
        // Create an empty ad request.
        //AdRequest request = new AdRequest.Builder().Build();
                
        // Register for ad events.
        //bannerView.OnAdLoaded += HandleAdLoaded;
        //bannerView.OnAdFailedToLoad += HandleAdFailedToLoad;

        // Load a banner ad.
        //replace createAdRequest with request when the games is submitting to store
        if (isTesting)
        {
            //bannerView.LoadAd(createAdRequest());
        }
        else
        {
            //bannerView.LoadAd(request);
        }
    }

    //we use this methode to get the Interstitial ads
    private void RequestInterstitial()
    {
#if UNITY_EDITOR
        string adUnitId = "unused";
#endif



        // Create an interstitial.
        //interstitial = new InterstitialAd(adUnitId);
        // Create an empty ad request.
        //AdRequest request = new AdRequest.Builder().Build();

        // Register for ad events.
        //interstitial.OnAdLoaded += HandleInterstitialLoaded;
        //interstitial.OnAdFailedToLoad += HandleInterstitialFailedToLoad;

        // Load an interstitial ad.
        //replace createAdRequest with request when the games is submitting to store
        if (isTesting)
        {
            //interstitial.LoadAd(createAdRequest());
        }
        else
        {
            //nterstitial.LoadAd(request);
        }
    }

    // the following method is used when we are testing the ads
    //private AdRequest createAdRequest()
    //{
        //return new AdRequest.Builder()
                //.AddTestDevice(AdRequest.TestDeviceSimulator)
                //add you device ID below if ubable to find ID try "Device ID Finder for AdMob" app
                //link:- https://play.google.com/store/apps/details?id=pe.go_com.admobdeviceidfinder&hl=en
                //.AddTestDevice(Device_ID)
                //.Build();
    //}

    //.............................................................Methods used to show for ads
    //use this methode to show ads
    public void ShowInterstitial()
    {

#if UNITY_EDITOR
        Debug.Log("Interstitial Working");
#endif


    }

    //this methode is used to call the banner ads
    public void ShowBannerAds()
    {
      //  bannerView.Show();
    }

    //this methode is used to hide banner ads
    public void HideBannerAds()
    {
      //  bannerView.Hide();
    }

    //this methode is used to destroy banner ads
    public void DestroyBannerAds()
    {
      //  bannerView.Destroy();
    }

    //.............................................................Methods used to show for ads


    //call banc handlers are used to detect the status of ads
    //for example if you are providing reward to the player on seeing reward ads then you can check wheather the
    //player has seen the complete ad and then only provide him the reward

    //................................................................Interstitial callback handlers
#region Interstitial callback handlers

    public void HandleInterstitialLoaded(object sender, EventArgs args)
    {
        print("HandleInterstitialLoaded event received.");
    }

   // public void HandleInterstitialFailedToLoad(object sender, AdFailedToLoadEventArgs args)
 //   {
        //print("HandleInterstitialFailedToLoad event received with message: " + args.Message);
  //  }

    private void print(object p)
    {
        throw new NotImplementedException();
    }

    public void HandleInterstitialOpened(object sender, EventArgs args)
    {
        print("HandleInterstitialOpened event received");
    }

    void HandleInterstitialClosing(object sender, EventArgs args)
    {
        print("HandleInterstitialClosing event received");
    }

    public void HandleInterstitialClosed(object sender, EventArgs args)
    {
        print("HandleInterstitialClosed event received");
    }

    public void HandleInterstitialLeftApplication(object sender, EventArgs args)
    {
        print("HandleInterstitialLeftApplication event received");
    }

#endregion
    //................................................................end of Interstitial callback handlers

    //................................................................Banner callback handlers
#region Banner callback handlers

    public void HandleAdLoaded(object sender, EventArgs args)
    {
        print("HandleAdLoaded event received.");
    }

  //  public void HandleAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
  //  {
        //print("HandleAdFailedToLoad event received with message: " + args.Message);
  //  }

    public void HandleAdOpened(object sender, EventArgs args)
    {
        print("HandleAdOpened event received");
    }

    void HandleAdClosing(object sender, EventArgs args)
    {
        print("HandleAdClosing event received");
    }

    public void HandleAdClosed(object sender, EventArgs args)
    {
        print("HandleAdClosed event received");
    }

    public void HandleAdLeftApplication(object sender, EventArgs args)
    {
        print("HandleAdLeftApplication event received");
    }

    #endregion
}
