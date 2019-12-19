using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : ItemManager, IItem
{
    public float itemDuration = 5f;
    private bool hasItem = false;
    private GameObject shield;

    void Start()
    {
        shield = Player.instance.gameObject.transform.GetChild(3).gameObject;
        // GetChild(3) : Hare Shield
        StartCoroutine("TimeCheckAndDestroy");
    }

    void Update()
    {
        ItemDurationAndDestroy();
    }


    /**************************************
    * @함수명: ItemDurationAndDestroy
    * @작성자: zeli
    * @입력: void
    * @출력: void
    * @설명: 쉴드의 지속시간이 지나면 쉴드 해제
    */
    private void ItemDurationAndDestroy()
    {
        if (!Player.instance.HasShield && hasItem)
        {
            hasItem = false;
            shield.SetActive(false);
            Destroy(gameObject);
        }
        if (Time.time - shieldItemTime >= itemDuration && hasItem)
        {
            Player.instance.HasShield = false;
            hasItem = false;
            shield.SetActive(false);
            Destroy(gameObject);
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
        GetComponent<Collider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
        Player.instance.HasShield = true;
        shield.SetActive(true); // 쉴드막 생성 (플레이어 자식 스프라이트 On)
        shieldItemTime = Time.time;
        hasItem = true;
    }


    /**************************************
    * @함수명: TimeCheckAndDestroy
    * @작성자: zeli
    * @입력: void
    * @출력: IEnumerator
    * @설명: Item의 생성시간이 끝나면, 해당 아이템의 실행여부 판단 후 파괴여부 결정
    */
    IEnumerator TimeCheckAndDestroy()
    {
        yield return new WaitForSeconds(itemLifeTime);
        if (!hasItem)
        {
            Destroy(gameObject);
        }
    }
}