using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shark : MonoBehaviour
{
    public float speed = 20.0f;
    public float lifeTime = 10.0f;
    private bool isHitOnPlayer = false;
    private Rigidbody2D enemyRigid;


    private void Start()
    {
        enemyRigid = GetComponent<Rigidbody2D>();

        //Destroy(gameObject, lifeTime);
    }


    private void Update()
    {
        Move();
    }


    public void Move()
    {
        enemyRigid.velocity = -transform.right * speed;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            //check isHitOnPlayer 
            //check player fever -> onDead()
            //Player.instance.TakeDamage(true);
            //isHitOnPlayer = true
        }
        else if (other.tag == "Shark")
        {
            //onDead();
        }
    }
    
    
    //OnDead() implement
}
