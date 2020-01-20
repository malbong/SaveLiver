﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;

public class MenuManager : MonoBehaviour
{
    public GameObject quitOuterPannel;
    public GameObject quitPanel;
    private bool quitFadeInRunning = false;
    private bool quitFadeOutRunning = false;
    private bool seeingQuitPanel = false;

    public GameObject rewardOuterPannel;
    public GameObject rewardPanel;
    private bool rewardFadeInRunning = false;
    private bool rewardFadeOutRunning = false;
    private bool seeingRewardPanel = false;

    public GameObject getSoulOuterPanel;
    public GameObject getSoulPanel;
    private bool getSoulFadeInRunning = false;
    private bool getSoulFadeOutRunning = false;
    private bool seeingGetSoulPanel = false;
    public Text getSoulPanelText;

    public GameObject storeOuterPanel;
    public GameObject storePanel;
    private bool storeFadeInRunning = false;
    private bool storeFadeOutRunning = false;
    private bool seeingStorePanel = false;

    public AbsManager absManager;
    public SceneTransition sceneTransition;

    public Text soulText;


    void Update()
    {
        soulText.text = PlayerInformation.SoulMoney.ToString(); // Soul Money 표시

        // 안드로이드에서 뒤로가기 누르면 종료되는 처리
        //if(Application.platform == RuntimePlatform.Android)
        //{
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (sceneTransition.isSceneTransitionRunning()) return;
            // Store 창 보고 있다면
            if (seeingStorePanel)
            {
                StartCoroutine(storePanelFadeOut());
                return;
            }
            // Get Soul 창 보고 있다면
            if (seeingGetSoulPanel)
            {
                StartCoroutine(GetSoulPanelFadeOut());
                return;
            }
            // reward Ad 창 보고 있다면
            if (seeingRewardPanel)
            {
                StartCoroutine(GameRewardPanelFadeOut());
                return;
            }
            // Quit 창을 보고 있다면
            if (seeingQuitPanel)
            {
                StartCoroutine(GameQuitPanelFadeOut());
                return;
            }
            StartCoroutine(GameQuitPanelFadeIn());
        }
        //}
    }

    public void test()
    {
        PlayerInformation.SoulMoney += 1;
    }

    public void test2()
    {
        PlayerInformation.UpdateMoney(3);
    }


    public void OnBtnPlay()
    {
        sceneTransition.LoadPlayScene();
    }



    // Quit Pannel

    public void OnBtnQuitYes()
    {
        if (!quitFadeInRunning && !quitFadeOutRunning)
        {
            Application.Quit();
        }
    }

    public void OnBtnQuitNo()
    {
        if (!quitFadeInRunning && !quitFadeOutRunning)
        {
            StartCoroutine(GameQuitPanelFadeOut());
        }
    }
    public void OnBtnOuterQuitNo()
    {
        if (seeingQuitPanel)
        {
            StartCoroutine(GameQuitPanelFadeOut());
        }
    }



    // Reward Panel

    public void OnBtnRewardPanel()
    {
        if (!seeingRewardPanel)
        {
            StartCoroutine(GameRewardPanelFadeIn());
        }
    }

    public void OnBtnRewardWatchAd()
    {
        if (!rewardFadeInRunning && !rewardFadeOutRunning)
        {
            absManager.ShowRewardAd();
        }
    }

    public void OnBtnRewardNo()
    {
        if (!rewardFadeInRunning && !rewardFadeOutRunning)
        {
            StartCoroutine(GameRewardPanelFadeOut());
        }
    }

    public void OnBtnOuterRewardNo()
    {
        if (seeingRewardPanel)
        {
            StartCoroutine(GameRewardPanelFadeOut());
        }
    }



    // Get Soul Panel

    public void RunGetSoulPanelFadeIn()
    {
        StartCoroutine(GetSoulPanelFadeIn());
    }

    public void OnBtnGetSoulOK()
    {
        if (seeingGetSoulPanel)
        {
            StartCoroutine(GetSoulPanelFadeOut());
        }
    }



    // Store Panel

    public void OnBtnStorePanel()
    {
        StartCoroutine(StorePanelFadeIn());
    }

    public void OnBtnStoreX()
    {
        if (seeingStorePanel)
        {
            StartCoroutine(storePanelFadeOut());
        }
    }


    IEnumerator GameQuitPanelFadeIn()
    {
        if (quitFadeOutRunning) yield break;

        quitPanel.SetActive(true);
        quitFadeInRunning = true;

        Image quitPanelImage = quitPanel.GetComponent<Image>();
        Color tmpColor = quitPanelImage.color;

        while (true)
        {
            quitPanel.transform.localScale -= new Vector3(0.2f, 0.25f, 0.2f);

            tmpColor.a += 0.2f;
            quitPanelImage.color = tmpColor;
            if (quitPanel.transform.localScale.x <= 1.6f) break;

            yield return new WaitForSecondsRealtime(0.01f);
        }

        quitPanel.transform.localScale = new Vector3(1.6f, 1.25f, 1.4f);
        tmpColor.a = 1f;
        quitPanelImage.color = tmpColor;

        absManager.ToggleAd(true);
        seeingQuitPanel = true;
        quitOuterPannel.SetActive(true);

        quitFadeInRunning = false;
    }


    IEnumerator GameQuitPanelFadeOut()
    {
        if (quitFadeInRunning) yield break;

        absManager.ToggleAd(false);
        quitFadeOutRunning = true;
        seeingQuitPanel = false;
        quitOuterPannel.SetActive(false);

        Image quitPanelImage = quitPanel.GetComponent<Image>();
        Color tmpColor = quitPanelImage.color;

        while (true)
        {
            quitPanel.transform.localScale -= new Vector3(0.2f, 0.2f, 0.2f);

            tmpColor.a -= 0.2f;
            quitPanelImage.color = tmpColor;

            if (quitPanel.transform.localScale.x <= 0) break;

            yield return new WaitForSecondsRealtime(0.01f);
        }

        quitPanel.transform.localScale = new Vector3(2, 2, 2);
        tmpColor.a = 0f;
        quitPanelImage.color = tmpColor;

        quitFadeOutRunning = false;
        quitPanel.SetActive(false);
    }


    IEnumerator GameRewardPanelFadeIn()
    {
        if (rewardFadeOutRunning) yield break;

        rewardPanel.SetActive(true);
        rewardFadeInRunning = true;

        Image rewardPanelImage = rewardPanel.GetComponent<Image>();
        Color tmpColor = rewardPanelImage.color;

        while (true)
        {
            rewardPanel.transform.localScale -= new Vector3(0.2f, 0.2f, 0.2f);

            tmpColor.a += 0.2f;
            rewardPanelImage.color = tmpColor;
            if (rewardPanel.transform.localScale.x <= 1f) break;

            yield return new WaitForSecondsRealtime(0.01f);
        }

        rewardPanel.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        tmpColor.a = 1f;
        rewardPanelImage.color = tmpColor;

        seeingRewardPanel = true;
        rewardOuterPannel.SetActive(true);

        rewardFadeInRunning = false;
    }


    IEnumerator GameRewardPanelFadeOut()
    {
        if (rewardFadeInRunning) yield break;

        rewardFadeOutRunning = true;
        seeingRewardPanel = false;
        rewardOuterPannel.SetActive(false);

        Image rewardPanelImage = rewardPanel.GetComponent<Image>();
        Color tmpColor = rewardPanelImage.color;

        while (true)
        {
            rewardPanel.transform.localScale -= new Vector3(0.2f, 0.2f, 0.2f);

            tmpColor.a -= 0.2f;
            rewardPanelImage.color = tmpColor;

            if (rewardPanel.transform.localScale.x <= 0) break;

            yield return new WaitForSecondsRealtime(0.01f);
        }

        rewardPanel.transform.localScale = new Vector3(2, 2, 2);
        tmpColor.a = 0f;
        rewardPanelImage.color = tmpColor;

        rewardFadeOutRunning = false;
        rewardPanel.SetActive(false);
    }


    IEnumerator GetSoulPanelFadeIn()
    {
        if (getSoulFadeOutRunning) yield break;

        getSoulPanel.SetActive(true);
        getSoulFadeInRunning = true;

        Image getSoulPanelImage = getSoulPanel.GetComponent<Image>();
        Color tmpColor = getSoulPanelImage.color;

        while (true)
        {
            getSoulPanel.transform.localScale -= new Vector3(0.2f, 0.2f, 0.2f);

            tmpColor.a += 0.2f;
            getSoulPanelImage.color = tmpColor;
            if (getSoulPanel.transform.localScale.x <= 1f) break;

            yield return new WaitForSecondsRealtime(0.01f);
        }

        getSoulPanel.transform.localScale = new Vector3(1f, 1f, 1f);
        tmpColor.a = 1f;
        getSoulPanelImage.color = tmpColor;

        seeingGetSoulPanel = true;
        getSoulOuterPanel.SetActive(true);

        getSoulFadeInRunning = false;
    }


    IEnumerator GetSoulPanelFadeOut()
    {
        if (getSoulFadeInRunning) yield break;

        getSoulFadeOutRunning = true;
        seeingGetSoulPanel = false;
        getSoulOuterPanel.SetActive(false);

        Image getSoulPanelImage = getSoulPanel.GetComponent<Image>();
        Color tmpColor = getSoulPanelImage.color;

        while (true)
        {
            getSoulPanel.transform.localScale -= new Vector3(0.2f, 0.2f, 0.2f);

            tmpColor.a -= 0.2f;
            getSoulPanelImage.color = tmpColor;

            if (getSoulPanel.transform.localScale.x <= 0) break;

            yield return new WaitForSecondsRealtime(0.01f);
        }

        getSoulPanel.transform.localScale = new Vector3(2, 2, 2);
        tmpColor.a = 0f;
        getSoulPanelImage.color = tmpColor;

        getSoulFadeOutRunning = false;
        getSoulPanel.SetActive(false);
    }


    IEnumerator StorePanelFadeIn()
    {
        if (storeFadeOutRunning) yield break;

        storePanel.SetActive(true);
        storeFadeInRunning = true;

        Image storePanelImage = storePanel.GetComponent<Image>();
        Color tmpColor = storePanelImage.color;

        while (true)
        {
            storePanel.transform.localScale -= new Vector3(0.2f, 0.2f, 0.2f);

            tmpColor.a += 0.2f;
            storePanelImage.color = tmpColor;
            if (storePanel.transform.localScale.x <= 1f) break;

            yield return new WaitForSecondsRealtime(0.01f);
        }

        storePanel.transform.localScale = new Vector3(1f, 1f, 1f);
        tmpColor.a = 1f;
        storePanelImage.color = tmpColor;

        seeingStorePanel = true;
        storeOuterPanel.SetActive(true);

        storeFadeInRunning = false;
    }


    IEnumerator storePanelFadeOut()
    {
        if (storeFadeInRunning) yield break;

        storeFadeOutRunning = true;
        seeingStorePanel = false;
        storeOuterPanel.SetActive(false);

        Image storePanelImage = storePanel.GetComponent<Image>();
        Color tmpColor = storePanelImage.color;

        while (true)
        {
            storePanel.transform.localScale -= new Vector3(0.2f, 0.2f, 0.2f);

            tmpColor.a -= 0.2f;
            storePanelImage.color = tmpColor;

            if (storePanel.transform.localScale.x <= 0) break;

            yield return new WaitForSecondsRealtime(0.01f);
        }

        storePanel.transform.localScale = new Vector3(2, 2, 2);
        tmpColor.a = 0f;
        storePanelImage.color = tmpColor;

        storeFadeOutRunning = false;
        storePanel.SetActive(false);
    }
}
