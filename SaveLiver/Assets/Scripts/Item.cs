using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public float lifeTime = 10.0f;
    protected GameObject shield;
    private GameObject player;

    void Start()
    {
        player = GameObject.Find("Player");
        shield = player.transform.GetChild(3).gameObject;
        // GetChild(3) : Hare Shield
    }

    void Update()
    {
        
    }

}
