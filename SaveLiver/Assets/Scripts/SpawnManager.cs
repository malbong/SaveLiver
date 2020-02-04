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
    public float itemRadius = 8.0f;

    public ObjectPooler objectPooler;
    
    private Vector3 playerPosition;
    private float spawnRadius;
    private float angle45Length;
    private float itemSpawnTimePerLevel;


    private void Start()
    {
        spawnRadius = SpawnManager.instance.radius;
        angle45Length = Mathf.Sqrt(Mathf.Pow(spawnRadius, 2) / 2.0f);

        StartCoroutine(EnemyCreate());
        StartCoroutine(ItemCreate());
    }


    /********************************************
     * @함수명 : CreateEnemy()
     * @작성자 : Malbong
     * @입력 : void
     * @출력 : IEnumerator time
     * @설명 : Coroutine을 사용하여 원하는 시간에 Spawn 호출
     *         레벨 디자인을 하여 난이도 조절
     */
    IEnumerator EnemyCreate()
    {
        itemSpawnTimePerLevel = 5.0f;

        while (true)
        {
            yield return new WaitForSeconds(3f);
            EnemySpawn(13);
            //Swirl(-250, 2, true);
            yield return new WaitForSeconds(3f);
            EnemySpawn(12);
            
            yield return new WaitForSeconds(3f);
            EnemySpawn(10);
            //EnemySpawn(2);
            yield return new WaitForSeconds(3f);
            EnemySpawn(8);
            //DiagonalBothSide(5);
            //Dragon(1, 1, 2);
            yield return new WaitForSeconds(3f);
            EnemySpawn(6);
            yield return new WaitForSeconds(3f);
            EnemySpawn(7);
            //Swirl(-250, 2, true);
            //EnemySpawn(1);
            yield return new WaitForSeconds(3f);
            EnemySpawn(9);
            yield return new WaitForSeconds(3f);
            EnemySpawn(6);
            //AllDirection4();
            
            
            /*
            yield return new WaitForSeconds(3f);
            EnemySpawn(8);
            //AllDirection8();
            //EnemySpawn(0);
            yield return new WaitForSeconds(3f);
            EnemySpawn(7);
            //DiagonalLeft(5);
            //EnemySpawn(1);
            yield return new WaitForSeconds(3f);
            //EnemySpawn(5);
            EnemySpawn(6);
            //DiagonalRight(5);
            //Swirl(-250);
            yield return new WaitForSeconds(3f);
            //DiagonalLeft(5);
            //EnemySpawn(4);
            EnemySpawn(6);
            yield return new WaitForSeconds(3f);
            //Dragon(-1, -1, 2);
            EnemySpawn(6);*/
        }
        
    }


    IEnumerator ItemCreate()
    {
        yield return new WaitForSeconds(0.1f);

        while (true)
        {
            ItemRandomSpawn();
            yield return new WaitForSeconds(itemSpawnTimePerLevel);
            //yield return new WaitForSeconds(3.0f);
        }
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
     *         Create()라는 코루틴함수에서 계속 사용
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
        int percentage = Random.Range(0, 9);
        int index;
        if (percentage < 2)
            index = 0; // SpeedUp 0 1
        else if (percentage >= 2 && percentage < 3)
            index = 1; // Fever 2
        else if (percentage >= 3 && percentage < 5)
            index = 2; // Liver 3 4
        else if (percentage >= 5 && percentage < 7)
            index = 3; // Rotate 5 6
        else
            index = 4; // Shield 7 8
        Vector3 randomPosition = GetRandomPosition(true);
        GameObject obj = objectPooler.GetItemObject(index);
        obj.transform.position = randomPosition;
        obj.SetActive(true);
    }


    public GameObject CreateLinearTurtle(Vector3 diffPosition, Vector3 targetPosition)
    {
        Vector3 createPosition = targetPosition + diffPosition;
        Quaternion rotation = SpawnManager.instance.GetAngleWithTargetFromY(createPosition, targetPosition);

        GameObject obj = objectPooler.GetEnemyObject(6); // 6:Linear Turtle
        obj.transform.position = createPosition;
        obj.transform.GetChild(0).localPosition = Vector3.zero;
        obj.transform.GetChild(0).rotation = rotation;
        
        obj.SetActive(true);

        return obj;
    }


    public void AllDirection4()
    {
        playerPosition = Player.instance.transform.position;

        Vector3 diffPosition = new Vector3(0, spawnRadius, 0);
        CreateLinearTurtle(diffPosition, playerPosition);

        diffPosition = new Vector3(0, -spawnRadius, 0);
        CreateLinearTurtle(diffPosition, playerPosition);

        diffPosition = new Vector3(spawnRadius, 0, 0);
        CreateLinearTurtle(diffPosition, playerPosition);

        diffPosition = new Vector3(-spawnRadius, 0, 0);
        CreateLinearTurtle(diffPosition, playerPosition);
    }


    public void AllDirection8()
    {
        AllDirection4();

        playerPosition = Player.instance.transform.position;

        Vector3 diffPosition = new Vector3(angle45Length, angle45Length, 0);
        CreateLinearTurtle(diffPosition, playerPosition);

        diffPosition = new Vector3(-angle45Length, angle45Length, 0);
        CreateLinearTurtle(diffPosition, playerPosition);

        diffPosition = new Vector3(-angle45Length, -angle45Length, 0);
        CreateLinearTurtle(diffPosition, playerPosition);

        diffPosition = new Vector3(angle45Length, -angle45Length, 0);
        CreateLinearTurtle(diffPosition, playerPosition);
    }


    public void DiagonalLeft(float interval)
    {
        playerPosition = Player.instance.transform.position;

        Vector3 targetPosition = playerPosition;
        Vector3 diffPosition = new Vector3(-angle45Length, angle45Length, 0);

        CreateLinearTurtle(diffPosition, targetPosition);

        targetPosition = playerPosition + new Vector3(0, interval, 0);
        CreateLinearTurtle(diffPosition, targetPosition);

        targetPosition = playerPosition + new Vector3(0, -interval, 0);
        CreateLinearTurtle(diffPosition, targetPosition);
    }


    public void DiagonalRight(float interval)
    {
        /*
        playerPosition = Player.instance.transform.position;

        Vector3 targetPosition = playerPosition;
        Vector3 diffPosition = new Vector3(-angle45Length, angle45Length, 0);
        GameObject obj = CreateLinearTurtle(diffPosition, targetPosition);
        obj.transform.Translate(obj.transform.position.x * (-2), 0, 0);
        //obj.transform.GetChild(0).Rotate(0, 180, 0);
        obj.transform.Rotate(0, 180, 0);

        targetPosition = playerPosition + new Vector3(0, interval, 0);
        obj = CreateLinearTurtle(diffPosition, targetPosition);
        obj.transform.Translate(obj.transform.position.x * (-2), 0, 0);
        //obj.transform.GetChild(0).Rotate(0, 180, 0);
        obj.transform.Rotate(0, 180, 0);

        targetPosition = playerPosition + new Vector3(0, -interval, 0);
        obj = CreateLinearTurtle(diffPosition, targetPosition);
        obj.transform.Translate(obj.transform.position.x * (-2), 0, 0);
        //obj.transform.GetChild(0).Rotate(0, 180, 0);
        obj.transform.Rotate(0, 180, 0);
        */

        playerPosition = Player.instance.transform.position;

        Vector3 targetPosition = playerPosition;
        Vector3 diffPosition = new Vector3(angle45Length, angle45Length, 0);
        GameObject obj = CreateLinearTurtle(diffPosition, targetPosition);
        obj.transform.GetChild(0).Rotate(0, 180, 0);

        targetPosition = playerPosition + new Vector3(0, interval, 0);
        diffPosition = new Vector3(angle45Length, angle45Length, 0);
        obj = CreateLinearTurtle(diffPosition, targetPosition);
        obj.transform.GetChild(0).Rotate(0, 180, 0);

        targetPosition = playerPosition + new Vector3(0, -interval, 0);
        diffPosition = new Vector3(angle45Length, angle45Length, 0);
        obj = CreateLinearTurtle(diffPosition, targetPosition);
        obj.transform.GetChild(0).Rotate(0, 180, 0);
    }


    public void DiagonalBothSide(float interval)
    {
        DiagonalLeft(interval);
        DiagonalRight(interval);
    }


    public void Swirl(float maxForce, float interval = 0, bool upDownPosition = false)
    {
        playerPosition = Player.instance.transform.position;
        if (upDownPosition == true) // generate swirl over and under from player
        {
            GameObject obj = ObjectPooler.instance.GetSwirl();
            obj.SetActive(true);
            obj.transform.position = playerPosition + new Vector3(0, interval, 0);
            obj.GetComponent<Swirl>().maxForce = maxForce;
            
            obj = ObjectPooler.instance.GetSwirl();
            obj.SetActive(true);
            obj.transform.position = playerPosition + new Vector3(0, -interval, 0);
            obj.GetComponent<Swirl>().maxForce = maxForce;
        }
        else
        {
            GameObject obj = ObjectPooler.instance.GetSwirl();
            obj.SetActive(true);
            obj.transform.position = playerPosition;
            obj.GetComponent<Swirl>().maxForce = maxForce;
        }

    }


    public void Dragon(int dir, int isOver, float interval)
    {
        playerPosition = Player.instance.transform.position;
        if (dir == 1) // create right 
        {
            if (isOver == 1) // create over player
            {
                Vector3 createPosition = playerPosition + new Vector3(20, interval, 0);
                GameObject obj = ObjectPooler.instance.GetDragonObject(1);
                obj.transform.position = createPosition;
                obj.SetActive(true);
            }
            else // create under player
            {
                Vector3 createPosition = playerPosition + new Vector3(20, -interval, 0);
                GameObject obj = ObjectPooler.instance.GetDragonObject(1);
                obj.transform.position = createPosition;
                obj.SetActive(true);
            }
        }
        else // create left
        {
            if (isOver == 1) // create over player
            {
                Vector3 createPosition = playerPosition + new Vector3(-20, interval, 0);
                GameObject obj = ObjectPooler.instance.GetDragonObject(0);
                obj.transform.position = createPosition;
                obj.SetActive(true);
            }
            else // create under player
            {
                Vector3 createPosition = playerPosition + new Vector3(-20, -interval, 0);
                GameObject obj = ObjectPooler.instance.GetDragonObject(0);
                obj.transform.position = createPosition;
                obj.SetActive(true);
            }
        }

    }
}