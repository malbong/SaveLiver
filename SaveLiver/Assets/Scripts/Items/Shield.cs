using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : Item, IItem
{
    public float itemDuration = 5f;
    private bool hasItem = false;
    //private GameObject shield;
    private Rigidbody2D parent;

    void Start()
    {
        parent = transform.GetComponentInParent<Rigidbody2D>();
        //shield = Player.instance.gameObject.transform.GetChild(2).gameObject;
        // GetChild(2) : Hare Shield
        StartCoroutine(TimeCheckAndDestroy());
    }

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
    * @설명: 쉴드의 지속시간이 지나면 쉴드 해제
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


    /**************************************
    * @함수명: TimeCheckAndDestroy
    * @작성자: zeli
    * @입력: void
    * @출력: IEnumerator
    * @설명: Item의 lifeTime이 끝나면, 해당 아이템의 실행여부 판단 후 파괴여부 결정
    */
    IEnumerator TimeCheckAndDestroy()
    {
        yield return new WaitForSeconds(ItemManager.instance.itemLifeTime);

        if (!hasItem)
        {
            SpriteRenderer spriteRenderer = transform.parent.GetComponent<SpriteRenderer>();
            Color color = spriteRenderer.color;

            while (true)
            {
                color.a -= 0.05f;
                spriteRenderer.color = color;
                yield return new WaitForSeconds(0.05f);
                if (spriteRenderer.color.a <= 0.1f) break;
            }
            parent.gameObject.SetActive(false);
        }
    }
}