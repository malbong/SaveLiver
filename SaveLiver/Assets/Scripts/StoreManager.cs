using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreManager : MonoBehaviour
{
    public bool seeingStore = false;
    public DatabaseManager databaseManager;

    public Sprite[] boatSprites;
    public Sprite[] faceSprites;
    public Sprite[] waveSprites;

    public Image boatImage;
    public Image faceImage;

    public int[] customs = { 0, 0, 0 };
    public int[] boatChargeList = { 2, 2, 2, 2, 2 };
    public int[] faceChargeList = { 2, 2, 2, 2, 2 };
    public int[] waveChargeList = { 2, 2, 2, 2, 2 };
    public Image[] boatLockImage;
    public Image[] faceLockImage;
    public Image[] waveLockImage;
    public Sprite lockImage;
    public Sprite checkImage;
    public Text[] boatPrice;
    public Text[] facePrice;
    public Text[] wavePrice;

    public GameObject facePanel;
    public GameObject boatPanel;
    public GameObject wavePanel;

    private int faceIndex = 0;
    private int faceCount = 5;
    private int boatIndex = 0;
    private int boatCount = 5;
    private int waveIndex = 0;
    private int waveCount = 5;

    public Text listText;

    

    private bool skipRunning = false;

    private enum PanelState { Face, Boat, Wave }
    private PanelState panelState;


    void Start()
    {
        InitStoreAsync();
        panelState = PanelState.Face;
    }


    void Update()
    {
        // Store가 열려있을 때 CurrentCustom을 계속 추적함
        if (seeingStore)
        {
            UpdateCurrentCustom();
            //UpdateLock();
            //Debug.Log(boatChargeList[0] + " " + boatChargeList[1] + " " + boatChargeList[2] + " " + boatChargeList[3] + " " + boatChargeList[4]);
        }
    }


    public void OnBtnPanelState(int onClickPanel) // onClickPanel - 0:face, 1:boat, 2:wave
    {
        switch (onClickPanel)
        {
            case 0:
                panelState = PanelState.Face;
                facePanel.SetActive(true);
                if(boatPanel.activeInHierarchy) boatPanel.SetActive(false);
                if(wavePanel.activeInHierarchy) wavePanel.SetActive(false);
                UpdateFaceSprite();
                break;
            case 1:
                panelState = PanelState.Boat;
                if (facePanel.activeInHierarchy) facePanel.SetActive(false);
                boatPanel.SetActive(true);
                if (wavePanel.activeInHierarchy) wavePanel.SetActive(false);
                UpdateBoatSprite();
                break;
            case 2:
                panelState = PanelState.Wave;
                if (facePanel.activeInHierarchy) facePanel.SetActive(false);
                if (boatPanel.activeInHierarchy) boatPanel.SetActive(false);
                wavePanel.SetActive(true);
                UpdateWaveSprite();
                break;
        }
    }


    public void Skip(int arrow) // arrow - 0:Left, 1:Right
    {
        if (skipRunning) return; // skip중이면 무시

        // Left Skip
        if (arrow == 0)
        {
            if (panelState == PanelState.Face)
            {
                faceIndex -= 1;
                if (faceIndex < 0) faceIndex = 0;
                else StartCoroutine(SkipLeft(facePanel));
                listText.text = faceIndex + 1 + " / " + faceCount;
            }
            else if (panelState == PanelState.Boat)
            {
                boatIndex -= 1;
                if (boatIndex < 0) boatIndex = 0;
                else StartCoroutine(SkipLeft(boatPanel));
                listText.text = boatIndex + 1 + " / " + boatCount;
            }
            else
            {
                waveIndex -= 1;
                if (waveIndex < 0) waveIndex = 0;
                else StartCoroutine(SkipLeft(wavePanel));
                listText.text = waveIndex + 1 + " / " + waveCount;
            }
        }
        // Right Skip
        else
        {
            if (panelState == PanelState.Face)
            {
                faceIndex += 1;
                if (faceIndex > faceCount - 1) faceIndex = faceCount - 1;
                else StartCoroutine(SkipRight(facePanel));
                listText.text = faceIndex + 1 + " / " + faceCount;
            }
            else if (panelState == PanelState.Boat)
            {
                boatIndex += 1;
                if (boatIndex > boatCount - 1) boatIndex = boatCount - 1;
                else StartCoroutine(SkipRight(boatPanel));
                listText.text = boatIndex + 1 + " / " + boatCount;
            }
            else
            {
                waveIndex += 1;
                if (waveIndex > waveCount - 1) waveIndex = waveCount - 1;
                else StartCoroutine(SkipRight(wavePanel));
                listText.text = waveIndex + 1 + " / " + waveCount;
            }
        }
    }


    IEnumerator SkipLeft(GameObject panel)
    {
        skipRunning = true;
        for(int i=0; i<9; i++)
        {
            panel.transform.localPosition += new Vector3(100, 0, 0);
            yield return new WaitForSeconds(0.01f);
        }
        skipRunning = false;
    }


    IEnumerator SkipRight(GameObject panel)
    {
        skipRunning = true;
        for (int i=0; i<9; i++)
        {
            panel.transform.localPosition -= new Vector3(100, 0, 0);
            yield return new WaitForSeconds(0.01f);
        }
        skipRunning = false;
    }


    private void UpdateFaceSprite()
    {
        listText.text = faceIndex + 1 + " / " + faceCount;

    }


    private void UpdateBoatSprite()
    {
        listText.text = boatIndex + 1 + " / " + boatCount;

    }


    private void UpdateWaveSprite()
    {
        listText.text = waveIndex + 1 + " / " + waveCount;

    }


    public void InitStoreAsync()
    {
        customs = databaseManager.GetCurrentCustom();
        // customs[0] : boat, 1 : face, 2: wave

        /*
        for(int i=0; i<boatChargeList.Length; i++)
        {
            boatChargeList[i] = databaseManager.GetChargeList("boat", i);
        }
        for (int i = 0; i < faceChargeList.Length; i++)
        {
            faceChargeList[i] = databaseManager.GetChargeList("face", i);
        }
        for (int i = 0; i < waveChargeList.Length; i++)
        {
            waveChargeList[i] = databaseManager.GetChargeList("wave", i);
        }
        */
    }


    public Text tmp;
    public void UpdateCurrentCustom()
    {
        // boat
        if (customs[0] == 0)
        {
            boatImage.sprite = boatSprites[0];
        }
        else if(customs[0] == 1)
        {

        }
        else if (customs[0] == 2)
        {

        }
        else if (customs[0] == 3)
        {

        }
        else if (customs[0] == 4)
        {

        }


        // face
        if (customs[1] == 0)
        {
            faceImage.sprite = faceSprites[0];
        }
        else if(customs[1] == 1)
        {
            faceImage.sprite = faceSprites[1];
        }
        else if (customs[1] == 2)
        {
            
        }
        else if (customs[1] == 3)
        {
            
        }
        else if (customs[1] == 4)
        {
            
        }


        // wave
        if (customs[2] == 0)
        {
            // default wave particle
        }
        else if (customs[2] == 1)
        {
            
        }
        else if (customs[2] == 2)
        {
            
        }
        else if (customs[2] == 3)
        {
            
        }
        else if (customs[2] == 4)
        {
            
        }
    }

    /*
    public void UpdateLock()
    {
        for(int i=0; i<boatLockImage.Length; i++)
        {
            if (boatChargeList[i] == -1) boatLockImage[i].sprite = lockImage;
            else if (boatChargeList[i] == 0) boatLockImage[i].sprite = null;
            else boatLockImage[i].sprite = checkImage;
        }

        for (int i = 0; i < faceLockImage.Length; i++)
        {
            if (faceChargeList[i] == -1) faceLockImage[i].sprite = lockImage;
            else if (faceChargeList[i] == 0) faceLockImage[i].sprite = null;
            else faceLockImage[i].sprite = checkImage;
        }

        for (int i = 0; i < waveLockImage.Length; i++)
        {
            if (waveChargeList[i] == -1) waveLockImage[i].sprite = lockImage;
            else if (waveChargeList[i] == 0) waveLockImage[i].sprite = null;
            else waveLockImage[i].sprite = checkImage;
        }
    }
    */


    public void OnBtnEquip()
    {
        if (skipRunning) return;

        CheckPurchasingAndEquip(panelState, faceIndex);

        if (panelState == PanelState.Face)
        {
            
            if(faceIndex == 0)
            {

            }
            else if(faceIndex == 1)
            {

            }
            else if (faceIndex == 2)
            {

            }
            else if (faceIndex == 3)
            {

            }
            else if (faceIndex == 4)
            {

            }
        }
        else if(panelState == PanelState.Boat)
        {
            if(boatIndex == 0)
            {

            }
            else if(boatIndex == 1)
            {

            }
            else if (boatIndex == 2)
            {

            }
            else if (boatIndex == 3)
            {

            }
            else if (boatIndex == 4)
            {

            }
        }
        else if(panelState == PanelState.Wave)
        {
            if(waveIndex == 0)
            {

            }
            else if(waveIndex == 1)
            {

            }
            else if (waveIndex == 2)
            {

            }
            else if (waveIndex == 3)
            {

            }
            else if (waveIndex == 4)
            {

            }
        }
    }

    
    private int CheckPurchasingAndEquip(PanelState panelState, int index)
    {
        int equipOrPurchase = 0;

        // 데이터베이스에서 정보 가져오기

        return equipOrPurchase;
    }
}
