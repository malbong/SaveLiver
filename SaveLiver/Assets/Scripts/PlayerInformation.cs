using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using GooglePlayGames;

public static class PlayerInformation
{
    public static int SoulMoney { get; set; }
    public static bool TryOnceAutoAuth { get; set; }
    public static bool isLogin { get; set; } = false;
    public static bool isVibrationOn { get; set; } = true;
    public static int[] customs { get; set; } = { 0, 0, 0 };
    public static int PlayNum { get; set; }
    public static bool IsWatched { get; set; } = false;
    public static float Seconds { get; set; } = 0;
    public static int FinalSeconds { get; set; } = 0;
    public static int Minutes { get; set; } = 0;
    private static int tmpSeconds { get; set; } = 0;
    private static int tmp2Seconds { get; set; } = 0;

    public static int BestScore { get; set; }

    public static Firebase.Auth.FirebaseAuth auth;


    public static DatabaseReference GetDatabaseReference()
    {
        DatabaseReference reference;
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://save-liver-d0f47.firebaseio.com/");
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        return reference;
    }


    public static void SetFinalSeconds(int amount)
    {
        tmp2Seconds = amount;
        tmpSeconds = amount;
    }


    public static void AdTimeCheck()
    {
        Seconds += Time.deltaTime;

        if(Seconds > 1.0f)
        {
            tmpSeconds -= 1;
            tmp2Seconds -= 1;
            Seconds %= 1.0f;
        }

        Minutes = tmpSeconds / 60;
        FinalSeconds = tmp2Seconds % 60;
        

        if(Minutes == 0 && FinalSeconds == 0)
        {
           IsWatched = false;
        }
    }


    public static void AchievementPlayNum()
    {
        // beginner (10 판)
        PlayGamesPlatform.Instance.IncrementAchievement(GPGSIds.achievement_beginner, 1, null);
        // habit (100 판)
        PlayGamesPlatform.Instance.IncrementAchievement(GPGSIds.achievement_habit, 1, null);
        // addict (500 판)
        PlayGamesPlatform.Instance.IncrementAchievement(GPGSIds.achievement_addict, 1, null);
    }


    public static void AchievementOncePlay(int soul, int item)
    {
        // persist
        if(soul >= 50)
        {
            PlayGamesPlatform.Instance.ReportProgress(GPGSIds.achievement_persist, 100, null);
        }
        // greeds
        if(item >= 50)
        {
            PlayGamesPlatform.Instance.ReportProgress(GPGSIds.achievement_greed, 100, null);
        }
    }


    public static void AchievementScore(int score)
    {
        // newbie
        if(score >= 30)
        {
            PlayGamesPlatform.Instance.ReportProgress(GPGSIds.achievement_newbie, 100f, null);
        }
        // normal
        if(score >= 200)
        {
            PlayGamesPlatform.Instance.ReportProgress(GPGSIds.achievement_normal, 100f, null);
        }
        // pro
        if(score >= 500)
        {
            PlayGamesPlatform.Instance.ReportProgress(GPGSIds.achievement_pro, 100f, null);
        }
        // expert
        if(score >= 1000)
        {
            PlayGamesPlatform.Instance.ReportProgress(GPGSIds.achievement_expert, 100f, null);
        }
        // master
        if(score >= 1500)
        {
            PlayGamesPlatform.Instance.ReportProgress(GPGSIds.achievement_master, 100f, null);
        }
        // developer
        if(score >= 3000)
        {
            PlayGamesPlatform.Instance.ReportProgress(GPGSIds.achievement_developer, 100f, null);
        }
    }


    public static void AchievementSoul()
    {
        // small
        if(SoulMoney >= 100)
        {
            PlayGamesPlatform.Instance.ReportProgress(GPGSIds.achievement_small, 100f, null);
        }
        // saver
        if(SoulMoney >= 500)
        {
            PlayGamesPlatform.Instance.ReportProgress(GPGSIds.achievement_saver, 100f, null);
        }
        // fortune
        if(SoulMoney >= 1000)
        {
            PlayGamesPlatform.Instance.ReportProgress(GPGSIds.achievement_fortune, 100f, null);
        }
        // collector
        if(SoulMoney >= 2000)
        {
            PlayGamesPlatform.Instance.ReportProgress(GPGSIds.achievement_collector, 100f, null);
        }
        // rich
        if(SoulMoney >= 5000)
        {
            PlayGamesPlatform.Instance.ReportProgress(GPGSIds.achievement_rich, 100f, null);
        }
        // fat
        if(SoulMoney >= 10000)
        {
            PlayGamesPlatform.Instance.ReportProgress(GPGSIds.achievement_fat, 100f, null);
        }
    }
}
