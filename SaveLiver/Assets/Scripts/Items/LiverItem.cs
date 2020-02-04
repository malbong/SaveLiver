using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiverItem : Item, IItem
{
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
        GameManager.instance.AddScore(10);
        GameManager.instance.totalGetItemCount += 1;
        ItemManager.instance.AudioPlay();
        parent.gameObject.SetActive(false);
    }
}