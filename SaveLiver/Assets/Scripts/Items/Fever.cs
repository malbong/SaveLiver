using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fever : Item, IItem
{
    public float itemDuration = 8f;
    public float amountSpeedUp = 2f;
    private float feverItemTime = 0f;

    public Sprite feverSprite;

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
        if (Time.time - feverItemTime >= itemDuration && hasItem)
        {
            hasItem = false;
            Player.instance.feverNum -= 1;
            if(Player.instance.feverNum == 0) Player.instance.EndFeverTime(); // Item 소진 시 무적종료 알림
            Player.instance.speed -= amountSpeedUp;
            GetComponentInParent<SpriteRenderer>().sprite = feverSprite;
            parent.gameObject.SetActive(false);
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
        GameManager.instance.AddScore(10);
        GameManager.instance.totalGetItemCount += 1;
        ItemManager.instance.AudioPlay();


        GetComponentInParent<Collider2D>().enabled = false;
        GetComponentInParent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
        feverItemTime = Time.time;
        Player.instance.speed += amountSpeedUp; // 스피드업
        Player.instance.feverNum += 1; // 먹은 fever Item 갯수 +1
        Player.instance.FeverTime(); // 무적시작을 알림
        hasItem = true;
    }
}
