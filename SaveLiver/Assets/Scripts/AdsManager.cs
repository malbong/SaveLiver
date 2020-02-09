using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using GoogleMobileAds.Api;
using UnityEngine.UI;

public class AdsManager : MonoBehaviour
{
    public MenuManager menuManager;

    private readonly string unitID = "ca-app-pub-6349048174225682/6935715262";
    private readonly string test_unitID = "ca-app-pub-3940256099942544/6300978111";

    private readonly string unitID_reward = "ca-app-pub-6349048174225682/6042603281";
    private readonly string test_unitID_reward = "ca-app-pub-3940256099942544/5224354917";

    private BannerView banner;
    private RewardBasedVideoAd rewardBasedVideo;

    public int rewardAdAmountSoul = 3;

    private bool rewarded = false;

    public bool seeingGetSoulText = false;

    void Start()
    {
        rewarded = false;

        rewardBasedVideo = RewardBasedVideoAd.Instance;

        rewardBasedVideo.OnAdRewarded += HandleRewardBasedVideoRewarded;

        InitBannerAd();
        if (!IsLoadedRewardAd())
        {
            InitRewardAd();
        }
    }


    void Update()
    {
        if (rewarded)
        {
            seeingGetSoulText = true;
            menuManager.OnBtnRewardNo();
            menuManager.RunGetSoulPanelFadeIn();
            menuManager.getSoulPanelText.text = "GOOD !" + "\nYOU GOT 30 SOUL !";
            PlayerInformation.SetFinalSeconds(120);
            PlayerInformation.IsWatched = true;
            PlayerInformation.AdTimeCheck();

            DatabaseManager.UpdateMoney(30);
            InitRewardAd();

            rewarded = false;
        }
    }


    public void EventMinus()
    {
        rewardBasedVideo.OnAdRewarded -= HandleRewardBasedVideoRewarded;
    }


    private void InitBannerAd()
    {
        string id = Debug.isDebugBuild ? test_unitID : unitID;
        banner = new BannerView(id, AdSize.MediumRectangle, AdPosition.Center);
        AdRequest request = new AdRequest.Builder().Build();
        banner.LoadAd(request);
        banner.Hide();
    }


    public void InitRewardAd()
    {
        string id = Debug.isDebugBuild ? test_unitID_reward : unitID_reward;
        AdRequest request = new AdRequest.Builder().Build();
        rewardBasedVideo.LoadAd(request, id);
    }


    public void ToggleAd(bool active)
    {
        if (active) { banner.Show(); }
        else { banner.Hide(); }
    }


    public void DestroyBannerAd()
    {
        banner.Destroy();
    }


    public bool IsLoadedRewardAd()
    {
        return rewardBasedVideo.IsLoaded();
    }


    public void ShowRewardAd()
    {
        if (rewardBasedVideo.IsLoaded())
        {
            rewardBasedVideo.Show();
            //InitRewardAd(); // 새 광고 로드
        }
        else
        {
            menuManager.OnBtnRewardNo();
            menuManager.RunGetSoulPanelFadeIn();
            menuManager.getSoulPanelText.text = "SORRY, NOT READY AD" + "\nTRY AGAIN";
        }
    }


    public void HandleRewardBasedVideoRewarded(object sender, Reward args)
    {
        rewarded = true;
    }
}