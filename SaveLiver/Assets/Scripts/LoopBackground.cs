using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LoopBackground : MonoBehaviour
{
    public GameObject[] tmp_tile;
    public GameObject[,] tile;
    private int currentIndex_i;
    private int currentIndex_j;
    private string tmpStringIndex;


    private void Start()
    {
        tile = new GameObject[3, 3];
        int cnt = 0;
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                tile[i, j] = tmp_tile[cnt];
                cnt += 1;
            }
        }
        tmpStringIndex = this.name;
        currentIndex_i = int.Parse(tmpStringIndex[0].ToString());
        currentIndex_j = int.Parse(tmpStringIndex[1].ToString());
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag != "MoveCollider") return;
        
        Vector3 dir = other.transform.position - transform.position;
        float angle = Vector3.Angle(transform.up, dir);
        int sign = Vector3.Cross(transform.up, dir).z < 0 ? -1 : 1;
        angle *= sign;
        Debug.Log(angle);
        if (-45 <= angle && angle <= 45)
        {
            for (int i = 0; i < 3; i++)
            {
                
                if (currentIndex_i + 1 <= 2)
                {
                    if (tile[currentIndex_i + 1, i].transform.position.y - transform.position.y == -24)
                    {
                        tile[currentIndex_i + 1, i].transform.position += new Vector3(0, 24 * 3, 0);
                    }
                }
                else
                {
                    if (tile[0, i].transform.position.y - transform.position.y == -24)
                    {
                        tile[0, i].transform.position += new Vector3(0, 24 * 3, 0);
                    }
                }
            }
        }
        else if (45 <= angle && angle <= 135)
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
        else if (135 <= angle || -135 >= angle) // -135 ~ -180 or 135 ~ 180
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
        else if (-135 <= angle && angle <= -45)
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
