using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreManager : MonoBehaviour
{
    public Sprite[] faceSprites;
    public Sprite[] boatSprites;
    public Sprite[] waveSprites;

    public GameObject facePanel;
    public GameObject boatPanel;
    public GameObject wavePanel;

    private bool lockSprite = false;

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
        panelState = PanelState.Face;
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
}
