using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TurtleFollow : Enemy
{
    public Sprite getLiverSprite;
    public int score = 5;
    public int hitCount = 1;
    public float speed = 5.0f;
    public float rotateSpeed = 3.0f;
    private Rigidbody2D enemyRigid;
    private Coroutine runningCoroutine;

    private GameObject indicatorObj;
    private bool hasIndicator = false;


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

        //Invoke("OnDead", lifeTime);
    }


    private void OnEnable()
    {
        Start();
    }


    private void Update()
    {
        if (GameManager.instance.isPause) return;

        SetMyIndicator();
        if (hasIndicator)
        {
            OffScreenIndicator.instance.DrawIndicator(gameObject, indicatorObj);
        }
        transform.parent.position = transform.position;
        //parentRigid.velocity = -transform.up * speed;
        transform.localPosition = Vector3.zero;
    }


    /********************************************
     * @함수명 : SetMyIndicator()
     * @작성자 : zeli
     * @입력 : void
     * @출력 : void
     * @설명 : 나의 Indicator를 생성, 제거
     *         Update에서 실행함
     */
    private void SetMyIndicator()
    {
        if (IsOffScreen() && !hasIndicator && isAlive) // 화면 밖인데 내 Indicator가 없으면
        {
            indicatorObj = ObjectPooler.instance.GetIndicatorObject(0); // 내 Indicator 생성
            indicatorObj.SetActive(true);
            hasIndicator = true;
        }
        else if (!IsOffScreen() && hasIndicator) // 화면 안인데 내 Indicator가 있으면
        {
            indicatorObj.SetActive(false); // 내 Indicator를 비활성화
            hasIndicator = false;
        }
    }


    /********************************************
     * @함수명 : IsOffScreen()
     * @작성자 : zeli
     * @입력 : void
     * @출력 : bool
     * @설명 : Enemy가 화면 밖에 있는지 검사
     *         밖: true, 안: false
     */
    private bool IsOffScreen()
    {
        Vector3 vec = Camera.main.WorldToViewportPoint(transform.position);
        if (vec.x <= 1 && vec.y <= 1 && vec.x >= 0 && vec.y >= 0)
            return false;
        else
            return true;
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

        Vector3 currentVec = -transform.up; //현재 이동방향 단위벡터
        Vector3 diffVec = (Player.instance.transform.position - transform.position).normalized; // 적 -> 플레이어 단위벡터

        float angle = Vector3.Angle(currentVec, diffVec); //0 ~ 180
        int sign = Vector3.Cross(currentVec, diffVec).z < 0 ? -1 : 1; // -1 or 1
        
        if(angle > rotateSpeed) transform.Rotate(0, 0, sign * rotateSpeed);
        else transform.Rotate(0, 0, sign * angle);

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
            yield return new WaitForSeconds(0.01f);
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
        if (hasIndicator && indicatorObj != null)
        {
            hasIndicator = false;
            indicatorObj.SetActive(false);
        }
        transform.GetChild(0).gameObject.SetActive(false);
        
        if (getLiver == true)
        {
            PlayParticle(true);
            StartCoroutine(GetLiverFadeOut()); //end setActive(false)
        }
        else //getLiver == false
        {
            PlayParticle(false);

            soul.CreateSoul(transform.position, 1);

            StartCoroutine(FadeOut(onDeadParticle.main.duration));//end setActive(false)
        }
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

        StartCoroutine(WaitSetActiveFalse(obj));
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


    private IEnumerator WaitSetActiveFalse(GameObject obj)
    {
        yield return new WaitForSeconds(2.0f);
        obj.SetActive(false);
    }


    private void CreateSoul()
    {
        //Instantiate()
    }
}
