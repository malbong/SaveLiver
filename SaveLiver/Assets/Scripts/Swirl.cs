﻿using System.Collections;
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

    private void Start()
    {

    }


    private void OnEnable()
    {
        currentForce = 0f;
        currentTime = 0f;
        isDisappear = false;
        pointEffector.forceMagnitude = minForce;
        currentForce = minForce;
        toMaxForceSpeed = (maxForce - minForce) / toMaxForceTime; // -150/2 => -75
    }


    private void Update()
    {
        if (GameManager.instance.isPause) return;

        currentTime += Time.deltaTime;
        transform.Rotate(0, 0, -1080 * Time.deltaTime);

        if (currentTime < toMaxForceTime)
        {
            if (currentForce < maxForce) return; //절댓값이 max보다 크면 (toMaxForceTime에 가기전에 이미 최대값이면)
            currentForce += toMaxForceSpeed * Time.deltaTime;
            pointEffector.forceMagnitude = currentForce;
        }
        else if (currentTime > lifeTime - toDisappearTime) //disappear 실행
        {
            if (currentForce > minForce) return; //절댓값이 최소값보다 작아지면
            if (isDisappear == false)
            {
                anim.SetTrigger("Disappear");
                StartCoroutine(Disappear());
                isDisappear = true;
            }
            currentForce += -(maxForce - minForce) * Time.deltaTime; // -(-200 - (-50)) 증가된 수치만큼, 다시 min으로
            pointEffector.forceMagnitude = currentForce;   
        }
    }


    private IEnumerator Disappear()
    {
        yield return new WaitForSeconds(toDisappearTime);

        gameObject.SetActive(false);
    }
}
