using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;

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
}
