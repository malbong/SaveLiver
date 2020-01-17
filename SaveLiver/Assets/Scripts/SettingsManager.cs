using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GooglePlayGames;
using UnityEngine.SceneManagement;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager instance { get; set; }

    public GameObject soundButton;
    public Sprite soundOnSprite;
    public Sprite soundOffSprite;

    public GameObject vibeButton;
    public Sprite vibeOnSprite;
    public Sprite vibeOffSprite;

    public GoogleAuth googleAuthController;
    public GameObject logButton;
    public Sprite loginSprite;
    public Sprite logoutSprite;

    public GameObject settingsOuterPanel;

    public GameObject helpOuterPanel;
    public GameObject pagePanel; //옮길 pagePanel
    public Text helpPageText;
    private int helpPanelPage = 1;


    private bool settingsPanelFadeInRunning = false;
    private bool settingsPanelFadeOutRunning = false;
    private bool helpPanelFadeInRunning = false;
    private bool helpPanelFadeOutRunning = false;
    private bool nextPageFadeRunning = false;
    private bool previousPageFadeRunning = false;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
        }
    }


    private void Start()
    {
        helpPanelPage = 1;
        helpPageText.text = helpPanelPage + " / 3";
    }


    
    public void OnSettingsButtonClick()
    {
        //Settings Panel Fade In
        StartCoroutine(SettingsPanelFadeIn());

        //2번 Toggle하여 원래의 상태를 표현하게 함
        OnSoundToggleButton();
        OnSoundToggleButton();

        OnVibeToggleButton();
        OnVibeToggleButton();
    }


    public void OnSoundToggleButton()
    {
        bool isSoundOn = AudioListener.volume == 1f ? true : false;

        if (isSoundOn)
        {
            AudioListener.volume = 0;
            isSoundOn = false;

            Transform soundImage = soundButton.transform.Find("Sound Image");
            Transform soundText = soundButton.transform.Find("Sound Text");

            if (soundImage == null)
            {
                Debug.Log("can't find Sound Image");
                return;
            }
            if (soundText == null)
            {
                Debug.Log("can't find Sound Text");
                return;
            }

            soundImage.GetComponent<Image>().sprite = soundOffSprite;
            soundText.GetComponent<Text>().text = "Sound Off";
        }
        else //Sound Off
        {
            AudioListener.volume = 1;
            isSoundOn = true;

            Transform soundImage = soundButton.transform.Find("Sound Image");
            Transform soundText = soundButton.transform.Find("Sound Text");

            if (soundImage == null)
            {
                Debug.Log("can't find Sound Image");
                return;
            }
            if (soundText == null)
            {
                Debug.Log("can't find Sound Text");
                return;
            }

            soundImage.GetComponent<Image>().sprite = soundOnSprite;
            soundText.GetComponent<Text>().text = "Sound On";

        }
    }


    public void OnVibeToggleButton()
    {
        if (PlayerInformation.isVibrationOn)
        {
            PlayerInformation.isVibrationOn = false;

            Transform vibeImage = vibeButton.transform.Find("Vibe Image");
            Transform vibeText = vibeButton.transform.Find("Vibe Text");

            if (vibeImage == null)
            {
                Debug.Log("can't find Vibe Image");
                return;
            }
            if (vibeText == null)
            {
                Debug.Log("can't find Vibe Text");
                return;
            }

            vibeImage.GetComponent<Image>().sprite = vibeOffSprite;
            vibeText.GetComponent<Text>().text = "Vibration Off";
        }
        else //Vibration off
        {
            PlayerInformation.isVibrationOn = true;

            Transform vibeImage = vibeButton.transform.Find("Vibe Image");
            Transform vibeText = vibeButton.transform.Find("Vibe Text");

            if (vibeImage == null)
            {
                Debug.Log("can't find Vibe Image");
                return;
            }
            if (vibeText == null)
            {
                Debug.Log("can't find Vibe Text");
                return;
            }

            vibeImage.GetComponent<Image>().sprite = vibeOnSprite;
            vibeText.GetComponent<Text>().text = "Vibration On";
        }
    }


    public void OnSettingsExitButton()
    {
        //Settings Panel Fade Out
        StartCoroutine(SettingsPanelFadeOut());
    }


    public void OnHelpButton()
    {
        //help page 초기화
        helpPanelPage = 1;
        helpPageText.text = helpPanelPage + " / 3";
        pagePanel.transform.localPosition = new Vector3(0, 0, 0);

        //Help Panel Fade In
        StartCoroutine(HelpPanelFadeIn());
    }


    public void OnHelpPanelExitButton()
    {
        //Help Panel Fade Out
        StartCoroutine(HelpPanelFadeOut());
    }


    public void OnHelpPanelNextButton()
    {
        if (helpPanelPage >= 3) //page 3 -> ignored
        {
            helpPanelPage = 3;
            helpPageText.text = helpPanelPage + " / 3";
            StartCoroutine(HelpPanelFadeOut());
        }
        else if (helpPanelPage >= 1) //page 1 2
        {
            helpPanelPage += 1;
            helpPageText.text = helpPanelPage + " / 3";
            //Slide Page
            StartCoroutine(NextPageFadeIn());
        }
    }


    public void OnHelpPanelPreviousButton()
    {
        if (helpPanelPage <= 1) //page 1 -> ignored
        {
            helpPanelPage = 1;
            helpPageText.text = helpPanelPage + " / 3";
        }
        else if (helpPanelPage <= 3) //page 2 3
        {
            helpPanelPage -= 1;
            helpPageText.text = helpPanelPage + " / 3";
            //Slider Page
            StartCoroutine(PreviousPageFadeIn());
        }
    }


    public void OnPrivacyButton()
    {

    }


    public void OnLogoutAndLogInButton()
    {
        googleAuthController.TryGoogleLoginOrLogout();
    }


    public void UpdateLoginAndLogout(bool isLogin)
    {
        Transform logImage = logButton.transform.Find("Log Image");
        Transform logText = logButton.transform.Find("Log Text");

        if (logImage == null) { Debug.Log("not found Log Image"); return; }
        if (logText == null) { Debug.Log("not found Log Text"); return; }

        if (isLogin) //로그인 되어있으면 logout 을 보여줌
        {
            logImage.GetComponent<Image>().sprite = logoutSprite;
            logText.GetComponent<Text>().text = "Log Out";
        }
        else
        {
            logImage.GetComponent<Image>().sprite = loginSprite;
            logText.GetComponent<Text>().text = "Log In";
        }
    }

    private IEnumerator SettingsPanelFadeIn()
    {
        if (settingsPanelFadeOutRunning) yield break;

        settingsPanelFadeInRunning = true;

        settingsOuterPanel.SetActive(true);

        Transform settingsInnerPanel = settingsOuterPanel.transform.GetChild(0);

        Image settingsOuterPanelImage = settingsOuterPanel.GetComponent<Image>();
        Color tmpOuterColor = settingsOuterPanelImage.color;

        Image settingsInnerPanelImage = settingsInnerPanel.GetComponent<Image>();
        Color tmpInnerColor = settingsInnerPanelImage.color;

        while (true)
        {
            settingsOuterPanel.transform.localScale -= new Vector3(0.2f, 0.2f, 0.2f);

            tmpOuterColor.a += 0.1f;
            tmpInnerColor.a += 0.2f;
            settingsInnerPanelImage.color = tmpInnerColor;
            settingsOuterPanelImage.color = tmpOuterColor;

            if (settingsOuterPanel.transform.localScale.x <= 1) break;

            yield return new WaitForSecondsRealtime(0.01f);
        }

        settingsOuterPanel.transform.localScale = new Vector3(1, 1, 1);

        tmpOuterColor.a = 0.5f;
        settingsOuterPanelImage.color = tmpOuterColor;

        tmpInnerColor.a = 1f;
        settingsInnerPanelImage.color = tmpInnerColor;

        settingsPanelFadeInRunning = false;
    }


    private IEnumerator SettingsPanelFadeOut()
    {
        if (settingsPanelFadeInRunning) yield break;

        settingsPanelFadeOutRunning = true;

        settingsOuterPanel.SetActive(true);

        Transform settingsInnerPanel = settingsOuterPanel.transform.GetChild(0);

        Image settingsOuterPanelImage = settingsOuterPanel.GetComponent<Image>();
        Color tmpOuterColor = settingsOuterPanelImage.color;

        Image settingsInnerPanelImage = settingsInnerPanel.GetComponent<Image>();
        Color tmpInnerColor = settingsInnerPanelImage.color;

        while (true)
        {
            settingsOuterPanel.transform.localScale -= new Vector3(0.2f, 0.2f, 0.2f);

            tmpOuterColor.a -= 0.1f;
            tmpInnerColor.a -= 0.2f;
            settingsInnerPanelImage.color = tmpInnerColor;
            settingsOuterPanelImage.color = tmpOuterColor;

            if (settingsOuterPanel.transform.localScale.x <= 0) break;

            yield return new WaitForSecondsRealtime(0.01f);
        }

        settingsOuterPanel.SetActive(false);

        settingsOuterPanel.transform.localScale = new Vector3(2, 2, 2);

        tmpOuterColor.a = 0f;
        settingsOuterPanelImage.color = tmpOuterColor;

        tmpInnerColor.a = 0f;
        settingsInnerPanelImage.color = tmpInnerColor;

        settingsPanelFadeOutRunning = false;
    }


    private IEnumerator HelpPanelFadeIn()
    {
        if (helpPanelFadeOutRunning) yield break;

        helpPanelFadeInRunning = true;

        helpOuterPanel.SetActive(true);

        Transform helpInnerPanel = helpOuterPanel.transform.GetChild(0);

        Image helpOuterImage = helpOuterPanel.GetComponent<Image>();
        Color tmpOuterColor = helpOuterImage.color;

        Image helpInnerImage = helpInnerPanel.GetComponent<Image>();
        Color tmpInnerColor = helpInnerImage.color;

        while (true)
        {
            helpOuterPanel.transform.localScale -= new Vector3(0.2f, 0.2f, 0.2f);

            tmpOuterColor.a += 0.1f;
            tmpInnerColor.a += 0.2f;
            helpOuterImage.color = tmpOuterColor;
            helpInnerImage.color = tmpInnerColor;

            if (helpOuterPanel.transform.localScale.x <= 1) break;

            yield return new WaitForSecondsRealtime(0.01f);
        }

        helpOuterPanel.transform.localScale = new Vector3(1, 1, 1);

        tmpOuterColor.a = 0.5f;
        helpOuterImage.color = tmpOuterColor;

        tmpInnerColor.a = 1f;
        helpInnerImage.color = tmpInnerColor;

        helpPanelFadeInRunning = false;
    }


    private IEnumerator HelpPanelFadeOut()
    {
        if (helpPanelFadeInRunning) yield break;

        helpPanelFadeOutRunning = true;

        Transform helpInnerPanel = helpOuterPanel.transform.GetChild(0);

        Image helpOuterImage = helpOuterPanel.GetComponent<Image>();
        Color tmpOuterColor = helpOuterImage.color;

        Image helpInnerImage = helpInnerPanel.GetComponent<Image>();
        Color tmpInnerColor = helpInnerImage.color;

        while (true)
        {
            helpOuterPanel.transform.localScale -= new Vector3(0.2f, 0.2f, 0.2f);

            tmpOuterColor.a -= 0.1f;
            tmpInnerColor.a -= 0.2f;
            helpOuterImage.color = tmpOuterColor;
            helpInnerImage.color = tmpInnerColor;

            if (helpOuterPanel.transform.localScale.x <= 0) break;

            yield return new WaitForSecondsRealtime(0.01f);
        }

        helpOuterPanel.SetActive(false);

        helpOuterPanel.transform.localScale = new Vector3(2, 2, 2);

        tmpOuterColor.a = 0f;
        helpOuterImage.color = tmpOuterColor;

        tmpInnerColor.a = 0f;
        helpInnerImage.color = tmpInnerColor;

        helpPanelFadeOutRunning = false;
    }


    private IEnumerator NextPageFadeIn()
    {
        if (previousPageFadeRunning) yield break;

        nextPageFadeRunning = true;

        Vector3 originPosition = pagePanel.transform.localPosition;

        while (true)
        {
            pagePanel.transform.localPosition += new Vector3(-100, 0, 0);
            if (pagePanel.transform.localPosition.x - originPosition.x <= -1200)
            {
                break;
            }

            yield return new WaitForSecondsRealtime(0.01f);
        }

        pagePanel.transform.localPosition = originPosition - new Vector3(1200, 0, 0);

        nextPageFadeRunning = false;
    }


    private IEnumerator PreviousPageFadeIn()
    {
        if (nextPageFadeRunning) yield break;

        previousPageFadeRunning = true;

        Vector3 originPosition = pagePanel.transform.localPosition;

        while (true)
        {
            pagePanel.transform.localPosition += new Vector3(+100, 0, 0);

            if (pagePanel.transform.localPosition.x - originPosition.x >= +1200)
            {
                break;
            }

            yield return new WaitForSecondsRealtime(0.01f);
        }

        pagePanel.transform.localPosition = originPosition + new Vector3(1200, 0, 0);

        previousPageFadeRunning = false;
    }
}
