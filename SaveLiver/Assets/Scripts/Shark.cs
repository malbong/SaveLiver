using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shark : MonoBehaviour
{
    public float speed = 20.0f;
    public float lifeTime = 10.0f;
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
        if (other.tag == "Enemy")
        {
            other.GetComponent<Enemy>().OnDead();
        }
        else if (other.tag == "Player")
        {
            //Player.instance.TakeDamage(-1);
        }
    }
}
