using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : Item, IItem
{
    public int breathAmout = 1;
    public Animator popAni;

    public void Use()
    {
        Player.instance.breath += 1;
        StartCoroutine("AnimationEndDestroy");
    }

    IEnumerator AnimationEndDestroy()
    {
        popAni.SetTrigger("Pop");
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
}
