using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swirl : MonoBehaviour
{
    public PointEffector2D pointEffector;
    public float lifeTime = 10.0f;
    public float minForce = -50;
    public float maxForce = -200;
    private float toMaxForceTime = 2.0f;
    private float toMaxForceSpeed;
    private float currentForce;


    public void Start()
    {
        pointEffector.forceMagnitude = minForce;
        currentForce = minForce;
        toMaxForceSpeed = (maxForce - minForce) / toMaxForceTime; // -150/2 => -75

        Destroy(gameObject, lifeTime);
    }

    public void Update()
    {
        transform.Rotate(0, 0, -720 * Time.deltaTime);
        if (currentForce < maxForce) // maxForce보다 크면 (절댓값)
        {
            return;
        }
        else
        {
            currentForce += toMaxForceSpeed * Time.deltaTime;
            pointEffector.forceMagnitude = currentForce;
        }
    }
}
