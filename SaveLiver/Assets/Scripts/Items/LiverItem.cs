using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiverItem : ItemManager, IItem
{
    private Rigidbody2D parent;

    void Start()
    {
        parent = GetComponentInParent<Rigidbody2D>();
        Destroy(parent.gameObject, itemLifeTime);
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
        AudioSource audio = GetComponent<AudioSource>();
        audio.Play();

        if (Player.instance.hp < 3)
        {
            Player.instance.hp += 1;
            GameManager.instance.UpdateLiverIcon(Player.instance.hp);
        }

        Destroy(parent.gameObject);
    }
}