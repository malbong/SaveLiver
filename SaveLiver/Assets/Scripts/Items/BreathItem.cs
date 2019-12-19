using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreathItem : ItemManager, IItem
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
    * @설명: Breath 아이템을 먹을 시 실행
    *        Breath를 더해줌
    *        아이템 오브젝트 파괴
    */
    public void Use()
    {
        BreathCheckAndPlus(3);
        Destroy(gameObject);
    }



    /**************************************
    * @함수명: BreathCheckAndPlus
    * @작성자: zeli
    * @입력: breath
    * @출력: void
    * @설명: LiverItem을 먹을 시 실행
    *        breath가 10이 넘어가지 않도록 더해줌
    */
    public void BreathCheckAndPlus(int breath)
    {
        Player.instance.breath += breath;
        if (Player.instance.breath > 10) Player.instance.breath = 10;
    }
}
