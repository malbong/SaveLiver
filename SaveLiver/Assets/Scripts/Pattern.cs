using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pattern : MonoBehaviour
{
    public Enemy enemy;
    private Vector3 playerPosition;
    private float spawnRadius;
    private float angle45Length;
    public float interval = 2.0f;

    private void Start()
    {
        spawnRadius = SpawnManager.instance.radius;
        angle45Length = Mathf.Sqrt(Mathf.Pow(spawnRadius, 2) / 2.0f);

        StartCoroutine(tmp());
    }


    IEnumerator tmp()
    {
        AllDirection4();
        yield return new WaitForSeconds(3.0f);
        AllDirection8();
        yield return new WaitForSeconds(3.0f);
        DiagonalLeft();
        yield return new WaitForSeconds(3.0f);
        DiagonalRight();
        yield return new WaitForSeconds(3.0f);
        DiagonalBothSide();
    }


    public Enemy CreateLinearTurtle(Vector3 diffPosition, Vector3 targetPosition)
    {
        Vector3 createPosition = targetPosition + diffPosition;
        Quaternion rotation = SpawnManager.instance.GetAngleWithTargetFromY(createPosition, targetPosition);
        Enemy instance = Instantiate(enemy, createPosition, rotation);
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


    public void DiagonalLeft()
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


    public void DiagonalRight()
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


    public void DiagonalBothSide()
    {
        DiagonalLeft();
        DiagonalRight();
    }


    public void Swirl()
    {

    }


    public void Shark()
    {

    }


    public void MeetBoss()
    {
        //Scene
    }
}
