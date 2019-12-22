using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragon : MonoBehaviour
{
    public float speed = 10.0f;
    public float lifeTime = 10.0f;
    private bool isDead = false;
    //private bool isHitOnPlayer = false;
    private Rigidbody2D enemyRigid;
    private Renderer dragonRenderer;

    public Animator DeadAnim;

    private void Start()
    {
        dragonRenderer = GetComponent<Renderer>();
        enemyRigid = GetComponent<Rigidbody2D>();

        //Destroy(gameObject, lifeTime);
    }


    private void Update()
    {
        Move();
    }


    public void Move()
    {
        if(!isDead) enemyRigid.velocity = -transform.right * speed;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            //check isHitOnPlayer 
            //check player fever -> onDead()
            //Player.instance.TakeDamage(true);
            //isHitOnPlayer = true
            if (Player.instance.isFevered) OnDead();
            Player.instance.TakeDamage(true);
        }
        else if(other.tag == "BoomEffect")
        {
            OnDead();
        }
    }



    private void OnDead()
    {
        isDead = true;
        DeadAnim.SetTrigger("Dead");
        StartCoroutine("FadeOut");
    }


    IEnumerator FadeOut()
    {
        Color color = GetComponent<SpriteRenderer>().color;
        for (int i = 0; i < 100; i++)
        {
            color.a -= 0.01f;
            dragonRenderer.material.color = color;
            yield return new WaitForSeconds(0.01f);
        }
    }
}
