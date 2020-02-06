using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : Item, IItem
{
    public float itemDuration = 5f;
    //private GameObject shield;


    void Update()
    {
        if (GameManager.instance.isPause) return;

        ItemDurationAndDestroy();
    }


    /**************************************
    * @함수명: ItemDurationAndDestroy
    * @작성자: zeli
    * @입력: void
    * @출력: void
    * @설명: 쉴드가 깨지면 실행
    *        Update에서 실행
    */
    private void ItemDurationAndDestroy()
    {
        if (!Player.instance.HasShield && hasItem)
        {
            hasItem = false;
            parent.gameObject.SetActive(false);
        }
    }


    /**************************************
    * @함수명: Use
    * @작성자: zeli
    * @입력: void
    * @출력: void
    * @설명: 쉴드 아이템과 충돌 시 발동
    *        Coliider와 Sprite를 꺼줌
    *        쉴드를 활성화
    */
    public void Use()
    {
        GameManager.instance.AddScore(20);
        GameManager.instance.totalGetItemCount += 1;
        ItemManager.instance.AudioPlay();

        GetComponentInParent<Collider2D>().enabled = false;
        GetComponentInParent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
        Player.instance.ShieldStart(); // 쉴드막 생성 (플레이어 자식 스프라이트 On)
        hasItem = true;
    }
}