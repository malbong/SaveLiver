using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fever : ItemManager, IItem
{
    public float itemDuration = 8f;
    private bool hasItem = false;
    //private float amountSpeedUp = 2f;

    private Rigidbody2D parent;

    void Start()
    {
        parent = GetComponentInParent<Rigidbody2D>();
        StartCoroutine("TimeCheckAndDestroy");
    }

    void Update()
    {
        ItemDurationAndDestroy();
    }


    /**************************************
    * @ Shield와 동일
    */
    private void ItemDurationAndDestroy()
    {
        if (Time.time - feverItemTime >= itemDuration && hasItem)
        {
            hasItem = false;
            Player.instance.EndFeverTime(); // 무적종료 알림
            //Player.instance.speed -= amountSpeedUp;
            Destroy(parent.gameObject);
        }
    }


    /**************************************
    * @함수명: Use
    * @작성자: zeli
    * @입력: void
    * @출력: void
    * @설명: 무적 아이템과 충돌 시 발동
    *        Coliider와 Sprite를 꺼줌
    *        플레이어에게 무적상태를 알림
    */
    public void Use()
    {
        GetComponentInParent<Collider2D>().enabled = false;
        GetComponentInParent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
        Player.instance.FeverTime(); // 무적시작을 알림
        feverItemTime = Time.time;
        /*if (!Player.instance.isFevered)
        {
            if (!Player.instance.HasSpeedUp) // 스피드업이 되어 있지 않을 때
            {
                Player.instance.speed += amountSpeedUp; // 스피드업
            }
        }
        else if(Player.instance.isFevered) // 무적상태일 경우
        {
            Destroy(parent.gameObject); // 이 아이템 삭제 (새로 먹은 아이템만 살아있음)
        }*/
        hasItem = true;
    }


    /**************************************
    * @ Shield와 동일
    */
    IEnumerator TimeCheckAndDestroy()
    {
        yield return new WaitForSeconds(itemLifeTime);
        if (!hasItem)
        {
            Destroy(parent.gameObject);
        }
    }
}
