using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soul : MonoBehaviour
{
    public float lifeTime = 10f;
    private bool isAbsorbed = false;
    private AudioSource audioSource;

    private bool isFadeOutRunning = false;

    private void Start()
    {
        isAbsorbed = false;

        isFadeOutRunning = false;

        audioSource = GetComponent<AudioSource>();

        GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<CircleCollider2D>().enabled = true;
        transform.GetChild(0).gameObject.SetActive(true);
        transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);

        StartCoroutine(EndLifeTime());
    }


    private void OnEnable()
    {
        Start();
    }


    public void CreateSoul(Vector3 createPosition, float percentage = 0.6f)
    {
        float tryCount = Player.instance.soulLucky; //customs[1] == 3
        if (tryCount == 1.5f)
        {
            tryCount = Random.Range(1, 3);
        }

        for (int i = 0; i < tryCount; i++)
        {
            float random = Random.Range(0f, 1f);
            if (random < percentage) // default 60%  &  follow enemy 100%
            {
                GameObject obj = ObjectPooler.instance.GetSoul();
                obj.transform.position = createPosition + new Vector3(i/5, i/5, 0);
                obj.SetActive(true);
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag != "AbsorbCollider") return;

        if (isAbsorbed) return;

        isAbsorbed = true;

        audioSource.Play();

        GameManager.instance.UpdateSoulCount(1);
        GameManager.instance.AddScore(20);

        GetComponent<CircleCollider2D>().enabled = false;
        transform.GetChild(0).gameObject.SetActive(false);

        StartCoroutine(FadeOut());
    }


    private IEnumerator FadeOut()
    {
        if (isFadeOutRunning) yield break;

        isFadeOutRunning = true;

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

        while (true)
        {
            yield return new WaitForSeconds(Time.deltaTime);
            if (!audioSource.isPlaying) break;
        }
        gameObject.SetActive(false);

        transform.localScale = originScale;
        isAbsorbed = false;

        isFadeOutRunning = false;
    }


    private IEnumerator EndLifeTime()
    {
        yield return new WaitForSeconds(lifeTime);

        GetComponent<CircleCollider2D>().enabled = false;
        transform.GetChild(0).gameObject.SetActive(false);

        StartCoroutine(FadeOut());
    }


    public void EndingDestroySoul()
    {
        GetComponent<CircleCollider2D>().enabled = false;
        transform.GetChild(0).gameObject.SetActive(false);
        gameObject.SetActive(false);
        isAbsorbed = false;
    }
}
