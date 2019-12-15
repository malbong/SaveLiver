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

    private Coroutine runningCoroutine = null;

    public Transform arrowRotate; // 화살표 회전 컨트롤

    private Rigidbody2D playerRigid;

    void Start()
    {
        playerRigid = GetComponent<Rigidbody2D>();
        StartCoroutine(RotateAngle(180, 1)); // 시작하면 Player를 180도 왼쪽으로 돌리기.
    }

    void Update()
    {
        playerRigid.velocity = -arrowRotate.up * speed; // 화살표 방향으로 speed만큼 직진
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
        Vector3 originJoystickVec = -arrowRotate.up; // 화살표 방향 벡터

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



        //arrowRotate.Rotate(0, 0, finalAngle * rotateSpeed * deltaTime);
        // Rotate을 이용한 회전
        // 문제점
        // 각도가 넓으면 빨리 회전하고, 각도가 좁으면 느리게 회전함
        // angle값이 계속 갱신돼서 회전값이 일정하지 않음 (각도가 좁아질수록 느리게 회전함)



        //Vector3 currentRotation = arrowRotate.rotation.eulerAngles;
        //Vector3 targetRotation = currentRotation + new Vector3(0, 0, finalAngle);
        //Quaternion finalTargetRotation = Quaternion.Euler(targetRotation);
        //arrowRotate.rotation = Quaternion.Lerp(arrowRotate.rotation, finalTargetRotation, 0.04f);
        // Quaternion 회전. angle값이 계속 갱신돼서 회전값이 일정하지 않음 (각도가 좁아질수록 느리게 회전함)
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
        for (float i=0; i<angle; i+=rotateSpeed)
        {
            arrowRotate.Rotate(0, 0, sign * rotateSpeed);
            yield return null; // 1프레임 대기
        }
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

}