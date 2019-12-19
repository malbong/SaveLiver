using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiverItem : ItemManager, IItem
{
    void Start()
    {
        Destroy(gameObject, itemLifeTime);
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
        LiverCheckAndPlus(1);
        Destroy(gameObject);
    }



    /**************************************
    * @함수명: LiverCheckAndPlus
    * @작성자: zeli
    * @입력: liver
    * @출력: void
    * @설명: LiverItem을 먹을 시 실행
    *        hp(liver)가 3이 넘어가지 않도록 더해줌
    */
    public void LiverCheckAndPlus(int liver)
    {
        Player.instance.hp += liver;
        if (Player.instance.hp > 3) Player.instance.hp = 3;
    }
}