using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using GoogleMobileAds.Api;
using UnityEngine.UI;

public class AbsManager : MonoBehaviour
{
    public MenuManager menuManager;

    private readonly string unitID = "ca-app-pub-3940256099942544/6300978111";
    private readonly string test_unitID = "ca-app-pub-3940256099942544/6300978111";

    private readonly string test_unitID_reward = "ca-app-pub-3940256099942544/5224354917";

    private BannerView banner;
    private RewardBasedVideoAd rewardBasedVideo;

    public int rewardAdAmountSoul = 3;

    private bool rewarded = false;

    void Start()
    {
        rewarded = false;

        rewardBasedVideo = RewardBasedVideoAd.Instance;

        rewardBasedVideo.OnAdRewarded += HandleRewardBasedVideoRewarded;
        InitBannerAd();
        if (!IsLoadedRewardAd())
        {
            tmp.text = "good";
            InitRewardAd();
        }
    }


    void Update()
    {
        if (rewarded)
        {
            menuManager.OnBtnRewardNo();
            menuManager.RunGetSoulPanelFadeIn();
            menuManager.getSoulPanelText.text = "GOOD !" + "\nYOU GOT 30 SOUL !";

            DatabaseManager.UpdateMoney(30);

            rewarded = false;
        }
    }


    private void InitBannerAd()
    {
        string id = Debug.isDebugBuild ? test_unitID : unitID;
        banner = new BannerView(id, AdSize.MediumRectangle, AdPosition.Center);
        AdRequest request = new AdRequest.Builder().Build();
        banner.LoadAd(request);
        banner.Hide();
    }


    private void InitRewardAd()
    {
        string id = test_unitID_reward;
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
            StartCoroutine(tmpRewardAd());
        }
        else
        {
            menuManager.OnBtnRewardNo();
            menuManager.RunGetSoulPanelFadeIn();
            menuManager.getSoulPanelText.text = "SORRY, NOT READY AD" + "\nTRY AGAIN";
        }
    }


    IEnumerator tmpRewardAd()
    {
        yield return new WaitForSeconds(3f);
        InitRewardAd();
        tmp.text = "OK";
    }


    public Text tmp;
    public void HandleRewardBasedVideoRewarded(object sender, Reward args)
    {
        rewarded = true;
    }
}