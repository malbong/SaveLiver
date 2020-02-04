﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soul : MonoBehaviour
{
    public float lifeTime = 10f;
    private bool isAbsorbed = false;
    private AudioSource audioSource;

    private void Start()
    {
        isAbsorbed = false;
        StartCoroutine(EndLifeTime());
        audioSource = GetComponent<AudioSource>();
        GetComponent<SpriteRenderer>().enabled = true;
    }


    private void OnEnable()
    {
        Start();
    }


    public void CreateSoul(Vector3 createPosition, float percentage = 0.6f)
    {
        float random = Random.Range(0f, 1f);
        if (random < percentage) // default 60%
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

        audioSource.Play();
        
        GameManager.instance.UpdateSoulCount(1);
        GameManager.instance.AddScore(20);

        StartCoroutine(FadeOut());
    }


    private IEnumerator FadeOut()
    {
        Vector3 originScale = transform.localScale;

        GetComponent<SpriteRenderer>().enabled = false;
        transform.GetChild(0).GetComponent<ParticleSystem>().Stop();

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
    }


    private IEnumerator EndLifeTime()
    {
        yield return new WaitForSeconds(lifeTime);
        StartCoroutine(FadeOut());
    }
}
