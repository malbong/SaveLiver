using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreManager : MonoBehaviour
{
    public bool seeingStore = false;
    public MenuManager menuManager;

    public Sprite[] boatSprites;
    public Sprite[] faceSprites;
    public Material[] waveMaterials;

    public SpriteRenderer boatImage;
    public SpriteRenderer faceImage;
    public ParticleSystemRenderer waveLeftParticle;
    public ParticleSystemRenderer waveRightParticle;

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
    public Sprite storeLoading;
    public Sprite nullSprite;

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

    public int[] faceSoulPrice = { 0, 500, 1000, 2000, 5000 };
    public int[] boatSoulPrice = { 0, 500, 1000, 2000, 5000 };
    public int[] waveSoulPrice = { 0, 100, 100, 100, 100 };

    public ParticleSystem[] waveParticle;

    private bool skipRunning = false;

    private enum PanelState { Face, Boat, Wave }
    private PanelState panelState;

    public GameObject shipPanel;


    void Start()
    {
        InitStoreAsync();
        InitFaceCharge();
        InitBoatCharge();
        InitWaveCharge();
        UpdateFaceSprite();
        UpdateBoatSprite();
        UpdateWaveSprite();
        panelState = PanelState.Face;
    }


    void Update()
    {
        // Store가 열려있을 때 CurrentCustom을 계속 추적함
        if (seeingStore)
        {
            UpdateCurrentCustom();
        }
    }


    public void PlayAllWave()
    {
        for(int i=0; i<waveParticle.Length; i++)
        {
            waveParticle[i].Play();
        }
    }


    public void StopAllWave()
    {
        for(int i=0; i<waveParticle.Length; i++)
        {
            waveParticle[i].Stop();
        }
    }


    private void ActiveFalseWave()
    {
        for (int i = 2; i < waveParticle.Length; i++)
        {
            waveParticle[i].gameObject.SetActive(false);
        }
    }


    private void ActiveTrueWave()
    {
        for(int i=2; i<waveParticle.Length; i++)
        {
            waveParticle[i].gameObject.SetActive(true);
        }
    }


    public void OnBtnPanelState(int onClickPanel) // onClickPanel - 0:face, 1:boat, 2:wave
    {
        SoundManager.instance.ButtonClick();
        switch (onClickPanel)
        {
            case 0:
                panelState = PanelState.Face;
                facePanel.SetActive(true);
                if(boatPanel.activeInHierarchy) boatPanel.SetActive(false);
                if(wavePanel.activeInHierarchy) wavePanel.SetActive(false);
                UpdateFaceSprite();
                InitFaceCharge();
                break;
            case 1:
                panelState = PanelState.Boat;
                if (facePanel.activeInHierarchy) facePanel.SetActive(false);
                boatPanel.SetActive(true);
                if (wavePanel.activeInHierarchy) wavePanel.SetActive(false);
                UpdateBoatSprite();
                InitBoatCharge();
                break;
            case 2:
                panelState = PanelState.Wave;
                if (facePanel.activeInHierarchy) facePanel.SetActive(false);
                if (boatPanel.activeInHierarchy) boatPanel.SetActive(false);
                wavePanel.SetActive(true);
                UpdateWaveSprite();
                InitWaveCharge();
                break;
        }
    }


    public void Skip(int arrow) // arrow - 0:Left, 1:Right
    {
        if (skipRunning) return; // skip중이면 무시

        SoundManager.instance.ButtonClick();

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
                else
                {
                    StartCoroutine(SkipLeft(wavePanel));
                }
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
                else
                {
                    StartCoroutine(SkipRight(wavePanel));
                }
                listText.text = waveIndex + 1 + " / " + waveCount;
            }
        }
    }


    IEnumerator SkipLeft(GameObject panel)
    {
        skipRunning = true;
        if (panelState == PanelState.Wave) ActiveFalseWave();
        for (int i=0; i<9; i++)
        {
            panel.transform.localPosition += new Vector3(100, 0, 0);
            yield return new WaitForSeconds(0.01f);
        }
        if (panelState == PanelState.Wave) ActiveTrueWave();
        skipRunning = false;
    }


    IEnumerator SkipRight(GameObject panel)
    {
        skipRunning = true;
        if (panelState == PanelState.Wave) ActiveFalseWave();
        for (int i=0; i<9; i++)
        {
            panel.transform.localPosition -= new Vector3(100, 0, 0);
            yield return new WaitForSeconds(0.01f);
        }
        if (panelState == PanelState.Wave) ActiveTrueWave();
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
        PlayerInformation.customs = DatabaseManager.GetCurrentCustom();
        // customs[0] : boat, 1 : face, 2: wave
    }


    public void InitBoatCharge()
    {
        for (int i = 0; i < boatChargeList.Length; i++)
        {
            boatChargeList[i] = DatabaseManager.BoatCharge(i);
        }
        UpdateLock();
    }


    public void InitFaceCharge()
    {
        for (int i = 0; i < faceChargeList.Length; i++)
        {
            faceChargeList[i] = DatabaseManager.FaceCharge(i);
        }
        UpdateLock();
    }


    public void InitWaveCharge()
    {
        for (int i = 0; i < waveChargeList.Length; i++)
        {
            waveChargeList[i] = DatabaseManager.WaveCharge(i);
        }
        UpdateLock();
    }

    
    public void UpdateCurrentCustom()
    {
        //boat
        

        if (PlayerInformation.customs[0] == 0)
        {
            boatImage.sprite = boatSprites[0];
            boatImage.transform.localScale = new Vector3(400, 400, 1);
            shipPanel.SetActive(false);
        }
        else if(PlayerInformation.customs[0] == 1)
        {
            boatImage.sprite = boatSprites[1];
            boatImage.transform.localScale = new Vector3(400, 400, 1);
            shipPanel.SetActive(false);
        }
        else if (PlayerInformation.customs[0] == 2)
        {
            boatImage.sprite = boatSprites[2];
            boatImage.transform.localScale = new Vector3(300, 300, 1);
            shipPanel.SetActive(false);
        }
        else if (PlayerInformation.customs[0] == 3)
        {
            boatImage.sprite = boatSprites[3];
            boatImage.transform.localScale = new Vector3(400, 400, 1);
            shipPanel.SetActive(false);
        }
        else if (PlayerInformation.customs[0] == 4)
        {
            boatImage.sprite = boatSprites[4];
            boatImage.transform.localScale = new Vector3(170, 130, 1);
            shipPanel.SetActive(true);
        }


        // face
        if (PlayerInformation.customs[0] == 4) //boat가 ship일때 face크기 조정
        {
            faceImage.transform.localScale = new Vector3(200, 200, 1);
        }
        else
        {
            faceImage.transform.localScale = new Vector3(400, 400, 1);
        }

        if (PlayerInformation.customs[1] == 0)
        {
            faceImage.sprite = faceSprites[0];
        }
        else if(PlayerInformation.customs[1] == 1)
        {
            faceImage.sprite = faceSprites[1];
        }
        else if (PlayerInformation.customs[1] == 2)
        {
            
        }
        else if (PlayerInformation.customs[1] == 3)
        {
            
        }
        else if (PlayerInformation.customs[1] == 4)
        {
            
        }


        // wave
        if (PlayerInformation.customs[2] == 0)
        {
            waveLeftParticle.trailMaterial = waveMaterials[0];
            waveRightParticle.trailMaterial = waveMaterials[0];
        }
        else if (PlayerInformation.customs[2] == 1)
        {
            waveLeftParticle.trailMaterial = waveMaterials[1];
            waveRightParticle.trailMaterial = waveMaterials[1];
        }
        else if (PlayerInformation.customs[2] == 2)
        {
            waveLeftParticle.trailMaterial = waveMaterials[2];
            waveRightParticle.trailMaterial = waveMaterials[2];
        }
        else if (PlayerInformation.customs[2] == 3)
        {
            waveLeftParticle.trailMaterial = waveMaterials[3];
            waveRightParticle.trailMaterial = waveMaterials[3];
        }
        else if (PlayerInformation.customs[2] == 4)
        {
            waveLeftParticle.trailMaterial = waveMaterials[4];
            waveRightParticle.trailMaterial = waveMaterials[4];
        }
    }

    
    public void UpdateLock()
    {
        CheckEquip();
        for (int i=0; i<boatCount; i++)
        {
            if (boatChargeList[i] == -1) boatLockImage[i].sprite = lockImage;
            else if (boatChargeList[i] == 0) // 샀는데 장착 안한거
            {
                boatLockImage[i].sprite = nullSprite;
                boatPrice[i].text = "Purchased";
                boatPrice[i].color = new Color32(255, 0, 0, 255);
            }
            else if (boatChargeList[i] == 1) 
            {
                boatLockImage[i].sprite = checkImage;
                boatPrice[i].text = "Purchased";
                boatPrice[i].color = new Color32(255, 0, 0, 255);
            }
            else boatLockImage[i].sprite = storeLoading;
        }
        
        for (int i = 0; i < faceCount; i++)
        {
            if (faceChargeList[i] == -1) faceLockImage[i].sprite = lockImage;
            else if (faceChargeList[i] == 0)
            {
                faceLockImage[i].sprite = nullSprite;
                facePrice[i].text = "Purchased";
                facePrice[i].color = new Color32(255, 0, 0, 255);
            }
            else if (faceChargeList[i] == 1)
            {
                faceLockImage[i].sprite = checkImage;
                facePrice[i].text = "Purchased";
                facePrice[i].color = new Color32(255, 0, 0, 255);
            }
            else faceLockImage[i].sprite = storeLoading;
        }

        for (int i = 0; i < waveCount; i++)
        {
            if (waveChargeList[i] == -1) waveLockImage[i].sprite = lockImage;
            else if (waveChargeList[i] == 0)
            {
                waveLockImage[i].sprite = nullSprite;
                wavePrice[i].text = "Purchased";
                wavePrice[i].color = new Color32(255, 0, 0, 255);
            }
            else if (waveChargeList[i] == 1)
            {
                waveLockImage[i].sprite = checkImage;
                wavePrice[i].text = "Purchased";
                wavePrice[i].color = new Color32(255, 0, 0, 255);
            }
            else waveLockImage[i].sprite = storeLoading;
        }
    }


    public void OnBtnEquip()
    {
        if (skipRunning) return;

        SoundManager.instance.ButtonClick();

        if (panelState == PanelState.Face)
        {
            for(int i=0; i<faceCount; i++)
            {
                if(faceIndex == i)
                {
                    if(faceChargeList[i] == -1) // 구매 X
                    {
                        menuManager.OnBtnChargePanel(); // 구매 패널 띄우기
                    }
                    else if(faceChargeList[i] == 0) // 구매 O, 장착 X
                    {
                        faceChargeList[i] = 1;
                        PlayerInformation.customs[1] = i; // 장착하기
                    }
                    break;
                }
            }
        }
        else if(panelState == PanelState.Boat)
        {
            for (int i=0; i<boatCount; i++)
            {
                if (boatIndex == i)
                {
                    if (boatChargeList[i] == -1) // 구매 X
                    {
                        menuManager.OnBtnChargePanel();
                    }
                    else if (boatChargeList[i] == 0) // 구매 O, 장착 X
                    {
                        boatChargeList[i] = 1;
                        PlayerInformation.customs[0] = i;
                    }
                    break;
                }
            }
        }
        else if(panelState == PanelState.Wave)
        {
            for (int i=0; i < waveCount; i++)
            {
                if (waveIndex == i)
                {
                    if (waveChargeList[i] == -1) // 구매 X
                    {
                        menuManager.OnBtnChargePanel();
                    }
                    else if (waveChargeList[i] == 0) // 구매 O, 장착 X
                    {
                        waveChargeList[i] = 1;
                        PlayerInformation.customs[2] = i;
                    }
                    break;
                }
            }
        }
        DatabaseManager.SetCurrentCustom(PlayerInformation.customs);
        UpdateLock();
    }
    

    public void CheckEquip()
    {
        int boat = PlayerInformation.customs[0];
        int face = PlayerInformation.customs[1];
        int wave = PlayerInformation.customs[2];

        for(int i=0; i<faceCount; i++)
        {
            if(faceChargeList[i] == 1 && face != i)
            {
                faceChargeList[i] = 0;
            }
        }

        for (int i = 0; i < boatCount; i++)
        {
            if (boatChargeList[i] == 1 && boat != i)
            {
                boatChargeList[i] = 0;
            }
        }

        for (int i = 0; i < waveCount; i++)
        {
            if (waveChargeList[i] == 1 && wave != i)
            {
                waveChargeList[i] = 0;
            }
        }
    }


    public void OnBtnPurchasing()
    {
        if (!menuManager.seeingChargePanel) return;

        SoundManager.instance.ButtonClick();

        if (panelState == PanelState.Face)
        {
            for (int i = 0; i < faceCount; i++)
            {
                if (faceIndex == i)
                {
                    if(PlayerInformation.SoulMoney > faceSoulPrice[i])
                    {
                        DatabaseManager.SetChargeNewData("face", i);
                        InitFaceCharge();
                        faceChargeList[i] = 0;
                        UpdateLock();
                        menuManager.OnBtnChargeNo();
                        PlayerInformation.SoulMoney -= faceSoulPrice[i];
                        DatabaseManager.UpdateMoney(-faceSoulPrice[i]);
                    }
                    else
                    {
                        menuManager.ChargeText.text = "Don't have" + "\nenough money";
                    }
                    break;
                }
            }
        }
        else if (panelState == PanelState.Boat)
        {
            for (int i = 0; i < boatCount; i++)
            {
                if (boatIndex == i)
                {
                    if (PlayerInformation.SoulMoney > boatSoulPrice[i])
                    {
                        DatabaseManager.SetChargeNewData("boat", i);
                        InitBoatCharge();
                        boatChargeList[i] = 0;
                        UpdateLock();
                        menuManager.OnBtnChargeNo();
                        PlayerInformation.SoulMoney -= boatSoulPrice[i];
                        DatabaseManager.UpdateMoney(-boatSoulPrice[i]);
                    }
                    else
                    {
                        menuManager.ChargeText.text = "Don't have" + "\nenough money";
                    }
                    break;
                }
            }
        }
        else if (panelState == PanelState.Wave)
        {
            for (int i = 0; i < waveCount; i++)
            {
                if (waveIndex == i)
                {
                    if (PlayerInformation.SoulMoney > waveSoulPrice[i])
                    {
                        DatabaseManager.SetChargeNewData("wave", i);
                        InitWaveCharge();
                        waveChargeList[i] = 0;
                        UpdateLock();
                        menuManager.OnBtnChargeNo();
                        PlayerInformation.SoulMoney -= waveSoulPrice[i];
                        DatabaseManager.UpdateMoney(-waveSoulPrice[i]);
                    }
                    else
                    {
                        menuManager.ChargeText.text = "Don't have" + "\nenough money";
                    }
                    break;
                }
            }
        }
    }
}