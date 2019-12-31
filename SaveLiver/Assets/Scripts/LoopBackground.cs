using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LoopBackground : MonoBehaviour
{
    public GameObject[] tmp_tile; //임시 타일 public으로 받기
    public GameObject[,] tile; //임시 타일을 3x3으로 바꿀 변수
    private int currentIndex_i; //현재 나의 타일 인덱스 i
    private int currentIndex_j; //현재 나의 타일 인덱스 j
    private string tmpStringIndex; //오브젝트 이름(ex. 00 ~ 22)로 받을 변수


    private void Start()
    {
        tile = new GameObject[3, 3]; // 3x3
        int cnt = 0; 
        for (int i = 0; i < 3; i++) 
        {
            for (int j = 0; j < 3; j++)
            {
                tile[i, j] = tmp_tile[cnt]; //타일을 2차원으로 변경
                cnt += 1;
            }
        }
        tmpStringIndex = this.name; //오브젝트 이름을 스트링으로 받아서 인덱스에 넣음
        currentIndex_i = int.Parse(tmpStringIndex[0].ToString());
        currentIndex_j = int.Parse(tmpStringIndex[1].ToString());
    }


    private void OnTriggerExit2D(Collider2D other) //충돌 Exit처리 -> 나가면 배경이 바뀌어야 함
    {
        if (other.tag != "MoveCollider") return; //다른 충돌이면 그냥 리턴
        if (Player.instance.isAlive == false) return; //플레이어가 죽으면 바꾸지 않음
        
        Vector3 dir = other.transform.position - transform.position; //플레이어의 위치 - 타일의 중심 =>>> 타일 중심에서의 벡터가 나옴
        float angle = Vector3.Angle(transform.up, dir); //각도
        int sign = Vector3.Cross(transform.up, dir).z < 0 ? -1 : 1; 
        angle *= sign;

        if (-45 <= angle && angle <= 45) // 위쪽
        {
            for (int i = 0; i < 3; i++) // 타일 3개를 옮겨야 함
            {
                if (currentIndex_i + 1 <= 2) // 배열에 대한 예외처리, 아래의 else문은 currentIndex_i가 2일때임
                {   // 예외 : 이미 옮긴 것을 또 옮길 수 있기 때문, position.y의 차이가 24면 옮김
                    if (tile[currentIndex_i + 1, i].transform.position.y - transform.position.y == -24) 
                    {
                        tile[currentIndex_i + 1, i].transform.position += new Vector3(0, 24 * 3, 0); //위쪽으로 가므로 아래행을 옮김
                    }
                }
                else
                {   
                    if (tile[0, i].transform.position.y - transform.position.y == -24) //currentIndex_i가 2일때는 아래가 0인덱스
                    {
                        tile[0, i].transform.position += new Vector3(0, 24 * 3, 0);
                    }
                }
            }
        }
        else if (45 <= angle && angle <= 135) // 왼쪽 
        {
            for (int i = 0; i < 3; i++)
            {
                if (currentIndex_j + 1 <= 2)
                {
                    if (tile[i, currentIndex_j + 1].transform.position.x - transform.position.x ==  24)
                    {
                        tile[i, currentIndex_j + 1].transform.position += new Vector3(-24 * 3, 0, 0);
                    }
                }
                else
                {
                    if (tile[i, 0].transform.position.x - transform.position.x == 24)
                    {
                        tile[i, 0].transform.position += new Vector3(-24 * 3, 0, 0);
                    }
                }
            }
        }
        else if (135 <= angle || -135 >= angle) // 아래쪽 -135 ~ -180 or 135 ~ 180 
        {
            for (int i = 0; i < 3; i++)
            {
                if (currentIndex_i - 1 >= 0)
                {
                    if (tile[currentIndex_i - 1, i].transform.position.y - transform.position.y == 24)
                    {
                        tile[currentIndex_i - 1, i].transform.position += new Vector3(0, -24 * 3, 0);
                    }
                }
                else
                {
                    if (tile[2, i].transform.position.y - transform.position.y == 24)
                    {
                        tile[2, i].transform.position += new Vector3(0, -24 * 3, 0);
                    }
                }
            }
        }
        else if (-135 <= angle && angle <= -45) // 오른쪽
        {
            for (int i = 0; i < 3; i++)
            {
                if (currentIndex_j - 1 >= 0)
                {
                    if (tile[i, currentIndex_j - 1].transform.position.x - transform.position.x == -24)
                    {
                        tile[i, currentIndex_j - 1].transform.position += new Vector3(24 * 3, 0, 0);
                    }
                }
                else
                {
                    if (tile[i, 2].transform.position.x - transform.position.x == -24)
                    {
                        tile[i, 2].transform.position += new Vector3(24 * 3, 0, 0);
                    }
                }
            }
        }
    }
}
