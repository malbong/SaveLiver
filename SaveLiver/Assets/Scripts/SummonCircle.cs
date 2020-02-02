using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonCircle : MonoBehaviour
{
    public Animator anim;
    private bool isSummon = false;

    void Start()
    {
        anim.SetTrigger("trigger");

        isSummon = false;

        StartCoroutine(SummonAndDestroy());
    }


    private void OnEnable()
    {
        Start();
    }


    private IEnumerator SummonAndDestroy()
    {
        yield return new WaitForSeconds(0.7f);
        
        Summon();

        yield return new WaitForSeconds(0.3f);

        gameObject.SetActive(false);
    }


    private void Summon()
    {
        if (isSummon == true) return;

        GameObject obj1 = ObjectPooler.instance.GetEnemyObject(11);
        obj1.transform.position = transform.position + new Vector3(0.7f, 0, 0);
        obj1.SetActive(true);

        GameObject obj2 = ObjectPooler.instance.GetEnemyObject(11);
        obj2.transform.position = transform.position + new Vector3(-0.7f, 0, 0);
        obj2.SetActive(true);

        isSummon = true;
    }
}
