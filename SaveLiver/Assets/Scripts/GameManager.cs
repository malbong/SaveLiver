using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GooglePlayGames;
using GooglePlayGames.BasicApi;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; set; }

    public Text liverCountText;
    public Text scoreText;

    public int totalScore = 0;

    private float currentplayTime = 0;
    private float secondsUnit = 0;

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
        totalScore = 0;
        UpdateLiverCountText(3);
        UpdateScoreText();
    }


    void Update()
    {
        if (Time.timeScale == 0) return;
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
}
