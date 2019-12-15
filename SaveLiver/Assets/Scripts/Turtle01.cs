using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turtle01 : Enemy
{
    public int score = 5;
    public int hitCount = 1;
    public float speed = 5.0f;

    
    public override void MoveToPlayer(float speed)
    {
        base.MoveToPlayer(this.speed);
    }

    
    public override void Hit(int hitCount)
    {
        base.Hit(this.hitCount);
    }

    
    public override void OnDead()
    {
        //use Animation or Particle

        //use AudioSource.Play()

        //add score

        //once used destroy instead SetActive(false);
        Destroy(transform.parent.gameObject);
    }
}
