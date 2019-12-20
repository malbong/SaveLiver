using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingHook : MonoBehaviour
{
    public float lifeTime = 10f;


    void Start()
    {
        StartCoroutine("TimeCheckAndDestroy");
    }

    void Update()
    {
        
    }



    // 플레이어 한방에 끌려가기? liver -1?
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {

        }
        else if(other.tag == "Enemy")
        {

        }
    }


    /**************************************
    * @ Shield와 동일
    */
    IEnumerator TimeCheckAndDestroy()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }
}
