using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soul : MonoBehaviour
{
    public float lifeTime = 10f;
    private bool isAbsorbed = false;

    private void Start()
    {
        StartCoroutine(EndLifeTime());
    }

    public void CreateSoul(Vector3 createPosition)
    {
        float random = Random.Range(0f, 1f);
        if (random < 0.6f) // 60%
        {
            GameObject obj = ObjectPooler.instance.GetSoul();
            obj.transform.position = createPosition;
            obj.SetActive(true);
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag != "AbsorbCollider") return;

        if (isAbsorbed) return;

        isAbsorbed = true;

        GetComponent<AudioSource>().Play();

        StartCoroutine(FadeOut());
    }


    private IEnumerator FadeOut()
    {
        Vector3 originScale = transform.localScale;
        
        while (true)
        {
            transform.localScale -= new Vector3(0.05f, 0.05f, 0);
            if (transform.localScale.x <= 0)
            {
                break;
            }
            yield return new WaitForSeconds(0.01f);
        }

        gameObject.SetActive(false);

        transform.localScale = originScale;
        isAbsorbed = false;
    }


    private IEnumerator EndLifeTime()
    {
        yield return new WaitForSeconds(lifeTime);
        StartCoroutine(FadeOut());
    }
}
