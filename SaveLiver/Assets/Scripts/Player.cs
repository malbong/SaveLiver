using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public static Player instance { get; set; }

    // 싱글톤
    private void Awake()
    {
        if (instance == null)
        {
            instance = this; // instance 초기화
        }
        else if(instance != this)
        {
            Destroy(gameObject);
        }
    }


    public int hp;
    public float speed;
    public float rotateSpeed;

    public bool isFevered = false;

    public ParticleSystem onDeadParticle;

    private Camera cam;

    private Coroutine runningCoroutine = null;

    public Transform arrowRotate; // 화살표 회전 컨트롤

    private Rigidbody2D playerRigid;
    public Animator feverAni;
    private SpriteRenderer playerSpriteRenderer;
    private BoxCollider2D playerCollider;

    public GameObject shield;
    public Sprite shieldSprite;
    
    public bool isAlive = true;

    public bool HasShield { get; set; } = false;
    public bool HasSpeedUp { get; set; } = false;
    public bool HasRotateUp { get; set; } = false;

    public int feverNum { get; set; } = 0;
    public int speedUpNum { get; set; } = 0;

    public Animator boatAnim;

    public Text decreaseHpText;

    void Start()
    {
        playerRigid = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<BoxCollider2D>();
        playerSpriteRenderer = GetComponent<SpriteRenderer>();
        runningCoroutine = StartCoroutine(RotateAngle(180, -1)); // 시작하면 Player를 180도 오른쪽으로 돌리기.
    }

    void FixedUpdate()
    {
        if (GameManager.instance.isPause) return;

        if (isAlive)
        {
            playerRigid.velocity = arrowRotate.up * speed; // 화살표 방향으로 speed만큼 직진
        }
        else
        {
            playerRigid.velocity = Vector3.zero;
        }
    }


    /**************************************
    * @함수명: TurnAngle(Vector3 currentJoystickVec)
    * @작성자: zeli
    * @입력: currentJoystickVec (조이스틱 방향)
    * @출력: void
    * @설명: 조이스틱의 방향을 읽고, 그 방향으로 화살표를 회전시키는 메소드
    *        터치가 발생하면 호출됨
    */
    public void TurnAngle(Vector3 currentJoystickVec)
    {
        Vector3 originJoystickVec = arrowRotate.up; // 화살표 방향 벡터

        float angle = Vector3.Angle(currentJoystickVec, originJoystickVec);
        int sign = (Vector3.Cross(currentJoystickVec, originJoystickVec).z > 0) ? -1 : 1;
        // angle: 현재 바라보고 있는 벡터와, 조이스틱 방향 벡터 사이의 각도
        // sign: Player가 바라보는 방향 기준으로, 왼쪽:+ 오른쪽:-

        //코루틴이 실행중이면 실행 중인 코루틴 중단 후 코루틴 실행 (코루틴이 한 개만 존재하도록) 
        if (runningCoroutine != null)
        {
            StopCoroutine(runningCoroutine);
        }
        runningCoroutine = StartCoroutine(RotateAngle(angle, sign));
    }
    

    /**************************************
    * @함수명: RotateAngle(float angle, int sign)
    * @작성자: zeli
    * @입력: angle(현재 바라보고 있는 방향과 클릭한 방향의 사이각도), sign(각도 부호)
    * @출력: IEnumerator
    * @설명: 일정한 속도(rotateSpeed)로 회전.
    *        터치가 발생하면 호출됨
    */
    IEnumerator RotateAngle(float angle, int sign)
    {
        float mod = angle % rotateSpeed;
        for (float i = mod; i < angle; i += rotateSpeed)
        {
            arrowRotate.Rotate(0, 0, sign * rotateSpeed);
            yield return new WaitForSeconds(0.01f); // 1프레임 대기
        }
        
        arrowRotate.Rotate(0, 0, sign * mod); // 남은 각도 회전
    }


    /**************************************
    * @함수명: TakeDamage
    * @작성자: zeli
    * @입력: damage, isDragon (overload)
    * @출력: void
    * @설명: 적들로부터 받은 데미지를 받은 후 계산
    *        hp(Liver)가 없으면 OnDead 처리
    *        피격효과 실행
    */
    public void TakeDamage(int damage)
    {
        if (HasShield) 
        {
            ShieldEnd();
            return;
        }

        if (isFevered)
        {
            return;
        }

        if(hp > 1)
        {
            StartCoroutine(PlayerBeat());
            StartCoroutine(ShowDecreaseHpText(damage));
        }

        hp -= damage;
        GameManager.instance.UpdateLiverCountText(hp);
        if(hp <= 0)
        {
            OnDead();
        }
    }

    public void TakeDamage(bool isDragon)
    {
        if (HasShield)
        {
            ShieldEnd();
            return;
        }
        if (isFevered)
        {
            return;
        }

        if(isDragon)
        {
            hp = 0;
            GameManager.instance.UpdateLiverCountText(hp);
            OnDead();
        }
    }



    /**************************************
    * @함수명: PlayerBeat
    * @작성자: zeli
    * @입력: void
    * @출력: IEnumertor
    * @설명: Player가 피격될 시 깜빡임 효과
    */
    IEnumerator PlayerBeat()
    {
        playerCollider.enabled = false;
        int count = 0;
        
        while(count < 10)
        {
            if(count%2 == 0)
                playerSpriteRenderer.color = new Color32(255, 255, 255, 90);
            else
                playerSpriteRenderer.color = new Color32(255, 255, 255, 180);

            yield return new WaitForSeconds(0.16f);
            count += 1;
        }

        playerSpriteRenderer.color = new Color32(255, 255, 255, 255);
        playerCollider.enabled = true;
    }




    /**************************************
    * @함수명: OnDead
    * @작성자: zeli, malbong
    * @입력: void
    * @출력: void
    * @설명: hp(Liver)가 없으면 발동
    */
    public void OnDead()
    {
        if (isAlive == false) return; // dont re died

        isAlive = false; // died

        GetComponent<BoxCollider2D>().enabled = false;

        cam = Camera.main;
        cam.transform.parent = null;

        ParticleSystem instance = Instantiate(onDeadParticle, transform.position, Quaternion.identity);
        instance.Play();
        instance.GetComponent<AudioSource>().Play();
        Destroy(instance.gameObject, instance.main.startLifetime.constant);

        KeepOnTrail();

        GameManager.instance.ReportScore(GameManager.instance.totalScore);

        transform.gameObject.SetActive(false);

        Invoke("Respawn", 2f);// test용. 죽으면 살아나기.

        //once used destroy instead SetActive(false);
        //transform.parent.gameObject.SetActive(false);
    }



    // test용 재시작
    private void Respawn()
    {
        SceneManager.LoadScene(0);
    }


    /**************************************
    * @함수명: FeverTime
    * @작성자: zeli
    * @입력: breath
    * @출력: void
    * @설명: 무적 활성화
    *        무적 애니메이션 재생
    *        Fever에서 사용함
    */
    public void FeverTime()
    {
        isFevered = true;
        feverAni.SetBool("feverAnimation", true);

        boatAnim.SetBool("feverAnimation", true);
    }


    /**************************************
    * @함수명: EndFeverTime
    * @작성자: zeli
    * @입력: breath
    * @출력: void
    * @설명: 무적 비활성화
    *        무적 애니메이션 정지
    *        Fever에서 사용
    */
    public void EndFeverTime()
    {
        isFevered = false;
        feverAni.SetBool("feverAnimation", false);

        boatAnim.SetBool("feverAnimation", false);
    }


    private void KeepOnTrail()
    {
        Transform rotator = transform.Find("Rotator");
        if (rotator != null)
        {
            Transform trailWaveLeft = rotator.Find("Trail Wave Left");
            Transform trailWaveRight = rotator.Find("Trail Wave Right");
            if (trailWaveLeft != null && trailWaveRight != null)
            {
                trailWaveLeft.parent = null;
                trailWaveRight.parent = null;

                ParticleSystem trailWaveLeftParticle = trailWaveLeft.GetComponent<ParticleSystem>();
                ParticleSystem trailWaveRightParticle = trailWaveRight.GetComponent<ParticleSystem>();

                trailWaveLeftParticle.Stop();
                trailWaveRightParticle.Stop();

                Destroy(trailWaveLeftParticle.gameObject, trailWaveLeftParticle.main.duration);
                Destroy(trailWaveRightParticle.gameObject, trailWaveLeftParticle.main.duration);
            }
            else
            {
                Debug.Log("Not Find Trail Wave");
            }
        }
        else
        {
            Debug.Log("Not Find Rotator");
        }
    }


    public void ShieldStart()
    {
        HasShield = true;
        shield.SetActive(true);
    }


    public void ShieldEnd()
    {
        HasShield = false;
        shield.GetComponent<Animator>().SetTrigger("Broken");
        ItemManager.instance.ShieldBrokenPlay();
        StartCoroutine(ShieldEndWait());
    }


    IEnumerator ShieldEndWait()
    {
        yield return new WaitForSeconds(0.2f);
        shield.GetComponent<SpriteRenderer>().sprite = shieldSprite;
        shield.SetActive(false);
    }


    public IEnumerator ShowDecreaseHpText(int damage)
    {
        decreaseHpText.text = "-" + damage;
        decreaseHpText.gameObject.SetActive(true);
        Vector3 tmpPosition = decreaseHpText.gameObject.transform.localPosition;

        Color tmpColor = decreaseHpText.color;
        while (true)
        {
            tmpColor.a -= Time.deltaTime;
            decreaseHpText.color = tmpColor;
            decreaseHpText.gameObject.transform.Translate(0, 0.002f, 0);
            if (decreaseHpText.color.a <= 0) break;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        decreaseHpText.gameObject.SetActive(false);
        tmpColor.a = 1.0f;
        decreaseHpText.color = tmpColor;
        decreaseHpText.gameObject.transform.localPosition = tmpPosition;
    }
}