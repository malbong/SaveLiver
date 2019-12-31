using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swirl : MonoBehaviour
{
    public Animator anim;
    public PointEffector2D pointEffector;
    public float lifeTime = 10.0f;
    public float minForce = -50;
    public float maxForce = -200;
    public float toMaxForceTime = 2.0f;
    private float toDisappearTime = 1.0f;
    private float toMaxForceSpeed;
    private float currentForce;
    private float currentTime = 0f;
    private bool isDisappear = false;

    public void Start()
    {
        pointEffector.forceMagnitude = minForce;
        currentForce = minForce;
        toMaxForceSpeed = (maxForce - minForce) / toMaxForceTime; // -150/2 => -75
    }


    public void Update()
    {
        if (Time.timeScale == 0) return;

        currentTime += Time.deltaTime;
        transform.Rotate(0, 0, -1080 * Time.deltaTime);

        if (currentTime < toMaxForceTime)
        {
            if (currentForce < maxForce) return; //절댓값이 max보다 크면
            currentForce += toMaxForceSpeed * Time.deltaTime;
            pointEffector.forceMagnitude = currentForce;
        }
        else if (currentTime > lifeTime - toDisappearTime) //disappear 실행
        {
            if (currentForce > minForce) return;
            if (isDisappear == false)
            {
                anim.SetTrigger("Disappear");
                Destroy(gameObject, toDisappearTime);
                isDisappear = true;
            }
            currentForce += -(maxForce - minForce) * Time.deltaTime; // -(-200 - (-50))
            pointEffector.forceMagnitude = currentForce;   
        }
    }
}
