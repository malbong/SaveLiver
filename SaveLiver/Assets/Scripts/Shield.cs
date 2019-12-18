﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : Item, IItem
{
    public float itemDuration = 5f;
    private bool hasItem = false;
    private GameObject shield;

    void Start()
    {
        shield = Player.instance.gameObject.transform.GetChild(3).gameObject;
        StartCoroutine("TimeCheckAndDestroy");
    }

    void Update()
    {
        ItemDurationAndDestroy();
    }

    private void ItemDurationAndDestroy()
    {
        if (Time.time - shieldItemTime >= itemDuration && hasItem)
        {
            hasItem = false;
            shield.SetActive(false);
            Destroy(gameObject);
        }
    }

    public void Use()
    {
        GetComponent<Collider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
        shield.SetActive(true);
        shieldItemTime = Time.time;
        hasItem = true;
    }

    IEnumerator TimeCheckAndDestroy()
    {
        yield return new WaitForSeconds(itemLifeTime);
        if (!hasItem)
        {
            Destroy(gameObject);
        }
    }
}