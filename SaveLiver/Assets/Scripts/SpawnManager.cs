﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public float radius = 10.0f;
    public Enemy[] enemies;
    public Item[] items;

    private void Start()
    {
        StartCoroutine("CreateEnemy");
    }


    /********************************************
     * @함수명 : GetRandomPosition()
     * @작성자 : Malbong
     * @입력 : void
     * @출력 : Vector3
     * @설명 : Player을 중심으로 거리가 10인 enemy위치를 랜덤생성
     *         그중에 플레이화면 하단은 제외
     */
    public Vector3 GetRandomPosition()
    {
        float posX, posY;
        while (true)
        {
            Vector3 targetPosition = Player.instance.transform.position; //player위치 정보얻기
            float tarX = targetPosition.x; //player의 좌표중 x좌표얻기
            posX = Random.Range(tarX - radius, tarX + radius); // x-10 ~  x+10 중 랜덤 x값 얻기
            posY = Mathf.Sqrt(Mathf.Pow(radius, 2) - Mathf.Pow(posX, 2)); // 얻은 x값을 이용해 y값도 얻기
            int randomSignPosY = 1;
            if (Random.Range(0f, 1f) < 0.5) randomSignPosY = -1; //posY 부호도 랜덤으로 정하기
            posY *= randomSignPosY;
            if (posY > -5) break; //플레이화면 하단이 아닐 때 break
        }

        Vector3 randomPosition = new Vector3(posX, posY, 0);
        return randomPosition;
    }



    /********************************************
     * @함수명 : Spawn()
     * @작성자 : Malbong, zeli
     * @입력 : Object
     * @출력 : void
     * @설명 : 임의의 위치에서 Spawn하기
     *         CreateEnemy()라는 코루틴함수에서 계속 사용
     *         입력된 오브젝트를 생성함
     *         Turtle: 캐릭터를 향해 회전한 상태로 생성
     */
    public void Spawn(Object objects)
    {   
        Vector3 randomPosition = GetRandomPosition();

        if(objects.GetType() == typeof(Turtle))
        {
            Vector3 targetPosition = Player.instance.transform.position;
            Vector3 dir = targetPosition - randomPosition;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            Vector3 tmp = new Vector3(0, 0, 90 + angle);
            Quaternion rotation = Quaternion.Euler(tmp);

            Instantiate(objects, randomPosition, rotation);
        }
        else if(objects.GetType().BaseType == typeof(Item))
        {
            Instantiate(objects, randomPosition, Quaternion.identity);
        }
        
    }


    /********************************************
     * @함수명 : GetRandomItemIndex()
     * @작성자 : zeli
     * @입력 : void
     * @출력 : int
     * @설명 : item을 랜덤으로 생성하기 위한 Index값 랜덤 생성
     */
    public int GetRandomItemIndex()
    {
        return Random.Range(0, items.Length);
    }


    /********************************************
     * @함수명 : CreateEnemy()
     * @작성자 : Malbong
     * @입력 : void
     * @출력 : IEnumerator time
     * @설명 : Coroutine을 사용하여 원하는 시간에 Spawn 호출
     *         레벨 디자인을 하여 난이도 조절
     */
    IEnumerator CreateEnemy()
    {
        Spawn(enemies[5]);
        Spawn(enemies[4]);
        Spawn(enemies[0]);
        Spawn(items[GetRandomItemIndex()]);
        Spawn(items[GetRandomItemIndex()]);
        yield return new WaitForSeconds(3.0f);
        Spawn(enemies[1]);
        Spawn(items[GetRandomItemIndex()]);
        yield return new WaitForSeconds(3.0f);
        Spawn(enemies[2]);
        Spawn(items[GetRandomItemIndex()]);
        yield return new WaitForSeconds(3.0f);
        Spawn(enemies[3]);
        Spawn(items[GetRandomItemIndex()]);
    }
}
