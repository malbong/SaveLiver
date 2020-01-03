﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; set; }

    public Text liverCountText;
    public Text scoreText;

    private int totalScore = 0;

    private float currentplayTime = 0;
    private float secondsUnit = 0;

    public bool isPause;

    public GameObject joyStickTouchArea;

    public GameObject pauseButton;

    public GameObject pausePanel;

    public float originTimeScale; //Time.timeScale 저장 변수 -> 원상태로 돌리기 위함 (timescale값을 잃어버리기 때문)

    private Color originColor; //원래 카메라 색 임시저장


    private void Awake()
    {
        if (instance == null)
        {
            instance = this; // instance 초기화
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }


    private void Start()
    {
        originTimeScale = Time.timeScale;
        isPause = false;
        totalScore = 0;
        UpdateLiverCountText(3);
        UpdateScoreText();
    }


    void Update()
    {
        if (isPause) return;
        TimeScorePlus();
    }


    /**************************************
    * @함수명: UpdateLiverText
    * @작성자: zeli, Malbong
    * @입력: liver
    * @출력: void
    * @설명: Player가 피격시마다 LiverUI를 새로 업데이트함.
    */
    public void UpdateLiverCountText(int liverCount)
    {
        liverCountText.text = "x " + liverCount;
    }


    public void UpdateScoreText()
    {
        scoreText.text = "score: " + totalScore;
    }


    public void AddScore(int score)
    {
        if (Player.instance.isAlive == false) return;
        totalScore += score;
        UpdateScoreText();
    }


    private void TimeScorePlus()
    {
        if (Player.instance.isAlive)
        {
            secondsUnit += Time.deltaTime;
            if (secondsUnit >= 0.5f)
            {
                secondsUnit = 0;
                currentplayTime += 0.5f;
                AddScore(1);
            }
        }
    }


    public void OnPause() //pause button에 onClick에 달아줌
    {
        if (isPause == true) //이미 pause라면 리턴
        {
            Debug.Log("Already Paused");
            return;
        }

        originTimeScale = Time.timeScale; //저장해놓기
        Time.timeScale = 0f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale; //바꾸는 것이 좋다고 함

        isPause = true;

        originColor = Camera.main.backgroundColor; //임시 저장 Color
        Camera.main.backgroundColor = new Color(0.5f, 0.5f, 0.5f); //컬러를 회색으로 바꿈

        pauseButton.GetComponent<AudioSource>().Play();
        //다른 탭 동작 처리
        StartCoroutine(PausePanelFadeIn());

        StartCoroutine(JoyStickFadeOut()); //JoyStick 닫기
        StartCoroutine(PauseButtonFadeOut());//PauseButton 닫기
    }


    private IEnumerator PauseButtonFadeOut()
    {
        Image image = pauseButton.GetComponent<Image>();
        Color tmpColor = image.color;
        while (isPause)
        {
            pauseButton.transform.localScale -= new Vector3(0.1f, 0.1f, 0.1f); //Scale과 color를 변경함
            tmpColor.a -= 0.1f;
            image.color = tmpColor;

            if (image.color.a <= 0 || pauseButton.transform.localScale.x <= 0) break;

            yield return new WaitForSecondsRealtime(0.01f); //RealTime으로 쉬면 Time.timeScale = 0 이어도 코루틴 진행가능
        }

        if (isPause)
        {
            //button 끄기 SetActive(false)
            pauseButton.SetActive(false);
        }

        //Color 와 Scale을 원상태로 둠
        tmpColor.a = 1.0f;
        image.color = tmpColor;
        pauseButton.transform.localScale = new Vector3(1f, 1f, 1f);
    }


    private IEnumerator JoyStickFadeOut()
    {
        while (isPause)
        {
            joyStickTouchArea.transform.localScale -= new Vector3(0.1f, 0.1f, 0.1f); //Scale을 줄임

            if (joyStickTouchArea.transform.localScale.x <= 0) break;

            yield return new WaitForSecondsRealtime(0.01f);
        }

        //멈춘 상태라면 잠시 꺼주고 상태를 돌려놓음
        if (isPause)
        {
            joyStickTouchArea.SetActive(false);
        }
        joyStickTouchArea.transform.localScale = new Vector3(1f, 1f, 1f);
    }


    private IEnumerator PausePanelFadeIn()
    {
        if (isPause)
        {
            pausePanel.SetActive(true);
        }

        while (isPause)
        {
            //pausePanel Scale(0 -> 1)
            pausePanel.transform.localScale += new Vector3(0.2f, 0.2f, 0.2f);

            if (pausePanel.transform.localScale.x >= 1) break;

            yield return new WaitForSecondsRealtime(0.01f);
        }
    }


    public void OnContinue() //pause panel에 play버튼에 onClick 달아줌
    {
        if (isPause == false)
        {
            Debug.Log("Already playing");
            return;
        }

        Time.timeScale = originTimeScale; //원래의 타임스케일로 돌려줌
        Time.fixedDeltaTime = 0.02f * Time.timeScale;

        isPause = false;

        StartCoroutine(PausePanelFadeOut()); //퍼즈 판넬 꺼줌

        joyStickTouchArea.SetActive(true); //조이스틱 다시 켜줌
        pauseButton.SetActive(true); //pause버튼 다시 활성화

        Camera.main.backgroundColor = originColor; //카메라색 원래대로 돌려줌
    }


    private IEnumerator PausePanelFadeOut()
    {
        while (!isPause) //진행중이라면 isPause == false
        {
            //pausePanel Scale(1 -> 0)
            pausePanel.transform.localScale -= new Vector3(0.2f, 0.2f, 0.2f);

            if (pausePanel.transform.localScale.x <= 0) break;
            yield return new WaitForSecondsRealtime(0.01f);
        }

        if (!isPause)
        {
            pausePanel.SetActive(false);
        }
    }


    public void OnReload()
    {
        if (!isPause)
        {
            Debug.Log("진행중이라 리로드 불가능");
            return;
        }

        Time.timeScale = originTimeScale; //원래의 타임스케일로 돌려줌
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
        SceneManager.LoadScene("Play Scene");
    }
}
