using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : Item, IItem
{
    Coroutine runningCoroutine = null;

    public void Use()
    {
        shield.SetActive(true);
        runningCoroutine = StartCoroutine(itemLife(lifeTime));
        Destroy(gameObject);
    }

    IEnumerator itemLife(float lifeTime)
    {
        yield return new WaitForSeconds(lifeTime);
        shield.SetActive(false);
    }
}
