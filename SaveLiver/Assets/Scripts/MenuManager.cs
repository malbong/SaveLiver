using System.Collections;
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

    public StoreManager storeManager;
    public GameObject storeOuterPanel;
    public GameObject storePanel;
    private bool storeFadeInRunning = false;
    private bool storeFadeOutRunning = false;
    private bool seeingStorePanel = false;

    public GameObject chargeOuterPanel;
    public GameObject chargePanel;
    private bool chargeFadeInRunning = false;
    private bool chargeFadeOutRunning = false;
    public bool seeingChargePanel = false;

    public AdsManager adsManager;
    public SceneTransition sceneTransition;

    public Text soulText;
    public Text ChargeText;

    public bool seeingTimer = false;

    public GameObject levelOuterPanel;
    public Animator levelAnimator;
    public RuntimeAnimatorController levelIn;
    public RuntimeAnimatorController levelOut;
    public Text HardBestScoreText;
    public Text EasyBestScoreText;
    private bool seeingLevel = false;

    public Slider EasyDestinationSlider;
    public Slider HardDestinationSlider;
    public GameObject EasyDesObj;
    public GameObject HardDesObj;
    public GameObject clearEasyImage;
    public GameObject clearHardImage;

    private const int END_SCORE = 3000;


    void Start()
    {
        DatabaseManager.UpdateMoney(0);
        DatabaseManager.GetScore(true);
        DatabaseManager.GetScore(false);

        //PlayerInformation.BestScore = DatabaseManager.GetScore(true);
        //PlayerInformation.EasyBestScore = DatabaseManager.GetScore(false);
        if (PlayerInformation.IsSawLogo)
        {
            SetDestination();
        }
    }


    void Update()
    {
        if (PlayerInformation.IsWatched) // 광고를 보았다면 3분 카운트.
        {
            PlayerInformation.AdTimeCheck();

            if (!adsManager.seeingGetSoulText)
            {
                getSoulPanelText.text = "Wait for next ad" + "\n"
                    + string.Format("{0:D2}", PlayerInformation.Minutes)
                    + " : "
                    + string.Format("{0:D2}", PlayerInformation.FinalSeconds);
            }
        }
        else if (!PlayerInformation.IsWatched && seeingTimer) // 타이머 패널 띄우고 있는 와중에 시간이 다 된다면,
        {
            StartCoroutine(GetSoulPanelFadeOut());
            StartCoroutine(GameRewardPanelFadeIn());
        }


        soulText.text = string.Format("{0:N0}", PlayerInformation.SoulMoney); // Soul Money 표시
        HardBestScoreText.text = string.Format("{0:N0}", PlayerInformation.BestScore);
        EasyBestScoreText.text = string.Format("{0:N0}", PlayerInformation.EasyBestScore);
        EasyDestinationSlider.value = PlayerInformation.EasyBestScore;
        HardDestinationSlider.value = PlayerInformation.BestScore;


        // 안드로이드에서 뒤로가기 누르면 종료되는 처리
        //if(Application.platform == RuntimePlatform.Android)
        //{
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // 로고 보고 있다면
            if (!PlayerInformation.IsSawLogo)
            {
                return;
            }

            SoundManager.instance.ButtonClick();
            // 난이도 창 보고 있다면
            if (seeingLevel)
            {
                OnBtnLevelOut();
                return;
            }
            // 구매 창 보고 있다면
            if (seeingChargePanel)
            {
                StartCoroutine(ChargePanelFadeOut());
                return;
            }
            // Store 창 보고 있다면
            if (seeingStorePanel && !seeingChargePanel)
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

            // help 창을 보고 있다면
            if (SettingsManager.instance.seeingHelpPanel)
            {
                SettingsManager.instance.OnHelpPanelExitButton();
                return;
            }
            //setting 창을 보고 있다면 
            if (SettingsManager.instance.seeingSettingPanel)
            {
                SettingsManager.instance.OnSettingsExitButton();
                return;
            }
            if (!quitFadeInRunning && !rewardFadeInRunning && !getSoulFadeInRunning
                && !storeFadeInRunning && !chargeFadeInRunning && !sceneTransition.isSceneTransitionRunning())
            {
                StartCoroutine(GameQuitPanelFadeIn());
            }
        }
        //}
    }


    public void SetDestination()
    {
        if (PlayerInformation.BestScore >= END_SCORE)
        {
            clearHardImage.SetActive(true);
            HardDesObj.SetActive(false);
        }
        if (PlayerInformation.EasyBestScore >= END_SCORE)
        {
            clearEasyImage.SetActive(true);
            EasyDesObj.SetActive(false);
        }
    }


    public void OnBtnPlay()
    {
        SoundManager.instance.ButtonClick();
        seeingLevel = true;
        levelAnimator.runtimeAnimatorController = levelIn;
        levelAnimator.SetTrigger("In");
        levelOuterPanel.SetActive(true);
    }

    public void OnBtnHard()
    {
        //adsManager.EventMinus();
        SoundManager.instance.ButtonClick();
        PlayerInformation.IsHard = true;
        SceneManager.LoadScene(1);
        //sceneTransition.LoadPlayScene();
    }

    public void OnBtnEasy()
    {
        //adsManager.EventMinus();
        SoundManager.instance.ButtonClick();
        PlayerInformation.IsHard = false;
        SceneManager.LoadScene(1);
        //sceneTransition.LoadPlayScene();
    }

    public void OnBtnLevelOut()
    {
        SoundManager.instance.ButtonClick();
        levelAnimator.runtimeAnimatorController = levelOut;
        levelAnimator.SetTrigger("Out");
        levelOuterPanel.SetActive(false);
        seeingLevel = false;
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
            SoundManager.instance.ButtonClick();
            StartCoroutine(GameQuitPanelFadeOut());
        }
    }
    public void OnBtnOuterQuitNo()
    {
        if (seeingQuitPanel)
        {
            SoundManager.instance.ButtonClick();
            StartCoroutine(GameQuitPanelFadeOut());
        }
    }


    // Reward Panel

    public void OnBtnRewardPanel()
    {
        if (PlayerInformation.IsWatched) // 광고를 이미 보았다면, (3분 미경과)
        {
            SoundManager.instance.ButtonClick();
            seeingTimer = true;
            RunGetSoulPanelFadeIn(); // 시간 패널 띄워주기
            return;
        }
        if (!seeingRewardPanel)
        {
            SoundManager.instance.ButtonClick();
            StartCoroutine(GameRewardPanelFadeIn());
        }
    }

    public void OnBtnRewardWatchAd()
    {
        if (!rewardFadeInRunning && !rewardFadeOutRunning)
        {
            adsManager.ShowRewardAd();
        }
    }

    public void OnBtnRewardNo()
    {
        if (!rewardFadeInRunning && !rewardFadeOutRunning)
        {
            SoundManager.instance.ButtonClick();
            StartCoroutine(GameRewardPanelFadeOut());
        }
    }

    public void OnBtnOuterRewardNo()
    {
        if (seeingRewardPanel)
        {
            SoundManager.instance.ButtonClick();
            StartCoroutine(GameRewardPanelFadeOut());
        }
    }



    // Get Soul Panel

    public void RunGetSoulPanelFadeIn()
    {
        SoundManager.instance.ButtonClick();
        StartCoroutine(GetSoulPanelFadeIn());
    }

    public void OnBtnGetSoulOK()
    {
        if (seeingGetSoulPanel)
        {
            SoundManager.instance.ButtonClick();
            StartCoroutine(GetSoulPanelFadeOut());
        }
    }



    // Store Panel

    public void OnBtnStorePanel()
    {
        SoundManager.instance.ButtonClick();
        storeManager.InitFaceCharge();
        storeManager.InitBoatCharge();
        storeManager.InitWaveCharge();
        StartCoroutine(StorePanelFadeIn());
    }

    public void OnBtnStoreX()
    {
        SoundManager.instance.ButtonClick();
        if (seeingStorePanel)
        {
            StartCoroutine(storePanelFadeOut());
        }
    }



    // Buy Panel

    public void OnBtnChargePanel()
    {
        SoundManager.instance.ButtonClick();
        ChargeText.text = "Do you want to" + "\nbuy this?";
        StartCoroutine(ChargePanelFadeIn());
    }

    public void OnBtnChargeNo()
    {
        if (seeingChargePanel)
        {
            SoundManager.instance.ButtonClick();
            StartCoroutine(ChargePanelFadeOut());
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

        adsManager.ToggleAd(true);
        seeingQuitPanel = true;
        quitOuterPannel.SetActive(true);

        quitFadeInRunning = false;
    }


    IEnumerator GameQuitPanelFadeOut()
    {
        if (quitFadeInRunning) yield break;

        adsManager.ToggleAd(false);
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
        if (seeingTimer) seeingTimer = false;
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

        if (adsManager.seeingGetSoulText)
        {
            adsManager.seeingGetSoulText = false;
        }
        getSoulFadeOutRunning = false;
        getSoulPanel.SetActive(false);
    }


    IEnumerator StorePanelFadeIn()
    {
        if (storeFadeOutRunning) yield break;

        storeManager.seeingStore = true;
        storePanel.SetActive(true);
        storeFadeInRunning = true;
        storeManager.StopAllWave();

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
        storeManager.PlayAllWave();

        storeFadeInRunning = false;
    }


    IEnumerator storePanelFadeOut()
    {
        if (storeFadeInRunning) yield break;

        storeManager.StopAllWave();
        storeManager.seeingStore = false;
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


    IEnumerator ChargePanelFadeIn()
    {
        if (chargeFadeOutRunning) yield break;

        chargePanel.SetActive(true);
        chargeFadeInRunning = true;

        Image chargePanelImage = chargePanel.GetComponent<Image>();
        Color tmpColor = chargePanelImage.color;

        while (true)
        {
            chargePanel.transform.localScale -= new Vector3(0.2f, 0.2f, 0.2f);

            tmpColor.a += 0.2f;
            chargePanelImage.color = tmpColor;
            if (chargePanel.transform.localScale.x <= 1f) break;

            yield return new WaitForSecondsRealtime(0.01f);
        }

        chargePanel.transform.localScale = new Vector3(1f, 1f, 1f);
        tmpColor.a = 1f;
        chargePanelImage.color = tmpColor;

        seeingChargePanel = true;
        chargeOuterPanel.SetActive(true);

        chargeFadeInRunning = false;
    }


    IEnumerator ChargePanelFadeOut()
    {
        if (chargeFadeInRunning) yield break;

        chargeFadeOutRunning = true;
        seeingChargePanel = false;
        chargeOuterPanel.SetActive(false);

        Image chargePanelImage = chargePanel.GetComponent<Image>();
        Color tmpColor = chargePanelImage.color;

        while (true)
        {
            chargePanel.transform.localScale -= new Vector3(0.2f, 0.2f, 0.2f);

            tmpColor.a -= 0.2f;
            chargePanelImage.color = tmpColor;

            if (chargePanel.transform.localScale.x <= 0) break;

            yield return new WaitForSecondsRealtime(0.01f);
        }

        chargePanel.transform.localScale = new Vector3(2, 2, 2);
        tmpColor.a = 0f;
        chargePanelImage.color = tmpColor;

        chargeFadeOutRunning = false;
        chargePanel.SetActive(false);
    }
}
