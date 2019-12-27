using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtleFollow : Enemy
{
    public Sprite getLiverSprite;
    public int score = 5;
    public int hitCount = 1;
    public float speed = 5.0f;
    public float rotateSpeed = 3.0f;
    private Rigidbody2D enemyRigid;
    private Coroutine runningCoroutine;
    

    private void Start()
    {
        enemyRigid = GetComponent<Rigidbody2D>();
    }


    /********************************************
     * @함수명 : Move()
     * @작성자 : Malbong
     * @입력 : void
     * @출력 : void
     * @설명 : 부모를 계속 따라가는 Move()
     *         각도를 구하고 천천히 따라감
     */
    public override void Move()
    {
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

        for (float i = angle % rotateSpeed; i < angle; i += rotateSpeed)
        {
            transform.Rotate(0, 0, rotateSpeed * sign);
            yield return null;
        }

        transform.Rotate(0, 0, angle % rotateSpeed);
    }


    public override void HitOnPlayer(int hitCount)
    {
        base.HitOnPlayer(this.hitCount);
    }

    
    public override void OnDead(bool getLiver)
    {
        if (base.isAlive == false) return; // dont re died

        base.isAlive = false; // died

        base.KeepOnTrail();

        transform.GetComponent<CircleCollider2D>().enabled = false;
        transform.GetChild(0).gameObject.SetActive(false);

        if (getLiver == true)
        {
            PlayParticle(onDeadParticleGetLiver);
            StartCoroutine(GetLiverFadeOut());
        }
        else
        {
            PlayParticle(onDeadParticle);
            StartCoroutine(FadeOut(onDeadParticle.main.duration));
        }
        //add score

        //add List

        //once used destroy instead SetActive(false);
        //transform.parent.gameObject.SetActive(false);


    }


    private IEnumerator GetLiverFadeOut()
    {
        SpriteRenderer spriteRenderer = transform.parent.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = getLiverSprite;
        while (true)
        {
            Color targetColor = spriteRenderer.color;
            targetColor.a -= Time.deltaTime;
            spriteRenderer.color = targetColor;
            yield return null;
            if (targetColor.a <= 0) break;
        }
        transform.parent.gameObject.SetActive(false);
    }


    private IEnumerator FadeOut(float waitTime)
    {
        SpriteRenderer spriteRenderer = transform.parent.GetComponent<SpriteRenderer>();
        while (true)
        {
            Color targetColor = spriteRenderer.color;
            targetColor.a -= Time.deltaTime;
            spriteRenderer.color = targetColor;
            yield return null;
            if (targetColor.a <= 0) break;
        }
        transform.parent.gameObject.SetActive(false);
    }


    private void PlayParticle(ParticleSystem targetParticle)
    {
        ParticleSystem particleInstance = Instantiate(targetParticle, transform.position, Quaternion.identity);
        particleInstance.Play();
        particleInstance.GetComponent<AudioSource>().Play();
        Destroy(particleInstance.gameObject, particleInstance.main.startLifetime.constant);
    }
}
