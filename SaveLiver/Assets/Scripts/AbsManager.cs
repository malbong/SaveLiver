using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using GoogleMobileAds.Api;

public class AbsManager : MonoBehaviour
{
    private readonly string unitID = "ca-app-pub-3940256099942544/6300978111";
    private readonly string test_unitID = "ca-app-pub-3940256099942544/6300978111";

    private BannerView banner;

    void Start()
    {
        InitAd();
    }


    private void InitAd()
    {
        string id = Debug.isDebugBuild ? test_unitID : unitID;

        banner = new BannerView(id, AdSize.MediumRectangle, AdPosition.Center);

        AdRequest request = new AdRequest.Builder().Build();

        banner.LoadAd(request);

        banner.Hide();
    }


    public void ToggleAd(bool active)
    {
        if (active)
        {
            banner.Show();
        }
        else
        {
            banner.Hide();
        }
    }


    public void DestroyAd()
    {
        banner.Destroy();
    }
}
