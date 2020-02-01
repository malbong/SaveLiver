using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spear : MonoBehaviour
{
    public float lifeTime = 10.0f;
    public bool isShootingSpear = false;

    private Rigidbody2D spearRigidbody;
    public float speed = 8.0f;

    private void Start()
    {
        StartCoroutine(TimeCheckAndDestroy());

        spearRigidbody = GetComponent<Rigidbody2D>();
    }


    private void OnEnable()
    {
        Start();
    }


    private void Update()
    {
        if (GameManager.instance.isPause) return;

        if (isShootingSpear)
        {
            spearRigidbody.velocity = transform.up * speed;
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player.instance.TakeDamage(1);
        }
    }


    private IEnumerator TimeCheckAndDestroy()
    {
        yield return new WaitForSeconds(lifeTime);

        isShootingSpear = false;

        gameObject.SetActive(false);
    }
}
