using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletShooter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
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
            yield return new WaitForSeconds(1f);

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
