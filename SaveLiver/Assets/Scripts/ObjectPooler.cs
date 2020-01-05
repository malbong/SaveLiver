using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler instance;

    private void Awake()
    {
        instance = this;
    }

    public OffScreenIndicator offScreenIndicator;

    public bool more = true;

    public List<List<GameObject>> poolsItems; // Item Pool
    public List<GameObject> Items; // Kinds of Item
    public int itemCount = 5; // 생성할 아이템 갯수

    public List<List<GameObject>> poolsEnemies;
    public List<GameObject> Enemies;
    public int enemyCount = 10;

    public List<List<GameObject>> poolsIndicators;
    public List<GameObject> Indicators;
    public int indicatorCount = 10;

    public List<GameObject> poolDragons;
    public GameObject dragon;
    public int dragonCount = 3;

    public List<List<GameObject>> poolsDeadParticles;
    public List<GameObject> deadParticle;
    public int deadParticleCount = 15;

    public List<GameObject> poolSoul;
    public GameObject soul;
    public int soulCount = 15;

    void Start()
    {
        poolsItems = new List<List<GameObject>>();
        poolsIndicators = new List<List<GameObject>>();
        poolsEnemies = new List<List<GameObject>>();
        poolDragons = new List<GameObject>();
        poolsDeadParticles = new List<List<GameObject>>();
        poolSoul = new List<GameObject>();

        for(int i = 0; i < Items.Count; i++)
        {
            poolsItems.Add(new List<GameObject>());
            for (int j = 0; j < itemCount; j++)
            {
                GameObject obj = Instantiate(Items[i]); // i번째 아이템 생성
                obj.SetActive(false); // 생성하자마자 비활성화
                poolsItems[i].Add(obj); // 생성한 아이템 pool에 넣어주기
            }
        }
        

        for (int i = 0; i < Indicators.Count; i++)
        {
            poolsIndicators.Add(new List<GameObject>());
            for (int j = 0; j < indicatorCount; j++)
            {
                GameObject obj = Instantiate(Indicators[i]);
                obj.transform.SetParent(offScreenIndicator.transform);
                obj.SetActive(false);
                poolsIndicators[i].Add(obj);
            }
        }

        for (int i = 0; i < Enemies.Count; i++)
        {
            poolsEnemies.Add(new List<GameObject>());
            for (int j = 0; j < enemyCount; j++)
            {
                GameObject obj = Instantiate(Enemies[i]);
                obj.transform.parent.gameObject.SetActive(false);
                poolsEnemies[i].Add(obj.transform.parent.gameObject);
            }
        }

        for (int i = 0; i < dragonCount; i++)
        {
            GameObject obj = Instantiate(dragon);
            obj.SetActive(false);
            poolDragons.Add(obj);
        }

        for (int i = 0; i < deadParticle.Count; i++)
        {
            poolsDeadParticles.Add(new List<GameObject>());
            for (int j = 0; j < deadParticleCount; j++)
            {
                GameObject obj = Instantiate(deadParticle[i]);
                obj.SetActive(false);
                poolsDeadParticles[i].Add(obj);
            }
        }

        for (int i = 0; i < soulCount; i++)
        {
            GameObject obj = Instantiate(soul);
            obj.SetActive(false);
            poolSoul.Add(obj);
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
            GameObject obj = Instantiate(Items[index]);
            poolsItems[index].Add(obj);
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
            GameObject obj = Instantiate(Indicators[index]);
            obj.transform.SetParent(offScreenIndicator.transform);
            poolsIndicators[index].Add(obj);
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
            GameObject obj = Instantiate(Enemies[index]);
            poolsEnemies[index].Add(obj.transform.parent.gameObject);
            return obj.transform.parent.gameObject;
        }
        return null; // null을 반환하는 경우: 기기에 메모리가 부족할 때
    }


    public GameObject GetDragonObject()
    {
        foreach (GameObject obj in poolDragons)
        {
            if (!obj.activeInHierarchy) { return obj; }
        }
        if (more) // 미리 생성해준 오브젝트로 부족할 때를 대비해, 새로 생성해서 반환해줌
        {
            GameObject obj = Instantiate(dragon);
            poolDragons.Add(obj);
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
            return obj;
        }
        return null;
    }
}
