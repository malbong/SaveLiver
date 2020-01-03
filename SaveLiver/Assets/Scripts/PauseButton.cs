using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PauseButton : MonoBehaviour
{
    public bool isPause;

    public GameObject joyStickTouchArea;

    public GameObject pausePanel;


    private void Start()
    {
        Time.timeScale = 1.2f;
        isPause = false;
        GetComponent<Button>().onClick.AddListener(OnPause);
    }


    public void OnPause()
    {
        if (isPause == true) //이미 pause라면 리턴
        {
            Debug.Log("Already Pause");
            return;
        }

        Time.timeScale = 0f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale; //바꾸는 것이 좋다고 함
        isPause = true;

        Color tmpColor = Camera.main.backgroundColor; //임시 저장 Color
        Camera.main.backgroundColor = new Color(0.5f, 0.5f, 0.5f); //컬러를 회색으로 바꿈

        //다른 탭 동작 처리
        StartCoroutine(AppearPausePanel());

        StartCoroutine(JoyStickFadeOut()); //JoyStick 닫기
        StartCoroutine(PauseButtonFadeOut());//PauseButton 닫기
    }


    private IEnumerator PauseButtonFadeOut()
    {
        Image image = GetComponent<Image>();
        Color tmpColor = image.color;
        while (true)
        {
            transform.localScale *= 0.9f; //Scale과 color를 변경함
            tmpColor.a -= 0.1f;
            image.color = tmpColor;

            if (image.color.a <= 0 || transform.localScale.x <= 0) break;

            yield return new WaitForSecondsRealtime(0.01f); //RealTime으로 쉬면 Time.timeScale = 0 이어도 코루틴 진행가능
        }

        //button 끄기 -> 이 버튼을 끄면 스크립트가 진행하지 않으므로 마지막에 꺼줌
        Button button = GetComponent<Button>();
        button.enabled = false;
        image.enabled = false;
        
        //Color 와 Scale을 원상태로 둠
        tmpColor.a = 1.0f;
        image.color = tmpColor;
        transform.localScale = new Vector3(1f, 1f, 1f);
    }


    private IEnumerator JoyStickFadeOut()
    {
        while (true)
        {
            joyStickTouchArea.transform.localScale *= 0.9f; //Scale을 줄임

            if (joyStickTouchArea.transform.localScale.x <= 0) break;

            yield return new WaitForSecondsRealtime(0.01f);
        }

        //잠시 꺼주고 상태를 돌려놓음
        joyStickTouchArea.SetActive(false);
        joyStickTouchArea.transform.localScale = new Vector3(1f, 1f, 1f);
    }


    private IEnumerator AppearPausePanel()
    {
        pausePanel.SetActive(true);

        while (true)
        {
            //pausePanel Scale(0 -> 1)
            pausePanel.transform.localScale += new Vector3(0.2f, 0.2f, 0.2f);

            if (pausePanel.transform.localScale.x >= 1) break;

            yield return new WaitForSecondsRealtime(0.01f);
        }
    }
}
