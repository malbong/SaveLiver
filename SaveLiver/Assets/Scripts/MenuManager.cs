using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject quitOuterPannel;
    public GameObject quitPanel;
    private bool quitFadeInRunning = false;
    private bool quitFadeOutRunning = false;
    private bool seeingQuitPanel = false;

    public AbsManager absManager;

    void Update()
    {
        // 안드로이드에서 뒤로가기 누르면 종료되는 처리
        if(Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                StartCoroutine(GameQuitPanelFadeIn());
            }
        }
    }
    

    public void OnBtnPlay()
    {
        SceneManager.LoadScene("Play Scene");
    }


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

            tmpColor.a += 0.1f;
            quitPanelImage.color = tmpColor;
            if (quitPanel.transform.localScale.x <= 1.6f) break;

            yield return new WaitForSecondsRealtime(0.01f);
        }

        quitPanel.transform.localScale = new Vector3(1.6f, 1.25f, 1.4f);
        tmpColor.a = 0.5f;
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

            tmpColor.a -= 0.1f;
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
}
