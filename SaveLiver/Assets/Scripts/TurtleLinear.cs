using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtleLinear : Enemy
{
    public Sprite getLiverSprite;
    public int score = 5;
    public int hitCount = 2;
    public float speed = 7.0f;
    private Rigidbody2D enemyRigid;
    

    private void Start()
    {
        enemyRigid = GetComponent<Rigidbody2D>();
        Destroy(transform.GetChild(1).gameObject, 1.5f);
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

    
    public override void OnDead(bool getLiver = false)
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
        //use AudioSource.Play()

        //add score

        //destroy instead SetActive(false);

        //Destroy(transform.parent.gameObject);

        //add List
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
        //yield return new WaitForSeconds(waitTime);
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
