using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletShooter : MonoBehaviour
{
    public Animator anim;

    private AudioSource audioSource;


    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(ShootBullet());
    }


    private void OnEnable()
    {
        Start();
    }


    private IEnumerator ShootBullet()
    {
        while (true)
        {
            anim.SetTrigger("charging");

            yield return new WaitForSeconds(1f);

            anim.SetTrigger("shooting");
            audioSource.Play();
            CreateBullet();
        }
    }


    private void CreateBullet()
    {
        GameObject obj = ObjectPooler.instance.GetBullet();
        obj.transform.position = transform.position;
        obj.transform.rotation = transform.rotation;
        obj.SetActive(true);
    }
}
