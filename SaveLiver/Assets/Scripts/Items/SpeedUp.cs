using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUp : ItemManager, IItem
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
        if (Time.time - speedUpItemTime >= itemDuration && hasItem)
        {
            hasItem = false;
            Player.instance.speed = 3f;
            Destroy(gameObject);
        }
    }


    /**************************************
    * @함수명: Use
    * @작성자: zeli
    * @입력: void
    * @출력: void
    * @설명: 스피드업 아이템과 충돌 시 발동
    *        Coliider와 Sprite를 꺼줌
    *        플레이어 속도를 5로 만듦
    */
    public void Use()
    {
        GetComponent<Collider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
        Player.instance.speed = 5f;
        speedUpItemTime = Time.time;
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