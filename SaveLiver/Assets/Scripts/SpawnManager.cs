using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this; // instance 초기화
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }


    public float radius = 10.0f;
    public float itemRadius = 5.0f;

    public ObjectPooler objectPooler;
    

    private void Start()
    {
        StartCoroutine(CreateEnemy());
    }


    /********************************************
     * @함수명 : GetRandomPosition()
     * @작성자 : Malbong
     * @입력 : void
     * @출력 : Vector3
     * @설명 : Player을 중심으로 거리가 10인 enemy위치를 랜덤생성
     *         그중에 플레이화면 하단은 제외
     */
    public Vector3 GetRandomPosition(bool isItem = false)
    {
        float posX, posY;
        while (true)
        {
            Vector3 targetPosition = Player.instance.transform.position; //player위치 정보얻기
            float tarX = targetPosition.x; //player의 좌표 (원 중심)
            float tarY = targetPosition.y;
            if (!isItem)
            {
                posX = Random.Range(tarX - radius, tarX + radius); // x-10 ~  x+10 중 랜덤 x값 얻기
                posY = Mathf.Sqrt(Mathf.Pow(radius, 2) - Mathf.Pow(posX - tarX, 2)); // 얻은 x값을 이용해 y값도 얻기
            }
            else
            {
                posX = Random.Range(tarX - itemRadius, tarX + itemRadius); // x-10 ~  x+10 중 랜덤 x값 얻기
                posY = Mathf.Sqrt(Mathf.Pow(itemRadius, 2) - Mathf.Pow(posX - tarX, 2)); // 얻은 x값을 이용해 y값도 얻기
            }
            int randomSignPosY = 1;
            if (Random.Range(0f, 1f) < 0.5) randomSignPosY = -1; //posY 부호도 랜덤으로 정하기
            posY *= randomSignPosY;
            posY += tarY;
            if (isItem == true) break;
            if (posY > tarY - 5) break; //플레이화면 하단이 아닐 때 break (enemy라면 하단으로 주지않음)
        }
        Vector3 randomPosition = new Vector3(posX, posY, 0);
        return randomPosition;
    }


    public Quaternion GetAngleWithTargetFromY(Vector3 currentPosition, Vector3 targetPosition)
    {
        Vector3 dir = targetPosition - currentPosition;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Vector3 tmp = new Vector3(0, 0, 90 + angle);
        Quaternion rotation = Quaternion.Euler(tmp);
        return rotation;
    }


    /********************************************
     * @함수명 : EnemySpawn(int index)
     * @작성자 : Malbong, zeli
     * @입력 : int
     * @출력 : void
     * @설명 : 임의의 위치에서 Spawn하기
     *         CreateEnemy()라는 코루틴함수에서 계속 사용
     *         입력된 인덱스의 Enemy를 생성함
     *         Turtle: 캐릭터를 향해 회전한 상태로 생성
     */
    private void EnemySpawn(int index)
    {
        Vector3 randomPosition = GetRandomPosition(false);
        Vector3 targetPosition = Player.instance.transform.position;
        Quaternion rotation = GetAngleWithTargetFromY(randomPosition, targetPosition);
        GameObject obj = objectPooler.GetEnemyObject(index);

        obj.transform.GetChild(0).transform.position = randomPosition;
        obj.transform.GetChild(0).rotation = rotation;
        obj.SetActive(true);
    }


    /********************************************
     * @함수명 : SpawnRandomItem()
     * @작성자 : zeli
     * @입력 : void
     * @출력 : void
     * @설명 : item을 랜덤으로 생성
     *         위치는 GetRandomPosition 함수를 이용해 랜덤위치를 받아옴
     *         
     *         index에 따른 아이템 =>
     *         0:Bomb  1:Fever  2:Liver  3:Rotate  4:Shield  5:SpeedUp
     */
    private void ItemRandomSpawn()
    {
        int percentage = Random.Range(0, 15);
        int index;
        if (percentage < 1)
            index = 0; // Bomb
        else if (percentage >= 1 && percentage < 3)
            index = 1; // Fever
        else if (percentage >= 3 && percentage < 6)
            index = 2; // Liver
        else if (percentage >= 6 && percentage < 9)
            index = 3; // Rotate
        else if (percentage >= 9 && percentage < 12)
            index = 4;
        else
            index = 5; // SpeedUp
        Vector3 randomPosition = GetRandomPosition(true);
        GameObject obj = objectPooler.GetItemObject(index);
        obj.transform.position = randomPosition;
        obj.SetActive(true);
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
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            EnemySpawn(0);
            ItemRandomSpawn();
            yield return new WaitForSeconds(3.0f);
            ItemRandomSpawn();
            EnemySpawn(1);
            yield return new WaitForSeconds(3.0f);
            ItemRandomSpawn();
            EnemySpawn(2);
            yield return new WaitForSeconds(3.0f);
            ItemRandomSpawn();
            EnemySpawn(3);
            yield return new WaitForSeconds(3.0f);
            ItemRandomSpawn();
            EnemySpawn(4);
            yield return new WaitForSeconds(3.0f);
            ItemRandomSpawn();
            EnemySpawn(5);
            yield return new WaitForSeconds(3.0f);
        }
    }
}