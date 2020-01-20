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

    public static Firebase.Auth.FirebaseAuth auth;

    public static DatabaseReference GetDatabaseReference()
    {
        DatabaseReference reference;
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://save-liver-d0f47.firebaseio.com/");
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        return reference;
    }
}
