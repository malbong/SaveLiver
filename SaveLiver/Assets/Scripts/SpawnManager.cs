using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public float radius = 10.0f;
    public Enemy enemy;
    

    /********************************************
     * @함수명 : GetRandomPosition()
     * @작성자 : Malbong
     * @입력 : void
     * @출력 : Vector3
     * @설명 : Player을 중심으로 거리가 10인 enemy위치를 랜덤생성
     */
    public Vector3 GetRandomPosition()
    {
        Vector3 targetPosition = Player.instance.transform.position; //player위치 정보얻기
        float tarX = targetPosition.x; //player의 좌표중 x좌표얻기
        float posX = Random.Range(tarX - radius, tarX + radius); // x-10 ~  x+10 중 랜덤 x값 얻기
        float posY = Mathf.Sqrt(Mathf.Pow(radius, 2) - Mathf.Pow(posX, 2)); // 얻은 x값을 이용해 y값도 얻기

        int randomSignPosY = 1;
        if (Random.Range(0f, 1f) < 0.5) randomSignPosY = -1; //posY 부호도 랜덤으로 정하기
        posY *= randomSignPosY;

        Vector3 randomPosition = new Vector3(posX, posY, 0);
        return randomPosition;
    }


    /********************************************
     * @함수명 : Spawn()
     * @작성자 : Malbong
     * @입력 : Enemy
     * @출력 : void
     * @설명 : 임의의 위치에서 Spawn하기
     */
    public void Spawn()
    {
        //yield return new WaitForSeconds(time);
        
        Vector3 randomPosition = GetRandomPosition();
        
        Vector3 targetPosition = Player.instance.transform.position;
        Vector3 dir = targetPosition - randomPosition;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Vector3 tmp = new Vector3(0, 0, 90 + angle);
        Quaternion rotation = Quaternion.Euler(tmp);

        //parameter enemy 
        Instantiate(enemy, randomPosition, rotation);
    }

    
    private void Start()
    {
        //time spawn 
        Spawn();
    }
}
