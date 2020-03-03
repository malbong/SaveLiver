using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using Firebase.Auth;

public class GoogleAuth : MonoBehaviour
{
    private FirebaseAuth auth;
    public StoreManager storeManager;


    void Start()
    {
        auth = FirebaseAuth.DefaultInstance;
        // 로그인 시도가 없었다면 로그인 시도
        if (!PlayerInformation.TryOnceAutoAuth)
        {
            PlayerInformation.TryOnceAutoAuth = true;
            TryGoogleLoginOrLogout();
        }
    }


    private void Update()
    {
        SettingsManager.instance.UpdateLoginAndLogout(Social.localUser.authenticated);
    }


    public bool TryGoogleLoginOrLogout()
    {
        InitializedGooglePlay(); // 구글 플레이 플랫폼 활성화(초기화)
        if (!Social.localUser.authenticated) // 로그인이 되어 있지 않다면
        {
            Social.localUser.Authenticate(success =>
            {
                if (success)
                {
                    StartCoroutine(TryFirebaseLogin());
                }
            });
        }
        else // 로그인 되어 있다면
        {
            // 로그아웃
            auth.SignOut();
            PlayerInformation.auth.SignOut();
            PlayGamesPlatform.Instance.SignOut();
            PlayerInformation.isLogin = false;
        }
        return PlayerInformation.isLogin;
    }


    private void InitializedGooglePlay()
    {
        PlayGamesPlatform.InitializeInstance(new PlayGamesClientConfiguration.Builder()
            .RequestIdToken()
            .RequestEmail()
            .Build());
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
    }


    IEnumerator TryFirebaseLogin()
    {
        while (string.IsNullOrEmpty(((PlayGamesLocalUser)Social.localUser).GetIdToken()))
            yield return null;
        string idToken = ((PlayGamesLocalUser)Social.localUser).GetIdToken();

        Credential credential = GoogleAuthProvider.GetCredential(idToken, null);
        auth.SignInWithCredentialAsync(credential).ContinueWith(task =>
        {
            if(task.IsCompleted && !task.IsCanceled && !task.IsFaulted)
            {
                FirebaseUser newUser = task.Result;
                PlayerInformation.auth = auth;
                PlayerInformation.isLogin = true;
                DatabaseManager.SetNewUserData();
                DatabaseManager.UpdateMoney(0);
                PlayerInformation.BestScore = DatabaseManager.GetScore(true);
                PlayerInformation.EasyBestScore = DatabaseManager.GetScore(false);

                storeManager.InitStoreAsync();
                storeManager.InitFaceCharge();
                storeManager.InitBoatCharge();
                storeManager.InitWaveCharge();
            }
        });
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
                    StartCoroutine(TryFirebaseLogin());
                    Social.ShowLeaderboardUI(); // 리더보드 띄우기
                    return;
                }
            });
        }

        PlayGamesPlatform.Instance.ShowLeaderboardUI();
    }


    public void ShowAchievement()
    {
        // 업적 띄우기 전에 로그인 여부 확인
        if (!Social.localUser.authenticated) // 로그인 안 되어 있다면
        {
            Social.localUser.Authenticate(success =>
            {
                if (success) // 로그인 성공하면
                {
                    StartCoroutine(TryFirebaseLogin());
                    Social.ShowAchievementsUI(); // 업적 띄우기
                    return;
                }
            });
        }

        PlayGamesPlatform.Instance.ShowAchievementsUI();
    }
}