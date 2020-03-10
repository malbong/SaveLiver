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
    private RewardedAd rewardedAd;

    public int rewardAdAmountSoul = 100;

    private bool rewarded = false;

    public bool seeingGetSoulText = false;

    void Start()
    {
        rewarded = false;

        InitBannerAd();
        InitRewardAd();
    }


    void Update()
    {
        if (rewarded)
        {
            seeingGetSoulText = true;
            menuManager.OnBtnRewardNo();
            menuManager.RunGetSoulPanelFadeIn();
            menuManager.getSoulPanelText.text = "GOOD !" + "\nYOU GOT 100 SOUL !";
            PlayerInformation.SetFinalSeconds(120);
            PlayerInformation.IsWatched = true;
            PlayerInformation.AdTimeCheck();

            DatabaseManager.UpdateMoney(rewardAdAmountSoul);

            rewarded = false;
        }
    }

    
    public void EventMinus()
    {
        rewardedAd.OnUserEarnedReward -= HandleUserEarnedReward;
        rewardedAd.OnAdClosed -= HandleRewardedAdClosed;
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

        rewardedAd = new RewardedAd(id);
        AdRequest request = new AdRequest.Builder().Build();
        rewardedAd.LoadAd(request);

        rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        // 사용자가 광고를 끝까지 시청했을 때
        rewardedAd.OnAdClosed += HandleRewardedAdClosed;
        // 사용자가 광고를 중간에 닫았을 때
    }


    public void CreateAndLoadRewardedAd() // 광고 다시 로드하는 함수
    {
        rewardedAd = new RewardedAd(unitID_reward);
        AdRequest request = new AdRequest.Builder().Build();
        rewardedAd.LoadAd(request);

        rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        rewardedAd.OnAdClosed += HandleRewardedAdClosed;
    }


    private void HandleRewardedAdClosed(object sender, EventArgs e)
    {
        CreateAndLoadRewardedAd();
    }

    private void HandleUserEarnedReward(object sender, Reward e)
    {
        rewarded = true;
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
        return rewardedAd.IsLoaded();
    }


    public void ShowRewardAd()
    {
        if (IsLoadedRewardAd())
        {
            rewardedAd.Show();
        }
        else
        {
            menuManager.OnBtnRewardNo();
            menuManager.RunGetSoulPanelFadeIn();
            menuManager.getSoulPanelText.text = "SORRY, NOT READY AD" + "\nTRY AGAIN";
        }
    }
}