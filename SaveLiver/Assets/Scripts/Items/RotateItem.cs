using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateItem : ItemManager, IItem
{
    public float itemDuration = 8f;
    private bool hasItem = false;
    private float amountRotateUp = 2f;

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
        if (Time.time - rotateUpItemTime >= itemDuration && hasItem)
        {
            hasItem = false;
            Player.instance.rotateSpeed -= amountRotateUp;
            Player.instance.HasRotateUp = false;
            Destroy(parent.gameObject);
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
        AudioSource audio = GetComponent<AudioSource>();
        audio.Play();

        GetComponentInParent<Collider2D>().enabled = false;
        GetComponentInParent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
        if (Player.instance.HasRotateUp)
        {
            Player.instance.rotateSpeed -= amountRotateUp;
            Destroy(parent.gameObject);
        }
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
        yield return new WaitForSeconds(itemLifeTime);
        if (!hasItem)
        {
            Destroy(parent.gameObject);
        }
    }
}
