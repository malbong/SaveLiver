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

    public int totalScore = 0;

    public int totalGetItemCount = 0;
    
    public int totalSoulCount = 0;
    public Text playingPanelSoulCountText;

    private int totalPlayTime = 0;
    private float secondsUnit = 0;

    public bool isPause;

    //public GameObject buttonLockObject;

    public GameObject joyStickTouchArea;

    public GameObject pauseButton;

    public GameObject pausePanel;

    public float originTimeScale; //Time.timeScale 저장 변수 -> 원상태로 돌리기 위함 (timescale값을 잃어버리기 때문)

    public GameObject diedPanel;
    public GameObject diedInnerPanel;
    public ParticleSystem diedSoulParticle;

    public Text totalTimeText;
    public Text totalItemText;
    public Text totalSoulText;
    public Text totalScoreText;
    public Text totalScoreBannerText;

    public Image totalScoreBannerFlashImage;
    public Image totalScoreFlashImage;

    private bool pauseButtonFadeOutRunning = false;
    private bool pauseButtonFadeInRunning = false;
    private bool joyStickFadeOutRunning = false;
    private bool pausePanelFadeInRunning = false;
    private bool pausePanelFadeOutRunning = false;
    private bool scoreUpRunning = false;
    private bool isFlashScoreImageRunning = false;

    public AudioClip scoreUpClip;

    private int dataScore;
    private int dataPlayNum;


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
        originTimeScale = Time.timeScale;

        isPause = false;

        secondsUnit = 0;
        totalScore = 0;

        UpdateLiverCountText(3);

        totalGetItemCount = 0;

        totalSoulCount = 0;
        UpdateSoulCount(0);

        totalPlayTime = 0;

        dataScore = DatabaseManager.GetScore();
        dataPlayNum = DatabaseManager.GetPlayNum();
    }


    void Update()
    {
        if (isPause) return;

        AddTimeScore();

        if (PlayerInformation.IsWatched) // 광고를 보았다면 3분 카운트.
        {
            PlayerInformation.AdTimeCheck();
        }

            if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKey(KeyCode.Escape) && Player.instance.isAlive)
            {
                OnPause();
            }
        }
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


    private void AddTimeScore()
    {
        if (Player.instance.isAlive)
        {
            secondsUnit += Time.deltaTime; //1초 기록 Unit
            if (secondsUnit > 1.0f)
            {
                totalPlayTime += 1;
                AddScore(1);
                secondsUnit = secondsUnit % 1.0f; //나머지 넣어 주기
            }
        }
    }


    public void UpdateSoulCount(int soulCount)
    {
        if (Player.instance.isAlive == false) return;

        totalSoulCount += soulCount;
        playingPanelSoulCountText.text = totalSoulCount.ToString();
    }


    public void AddScore(int score)
    {
        if (Player.instance.isAlive == false) return;
        totalScore += score;
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
        PlayGamesPlatform.Instance.ReportScore(score, GPGSIds.leaderboard_ranking, null);
    }


    public void OnPause() //pause button에 onClick에 달아줌
    {
        if (pausePanelFadeOutRunning || pauseButtonFadeInRunning) return;

        if (isPause) return;

        SoundManager.instance.ButtonClick();

        originTimeScale = Time.timeScale; //저장해놓기
        Time.timeScale = 0f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale; //바꾸는 것이 좋다고 함

        isPause = true;

        //originColor = Camera.main.backgroundColor; //원래 색 임시 저장 Color
        //Camera.main.backgroundColor = new Color(0.5f, 0.5f, 0.5f); //컬러를 회색으로 바꿈

        //다른 탭 동작 처리
        StartCoroutine(PauseButtonFadeOut());//PauseButton 닫기
        StartCoroutine(JoyStickFadeOut()); //JoyStick 닫기
        StartCoroutine(PausePanelFadeIn());
    }


    public void OnContinue() //pause panel에 play버튼에 onClick 달아줌
    {
        if (pauseButtonFadeOutRunning || joyStickFadeOutRunning || pausePanelFadeInRunning) return;

        if (!isPause) return;

        SoundManager.instance.ButtonClick();

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

        SoundManager.instance.ButtonClick();

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

        SoundManager.instance.ButtonClick();

        Time.timeScale = 1.0f; //처음의 타임스케일로 돌려줌
        Time.fixedDeltaTime = 0.02f * Time.timeScale;

        isPause = false;

        //Camera.main.backgroundColor = originColor; //카메라색 원래대로 돌려줌

        StartCoroutine(PausePanelFadeOut());

        StartCoroutine(LoadSceneAfterWaiting("Menu Scene"));
    }
    

    public void PlayerDied()
    {
        dataScore = DatabaseManager.GetScore();
        dataPlayNum = DatabaseManager.GetPlayNum();
        DatabaseManager.UpdateMoney(totalSoulCount);
        DatabaseManager.SetPlayNum(dataPlayNum + 1);
        PlayerInformation.PlayNum += dataPlayNum + 1;

        StartCoroutine(ShowDiedPanelFadeIn());
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
                ObjectPooler.instance.OnReset();
                SceneManager.LoadScene(sceneName);
                yield break;
            }
            yield return new WaitForSecondsRealtime(0.01f);
        }

        while (sceneName == "Menu Scene")
        {
            if (!pausePanelFadeOutRunning)
            {
                ObjectPooler.instance.OnReset();
                SceneManager.LoadScene(sceneName);
                yield break;
            }
            yield return new WaitForSecondsRealtime(0.01f);
        }
    }


    private IEnumerator ShowDiedPanelFadeIn()
    {
        if (Player.instance.isAlive) yield break;

        StartCoroutine(PauseButtonFadeOut());
        StartCoroutine(JoyStickFadeOut());
        yield return new WaitForSecondsRealtime(2.0f); //wait for plyer died
        
        Time.timeScale = 0f;
        GameManager.instance.isPause = true;

        diedPanel.SetActive(true);

        Transform diedTextTransform = diedPanel.transform.Find("Died Text");
        if (diedTextTransform == null) { Debug.Log("Not Found diedText"); }
        else
        {
            Text diedText = diedTextTransform.GetComponent<Text>();
            if (CheckBestScore())
            {
                DatabaseManager.SetScore(totalScore);
                diedText.color = Color.yellow;
                diedText.fontSize = 140;
                diedText.text = "Your\n Best Score";

                totalScoreBannerText.color = Color.yellow;
            }
            else
            {
                diedText.GetComponent<Text>().text = "Your Score";
            }
        }
        

        Image diedPanelImage = diedPanel.GetComponent<Image>();
        Color tmpColor = diedPanelImage.color;

        while (true)
        {
            diedPanel.transform.localScale -= new Vector3(0.2f, 0.2f, 0);

            tmpColor.a += 0.17f;
            diedPanelImage.color = tmpColor;

            if (diedPanel.transform.localScale.x <= 1.0f)
            {
                break;
            }

            yield return new WaitForSecondsRealtime(0.01f);
        }

        StartCoroutine("ScoreUp");
    }


    private string GetTimeToString(int time)
    {   //01:04:01  3600 + 240 + 1
        string hour, minute, seconds;
        if (time >= 3600) //hour
        {
            hour = (time / 3600).ToString();
            time = time % 3600;
            if (hour.Length == 1) // 1 ~ 9 -> 01 ~ 09
            {
                hour = "0" + hour;
            }
        }
        else
        {
            hour = "00";
        }

        if (time >= 60) //minute
        {
            minute = (time / 60).ToString();
            time = time % 60;
            if (minute.Length == 1) //1 ~ 9 ~ 01 ~ 09
            {
                minute = "0" + minute;
            }
        }
        else
        {
            minute = "00";
        }

        if (time > 0) //seconds
        {
            seconds = time.ToString();
            if (seconds.Length == 1)
            {
                seconds = "0" + seconds;
            }
        }
        else
        {
            seconds = "00";
        }

        string ret = minute + ":" + seconds;
        if (hour != "00") ret = hour + ":" + ret;
        return ret;
    }


    private IEnumerator ScoreUp()
    {
        scoreUpRunning = true;

        AudioSource audioSystem = GetComponent<AudioSource>();
        audioSystem.clip = scoreUpClip;
        audioSystem.Play();

        StartCoroutine(IndependentTimeScaleParticle());
        

        //time
        int tmpTotalTime = 0;
        while (true)
        {
            totalTimeText.text = GetTimeToString(tmpTotalTime);
            if (tmpTotalTime == totalPlayTime) break;
            tmpTotalTime += 1;
            yield return new WaitForSecondsRealtime(0.03f);
        }

        //item
        int tmpItemCount = 0;
        while (true)
        {
            totalItemText.text = "X " + tmpItemCount.ToString();
            if (tmpItemCount == totalGetItemCount) break;
            tmpItemCount += 1;
            yield return new WaitForSecondsRealtime(0.03f);
        }

        //soul
        int tmpSoulCount = 0;
        while (true)
        {
            totalSoulText.text = "X " + tmpSoulCount.ToString();
            if (tmpSoulCount == totalSoulCount) break;
            tmpSoulCount += 1;
            yield return new WaitForSecondsRealtime(0.03f);
        }

        //total score flash
        StartCoroutine(FlashScoreImage());

        scoreUpRunning = false;
    }


    public void SkipScoreUp()
    {
        if (!scoreUpRunning)
        {
            diedInnerPanel.SetActive(false);
            return;
        }
        
        StopCoroutine("ScoreUp");
        
        totalTimeText.text = GetTimeToString(totalPlayTime);
        totalItemText.text = "X " + totalGetItemCount.ToString();
        totalSoulText.text = "X " + totalSoulCount.ToString();
        totalScoreText.text = "Total: " + totalScore.ToString();
        totalScoreBannerText.text = totalScore.ToString();

        if (!isFlashScoreImageRunning)
        {
            StartCoroutine(FlashScoreImage());
        }

        scoreUpRunning = false;
        diedInnerPanel.SetActive(false);
    }


    private IEnumerator IndependentTimeScaleParticle()
    {
        while (true)
        {
            diedSoulParticle.Simulate(Time.unscaledDeltaTime, true, false);

            yield return new WaitForSecondsRealtime(0.01f);
        }
    }


    private IEnumerator FlashScoreImage()
    {
        isFlashScoreImageRunning = true;

        Color tmpColor = totalScoreBannerFlashImage.color;

        while (true)
        {
            tmpColor.a += 0.2f;
            totalScoreFlashImage.color = tmpColor;
            totalScoreBannerFlashImage.color = tmpColor;

            totalScoreFlashImage.transform.localScale += new Vector3(0.2f, 0.2f, 0);
            totalScoreBannerFlashImage.transform.localScale += new Vector3(0.2f, 0.2f, 0);
            if (tmpColor.a >= 1.0f) break;
            yield return new WaitForSecondsRealtime(0.01f);
        }

        totalScoreBannerText.text = totalScore.ToString();
        totalScoreText.text = "Total: " + totalScore.ToString();

        while (true)
        {
            tmpColor.a -= 0.05f;
            totalScoreFlashImage.color = tmpColor;
            totalScoreBannerFlashImage.color = tmpColor;

            totalScoreFlashImage.transform.localScale -= new Vector3(0.05f, 0.05f, 0);
            totalScoreBannerFlashImage.transform.localScale -= new Vector3(0.05f, 0.05f, 0);
            if (tmpColor.a <= 0) break;
            yield return new WaitForSecondsRealtime(0.01f);
        }

        diedInnerPanel.SetActive(false);

        //원상태로
        totalScoreFlashImage.transform.localScale = new Vector3(0, 0, 0);
        totalScoreBannerFlashImage.transform.localScale -= new Vector3(0, 0, 0);
        tmpColor.a = 0f;
        totalScoreFlashImage.color = tmpColor;
        totalScoreBannerFlashImage.color = tmpColor;

        isFlashScoreImageRunning = false;
    }


    private bool CheckBestScore()
    {
        if (dataScore < totalScore) return true;
        else return false;
    }


    private void SetBestScore()
    {

    }
}
