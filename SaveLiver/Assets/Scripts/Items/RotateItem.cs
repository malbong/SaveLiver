using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateItem : Item, IItem
{
    public float itemDuration = 8f;
    private bool hasItem = false;
    public float amountRotateUp = 2f;
    private float rotateUpItemTime = 0f;

    private Rigidbody2D parent;

    void Start()
    {
        parent = GetComponentInParent<Rigidbody2D>();
        StartCoroutine(TimeCheckAndDestroy());
    }

    void Update()
    {
        if (GameManager.instance.isPause) return;
        ItemDurationAndDestroy();
    }


    /**************************************
    * @ Shield와 동일
    */
    private void ItemDurationAndDestroy()
    {
        if (Time.time - rotateUpItemTime >= itemDuration && hasItem)
        {
            hasItem = false;
            Player.instance.rotateSpeed -= amountRotateUp;
            Player.instance.HasRotateUp = false;
            parent.gameObject.SetActive(false);
        }
    }


    /**************************************
    * @함수명: Use
    * @작성자: zeli
    * @입력: void
    * @출력: void
    * @설명: 로테이트업 아이템과 충돌 시 발동
    *        Coliider와 Sprite를 꺼줌
    *        플레이어 로테이트를 더해줌
    */
    public void Use()
    {
        ItemManager.instance.AudioPlay();

        GetComponentInParent<Collider2D>().enabled = false;
        GetComponentInParent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
        Player.instance.rotateSpeed += amountRotateUp;
        Player.instance.HasRotateUp = true;
        rotateUpItemTime = Time.time;
        hasItem = true;
    }


    /**************************************
    * @ Shield와 동일
    */
    IEnumerator TimeCheckAndDestroy()
    {
        yield return new WaitForSeconds(ItemManager.instance.itemLifeTime);
        if (!hasItem)
        {
            SpriteRenderer spriteRenderer = transform.parent.GetComponent<SpriteRenderer>();
            while (true)
            {
                Color color = spriteRenderer.color;
                color.a -= 0.05f;
                spriteRenderer.color = color;
                yield return new WaitForSeconds(0.05f);
                if (spriteRenderer.color.a <= 0.1f) break;
            }
            parent.gameObject.SetActive(false);
        }
    }
}
