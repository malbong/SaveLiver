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

    public static void UpdateMoney(int amount)
    {
        if (!isLogin) SoulMoney -= 5; // test

        DatabaseReference reference = GetDatabaseReference()
            .Child("user")
            .Child("pnRD68Js9kU5O4UNvRaPcoueTsy2")
            //.Child(auth.CurrentUser.UserId)
            .Child("money");

        reference.GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                IDictionary data = (IDictionary)snapshot.Value;
                int dataMoney = int.Parse(data["money"].ToString());
                SoulMoney = dataMoney + amount;


            }
        });
    }
}
