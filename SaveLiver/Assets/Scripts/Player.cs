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

    public Transform arrowRotate; // 화살표 회전 컨트롤

    public Vector3 JoystickVec { get; set; } // 조이스틱 방향

    private Rigidbody2D playerRigid;
    private Vector3 velocity;

    void Start()
    {
        playerRigid = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        TurnAngle(JoystickVec);

        velocity = -arrowRotate.up;
        playerRigid.velocity = velocity * speed;
    }


    /**************************************
    * @함수명: TurnAngle(Vector3 currentJoystickVec)
    * @작성자: zeli
    * @입력: currentJoystickVec
    * @출력: void
    * @설명: 조이스틱의 방향을 읽고, 그 방향으로 화살표를 회전시키는 메소드
    */
    public void TurnAngle(Vector3 currentJoystickVec)
    {
        Vector3 originJoystickVec = -arrowRotate.up;

        float angle = Vector3.Angle(currentJoystickVec, originJoystickVec);
        int sign = (Vector3.Cross(currentJoystickVec, originJoystickVec).z > 0) ? -1 : 1;
        float finalAngle = sign * angle;

        Vector3 currentRotation = arrowRotate.rotation.eulerAngles;
        Vector3 targetRotation = currentRotation + new Vector3(0, 0, finalAngle);

        Quaternion finalTargetRotation = Quaternion.Euler(targetRotation);

        arrowRotate.rotation = Quaternion.Lerp(arrowRotate.rotation, finalTargetRotation, 0.04f);
    }




    /**************************************
    * @함수명: OnTriggerEnter2D(Collider2D other)
    * @작성자: zeli
    * @입력: other
    * @출력: void
    * @설명: collider가 충돌하면 어떤 종류인지 검사하고 사용
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