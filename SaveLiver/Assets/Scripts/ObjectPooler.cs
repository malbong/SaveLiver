using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            Create();
            DontDestroyOnLoad(instance);
        }
        else if(instance != this)
        {
            Destroy(this.gameObject);
        }
    }

    public bool more = true;

    public static List<List<GameObject>> poolsItems; // Item Pool
    public List<GameObject> items; // Kinds of Item
    public int itemCount = 5; // 생성할 아이템 갯수

    public static List<List<GameObject>> poolsEnemies;
    public List<GameObject> enemies;
    public int enemyCount = 10;

    public static List<List<GameObject>> poolsIndicators;
    public List<GameObject> indicators;
    public int indicatorCount = 10;

    public static List<List<GameObject>> poolsDragons;
    public List<GameObject> dragons;
    public int dragonCount = 3;

    public static List<List<GameObject>> poolsDeadParticles;
    public List<GameObject> deadParticle;
    public int deadParticleCount = 15;

    public static List<GameObject> poolSoul;
    public GameObject soul;
    public int soulCount = 15;

    public static List<GameObject> poolSpear;
    public GameObject spear;
    public int spearCount = 10;

    public static List<GameObject> poolBoom;
    public GameObject boom;
    public int boomCount = 3;

    public static List<GameObject> poolSummonCircle;
    public GameObject summonCircle;
    public int summonCircleCount = 3;


    private void Start()
    {
        OnReset();
    }


    public void OnReset()
    {
        for (int i = 0; i < poolsItems.Count; i++)
        {
            foreach (GameObject obj in poolsItems[i])
            {
                if (obj.activeInHierarchy) { obj.SetActive(false); }
            }
        }

        for (int i = 0; i < poolsIndicators.Count; i++)
        {
            foreach (GameObject obj in poolsIndicators[i])
            {
                if (obj.activeInHierarchy) { obj.SetActive(false); }
            }
        }

        for (int i = 0; i < poolsEnemies.Count; i++)
        {
            foreach (GameObject obj in poolsEnemies[i])
            {
                if (obj.activeInHierarchy) { obj.SetActive(false); }
            }
        }

        for (int i = 0; i < poolsDragons.Count; i++)
        {
            foreach (GameObject obj in poolsDragons[i])
            {
                if (obj.activeInHierarchy) { obj.SetActive(false); }
            }
        }

        for (int i = 0; i < poolsDeadParticles.Count; i++)
        {
            foreach (GameObject obj in poolsDeadParticles[i])
            {
                if (obj.activeInHierarchy) { obj.SetActive(false); }
            }
        }

        foreach (GameObject obj in poolSoul)
        {
            if (obj.activeInHierarchy) { obj.SetActive(false); }
        }

        foreach (GameObject obj in poolSpear)
        {
            if (obj.activeInHierarchy) { obj.SetActive(false); }
        }

        foreach (GameObject obj in poolBoom)
        {
            if (obj.activeInHierarchy) { obj.SetActive(false); }
        }

        foreach (GameObject obj in poolSummonCircle)
        {
            if (obj.activeInHierarchy) { obj.SetActive(false); }
        }
    }


    void Create()
    {
        poolsItems = new List<List<GameObject>>();
        poolsIndicators = new List<List<GameObject>>();
        poolsEnemies = new List<List<GameObject>>();
        poolsDragons = new List<List<GameObject>>();
        poolsDeadParticles = new List<List<GameObject>>();
        poolSoul = new List<GameObject>();
        poolSpear = new List<GameObject>();
        poolBoom = new List<GameObject>();
        poolSummonCircle = new List<GameObject>();

        for (int i = 0; i < items.Count; i++)
        {
            poolsItems.Add(new List<GameObject>());
            for (int j = 0; j < itemCount; j++)
            {
                GameObject obj = Instantiate(items[i]); // i번째 아이템 생성
                obj.SetActive(false); // 생성하자마자 비활성화
                poolsItems[i].Add(obj); // 생성한 아이템 pool에 넣어주기
                DontDestroyOnLoad(obj);
            }
        }
        

        for (int i = 0; i < indicators.Count; i++)
        {
            poolsIndicators.Add(new List<GameObject>());
            for (int j = 0; j < indicatorCount; j++)
            {
                GameObject obj = Instantiate(indicators[i]);
                obj.SetActive(false);
                poolsIndicators[i].Add(obj);
                DontDestroyOnLoad(obj);
            }
        }

        for (int i = 0; i < enemies.Count; i++)
        {
            poolsEnemies.Add(new List<GameObject>());
            for (int j = 0; j < enemyCount; j++)
            {
                GameObject obj = Instantiate(enemies[i]);
                obj.transform.parent.gameObject.SetActive(false);
                poolsEnemies[i].Add(obj.transform.parent.gameObject);
                DontDestroyOnLoad(obj.transform.parent.gameObject);
            }
        }
        
        for (int i = 0; i < dragons.Count; i++)
        {
            poolsDragons.Add(new List<GameObject>());
            for (int j = 0; j < dragonCount; j++)
            {
                GameObject obj = Instantiate(dragons[i]);
                obj.SetActive(false);
                poolsDragons[i].Add(obj);
                DontDestroyOnLoad(obj);
            }
        }

        for (int i = 0; i < deadParticle.Count; i++)
        {
            poolsDeadParticles.Add(new List<GameObject>());
            for (int j = 0; j < deadParticleCount; j++)
            {
                GameObject obj = Instantiate(deadParticle[i]);
                obj.SetActive(false);
                poolsDeadParticles[i].Add(obj);
                DontDestroyOnLoad(obj);
            }
        }

        for (int i = 0; i < soulCount; i++)
        {
            GameObject obj = Instantiate(soul);
            obj.SetActive(false);
            poolSoul.Add(obj);
            DontDestroyOnLoad(obj);
        }

        for (int i = 0; i < spearCount; i++)
        {
            GameObject obj = Instantiate(spear);
            obj.SetActive(false);
            poolSpear.Add(obj);
            DontDestroyOnLoad(obj);
        }

        for (int i = 0; i < boomCount; i++)
        {
            GameObject obj = Instantiate(boom);
            obj.SetActive(false);
            poolBoom.Add(obj);
            DontDestroyOnLoad(obj);
        }

        for (int i = 0; i < summonCircleCount; i++)
        {
            GameObject obj = Instantiate(summonCircle);
            obj.SetActive(false);
            poolSummonCircle.Add(obj);
            DontDestroyOnLoad(obj);
        }
    }
    

    public GameObject GetItemObject(int index)
    {
        foreach(GameObject obj in poolsItems[index])
        {
            if (!obj.activeInHierarchy) { return obj; }
        }
        if (more) // 미리 생성해준 오브젝트로 부족할 때를 대비해, 새로 생성해서 반환해줌
        {
            GameObject obj = Instantiate(items[index]);
            poolsItems[index].Add(obj);
            DontDestroyOnLoad(obj);
            return obj;
        }
        return null; // null을 반환하는 경우: 기기에 메모리가 부족할 때
    }


    public GameObject GetIndicatorObject(int index)
    {
        foreach (GameObject obj in poolsIndicators[index])
        {
            if (!obj.activeInHierarchy) { return obj; }
        }
        if (more)
        {
            GameObject obj = Instantiate(indicators[index]);
            poolsIndicators[index].Add(obj);
            DontDestroyOnLoad(obj);
            return obj;
        }
        return null;
    }


    public GameObject GetEnemyObject(int index)
    {
        foreach (GameObject obj in poolsEnemies[index])
        {
            if (!obj.activeInHierarchy) { return obj; }
        }
        if (more) // 미리 생성해준 오브젝트로 부족할 때를 대비해, 새로 생성해서 반환해줌
        {
            GameObject obj = Instantiate(enemies[index]);
            poolsEnemies[index].Add(obj.transform.parent.gameObject);
            DontDestroyOnLoad(obj.transform.parent.gameObject);
            return obj.transform.parent.gameObject;
        }
        return null; // null을 반환하는 경우: 기기에 메모리가 부족할 때
    }


    public GameObject GetDragonObject(int index) //fromLeftToRight: 0, fromRightToLeft : 1
    {
        foreach (GameObject obj in poolsDragons[index])
        {
            if (!obj.activeInHierarchy) { return obj; }
        }
        if (more) // 미리 생성해준 오브젝트로 부족할 때를 대비해, 새로 생성해서 반환해줌
        {
            GameObject obj = Instantiate(dragons[index]);
            poolsDragons[index].Add(obj);
            DontDestroyOnLoad(obj);
            return obj;
        }
        return null; // null을 반환하는 경우: 기기에 메모리가 부족할 때
    }


    public GameObject GetDeadParticle(int index)
    {
        foreach (GameObject obj in poolsDeadParticles[index])
        {
            if (!obj.activeInHierarchy) { return obj; }
        }
        if (more)
        {
            GameObject obj = Instantiate(deadParticle[index]);
            poolsDeadParticles[index].Add(obj);
            DontDestroyOnLoad(obj);
            return obj;
        }
        return null;
    }


    public GameObject GetSoul()
    {
        foreach (GameObject obj in poolSoul)
        {
            if (!obj.activeInHierarchy) { return obj; }
        }
        if (more)
        {
            GameObject obj = Instantiate(soul);
            poolSoul.Add(obj);
            DontDestroyOnLoad(obj);
            return obj;
        }
        return null;
    }


    public GameObject GetSpear()
    {
        foreach (GameObject obj in poolSpear)
        {
            if (!obj.activeInHierarchy) { return obj; }
        }
        if (more)
        {
            GameObject obj = Instantiate(spear);
            poolSpear.Add(obj);
            DontDestroyOnLoad(obj);
            return obj;
        }
        return null;
    }


    public GameObject GetBoom()
    {
        foreach (GameObject obj in poolBoom)
        {
            if (!obj.activeInHierarchy) { return obj; }
        }
        if (more)
        {
            GameObject obj = Instantiate(boom);
            poolBoom.Add(obj);
            DontDestroyOnLoad(obj);
            return obj;
        }
        return null;
    }


    public GameObject GetSummonCircle()
    {
        foreach (GameObject obj in poolSummonCircle)
        {
            if (!obj.activeInHierarchy) { return obj; }
        }
        if (more)
        {
            GameObject obj = Instantiate(summonCircle);
            poolSummonCircle.Add(obj);
            DontDestroyOnLoad(obj);
            return obj;
        }
        return null;
    }
}
