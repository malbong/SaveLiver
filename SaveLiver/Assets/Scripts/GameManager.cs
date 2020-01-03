using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GooglePlayGames;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; set; }

    public Text liverCountText;
    public Text scoreText;

    public int totalScore = 0;

    private float currentplayTime = 0;
    private float secondsUnit = 0;

    public bool isPause;

    public GameObject buttonLockObject;

    public GameObject joyStickTouchArea;

    public GameObject pauseButton;

    public GameObject pausePanel;

    public float originTimeScale; //Time.timeScale 저장 변수 -> 원상태로 돌리기 위함 (timescale값을 잃어버리기 때문)

    //private Color originColor; //원래 카메라 색 임시저장

    public GameObject settingsOuterPanel;

    private bool pauseButtonFadeOutRunning = false;
    private bool pauseButtonFadeInRunning = false;
    private bool joyStickFadeOutRunning = false;
    private bool pausePanelFadeInRunning = false;
    private bool pausePanelFadeOutRunning = false;
    private bool settingsPanelFadeInRunning = false;
    private bool settingsPanelFadeOutRunning = false;

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
            currentplayTime += secondsUnit;
            if (secondsUnit >= 0.5f)
            {
                secondsUnit = secondsUnit - 0.5f;
                AddScore(1);
            }
        }
    }


    /**************************************
    * @함수명: ReportScore(int score)
    * @작성자: zeli
    * @입력: score
    * @출력: void
    * @설명: Player가 죽을 때 score를 받기
    *        Google Play 리더보드에 자신의 HighScore를 기록함
    */
    public void ReportScore(int score)
    {
        // ReportScore는 현재 score와 기록된 score를 비교해 Leaderboard에 기록.
        PlayGamesPlatform.Instance.ReportScore(score, GPGSIds.leaderboard_score, null);
    }


    public void OnPause() //pause button에 onClick에 달아줌
    {
        if (pausePanelFadeOutRunning || pauseButtonFadeInRunning) return;

        if (isPause) return;

        originTimeScale = Time.timeScale; //저장해놓기
        Time.timeScale = 0f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale; //바꾸는 것이 좋다고 함

        isPause = true;

        //originColor = Camera.main.backgroundColor; //원래 색 임시 저장 Color
        //Camera.main.backgroundColor = new Color(0.5f, 0.5f, 0.5f); //컬러를 회색으로 바꿈

        //pauseButton.GetComponent<AudioSource>().Play(); //오디오 처리

        //다른 탭 동작 처리
        StartCoroutine(PauseButtonFadeOut());//PauseButton 닫기
        StartCoroutine(JoyStickFadeOut()); //JoyStick 닫기
        StartCoroutine(PausePanelFadeIn());
    }


    public void OnContinue() //pause panel에 play버튼에 onClick 달아줌
    {
        if (pauseButtonFadeOutRunning || joyStickFadeOutRunning || pausePanelFadeInRunning) return;

        if (!isPause) return;

        Time.timeScale = originTimeScale; //원래의 타임스케일로 돌려줌
        Time.fixedDeltaTime = 0.02f * Time.timeScale;

        isPause = false;

        StartCoroutine(PausePanelFadeOut()); //퍼즈 패널 꺼줌
        StartCoroutine(PauseButtonFadeIn()); //퍼즈 버튼 다시 켜줌
        joyStickTouchArea.SetActive(true); //조이스틱 다시 켜줌

        //Camera.main.backgroundColor = originColor; //카메라색 원래대로 돌려줌
    }


    public void OnReload() //pause panel에 reload버튼에 onClick 달아줌
    {
        if (!isPause) return;

        Time.timeScale = 1.0f; //처음의 타임스케일로 돌려줌
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
        isPause = false;

        //Camera.main.backgroundColor = originColor; //카메라색 원래대로 돌려줌

        StartCoroutine(PausePanelFadeOut()); //퍼즈패널 꺼주기

        StartCoroutine(LoadSceneAfterWaiting("Play Scene"));
    }


    public void OnHomeButtonClick()
    {
        if (!isPause) return;

        Time.timeScale = 1.0f; //처음의 타임스케일로 돌려줌
        Time.fixedDeltaTime = 0.02f * Time.timeScale;

        isPause = false;

        //Camera.main.backgroundColor = originColor; //카메라색 원래대로 돌려줌

        StartCoroutine(PausePanelFadeOut());

        StartCoroutine(LoadSceneAfterWaiting("Menu Scene"));
    }
    

    public void OnSettingsButtonClick()
    {
        if (!isPause) return;

        //Settings Panel Fade In
        StartCoroutine(SettingsPanelFadeIn());
    }


    public void OnSettingsExitButton()
    {
        if (!isPause) return;

        //Settings Panel Fade Out
        StartCoroutine(SettingsPanelFadeOut());
    }


    private IEnumerator PauseButtonFadeOut()
    {
        if (pausePanelFadeOutRunning || pauseButtonFadeInRunning) {
            yield break;
        }

        pauseButtonFadeOutRunning = true;

        Image image = pauseButton.GetComponent<Image>();
        Color tmpColor = image.color;
        while (true)
        {
            pauseButton.transform.localScale -= new Vector3(0.1f, 0.1f, 0.1f); //Scale과 color를 변경함
            tmpColor.a -= 0.1f;
            image.color = tmpColor;

            if (image.color.a <= 0 || pauseButton.transform.localScale.x <= 0) break;

            yield return new WaitForSecondsRealtime(0.01f); //RealTime으로 쉬면 Time.timeScale = 0 이어도 코루틴 진행가능
        }

        //button 끄기 SetActive(false)
        pauseButton.SetActive(false);
        pauseButton.GetComponent<Button>().enabled = false;
        
        pauseButtonFadeOutRunning = false;
    }


    private IEnumerator PauseButtonFadeIn()
    {
        if (pauseButtonFadeOutRunning || joyStickFadeOutRunning || pausePanelFadeInRunning)
        {
            yield break;
        }

        pauseButtonFadeInRunning = true;

        Image image = pauseButton.GetComponent<Image>();
        Color tmpColor = image.color;

        //button 켜기 SetActive(true)
        pauseButton.SetActive(true);
        

        while (true)
        {
            pauseButton.transform.localScale += new Vector3(0.1f, 0.1f, 0.1f); //Scale과 color를 변경함
            tmpColor.a += 0.1f;
            image.color = tmpColor;

            if (image.color.a >= 1f || pauseButton.transform.localScale.x >= 1f) break;

            yield return new WaitForSecondsRealtime(0.01f); //RealTime으로 쉬면 Time.timeScale = 0 이어도 코루틴 진행가능
        }

        //Color 와 Scale을 원상태로 둠
        tmpColor.a = 1.0f;
        image.color = tmpColor;
        pauseButton.transform.localScale = new Vector3(1f, 1f, 1f);
        pauseButton.GetComponent<Button>().enabled = true;

        pauseButtonFadeInRunning = false;
    }


    private IEnumerator JoyStickFadeOut()
    {
        if (pausePanelFadeOutRunning || pauseButtonFadeInRunning)
        {
            yield break;
        }

        joyStickFadeOutRunning = true;

        while (true)
        {
            joyStickTouchArea.transform.localScale -= new Vector3(0.1f, 0.1f, 0.1f); //Scale을 줄임

            if (joyStickTouchArea.transform.localScale.x <= 0) break;

            yield return new WaitForSecondsRealtime(0.01f);
        }
        
        joyStickTouchArea.SetActive(false);
        
        joyStickTouchArea.transform.localScale = new Vector3(1f, 1f, 1f);

        joyStickFadeOutRunning = false;
    }


    private IEnumerator PausePanelFadeIn()
    {
        if (pausePanelFadeOutRunning || pauseButtonFadeInRunning)
        {
            yield break;
        }

        pausePanelFadeInRunning = true;
        
        pausePanel.SetActive(true);

        Image pausePanelImage = pausePanel.GetComponent<Image>();
        Color tmpColor = pausePanelImage.color;

        while (true)
        {
            pausePanel.transform.localScale -= new Vector3(0.2f, 0.2f, 0.2f);

            tmpColor.a += 0.1f;
            pausePanelImage.color = tmpColor;

            if (pausePanel.transform.localScale.x <= 1) break;

            yield return new WaitForSecondsRealtime(0.01f);
        }

        pausePanel.transform.localScale = new Vector3(1, 1, 1);
        tmpColor.a = 0.5f;
        pausePanelImage.color = tmpColor;

        pausePanelFadeInRunning = false;
    }


    private IEnumerator PausePanelFadeOut()
    {
        if (pauseButtonFadeOutRunning || joyStickFadeOutRunning || pausePanelFadeInRunning)
        {
            yield break;
        }

        pausePanelFadeOutRunning = true;

        Image pausePanelImage = pausePanel.GetComponent<Image>();
        Color tmpColor = pausePanelImage.color;

        while (true) //진행중이라면 isPause == false
        {
            pausePanel.transform.localScale -= new Vector3(0.2f, 0.2f, 0.2f);

            tmpColor.a -= 0.1f;
            pausePanelImage.color = tmpColor;

            if (pausePanel.transform.localScale.x <= 0) break;

            yield return new WaitForSecondsRealtime(0.01f);
        }

        pausePanel.SetActive(false);

        pausePanel.transform.localScale = new Vector3(2, 2, 2);
        tmpColor.a = 0;
        pausePanelImage.color = tmpColor;

        pausePanelFadeOutRunning = false;
    }


    private IEnumerator LoadSceneAfterWaiting(string sceneName)
    {
        while (sceneName == "Play Scene")
        {
            if (!pausePanelFadeOutRunning)
            {
                SceneManager.LoadScene(sceneName);
                yield break;
            }
            yield return new WaitForSecondsRealtime(0.01f);
        }

        while (sceneName == "Menu Scene")
        {
            if (!pausePanelFadeOutRunning)
            {
                SceneManager.LoadScene(sceneName);
                yield break;
            }
            yield return new WaitForSecondsRealtime(0.01f);
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
}
