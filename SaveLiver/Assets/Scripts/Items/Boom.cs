using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boom : MonoBehaviour
{
    public Animator anim;


    void Start()
    {
        anim.SetTrigger("trigger");

        StartCoroutine(TimeCheckAndDestroy());
    }


    private void OnEnable()
    {
        Start();
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            Player.instance.TakeDamage(1);
        }
    }


    private IEnumerator TimeCheckAndDestroy()
    {
        yield return new WaitForSeconds(0.68f);

        gameObject.SetActive(false);
    }
}
