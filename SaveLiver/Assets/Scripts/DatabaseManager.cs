using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System;
using UnityEngine.UI;

public class DatabaseManager : MonoBehaviour
{
    public bool loadingLock = true;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }


    public class SoulMoney
    {
        public int money = 0;

        public SoulMoney(int money)
        {
            this.money = money;
        }
    }


    public int GetTimestamp()
    {
        DateTime now = DateTime.Now.ToLocalTime();
        TimeSpan span = (now - new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime());
        int timestamp = (int)span.TotalSeconds;
        return timestamp;
    }


    public void UpdateMoney(int amount)
    {
        DatabaseReference reference = PlayerInformation.GetDatabaseReference()
            .Child("user")
            .Child(PlayerInformation.auth.CurrentUser.UserId)
            .Child("money");

        reference.GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                // Read
                DataSnapshot snapshot = task.Result;
                IDictionary data = (IDictionary)snapshot.Value;
                int dataMoney = int.Parse(data["money"].ToString());
                int finalAmount = dataMoney + amount;
                PlayerInformation.SoulMoney = finalAmount;

                // Write
                SoulMoney soulMoney = new SoulMoney(PlayerInformation.SoulMoney);
                string json = JsonUtility.ToJson(soulMoney);
                reference.SetRawJsonValueAsync(json);

            }
        });
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


    public void SetNewUserData()
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

                    /*
                    Dictionary<string, object> childUpdates = new Dictionary<string, object>();
                    
                    // charge list
                    childUpdates["/user/" + userId + "/charge/" + "/boat/" + "/0/" + "/" + "timestamp"] = GetTimestamp().ToString();
                    childUpdates["/user/" + userId + "/charge/" + "/face/" + "/0/" + "/" + "timestamp"] = GetTimestamp().ToString();
                    childUpdates["/user/" + userId + "/charge/" + "/wave/" + "/0/" + "/" + "timestamp"] = GetTimestamp().ToString();

                    // custom list
                    childUpdates["/user/" + userId + "/custom/" + "/" + "boat"] = "0";
                    childUpdates["/user/" + userId + "/custom/" + "/" + "face"] = "0";
                    childUpdates["/user/" + userId + "/custom/" + "/" + "wave"] = "0";

                    childUpdates["/user/" + userId + "/money/" + "/" + "money"] = "0";

                    childUpdates["/user/" + userId + "/score/" + "/" + "score"] = "0";
                    childUpdates["/user/" + userId + "/score/" + "/" + "timestamp"] = GetTimestamp().ToString();

                    reference.UpdateChildrenAsync(childUpdates);
                    */
                }
            }
        });
    }

    public int[] GetCurrentCustom()
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

    /*
    int[] chargeList = { 2, 2, 2, 2, 2 };

    public int GetChargeList(string name, int index)
    {
        DatabaseReference reference = PlayerInformation.GetDatabaseReference()
            .Child("user")
            .Child("pnRD68Js9kU5O4UNvRaPcoueTsy2")
            //.Child(PlayerInformation.auth.CurrentUser.UserId)
            .Child("charge");

        if(name == "boat")
        {
            reference.Child("boat").Child(index.ToString()).GetValueAsync().ContinueWith(task =>
            {
                if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;
                    if (snapshot.Exists)
                    {
                        chargeList[index] = 1;
                    }
                    else
                    {
                        chargeList[index] = -1;
                    }
                }
                loadingLock = false;
                return chargeList[index];
            });
        }
        else if(name == "face")
        {
            reference.Child("face").Child(index.ToString()).GetValueAsync().ContinueWith(task =>
            {
                if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;
                    if (snapshot.Exists)
                    {
                        Debug.Log(snapshot);
                        chargeList[index] = 1;
                    }
                    else
                    {
                        chargeList[index] = -1;
                    }
                }
                loadingLock = false;
                return chargeList[index];
            });
        }
        else
        {
            reference.Child("wave").Child(index.ToString()).GetValueAsync().ContinueWith(task =>
            {
                if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;
                    if (snapshot.Exists)
                    {
                        chargeList[index] = 1;
                    }
                    else
                    {
                        chargeList[index] = -1;
                    }
                }
                loadingLock = false;
                return chargeList[index];
            });
        }
        return 2;
    }
    */

}