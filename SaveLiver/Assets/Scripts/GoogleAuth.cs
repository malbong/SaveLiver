using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GooglePlayGames;
using GooglePlayGames.BasicApi;

public class GoogleAuth : MonoBehaviour
{
    public Text logText;

    void Start()
    {
        // 로그인 시도가 없었다면 로그인 시도
        if (!PlayerInformation.TryOnceAutoAuth) TryGoogleLoginOrLogout();
    }


    private void Update()
    {
        if (PlayerInformation.isLogin) logText.text = "Log Out";
        else logText.text = "Log In";
    }


    public void TryGoogleLoginOrLogout()
    {
        InitializedGooglePlay(); // 구글 플레이 플랫폼 활성화(초기화)
        if (!Social.localUser.authenticated) // 로그인이 되어 있지 않다면
        {
            PlayerInformation.TryOnceAutoAuth = true;
            Social.localUser.Authenticate(success =>
            {
                if (success)
                {
                    PlayerInformation.isLogin = true;
                }
            });
        }
        else // 로그인 되어 있다면
        {
            PlayerInformation.isLogin = false;
            // 로그아웃
            PlayGamesPlatform.Instance.SignOut();
        }
    }


    private void InitializedGooglePlay()
    {
        PlayGamesPlatform.InitializeInstance(new PlayGamesClientConfiguration.Builder().Build());
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
    }


    public void ShowLeaderboard()
    {
        // 리더보드 띄우기 전에 로그인 여부 확인
        if (!Social.localUser.authenticated) // 로그인 안 되어 있다면
        {
            Social.localUser.Authenticate(success =>
            {
                if (success) // 로그인 성공하면
                {
                    Social.ShowLeaderboardUI(); // 리더보드 띄우기
                    return;
                }
                else // 로그인 실패하면
                {
                    // 로그인이 안 되어 있다고 보여주는 처리
                    return;
                }
            });
        }

        PlayGamesPlatform.Instance.ShowLeaderboardUI();
    }
}