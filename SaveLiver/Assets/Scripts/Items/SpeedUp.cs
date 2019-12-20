using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUp : ItemManager, IItem
{
    public float itemDuration = 8f;
    private bool hasItem = false;
    public float amountSpeedUp = 2f;
    private Rigidbody2D parent;

    void Start()
    {
        parent = gameObject.GetComponentInParent<Rigidbody2D>();
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
            Player.instance.speed -= amountSpeedUp;
            Player.instance.HasSpeedUp = false;
            Destroy(parent.gameObject);
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
        transform.GetComponentInParent<Collider2D>().enabled = false;
        transform.GetComponentInParent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
        if (Player.instance.HasSpeedUp)
        {
            Player.instance.speed -= amountSpeedUp;
            Destroy(parent.gameObject);
        }
        
        Player.instance.speed += amountSpeedUp;
        Player.instance.HasSpeedUp = true;
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
            Destroy(parent.gameObject);
        }
    }
}