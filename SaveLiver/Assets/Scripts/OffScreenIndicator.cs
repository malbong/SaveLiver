using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OffScreenIndicator : MonoBehaviour
{
    public static OffScreenIndicator instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else if (instance != this)
        {
            instance.GetComponent<Canvas>().worldCamera = Camera.main;
            Destroy(this.gameObject);
        }
    }


    public Image screen;

    private Vector3 playerVec;
    private Vector3 playerScreenVec;
    private float upAngle;
    private float rightAngle;

    float playerToScreenTop;
    float playerToScreenBottom;
    float screenHalfWidth;


    void Start()
    {
        playerScreenVec = Camera.main.WorldToScreenPoint(Player.instance.transform.position);
        playerVec = Camera.main.WorldToViewportPoint(Player.instance.transform.position);
        playerToScreenTop = 1f - playerVec.y;
        playerToScreenBottom = playerVec.y;
        screenHalfWidth = 0.5f;

        // Up
        Vector3 vec = new Vector3(Screen.width, Screen.height, 10) - playerScreenVec;
        vec = vec.normalized;
        upAngle = Vector3.Angle(vec, Vector3.up);

        // Right
        Vector3 vecR = new Vector3(Screen.width, 0, 10) - playerScreenVec;
        vecR = vecR.normalized;
        rightAngle = Vector3.Angle(vecR, Vector3.up);
    }



    public void DrawIndicator(GameObject obj, GameObject indicatorObj)
    {
        Image indicator = indicatorObj.GetComponent<Image>();

        Vector3 objScreenVec = Camera.main.WorldToScreenPoint(obj.transform.position);
        Vector3 objVec = Camera.main.WorldToViewportPoint(obj.transform.position);

        Vector3 targetVec = objScreenVec - playerScreenVec;
        targetVec = targetVec.normalized;

        float targetAngle = Vector3.Angle(targetVec, Vector3.up);
        int sign = Vector3.Cross(targetVec, Vector3.up).z < 0 ? -1 : 1;
        targetAngle *= sign;
        
        float xPrime = objVec.x - playerVec.x;
        float yPrime = objVec.y - playerVec.y;

        float anchorMinX;
        float anchorMinY;
        float anchorMaxX;
        float anchorMaxY;

        if (upAngle < targetAngle && rightAngle >= targetAngle) // Right
        {
            anchorMinX = 0.965f;
            anchorMaxX = 0.965f;
            anchorMinY = 0.528f;
            anchorMaxY = 0.528f;
            
            float posY = (screenHalfWidth * Mathf.Abs(yPrime)) / xPrime;
            if (yPrime > 0)
            {
                anchorMinY = playerToScreenBottom + posY;
                anchorMaxY = playerToScreenBottom + posY;

                if (anchorMinY > 0.982f) anchorMinY = 0.982f;
                if (anchorMaxY > 0.982f) anchorMaxY = 0.982f;

                indicator.rectTransform.anchorMin = new Vector2(anchorMinX, anchorMinY);
                indicator.rectTransform.anchorMax = new Vector2(anchorMaxX, anchorMaxY);
            }
            else
            {
                anchorMinY = playerToScreenBottom - posY;
                anchorMaxY = playerToScreenBottom - posY;

                if (anchorMinY < 0.018f) anchorMinY = 0.018f;
                if (anchorMaxY < 0.018f) anchorMaxY = 0.018f;

                indicator.rectTransform.anchorMin = new Vector2(anchorMinX, anchorMinY);
                indicator.rectTransform.anchorMax = new Vector2(anchorMaxX, anchorMaxY);
                //indicator.rectTransform.anchoredPosition = new Vector3(0, -posY);
            }
        }
        else if (-upAngle < targetAngle && upAngle > targetAngle) // Up
        {
            anchorMinX = 0.5f;
            anchorMaxX = 0.5f;
            anchorMinY = 0.982f;
            anchorMaxY = 0.982f;

            float posX = (Mathf.Abs(xPrime) * playerToScreenTop) / yPrime;

            if (xPrime > 0) // Left
            {
                anchorMinX = screenHalfWidth + posX;
                anchorMaxX = screenHalfWidth + posX;

                if (anchorMinX > 0.965f) anchorMinX = 0.965f;
                if (anchorMaxX > 0.965f) anchorMaxX = 0.965f;

                indicator.rectTransform.anchorMin = new Vector2(anchorMinX, anchorMinY);
                indicator.rectTransform.anchorMax = new Vector2(anchorMaxX, anchorMaxY);
            }
            else // Right
            {
                anchorMinX = screenHalfWidth - posX;
                anchorMaxX = screenHalfWidth - posX;

                if (anchorMinX < 0.035f) anchorMinX = 0.035f;
                if (anchorMaxX < 0.035f) anchorMaxX = 0.035f;

                indicator.rectTransform.anchorMin = new Vector2(anchorMinX, anchorMinY);
                indicator.rectTransform.anchorMax = new Vector2(anchorMaxX, anchorMaxY);
            }
        }
        else if((-180 <= targetAngle && -rightAngle > targetAngle) || (rightAngle < targetAngle && 180 >= targetAngle)) // Down
        {
            anchorMinX = 0.5f;
            anchorMaxX = 0.5f;
            anchorMinY = 0.018f;
            anchorMaxY = 0.018f;

            float posX = (Mathf.Abs(xPrime) * playerToScreenBottom) / -yPrime;

            if (xPrime > 0)
            {
                anchorMinX = screenHalfWidth + posX;
                anchorMaxX = screenHalfWidth + posX;

                if (anchorMinX > 0.965f) anchorMinX = 0.965f;
                if (anchorMaxX > 0.965f) anchorMaxX = 0.965f;

                indicator.rectTransform.anchorMin = new Vector2(anchorMinX, anchorMinY);
                indicator.rectTransform.anchorMax = new Vector2(anchorMaxX, anchorMaxY);
            }
            else
            {
                anchorMinX = screenHalfWidth - posX;
                anchorMaxX = screenHalfWidth - posX;

                if (anchorMinX < 0.035f) anchorMinX = 0.035f;
                if (anchorMaxX < 0.035f) anchorMaxX = 0.035f;

                indicator.rectTransform.anchorMin = new Vector2(anchorMinX, anchorMinY);
                indicator.rectTransform.anchorMax = new Vector2(anchorMaxX, anchorMaxY);
            }
        }
        else if(-upAngle >= targetAngle && -rightAngle < targetAngle) // Left
        {
            anchorMinX = 0.035f;
            anchorMaxX = 0.035f;
            anchorMinY = 0.528f;
            anchorMaxY = 0.528f;

            float posY = (screenHalfWidth * Mathf.Abs(yPrime)) / -xPrime;
            
            if (yPrime > 0)
            {
                anchorMinY = playerToScreenBottom + posY;
                anchorMaxY = playerToScreenBottom + posY;

                if (anchorMinY > 0.982f) anchorMinY = 0.982f;
                if (anchorMaxY > 0.982f) anchorMaxY = 0.982f;

                indicator.rectTransform.anchorMin = new Vector2(anchorMinX, anchorMinY);
                indicator.rectTransform.anchorMax = new Vector2(anchorMaxX, anchorMaxY);
            }
            else
            {
                anchorMinY = playerToScreenBottom - posY;
                anchorMaxY = playerToScreenBottom - posY;

                if (anchorMinY < 0.018f) anchorMinY = 0.018f;
                if (anchorMaxY < 0.018f) anchorMaxY = 0.018f;

                indicator.rectTransform.anchorMin = new Vector2(anchorMinX, anchorMinY);
                indicator.rectTransform.anchorMax = new Vector2(anchorMaxX, anchorMaxY);
            }
        }
        indicator.rectTransform.anchoredPosition = new Vector3(0, 0);
    }
}
