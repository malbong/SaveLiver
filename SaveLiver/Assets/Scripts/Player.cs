using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance { get; set; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this; // instance 초기화
        }
        DontDestroyOnLoad(gameObject); // 씬이 새롭게 로드되어도 변경(파괴)되지 않도록.
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}