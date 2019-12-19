using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtleLinear : Enemy
{
    public int score = 5;
    public int hitCount = 2;
    public float speed = 7.0f;
    private Rigidbody2D enemyRigid;
    

    private void Start()
    {
        enemyRigid = GetComponent<Rigidbody2D>();
        Destroy(transform.GetChild(1).gameObject, 1f);
    }


    /********************************************
     * @함수명 : Move()
     * @작성자 : Malbong
     * @입력 : void
     * @출력 : void
     * @설명 : 직선으로 가는 Turtle의 Move override
     *         생성당시의 Player의 위치를 기준으로 직선이동
     */
    public override void Move()
    {
        enemyRigid.velocity = -transform.up * speed;
        transform.parent.position = transform.position;
        transform.localPosition = Vector3.zero;
    }
    

    public override void HitOnPlayer(int hitCount)
    {
        base.HitOnPlayer(this.hitCount);
    }

    
    public override void OnDead()
    {
        //use Animation or Particle

        //use AudioSource.Play()

        //add score

        //destroy instead SetActive(false);

        //Destroy(transform.parent.gameObject);

        //add List
    }
}
