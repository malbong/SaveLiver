using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; set; }
    public Image[] liverIconImage;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this; // instance 초기화
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }


    void Update()
    {

    }


    /**************************************
    * @함수명: UpdateLiverIcon
    * @작성자: zeli
    * @입력: liver
    * @출력: void
    * @설명: Player가 피격시마다 LiverUI를 새로 업데이트함.
    */
    public void UpdateLiverIcon(int liver)
    {
        // Liver 3개를 일단 비활성화하고,
        for (int i = 0; i < 3; i++)
        {
            liverIconImage[i].gameObject.SetActive(false);
        }

        // 남아있는 Liver만큼만 활성화
        for (int i = 0; i < liver; i++)
        {
            liverIconImage[i].gameObject.SetActive(true);
        }
    }
}
