using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiverItem : Item, IItem
{
    private Rigidbody2D parent;


    void Start()
    {
        parent = GetComponentInParent<Rigidbody2D>();
        StartCoroutine(TimeCheckAndDestroy());
    }


    /**************************************
    * @함수명: Use
    * @작성자: zeli
    * @입력: void
    * @출력: void
    * @설명: LiverItem을 먹을 시 실행
    *        Hp(Liver)를 더해줌
    *        아이템 오브젝트 파괴
    */
    public void Use()
    {
        if (Player.instance.hp < 3)
        {
            Player.instance.hp += 1;
            GameManager.instance.UpdateLiverCountText(Player.instance.hp);
        }
        GameManager.instance.AddScore(20);
        GameManager.instance.totalGetItemCount += 1;
        ItemManager.instance.AudioPlay();
        parent.gameObject.SetActive(false);
    }


    IEnumerator TimeCheckAndDestroy()
    {
        yield return new WaitForSeconds(ItemManager.instance.itemLifeTime);
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