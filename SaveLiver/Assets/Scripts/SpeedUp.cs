using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUp : Item, IItem
{
    public float itemDuration = 8f;
    private bool hasItem = false;

    void Start()
    {
        StartCoroutine("TimeCheckAndDestroy");
    }

    void Update()
    {
        ItemDurationAndDestroy();
    }

    private void ItemDurationAndDestroy()
    {
        if (Time.time - speedUpItemTime >= itemDuration && hasItem)
        {
            hasItem = false;
            Player.instance.speed = 3f;
            Destroy(gameObject);
        }
    }

    public void Use()
    {
        GetComponent<Collider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
        Player.instance.speed = 5f;
        speedUpItemTime = Time.time;
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