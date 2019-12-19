using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fever : ItemManager, IItem
{
    public float itemDuration = 8f;
    private bool hasItem = false;

    void Start()
    {
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
            Destroy(gameObject);
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
        GetComponent<Collider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
        Player.instance.FeverTime(); // 무적시작을 알림
        feverItemTime = Time.time;
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
            Destroy(gameObject);
        }
    }
}
