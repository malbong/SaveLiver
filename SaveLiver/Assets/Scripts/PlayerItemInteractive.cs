using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemInteractive : MonoBehaviour
{
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
