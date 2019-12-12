using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turtle01 : Enemy
{
    public int score = 5;
    public int hitCount = 1;
    public float speed = 5.0f;


    /********************************************
     * @함수명 : MoveToPlayer(float speed)
     * @작성자 : Malbong
     * @입력 : speed
     * @출력 : void
     * @설명 : base의 MoveToplayer(speed)에 개별 speed를 넣어 실행
     *         MoveToplayer(speed)의 내용은 Enemy.cs에 상세설명
     */
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

        //once used destroy instead SetActive(false);
        Destroy(gameObject);
    }
}
