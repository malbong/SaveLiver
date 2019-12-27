using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Joystick : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    private Image innerPad; // 조이스틱 안쪽 패드
    private Image backgroundPad; // 조이스틱 바깥쪽 패드 (터치영역)
    private RectTransform touchArea;
    public Vector3 JoystickVector { get; set; } // 조이스틱 벡터 방향


    void Start()
    {
        touchArea = GetComponent<RectTransform>();
        backgroundPad = transform.GetChild(0).GetComponent<Image>();
        innerPad = transform.GetChild(0).GetChild(0).GetComponent<Image>();

        //backgroundPad = transform.GetComponent<Image>();
        //innerPad = transform.GetChild(0).GetComponent<Image>();
    }




    /**************************************
    * @함수명: OnDrag(PointerEventData data)
    * @작성자: zeli
    * @입력: data (터치에 대한 정보)
    * @출력: void
    * @설명: 조이스틱 드래그 모션 구현, 드래그 방향 구하기
    *        드래그할 때마다 발생
    *        조이스틱 방향(JoystickVector)을 Player로 넘김
    */
    public void OnDrag(PointerEventData data)
    {
        if (Player.instance.isAlive == false) return;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(touchArea,
            data.position, data.pressEventCamera, out Vector2 pos))
        {
            // 배경패드의 x, y 사이즈 구하기
            pos.x = (pos.x / touchArea.sizeDelta.x);
            pos.y = (pos.y / touchArea.sizeDelta.y);
            
            // 조이스틱방향 설정 (2.6과 2를 곱해준 것은 TouchArea의 비율 때문임)
            JoystickVector = new Vector3(pos.x * 2.6f, pos.y * 2, 0);
            // 조이스틱 범위 벗어나지 않게.
            JoystickVector = (JoystickVector.magnitude > 1.0f) ? JoystickVector.normalized : JoystickVector;

            // Player에게 조이스틱 방향 넘기기
            Player.instance.TurnAngle(JoystickVector);

            // innerPad 이미지가 배경조이스틱의 원을 넘기지 않도록 길이 컷
            JoystickVector = (JoystickVector.magnitude > 0.35f) ? JoystickVector.normalized * 0.35f : JoystickVector;

            // innerPad 이미지 터치한 곳으로 옮기기
            innerPad.rectTransform.anchoredPosition = new Vector3(JoystickVector.x * (backgroundPad.rectTransform.sizeDelta.x),
                JoystickVector.y * (backgroundPad.rectTransform.sizeDelta.y));
        }
    }




    /**************************************
    * @함수명: OnPointerDown(PointerEventData data)
    * @작성자: zeli
    * @입력: data
    * @출력: void
    * @설명: 조이스틱 패드 "클릭"할 시 드래그 모션 구현
    */
    public void OnPointerDown(PointerEventData data)
    {
        OnDrag(data);
    }




    /**************************************
    * @함수명: OnPointerUp(PointerEventData data)
    * @작성자: zeli
    * @입력: data
    * @출력: void
    * @설명: 조이스틱 패드 터치가 끝나면 innerPad 원위치로 옮기기
    *        터치를 떼면 발동
    */
    public void OnPointerUp(PointerEventData data)
    {
        innerPad.rectTransform.anchoredPosition = Vector3.zero;
    }
}
