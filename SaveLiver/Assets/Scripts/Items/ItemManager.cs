using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager: MonoBehaviour
{
    public static ItemManager instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(this);
        }
    }


    public float itemLifeTime = 20.0f;

    private AudioSource itemAudio;

    void Start()
    {
        itemAudio = GetComponent<AudioSource>();
    }


    public void AudioPlay()
    {
        itemAudio.Play();
    }
}
