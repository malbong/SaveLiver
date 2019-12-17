using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public float lifeTime = 10.0f;
    public float rotateSpeed = 3.0f;
    private Rigidbody2D enemyRigid;
    private Coroutine runningCoroutine;
    //private Rigidbody2D parentRigid;
    

    private void Start()
    {
        //Invoke("OnDead", lifeTime);
        enemyRigid = GetComponent<Rigidbody2D>();
        //parentRigid = transform.parent.GetComponent<Rigidbody2D>();
    }


    private void FixedUpdate()
    {
        MoveToPlayer();
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Hit();
        }
        else if (other.tag == "Enemy")
        {
            OnDead();
        }
    }


    /********************************************
     * @함수명 : OnDead()
     * @작성자 : Malbong
     * @입력 : void
     * @출력 : void
     * @설명 : 제한시간이 초과하거나 Trigger로 인해 죽을 때 실행시킬 함수
     *         Animation 혹은 Particle을 실행
     *         자식에서 오버라이딩 필요
     */
    public abstract void OnDead();


    /********************************************
     * @함수명 : MoveToPlayer(float speed)
     * @작성자 : Malbong
     * @입력 : speed (default speed == 10)
     * @출력 : void
     * @설명 : Update에 기술하여 enemy 생성 후 소멸될 때까지 진행 
     *         Player로 이동하는 함수 + 회전
     *         Enemy class에 기본구현
     *         Enemy class를 상속받는 쪽에서 base.MoveToPlayer(speed)로 개별 인자를 보냄
     */
    public virtual void MoveToPlayer(float speed = 1.0f)
    {
        //transform.Translate(0, -speed * Time.deltaTime, 0);
        
        enemyRigid.velocity = -transform.up * speed;
        transform.parent.position = transform.position;
        //parentRigid.velocity = -transform.up * speed;
        transform.localPosition = Vector3.zero;

        Vector3 currentVec = -transform.up; //현재 이동방향 단위벡터
        Vector3 diffVec = (Player.instance.transform.position - transform.position).normalized; // 적 -> 플레이어 단위벡터

        float angle = Vector3.Angle(currentVec, diffVec); //0 ~ 180
        int sign = Vector3.Cross(currentVec, diffVec).z < 0 ? -1 : 1; // -1 or 1

        if (runningCoroutine != null)
        {
            StopCoroutine(runningCoroutine);
        }
        runningCoroutine = StartCoroutine(RotateAngle(angle, sign));

        //float finalAngle = angle * sign;
        //Vector3 rotationVec = new Vector3(0, 0, finalAngle); //finalAngle의 벡터화
        //Quaternion targetRotation = Quaternion.Euler(transform.rotation.eulerAngles + rotationVec); //쿼터니언 <- 현재 회전값 + 회전해야할 값 
        //transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 0.03f); //부드러운 회전을 위함
    }

    IEnumerator RotateAngle(float angle, int sign)
    {
        
        for (float i = angle%rotateSpeed; i < angle; i += rotateSpeed)
        {
            transform.Rotate(0, 0, rotateSpeed * sign);
            yield return null;
        }
        
        transform.Rotate(0, 0, angle%rotateSpeed);   
    }

    /********************************************
     * @함수명 : Hit()
     * @작성자 : Malbong
     * @입력 : void
     * @출력 : void
     * @설명 : OnTriggerEnter2D()에서 실행시킬 함수
     *         Player의 hp를 깎음
     */
    public virtual void Hit(int hitCount = 1)
    {
        Player.instance.hp -= hitCount;
        if (Player.instance.hp <= 0)
        {
            Player.instance.OnDead();
        }
    }
}