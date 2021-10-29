using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GreedyGame;
using System;

public class Admanager : MonoBehaviour
{
    public static Admanager Instance;    
    public bool appOpenAdFinished;
    public string RewardAdStatus;
    int coins;
    //public int score;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        Initialisation();
    }
    void Start()
    {        
        if(!(PlayerPrefs.GetInt("Coins")<=0)) 
        {
            coins = PlayerPrefs.GetInt("Coins");
        } 
    }
    void Initialisation()
    {
        GreedyGameAds.Instance.Init(callback);
    }

    private void callback(bool intitialise, string message)
    {
        if (intitialise)
        {
            LoadAppOpen();
            LoadInterstitial();
            LoadReward();            
        }
        else
        {

        }
    }

    public void LoadBanner()
    {
        GreedyGameAds.Instance.LoadBanner("float-9215", BannerSize.ADAPTIVE_BANNER, BannerPosition.BOTTOM);
    }  
    public void DestroyBanner()
    {
        GreedyGameAds.Instance.DestroyBanner();
    }
    public void LoadInterstitial()
    {
        GreedyGameAds.Instance.LoadInterstitialAd("float-8044");
        GreedyGameAds.Instance.interstitialAd.OnAdClosed += InterstitialAdClosed;
    }

    private void InterstitialAdClosed()
    {
        LoadBanner();
        LoadInterstitial();
    }

    public void ShowInterstitial()
    {
        GreedyGameAds.Instance.ShowInterstitialAd();
    }
    void LoadReward()
    {
        GreedyGameAds.Instance.LoadRewardAd("float-9078");           
    }  

    public void ShowReward()
    {
        GreedyGameAds.Instance.ShowRewardAd(GetReward);
    }

    private void GetReward(bool canReward)
    {
        if (canReward)
        {
            LoadReward();
            FindObjectOfType<TheStackver2>().Rewards();
        }
    }

    void LoadAppOpen()
    {
        GreedyGameAds.Instance.LoadAppOpen("float-8289");       
        if(Application.platform == RuntimePlatform.Android)
        {
            GreedyGameAds.Instance.appOpenAd.OnAdLoaded += AppOpenOnLoaded;
            GreedyGameAds.Instance.appOpenAd.OnAdClosed += AppOpenOnAdClosed;
            GreedyGameAds.Instance.appOpenAd.OnAdFailedToLoad += AppOpenOnAdFailedToLoad;
        }
        else
        {
            AppOpenOnAdClosed();
        }
      
    }

    private void AppOpenOnAdFailedToLoad(string obj)
    {
        LoadBanner();
        appOpenAdFinished = true;
        FindObjectOfType<MainMenu>().DisableLoadPanel();
    }

    private void AppOpenOnAdClosed()
    {
        LoadBanner();
        appOpenAdFinished = true;
        FindObjectOfType<MainMenu>().DisableLoadPanel();
    }

    private void AppOpenOnLoaded()
    {
        ShowAppOpen();
    }

    void ShowAppOpen()
    {
        GreedyGameAds.Instance.ShowAppOpen();
    }
   
}
