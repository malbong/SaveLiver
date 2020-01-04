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

    void Update()
    {
        // 안드로이드에서 뒤로가기 누르면 종료되는 처리
        if(Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                //StartCoroutine(GameQuitPanelFadeIn());
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
        //StartCoroutine(GameQuitPanelFadeOut());
    }

    
    /*private IEnumerator GameQuitPanelFadeIn()
    {
        
    }


    IEnumerator GameQuitPanelFadeOut()
    {

    }
    */
}
