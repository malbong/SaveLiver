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

    public float itemSpawnTime = 8;

    private bool isRunningNormal = false;
    private bool isRunningSpecial = false;
    private bool isRunningPattern = false;


    private void Start()
    {
        spawnRadius = SpawnManager.instance.radius;
        angle45Length = Mathf.Sqrt(Mathf.Pow(spawnRadius, 2) / 2.0f);

        isRunningNormal = false;
        isRunningSpecial = false;
        isRunningPattern = false;

        StartCoroutine(Play());
        StartCoroutine(ItemCreate());
    }


    private IEnumerator Play()
    {
        if (PlayerInformation.IsHard) // Hard 모드일 때
        {
            yield return new WaitForSeconds(0.1f);
            EnemySpawn(0);

            yield return new WaitForSeconds(3f);
            int randomIndex = Random.Range(0, 2);
            EnemySpawn(randomIndex);

            yield return new WaitForSeconds(3f);
            EnemySpawn(1);

            yield return new WaitForSeconds(3f);
            CreateEasySpecialEnemy();

            yield return new WaitForSeconds(3f);
            EnemySpawn(2);

            yield return new WaitForSeconds(3f);
            randomIndex = Random.Range(1, 3);
            EnemySpawn(randomIndex);

            yield return new WaitForSeconds(3f);
            EnemySpawn(3);

            yield return new WaitForSeconds(3f);
            CreateEasySpecialEnemy();

            yield return new WaitForSeconds(0.5f);
            EnemySpawn(3);

            yield return new WaitForSeconds(3f);
            CreateEasySpecialEnemy();


            yield return new WaitForSeconds(4f);
            randomIndex = Random.Range(2, 4);
            EnemySpawn(randomIndex);
            AllDirection4();

            yield return new WaitForSeconds(4f);
            CreateEasySpecialEnemy();

            yield return new WaitForSeconds(0.5f);
            randomIndex = Random.Range(2, 4);
            EnemySpawn(randomIndex);

            yield return new WaitForSeconds(3f);
            randomIndex = Random.Range(2, 4);
            EnemySpawn(randomIndex);

            int tmpRandom = Random.Range(0, 2);
            if (tmpRandom == 0)
            {
                DiagonalLeft(3, Random.Range(0, 2) == 0 ? 1 : -1);
            }
            else
            {
                DiagonalRight(3, Random.Range(0, 2) == 0 ? 1 : -1);
            }

            yield return new WaitForSeconds(3f);
            EnemySpawn(4);

            yield return new WaitForSeconds(3f);
            CreateMiddleSpecialEnemy();
            yield return new WaitForSeconds(1f);
            Swirl(-250);


            yield return new WaitForSeconds(4f);
            EnemySpawn(4);
            DiagonalBothSide(3, Random.Range(0, 2) == 0 ? 1 : -1);
            yield return new WaitForSeconds(0.5f);
            CreateMiddleSpecialEnemy();

            yield return new WaitForSeconds(4f);
            randomIndex = Random.Range(3, 5);
            EnemySpawn(randomIndex);

            yield return new WaitForSeconds(4.5f);
            CreateHardSpecialEnemy();
            Dragon(Random.Range(0, 2) == 0 ? 1 : -1, -1, Random.Range(3, 5));

            yield return new WaitForSeconds(3f);
            EnemySpawn(5);
            yield return new WaitForSeconds(0.5f);

            tmpRandom = Random.Range(0, 2);
            if (tmpRandom == 0) //left
            {
                DiagonalLeftSeqStart(2, Random.Range(0, 2) == 0 ? 1 : -1);
            }
            else //right
            {
                DiagonalRightSeqStart(2, Random.Range(0, 2) == 0 ? 1 : -1);
            }

            yield return new WaitForSeconds(3f);
            EnemySpawn(9);
            yield return new WaitForSeconds(0.3f);
            EnemySpawn(9);
            yield return new WaitForSeconds(0.3f);
            EnemySpawn(9);
            yield return new WaitForSeconds(0.3f);
            EnemySpawn(9);

            //1분 이후 (60초)

            //totalPlayTime :        60  90   120  150   180  210      ~  210이상
            //level                  1   2    3    4     5    6             7
            //NomalEnemyRandom :     10  9    8    7     6    5  (time)     
            //SpecialEnemyRandom :   4   3.8  3.6  3.4   3.2  3  (time)
            //PatternEnemyRandom :   4   3.8  3.6  3.4   3.2  3  (time)

            while (true)
            {
                if (isRunningNormal == false)
                {
                    float delayTime1 = GetDelayTime(1);
                    StartCoroutine(NomalEnemyRandomCreate(delayTime1));
                }
                if (isRunningSpecial == false)
                {
                    float delayTime2 = GetDelayTime(2);
                    StartCoroutine(SpecialEnemyRandomCreate(delayTime2));
                }
                if (isRunningPattern == false)
                {
                    float delayTime3 = GetDelayTime(3);
                    StartCoroutine(PatterRandomCreate(delayTime3));
                }
                Debug.Log(GetLevel());
                yield return new WaitForSeconds(Time.deltaTime);
            }
        }
        else // Easy 모드일 때
        {
            yield return new WaitForSeconds(0.1f);
            EnemySpawn(0);

            yield return new WaitForSeconds(5f);
            EnemySpawn(0);

            yield return new WaitForSeconds(5f);
            EnemySpawn(1);

            yield return new WaitForSeconds(7f);
            EnemySpawn(1);

            yield return new WaitForSeconds(7f);
            EnemySpawn(1);

            yield return new WaitForSeconds(7f);
            EnemySpawn(2);

            yield return new WaitForSeconds(10f);
            EnemySpawn(2);

            yield return new WaitForSeconds(10f);
            EnemySpawn(2);
            EnemySpawn(3);
        }
    }
    

    private float GetDelayTime(int flag)
    {
        float delayTime = 0; //y

        float level = GetLevel();
        switch (flag)
        {
            case 1://nomal >>> y = -x + 11
                delayTime = -1 * level + 11;
                if (delayTime < 5) delayTime = 5;
                break;
            case 2 ://special
                delayTime = -1 / 5.0f * level + 21 / 5.0f;
                if (delayTime < 3) delayTime = 3;
                break;
            case 3://pattern
                delayTime = -1 / 5.0f * level + 21 / 5.0f;
                if (delayTime < 3) delayTime = 3;
                break;
        }
        return delayTime; //y
    }


    private float GetLevel()
    {
        float level = GameManager.instance.totalPlayTime / 30.0f - 1;
        if (level > 6) level = 7;
        return level;
    }


    private IEnumerator NomalEnemyRandomCreate(float time) 
    {
        isRunningNormal = true;

        yield return new WaitForSeconds(time);
        EnemySpawn(Random.Range(3, 6));

        isRunningNormal = false;
    }


    private IEnumerator SpecialEnemyRandomCreate(float time)
    {
        isRunningSpecial = true;

        yield return new WaitForSeconds(time);

        //level 1-2.66  2.66-4.33  4.33-6
        //easy : Linear, SuddenAttack, Blinder
        //middle : Spear, Boom 
        //hard : Summons, Confuser, Summoner
        int random = Random.Range(1, 101); // 1 ~ 100
        float level = GetLevel();
        if (1 <= level && level < 2.66)
        {
            /*//easy 50 middle 30 hard 20
            if (1 <= random && random <= 50) CreateEasySpecialEnemy();
            else if (50 < random && random <= 80) CreateMiddleSpecialEnemy();
            else CreateHardSpecialEnemy();*/// 81~100

            //easy 30 middle 40 hard 30
            if (1 <= random && random <= 30) CreateEasySpecialEnemy();
            else if (30 < random && random <= 70) CreateMiddleSpecialEnemy();
            else CreateHardSpecialEnemy(); // 71~100
        }
        else if (2.66 <= level && level < 4.33)
        {
            //easy 20 middle 30 hard 50
            if (1 <= random && random <= 20) CreateEasySpecialEnemy();
            else if (20 < random && random <= 50) CreateMiddleSpecialEnemy();
            else CreateHardSpecialEnemy(); // 51~100
        }
        else if (4.33 <= level && level <= 6) //4.33 ~ 6
        {
            //easy 0 middle 30 hard 70
            if (1 <= random && random <= 30) CreateMiddleSpecialEnemy();
            else CreateHardSpecialEnemy();
        }
        else // level 7 max
        {
            //easy 0 middle 10 hard 90
            if (1 <= random && random <= 10) CreateMiddleSpecialEnemy();
            else CreateHardSpecialEnemy();
        }
        
        isRunningSpecial = false;
    }


    private void CreateEasySpecialEnemy()
    {
        int randomInt = Random.Range(0, 3);
        switch (randomInt)
        {
            case 0:
                EnemySpawn(6);
                break;
            case 1:
                EnemySpawn(9);
                break;
            case 2:
                EnemySpawn(10);
                break;
        }
    }


    private void CreateMiddleSpecialEnemy()
    {
        int randomInt = Random.Range(0, 2);
        switch (randomInt)
        {
            case 0:
                EnemySpawn(7);
                break;
            case 1:
                EnemySpawn(8);
                break;
        }
        //EnemySpawn(Random.Range(7, 9));
    }


    private void CreateHardSpecialEnemy()
    {
        int randomInt = Random.Range(0, 3);
        switch (randomInt)
        {
            case 0:
                EnemySpawn(11);
                break;
            case 1:
                EnemySpawn(12);
                break;
            case 2:
                EnemySpawn(13);
                break;
        }
        //EnemySpawn(Random.Range(11, 14));
    }


    private IEnumerator PatterRandomCreate(float time)
    {
        isRunningPattern = true;
        yield return new WaitForSeconds(time);

        //level 1-2.66  2.66-4.33  4.33-6

        //easy : all4, left, right 
        //middle : all8, swirl, both 
        //hard : dragon, diagonalseq, swirl 2
        int random = Random.Range(1, 101); // 1 ~ 100
        float level = GetLevel();
        if (1 <= level && level < 2.66)
        {
            /*//easy 20 middle 30 hard 50
            if (1 <= random && random <= 50) CreateEasyPattern();
            else if (50 < random && random <= 80) CreateMiddlePattern();
            else CreateHardPattern(); // 81~100
            */
            //easy 30 middle 40 hard 30
            if (1 <= random && random <= 30) CreateEasyPattern();
            else if (30 < random && random <= 70) CreateMiddlePattern();
            else CreateHardPattern(); // 71 ~ 100
        }
        else if (2.66 <= level && level < 4.33)
        {
            //easy 20 middle 30 hard 50
            if (1 <= random && random <= 20) CreateEasyPattern();
            else if (20 < random && random <= 50) CreateMiddlePattern();
            else CreateHardPattern(); // 51 ~ 100
        }
        else if (4.33 <= level && level <= 6)//4.33 ~ 6
        {
            //easy 0 middle 30 hard 70
            if (1 <= random && random <= 30) CreateMiddlePattern();
            else CreateHardPattern();
        }
        else //level 7 max
        {
            //easy 0 middle 10 hard 90
            if (1 <= random && random <= 10) CreateMiddlePattern();
            else CreateHardPattern();
        }

        isRunningPattern = false;
    }


    private void CreateEasyPattern()
    {
        int randomInt = Random.Range(0, 3);
        switch (randomInt)
        {
            case 0:
                AllDirection4();
                break;
            case 1:
                DiagonalLeft(3, Random.Range(0, 2) == 0 ? 1 : -1);
                break;
            case 2:
                DiagonalRight(3, Random.Range(0, 2) == 0 ? 1 : -1);
                break;
        }
    }


    private void CreateMiddlePattern()
    {
        int randomInt = Random.Range(0, 3);
        switch (randomInt)
        {
            case 0:
                AllDirection8();
                break;
            case 1:
                Swirl(-250);
                break;
            case 2:
                DiagonalBothSide(3, Random.Range(0, 2) == 0 ? 1 : -1);
                break;
        }
    }


    private void CreateHardPattern()
    {
        int randomInt = Random.Range(0, 3);
        switch (randomInt)
        {
            case 0:
                if (Random.Range(1, 3) == 1) // 1 마리
                {
                    Dragon(Random.Range(0, 2) == 0 ? 1 : -1, Random.Range(0, 2) == 0 ? 1 : -1, Random.Range(2.0f, 3.0f));
                }
                else //2마리
                {
                    Dragon(Random.Range(0, 2) == 0 ? 1 : -1, 1, Random.Range(2.0f, 3.0f));
                    Dragon(Random.Range(0, 2) == 0 ? 1 : -1, -1, Random.Range(2.0f, 3.0f));
                }
                break;
            case 1:
                if (Random.Range(0, 2) == 0)
                {
                    DiagonalLeftSeqStart(2, Random.Range(0, 2) == 0 ? 1 : -1);
                }
                else
                {
                    DiagonalRightSeqStart(2, Random.Range(0, 2) == 0 ? 1 : -1);
                }
                break;
            case 2:
                Swirl(-250, Random.Range(2.0f, 3.0f), true);
                break;
        }
    }


    IEnumerator ItemCreate()
    {
        yield return new WaitForSeconds(0.1f);

        while (true)
        {
            ItemRandomSpawn();
            yield return new WaitForSeconds(itemSpawnTime);
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


    public void DiagonalLeft(float interval, int sign = 1)
    {
        playerPosition = Player.instance.transform.position;

        Vector3 targetPosition = playerPosition;
        Vector3 diffPosition = new Vector3(-angle45Length, sign * angle45Length, 0);

        CreateLinearTurtle(diffPosition, targetPosition);

        targetPosition = playerPosition + new Vector3(0, interval, 0);
        CreateLinearTurtle(diffPosition, targetPosition);

        targetPosition = playerPosition + new Vector3(0, -interval, 0);
        CreateLinearTurtle(diffPosition, targetPosition);
    }


    public IEnumerator DiagonalLeftSequence(float interval, int sign = 1)
    {
        playerPosition = Player.instance.transform.position;
        
        Vector3 diffPosition = new Vector3(-angle45Length, sign * angle45Length, 0);
        
        Vector3 targetPosition = playerPosition;
        CreateLinearTurtle(diffPosition, targetPosition);
        yield return new WaitForSeconds(0.25f);

        targetPosition = playerPosition + new Vector3(sign * interval * 1 * Mathf.Sqrt(2) / 2, interval * 1 * Mathf.Sqrt(2)/2, 0);
        CreateLinearTurtle(diffPosition, targetPosition);
        targetPosition = playerPosition + new Vector3(sign * -interval * 1 * Mathf.Sqrt(2) / 2, -interval * 1, 0);
        CreateLinearTurtle(diffPosition, targetPosition);
        yield return new WaitForSeconds(0.25f);

        targetPosition = playerPosition + new Vector3(sign * interval * 2 * Mathf.Sqrt(2) / 2, interval * 2 * Mathf.Sqrt(2) / 2, 0);
        CreateLinearTurtle(diffPosition, targetPosition);
        targetPosition = playerPosition + new Vector3(sign * -interval * 2 * Mathf.Sqrt(2) / 2, -interval * 2, 0);
        CreateLinearTurtle(diffPosition, targetPosition);
        yield return new WaitForSeconds(0.25f);

        targetPosition = playerPosition + new Vector3(sign * interval * 3 * Mathf.Sqrt(2) / 2, interval * 3 * Mathf.Sqrt(2) / 2, 0);
        CreateLinearTurtle(diffPosition, targetPosition);
        targetPosition = playerPosition + new Vector3(sign * -interval * 3 * Mathf.Sqrt(2) / 2, -interval * 3, 0);
        CreateLinearTurtle(diffPosition, targetPosition);
        yield return new WaitForSeconds(0.25f);

        targetPosition = playerPosition + new Vector3(sign * interval * 4 * Mathf.Sqrt(2) / 2, interval * 4 * Mathf.Sqrt(2) / 2, 0);
        CreateLinearTurtle(diffPosition, targetPosition);
        targetPosition = playerPosition + new Vector3(sign * -interval * 4 * Mathf.Sqrt(2) / 2, -interval * 4, 0);
        CreateLinearTurtle(diffPosition, targetPosition);
        yield return new WaitForSeconds(0.25f);

        targetPosition = playerPosition + new Vector3(sign * interval * 5 * Mathf.Sqrt(2) / 2, interval * 5 * Mathf.Sqrt(2) / 2, 0);
        CreateLinearTurtle(diffPosition, targetPosition);
        targetPosition = playerPosition + new Vector3(sign * -interval * 5 * Mathf.Sqrt(2) / 2, -interval * 5, 0);
        CreateLinearTurtle(diffPosition, targetPosition);
        yield return new WaitForSeconds(0.25f);

        targetPosition = playerPosition + new Vector3(sign * interval * 6 * Mathf.Sqrt(2) / 2, interval * 6 * Mathf.Sqrt(2) / 2, 0);
        CreateLinearTurtle(diffPosition, targetPosition);
        targetPosition = playerPosition + new Vector3(sign * -interval * 6 * Mathf.Sqrt(2) / 2, -interval * 6, 0);
        CreateLinearTurtle(diffPosition, targetPosition);
        yield return new WaitForSeconds(0.25f);
    }
    public void DiagonalLeftSeqStart(float interval, int sign)
    {
        StartCoroutine(DiagonalLeftSequence(interval, sign));
    }


    public void DiagonalRight(float interval, int sign = 1)
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
        Vector3 diffPosition = new Vector3(angle45Length, sign * angle45Length, 0);
        GameObject obj = CreateLinearTurtle(diffPosition, targetPosition);
        obj.transform.GetChild(0).Rotate(0, 180, 0);

        targetPosition = playerPosition + new Vector3(0, interval, 0);
        obj = CreateLinearTurtle(diffPosition, targetPosition);
        obj.transform.GetChild(0).Rotate(0, 180, 0);

        targetPosition = playerPosition + new Vector3(0, -interval, 0);
        obj = CreateLinearTurtle(diffPosition, targetPosition);
        obj.transform.GetChild(0).Rotate(0, 180, 0);
    }


    public IEnumerator DiagonalRightSequence(float interval, int sign = 1)
    {
        playerPosition = Player.instance.transform.position;

        Vector3 diffPosition = new Vector3(angle45Length, sign * angle45Length, 0);

        Vector3 targetPosition = playerPosition;
        GameObject obj = CreateLinearTurtle(diffPosition, targetPosition);
        obj.transform.GetChild(0).Rotate(0, 180, 0);
        yield return new WaitForSeconds(0.25f);

        targetPosition = playerPosition + new Vector3(sign * -interval * 1 * Mathf.Sqrt(2) / 2, interval * 1 * Mathf.Sqrt(2) / 2, 0);
        obj = CreateLinearTurtle(diffPosition, targetPosition);
        obj.transform.GetChild(0).Rotate(0, 180, 0);
        targetPosition = playerPosition + new Vector3(sign * interval * 1 * Mathf.Sqrt(2) / 2, -interval * 1 * Mathf.Sqrt(2) / 2, 0);
        obj = CreateLinearTurtle(diffPosition, targetPosition);
        obj.transform.GetChild(0).Rotate(0, 180, 0);
        yield return new WaitForSeconds(0.25f);

        targetPosition = playerPosition + new Vector3(sign * -interval * 2 * Mathf.Sqrt(2) / 2, interval * 2 * Mathf.Sqrt(2) / 2, 0);
        obj = CreateLinearTurtle(diffPosition, targetPosition);
        obj.transform.GetChild(0).Rotate(0, 180, 0);
        targetPosition = playerPosition + new Vector3(sign * interval * 2 * Mathf.Sqrt(2) / 2, -interval * 2 * Mathf.Sqrt(2) / 2, 0);
        obj = CreateLinearTurtle(diffPosition, targetPosition);
        obj.transform.GetChild(0).Rotate(0, 180, 0);
        yield return new WaitForSeconds(0.25f);

        targetPosition = playerPosition + new Vector3(sign * -interval * 3 * Mathf.Sqrt(2) / 2, interval * 3 * Mathf.Sqrt(2) / 2, 0);
        obj = CreateLinearTurtle(diffPosition, targetPosition);
        obj.transform.GetChild(0).Rotate(0, 180, 0);
        targetPosition = playerPosition + new Vector3(sign * interval * 3 * Mathf.Sqrt(2) / 2, -interval * 3 * Mathf.Sqrt(2) / 2, 0);
        obj = CreateLinearTurtle(diffPosition, targetPosition);
        obj.transform.GetChild(0).Rotate(0, 180, 0);
        yield return new WaitForSeconds(0.25f);

        targetPosition = playerPosition + new Vector3(sign * -interval * 4 * Mathf.Sqrt(2) / 2, interval * 4 * Mathf.Sqrt(2) / 2, 0);
        obj = CreateLinearTurtle(diffPosition, targetPosition);
        obj.transform.GetChild(0).Rotate(0, 180, 0);
        targetPosition = playerPosition + new Vector3(sign * interval * 4 * Mathf.Sqrt(2) / 2, -interval * 4 * Mathf.Sqrt(2) / 2, 0);
        obj = CreateLinearTurtle(diffPosition, targetPosition);
        obj.transform.GetChild(0).Rotate(0, 180, 0);
        yield return new WaitForSeconds(0.25f);

        targetPosition = playerPosition + new Vector3(sign * -interval * 5 * Mathf.Sqrt(2) / 2, interval * 5 * Mathf.Sqrt(2) / 2, 0);
        obj = CreateLinearTurtle(diffPosition, targetPosition);
        obj.transform.GetChild(0).Rotate(0, 180, 0);
        targetPosition = playerPosition + new Vector3(sign * interval * 5 * Mathf.Sqrt(2) / 2, -interval * 5 * Mathf.Sqrt(2) / 2, 0);
        obj = CreateLinearTurtle(diffPosition, targetPosition);
        obj.transform.GetChild(0).Rotate(0, 180, 0);
        yield return new WaitForSeconds(0.25f);

        targetPosition = playerPosition + new Vector3(sign * -interval * 6 * Mathf.Sqrt(2) / 2, interval * 6 * Mathf.Sqrt(2) / 2, 0);
        obj = CreateLinearTurtle(diffPosition, targetPosition);
        obj.transform.GetChild(0).Rotate(0, 180, 0);
        targetPosition = playerPosition + new Vector3(sign * interval * 6 * Mathf.Sqrt(2) / 2, -interval * 6 * Mathf.Sqrt(2) / 2, 0);
        obj = CreateLinearTurtle(diffPosition, targetPosition);
        obj.transform.GetChild(0).Rotate(0, 180, 0);
        yield return new WaitForSeconds(0.25f);
    }
    public void DiagonalRightSeqStart(float interval, int sign = 1)
    {
        StartCoroutine(DiagonalRightSequence(interval, sign));
    }


    public void DiagonalBothSide(float interval, int sign = 1)
    {
        DiagonalLeft(interval, sign);
        DiagonalRight(interval, sign);
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