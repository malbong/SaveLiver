using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boom : MonoBehaviour
{
    void Start()
    {
        Destroy(gameObject, 0.68f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            Player.instance.TakeDamage(1);
        }
    }
}
