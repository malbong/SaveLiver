using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System;
using UnityEngine.UI;

public static class DatabaseManager
{
    private static int score = 0;
    private static int[] boatChargeList = { 2, 2, 2, 2, 2 };
    private static int[] faceChargeList = { 2, 2, 2, 2, 2 };
    private static int[] waveChargeList = { 2, 2, 2, 2, 2 };


    public class SoulMoney
    {
        public int money = 0;

        public SoulMoney(int money)
        {
            this.money = money;
        }
    }

    public class Charge
    {
        public int timestamp;

        public Charge(int timestamp)
        {
            this.timestamp = timestamp;
        }
    }

    public class Custom
    {
        public int boat;
        public int face;
        public int wave;

        public Custom(int boat, int face, int wave)
        {
            this.boat = boat;
            this.face = face;
            this.wave = wave;
        }
    }

    public class Score
    {
        public int score;
        public int timestamp;

        public Score(int score, int timestamp)
        {
            this.score = score;
            this.timestamp = timestamp;
        }
    }


    public static int GetTimestamp()
    {
        DateTime now = DateTime.Now.ToLocalTime();
        TimeSpan span = (now - new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime());
        int timestamp = (int)span.TotalSeconds;
        return timestamp;
    }


    public static void UpdateMoney(int amount)
    {
        DatabaseReference reference = PlayerInformation.GetDatabaseReference()
            .Child("user")
            .Child("pnRD68Js9kU5O4UNvRaPcoueTsy2")
            //.Child(PlayerInformation.auth.CurrentUser.UserId)
            .Child("money");

        reference.GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                // Read
                DataSnapshot snapshot = task.Result;
                IDictionary data = (IDictionary)snapshot.Value;
                string dataMoney = data["money"].ToString();
                int tmpMoney = int.Parse(dataMoney);
                int finalAmount = amount + tmpMoney;
                PlayerInformation.SoulMoney = finalAmount;

                // Write
                SoulMoney soulMoney = new SoulMoney(PlayerInformation.SoulMoney);
                string json = JsonUtility.ToJson(soulMoney);
                reference.SetRawJsonValueAsync(json);

            }
        });
    }


    public static void SetNewUserData()
    {
        string userId = PlayerInformation.auth.CurrentUser.UserId;
        DatabaseReference reference = PlayerInformation.GetDatabaseReference()
            .Child("user");

        reference.GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                // New User
                if (!snapshot.Child(userId).Exists)
                {
                    Charge charge = new Charge(GetTimestamp());
                    string jsonCharge = JsonUtility.ToJson(charge);

                    Custom custom = new Custom(0, 0, 0);
                    string jsonCustom = JsonUtility.ToJson(custom);

                    SoulMoney soulMoney = new SoulMoney(0);
                    string jsonMoney = JsonUtility.ToJson(soulMoney);

                    Score score = new Score(0, GetTimestamp());
                    string jsonScore = JsonUtility.ToJson(score);

                    reference.Child(userId).Child("charge").Child("boat").Child("0").SetRawJsonValueAsync(jsonCharge);
                    reference.Child(userId).Child("charge").Child("face").Child("0").SetRawJsonValueAsync(jsonCharge);
                    reference.Child(userId).Child("charge").Child("wave").Child("0").SetRawJsonValueAsync(jsonCharge);
                    reference.Child(userId).Child("custom").SetRawJsonValueAsync(jsonCustom);
                    reference.Child(userId).Child("money").SetRawJsonValueAsync(jsonMoney);
                    reference.Child(userId).Child("score").SetRawJsonValueAsync(jsonScore);
                }
            }
        });
    }


    public static int[] GetCurrentCustom()
    {
        int[] customs = { 0, 0, 0 };
        DatabaseReference reference = PlayerInformation.GetDatabaseReference()
            .Child("user")
            .Child("pnRD68Js9kU5O4UNvRaPcoueTsy2")
            //.Child(PlayerInformation.auth.CurrentUser.UserId)
            .Child("custom");

        reference.GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                foreach (DataSnapshot data in snapshot.Children)
                {
                    if (data.Key.ToString() == "boat")
                        customs[0] = int.Parse(data.Value.ToString());
                    else if (data.Key.ToString() == "face")
                        customs[1] = int.Parse(data.Value.ToString());
                    else if (data.Key.ToString() == "wave")
                        customs[2] = int.Parse(data.Value.ToString());
                }
            }
            return customs;
        });
        return customs;
    }


    public static void SetCurrentCustom(int[] customs)
    {
        DatabaseReference reference = PlayerInformation.GetDatabaseReference()
            .Child("user")
            .Child("pnRD68Js9kU5O4UNvRaPcoueTsy2")
            //.Child(PlayerInformation.auth.CurrentUser.UserId)
            .Child("custom");

        Custom custom = new Custom(customs[0], customs[1], customs[2]);
        string jsonCustom = JsonUtility.ToJson(custom);

        reference.SetRawJsonValueAsync(jsonCustom);
    }


    public static int BoatCharge(int index)
    {
        DatabaseReference reference = PlayerInformation.GetDatabaseReference()
            .Child("user")
            .Child("pnRD68Js9kU5O4UNvRaPcoueTsy2")
            //.Child(PlayerInformation.auth.CurrentUser.UserId)
            .Child("charge")
            .Child("boat")
            .Child(index.ToString());

        reference.GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                if(snapshot == null || !snapshot.Exists) // 구매하지 않은 항목이라면
                {
                    boatChargeList[index] = -1;
                }
                else
                {
                    boatChargeList[index] = 1;
                }
            }
            return boatChargeList[index];
        });
        return boatChargeList[index];
    }


    public static int FaceCharge(int index)
    {
        DatabaseReference reference = PlayerInformation.GetDatabaseReference()
            .Child("user")
            .Child("pnRD68Js9kU5O4UNvRaPcoueTsy2")
            //.Child(PlayerInformation.auth.CurrentUser.UserId)
            .Child("charge")
            .Child("face")
            .Child(index.ToString());

        reference.GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                if (snapshot == null || !snapshot.Exists) // 구매하지 않은 곡이라면
                {
                    faceChargeList[index] = -1;
                }
                else
                {
                    faceChargeList[index] = 1;
                }
            }
            return faceChargeList[index];
        });
        return faceChargeList[index];
    }


    public static int WaveCharge(int index)
    {
        DatabaseReference reference = PlayerInformation.GetDatabaseReference()
            .Child("user")
            .Child("pnRD68Js9kU5O4UNvRaPcoueTsy2")
            //.Child(PlayerInformation.auth.CurrentUser.UserId)
            .Child("charge")
            .Child("wave")
            .Child(index.ToString());

        reference.GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                if (snapshot == null || !snapshot.Exists) // 구매하지 않은 곡이라면
                {
                    waveChargeList[index] = -1;
                }
                else
                {
                    waveChargeList[index] = 1;
                }
            }
            return waveChargeList[index];
        });
        return waveChargeList[index];
    }


    public static void SetChargeNewData(string name, int index)
    {
        DatabaseReference reference = PlayerInformation.GetDatabaseReference()
            .Child("user")
            .Child("pnRD68Js9kU5O4UNvRaPcoueTsy2")
            //.Child(PlayerInformation.auth.CurrentUser.UserId)
            .Child("charge");

        Charge charge = new Charge(GetTimestamp());
        string jsonCharge = JsonUtility.ToJson(charge);

        if (name == "boat")
        {
            reference.Child("boat").Child(index.ToString()).SetRawJsonValueAsync(jsonCharge);
        }
        else if(name == "face")
        {
            reference.Child("face").Child(index.ToString()).SetRawJsonValueAsync(jsonCharge);
        }
        else if(name == "wave")
        {
            reference.Child("wave").Child(index.ToString()).SetRawJsonValueAsync(jsonCharge);
        }
    }


    public static void SetScore(int newScore)
    {
        DatabaseReference reference = PlayerInformation.GetDatabaseReference()
            .Child("user")
            .Child("pnRD68Js9kU5O4UNvRaPcoueTsy2")
            //.Child(PlayerInformation.auth.CurrentUser.UserId)
            .Child("score");

        Score score = new Score(newScore, GetTimestamp());
        string jsonScore = JsonUtility.ToJson(score);

        reference.SetRawJsonValueAsync(jsonScore);
    }


    public static int GetScore()
    {
        DatabaseReference reference = PlayerInformation.GetDatabaseReference()
            .Child("user")
            .Child("pnRD68Js9kU5O4UNvRaPcoueTsy2")
            //.Child(PlayerInformation.auth.CurrentUser.UserId)
            .Child("score");

        reference.Child("score").GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot data = task.Result;

                string tmpScore = data.Value.ToString();
                score = int.Parse(tmpScore);

                PlayerInformation.BestScore = score;
            }
            return score;
        });
        return score;
    }
}