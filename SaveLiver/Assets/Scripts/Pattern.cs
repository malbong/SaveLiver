using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pattern : MonoBehaviour
{
    public TurtleLinear linearTutle;
    public Dragon dragon;
    public Swirl swirl;
    private Vector3 playerPosition;
    private float spawnRadius;
    private float angle45Length;

    private void Start()
    {
        spawnRadius = SpawnManager.instance.radius;
        angle45Length = Mathf.Sqrt(Mathf.Pow(spawnRadius, 2) / 2.0f);

        StartCoroutine(tmp());
    }


    IEnumerator tmp()
    {
        yield return new WaitForSeconds(3.0f);
        Swirl(-250f, 3, true);
        yield return new WaitForSeconds(3.0f);
        Swirl(-250f);
        
        AllDirection4();
        Dragon(-1, 1, 2.5f);
        yield return new WaitForSeconds(3.0f);
        AllDirection8();
        Dragon(1, 1, 2f);
        yield return new WaitForSeconds(3.0f);
        Dragon(1, -1, 2f);
        DiagonalLeft(2f);
        yield return new WaitForSeconds(3.0f);
        Dragon(-1, -1, 2.5f);
        DiagonalRight(2f);
        yield return new WaitForSeconds(3.0f);
        DiagonalBothSide(2f);
        
    }


    public Enemy CreateLinearTurtle(Vector3 diffPosition, Vector3 targetPosition)
    {
        Vector3 createPosition = targetPosition + diffPosition;
        Quaternion rotation = SpawnManager.instance.GetAngleWithTargetFromY(createPosition, targetPosition);
        Enemy instance = Instantiate(linearTutle, createPosition, rotation);
        return instance;
    }


    public void AllDirection4()
    {
        playerPosition = Player.instance.transform.position;

        Vector3 diffPosition = new Vector3(0, spawnRadius, 0);
        CreateLinearTurtle(diffPosition, playerPosition);

        diffPosition = new Vector3(0, -spawnRadius, 0);
        CreateLinearTurtle(diffPosition, playerPosition);

        diffPosition = new Vector3(spawnRadius, 0, 0);
        CreateLinearTurtle(diffPosition, playerPosition);

        diffPosition = new Vector3(-spawnRadius, 0, 0);
        CreateLinearTurtle(diffPosition, playerPosition);
    }


    public void AllDirection8()
    {
        AllDirection4();

        playerPosition = Player.instance.transform.position;

        Vector3 diffPosition = new Vector3(angle45Length, angle45Length, 0);
        CreateLinearTurtle(diffPosition, playerPosition);

        diffPosition = new Vector3(-angle45Length, angle45Length, 0);
        CreateLinearTurtle(diffPosition, playerPosition);

        diffPosition = new Vector3(-angle45Length, -angle45Length, 0);
        CreateLinearTurtle(diffPosition, playerPosition);

        diffPosition = new Vector3(angle45Length, -angle45Length, 0);
        CreateLinearTurtle(diffPosition, playerPosition);
    }


    public void DiagonalLeft(float interval)
    {
        playerPosition = Player.instance.transform.position;
        
        Vector3 targetPosition = playerPosition;
        Vector3 diffPosition = new Vector3(-angle45Length, angle45Length, 0);
        CreateLinearTurtle(diffPosition, targetPosition);
        
        targetPosition = playerPosition + new Vector3(0, interval, 0);
        diffPosition = new Vector3(-angle45Length, angle45Length, 0);
        CreateLinearTurtle(diffPosition, targetPosition);

        targetPosition = playerPosition + new Vector3(0, -interval, 0);
        diffPosition = new Vector3(-angle45Length, angle45Length, 0);
        CreateLinearTurtle(diffPosition, targetPosition);
    }


    public void DiagonalRight(float interval)
    {
        playerPosition = Player.instance.transform.position;

        Vector3 targetPosition = playerPosition;
        Vector3 diffPosition = new Vector3(angle45Length, angle45Length, 0);
        CreateLinearTurtle(diffPosition, targetPosition);

        targetPosition = playerPosition + new Vector3(0, interval, 0);
        diffPosition = new Vector3(angle45Length, angle45Length, 0);
        CreateLinearTurtle(diffPosition, targetPosition);

        targetPosition = playerPosition + new Vector3(0, -interval, 0);
        diffPosition = new Vector3(angle45Length, angle45Length, 0);
        CreateLinearTurtle(diffPosition, targetPosition);
    }


    public void DiagonalBothSide(float interval)
    {
        DiagonalLeft(interval);
        DiagonalRight(interval);
    }


    public void Swirl(float maxForce, float interval = 0, bool upDown = false)
    {
        playerPosition = Player.instance.transform.position;
        if (upDown == true)
        {
            Vector3 targetPosition = playerPosition + new Vector3(0, interval, 0);
            Swirl instance = Instantiate(swirl, targetPosition, Quaternion.identity);
            instance.maxForce = maxForce;
            
            targetPosition = playerPosition + new Vector3(0, -interval, 0);
            instance = Instantiate(swirl, targetPosition, Quaternion.identity);
            instance.maxForce = maxForce;
        }
        else
        {
            Swirl instance = Instantiate(swirl, playerPosition, Quaternion.identity);
            instance.maxForce = maxForce;
        }
        
    }
    

    public void Dragon(int dir, int isOver, float interval)
    {
        playerPosition = Player.instance.transform.position;
        if (dir == 1) // create right 
        {
            if (isOver == 1) // create over player
            {
                Vector3 createPosition = playerPosition + new Vector3(25, interval, 0);
                Dragon instance = Instantiate(dragon, createPosition, Quaternion.identity);
            }
            else // create under player
            {
                Vector3 createPosition = playerPosition + new Vector3(25, -interval, 0);
                Dragon instance = Instantiate(dragon, createPosition, Quaternion.identity);
            }
        }
        else // create left
        {
            if (isOver == 1) // create over player
            {
                Vector3 createPosition = playerPosition + new Vector3(-25, interval, 0);
                Dragon instance = Instantiate(dragon, createPosition, Quaternion.identity);
                instance.transform.Rotate(0, 180, 0);
            }
            else // create under player
            {
                Vector3 createPosition = playerPosition + new Vector3(-25, -interval, 0);
                Dragon instance = Instantiate(dragon, createPosition, Quaternion.identity);
                instance.transform.Rotate(0, 180, 0);
            }
        }
    }
    

    public void MeetBoss()
    {
        //Scene
    }
}
