using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public int breath;
    public float speed;
    public float rotateSpeed;

    public bool isFevered = false;

    public ParticleSystem onDeadParticle;

    private Camera cam;

    private Coroutine runningCoroutine = null;

    public Transform arrowRotate; // 화살표 회전 컨트롤

    private Rigidbody2D playerRigid;
    public Animator anim;
    
    public bool isAlive = true;

    public bool HasShield { get; set; } = false;
    public bool HasSpeedUp { get; set; } = false;
    public bool HasRotateUp { get; set; } = false;

    public int feverNum = 0;
    public int speedUpNum = 0;


    void Start()
    {
        playerRigid = GetComponent<Rigidbody2D>();
        runningCoroutine = StartCoroutine(RotateAngle(180, -1)); // 시작하면 Player를 180도 오른쪽으로 돌리기.
    }

    void Update()
    {
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
        for (float i=mod; i<angle; i+=rotateSpeed)
        {
            arrowRotate.Rotate(0, 0, sign * rotateSpeed);
            yield return null; // 1프레임 대기
        }
        arrowRotate.Rotate(0, 0, sign * mod); // 남은 각도 회전
    }




    /**************************************
    * @함수명: OnTriggerEnter2D(Collider2D other)
    * @작성자: zeli
    * @입력: other
    * @출력: void
    * @설명: collider와 충돌하면 어떤 종류인지 검사하고 사용
    */
    private void OnTriggerEnter2D(Collider2D other)
    {
        IItem item = other.GetComponent<IItem>();
        if (item != null)
        {
            item.Use();
        }
    }



    /**************************************
    * @함수명: TakeDamage
    * @작성자: zeli
    * @입력: damage
    * @출력: void
    * @설명: Turtle로부터 받은 데미지를 받은 후 계산
    *        hp(Liver)가 없으면 OnDead 처리
    */
    public void TakeDamage(int damage)
    {
        if (HasShield) 
        {
            HasShield = false;
            return;
        }

        if (isFevered)
        {
            return;
        }

        hp -= damage;
        if(hp <= 0)
        {
            OnDead();
        }

    }

    public void TakeDamage(bool isDragon)
    {
        if (HasShield)
        {
            HasShield = false;
            return;
        }
        if (isFevered)
        {
            return;
        }

        if(isDragon)
        {
            OnDead();
        }
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

        transform.gameObject.SetActive(false);
        

        //once used destroy instead SetActive(false);
        //transform.parent.gameObject.SetActive(false);


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
        anim.SetBool("feverAnimation", true);
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
        anim.SetBool("feverAnimation", false);
    }
}