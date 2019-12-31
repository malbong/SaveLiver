using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    public GameObject[] tile; //임시 타일 public으로 받기


    void Awake()
    {
        SwapTile();
        SetPlayerRandomPosition();
    }


    /**************************************
    * @함수명: SwapTile()
    * @작성자: malbong
    * @입력: void
    * @출력: void
    * @설명: Player의 시작 타일을 변경
    *        랜덤으로 뽑아 현재 "11" 타일과 자리를 바꿈
    */
    private void SwapTile()
    {
        int i = Random.Range(0, 3);
        int j = Random.Range(0, 3);

        //Get Index2D Name
        string randomIndex2D = i.ToString() + j.ToString(); // ex. "12"
        Transform randomTile = transform.Find(randomIndex2D);
        string originIndex2D = "11";
        Transform originTile = transform.Find(originIndex2D); // 11

        //Swap Position 
        Vector3 tmpPosition = randomTile.position;
        randomTile.position = originTile.position;
        originTile.position = tmpPosition;

        //Swap Object Name
        string tmpName = randomTile.name;
        randomTile.name = originTile.name;
        originTile.name = tmpName;

        //Swap Array
        //00:0, 01:1, 02:2, 10:3, 11:4, 12:5 , 20:6, 21:7, 22:8
        //randomIndex = 3 * i + j
        int randomIndex1D = 3 * i + j;
        int originIndex1D = 3 * 1 + 1; // 11
        GameObject tmpObject = tile[randomIndex1D];
        tile[randomIndex1D] = tile[originIndex1D];
        tile[originIndex1D] = tmpObject;
    }


    private void SetPlayerRandomPosition()
    {
        float randomPosX = Random.Range(-11f, 11f); // -11 <= x < 11
        float randomPosY = Random.Range(-11f, 11f); // -11 <= y < 11

        transform.Translate(randomPosX, randomPosY, 0);
    }
}
