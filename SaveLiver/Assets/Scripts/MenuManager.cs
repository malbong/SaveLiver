using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    void Update()
    {
        // 안드로이드에서 뒤로가기 누르면 종료되는 처리
        if(Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                // 종료처리
                Application.Quit();
            }
        }
    }
    

    public void OnBtnPlay()
    {
        SceneManager.LoadScene("Play Scene");
    }
}
