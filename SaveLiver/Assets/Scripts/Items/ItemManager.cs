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


    /********************************************
     * @함수명 : AudioPlay()
     * @작성자 : zeli
     * @입력 : void
     * @출력 : void
     * @설명 : 아이템 획득 시 나는 오디오 플레이
     *         모든 아이템들이 사용함
     */
    public void AudioPlay()
    {
        itemAudio.Play();
    }

}
