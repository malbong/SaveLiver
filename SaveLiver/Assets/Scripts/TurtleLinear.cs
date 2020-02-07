using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TurtleLinear : Enemy
{
    public Sprite getLiverSprite;
    public int score = 2;
    public int hitCount = 1;
    public float speed = 7.0f;
    private Rigidbody2D enemyRigid;

    private bool isFadeOutRunning = false;


    private void Start()
    {
        base.isAlive = true;

        enemyRigid = GetComponent<Rigidbody2D>();

        transform.parent.position = transform.position;
        transform.localPosition = Vector3.zero;

        CircleCollider2D collider = transform.GetComponent<CircleCollider2D>();
        collider.enabled = true;
        transform.GetChild(0).gameObject.SetActive(true);

        SpriteRenderer spriteRenderer = transform.parent.GetComponent<SpriteRenderer>();
        spriteRenderer.color = new Color32(255, 255, 255, 255);
        spriteRenderer.sprite = sprite;
        
        StartCoroutine(DirectionSetActiveFalseAndFadeOut());

        StartCoroutine(TimeCheckAndDestroy());
    }


    private void OnEnable()
    {
        Start();
    }


    IEnumerator DirectionSetActiveFalseAndFadeOut()
    {
        Transform direction = transform.GetChild(1);

        SpriteRenderer directionSprite = direction.GetComponent<SpriteRenderer>();
        Color tmpColor = directionSprite.color;

        tmpColor.a = 1.0f;
        directionSprite.color = tmpColor;

        direction.gameObject.SetActive(true);
        
        while (true)
        {
            tmpColor.a -= Time.deltaTime;
            directionSprite.color = tmpColor;
            
            if (tmpColor.a <= 0) break;

            yield return new WaitForSeconds(Time.deltaTime);
        }

        direction.gameObject.SetActive(false);
        
        tmpColor.a = 1.0f;
        directionSprite.color = tmpColor;
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
            PlayParticle(true);

            StartCoroutine(GetLiverFadeOut());
        }
        else //getLiver == false
        {
            PlayParticle(false);

            soul.CreateSoul(transform.position, 0.3f);

            StartCoroutine(FadeOut());
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

        base.isAlive = false;

        transform.parent.gameObject.SetActive(false);
    }


    private IEnumerator FadeOut()
    {
        isFadeOutRunning = true;

        SpriteRenderer spriteRenderer = transform.parent.GetComponent<SpriteRenderer>();
        while (true)
        {
            Color targetColor = spriteRenderer.color;
            targetColor.a -= Time.deltaTime;
            spriteRenderer.color = targetColor;
            yield return null;
            if (targetColor.a <= 0) break;
        }

        isFadeOutRunning = false;

        base.isAlive = false;

        transform.parent.gameObject.SetActive(false);
    }


    private void PlayParticle(bool isGetLiver = false)
    {
        int index = isGetLiver == true ? 1 : 0;

        GameObject obj = ObjectPooler.instance.GetDeadParticle(index);
        obj.transform.position = transform.position;
        obj.SetActive(true);

        ParticleSystem particleInstance = obj.GetComponent<ParticleSystem>();
        particleInstance.Play();
        if (!isGetLiver)
        {
            //StartCoroutine(ShowIncreaseScoreText(particleInstance, score));
        }
        particleInstance.GetComponent<AudioSource>().Play();

        StartCoroutine(WaitSetActiveFalse(obj, 2.0f));
    }


    private IEnumerator WaitSetActiveFalse(GameObject obj, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        obj.SetActive(false);
    }


    public IEnumerator ShowIncreaseScoreText(ParticleSystem onDeadParticle, int score)
    {
        if (Player.instance.isAlive == true)
        {
            Transform canvas = onDeadParticle.transform.Find("ScoreCanvas");
            if (canvas != null)
            {
                Transform scoreTextTransform = canvas.Find("ScoreText");
                Vector3 tmpPosition = scoreTextTransform.localPosition;

                if (scoreTextTransform != null)
                {
                    Text scoreText = scoreTextTransform.GetComponent<Text>();
                    scoreText.text = "+" + score;
                    scoreText.gameObject.SetActive(true);

                    Color tmpColor = scoreText.color;
                    tmpColor.a = 1.0f;
                    scoreText.color = tmpColor;
                    while (true)
                    {
                        tmpColor.a -= Time.deltaTime;
                        scoreText.color = tmpColor;
                        scoreTextTransform.Translate(0, 0.002f, 0);
                        if (scoreText.color.a <= 0) break;
                        yield return new WaitForSeconds(Time.deltaTime);
                    }

                    scoreText.gameObject.SetActive(false);
                    tmpColor.a = 1.0f;
                    scoreText.color = tmpColor;
                    scoreTextTransform.localPosition = tmpPosition;

                }
            }
        }
    }


    private IEnumerator TimeCheckAndDestroy()
    {
        yield return new WaitForSeconds(lifeTime);

        if (base.isAlive == false) yield break; // dont re died

        base.isAlive = false;

        base.KeepOnTrail();

        transform.GetComponent<CircleCollider2D>().enabled = false;
        
        transform.GetChild(0).gameObject.SetActive(false);

        if (!isFadeOutRunning)
        {
            StartCoroutine(FadeOut());
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            base.HitOnPlayer();
        }
        else if (other.tag == "Enemy" || other.tag == "Dragon" || other.tag == "BoomEffect" || other.tag == "Bullet")
        {
            OnDead(false);
        }
        else if (other.tag == "Spear")
        {
            Transform spear = transform.Find("Spear");
            if (spear != null && other == spear.GetComponent<Collider2D>())
            {
                return; //내 spear에 부딪힌 경우 -> 속도차이 -> 안죽고 리턴
            }
            else
            {
                OnDead(false);
            }
        }
    }
}
