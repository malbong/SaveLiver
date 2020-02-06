using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifeTime = 10.0f;

    private Rigidbody2D bulletRigidbody;

    public float speed = 8.0f;
    

    private void Start()
    {
        StartCoroutine(TimeCheckAndDestroy());

        bulletRigidbody = GetComponent<Rigidbody2D>();
    }


    private void OnEnable()
    {
        Start();
    }


    private void Update()
    {
        if (GameManager.instance.isPause) return;

        bulletRigidbody.velocity = transform.up * speed;
        
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy" || other.tag == "Dragon") //드래곤이나 적을 만나면 파티클 재생 후 종료
        {
            PlayExplosionParticle();
            gameObject.SetActive(false);
        }
        else if (other.tag == "Spear")
        {
            if (other.GetComponent<Spear>() != null)
            {
                if (other.GetComponent<Spear>().isShootingSpear)
                {
                    PlayExplosionParticle();
                }
            }
        }
    }


    private IEnumerator TimeCheckAndDestroy()
    {
        yield return new WaitForSeconds(lifeTime);
        
        gameObject.SetActive(false);
    }


    private IEnumerator WaitSetActiveFalse(GameObject obj, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        obj.SetActive(false);
    }


    private void PlayExplosionParticle()
    {
        //Get particle and Play it
        GameObject explosionParticleObject = ObjectPooler.instance.GetExplosionParticle();
        explosionParticleObject.transform.position = transform.position + new Vector3(0, 0.3f, 0);
        explosionParticleObject.SetActive(true);

        ParticleSystem explosionParticle = explosionParticleObject.GetComponent<ParticleSystem>();
        explosionParticle.Play();
        explosionParticle.GetComponent<AudioSource>().Play();

        //particle setActive false
        StartCoroutine(WaitSetActiveFalse(explosionParticleObject, 2.0f));
    }
}
