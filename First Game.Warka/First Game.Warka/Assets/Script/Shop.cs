using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class Shop : MonoBehaviour
{
    [SerializeField] Button[] buyButtons;
    [SerializeField] TextMeshProUGUI[] boughtTexts;
    [SerializeField] int[] prises;
    [SerializeField] GameObject shopPanel;
    public delegate void BuySeconPosition();
    public delegate void BuySeconPositionDash();

    public event BuySeconPosition buySeconPosition;
    public event BuySeconPositionDash buySeconPositionDash;

    public static Shop instance;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        for (int i = 0; i < buyButtons.Length; i++)
        {
            if (!PlayerPrefs.HasKey("Position" + i))
            {
                PlayerPrefs.SetInt("Position" + i, 0);
            }
            else 
            {
                if (PlayerPrefs.GetInt("Position" + i) == 1)
                {
                    buyButtons[i].interactable = false;
                    boughtTexts[i].text = "��������";

                    if (i == 2) buySeconPosition.Invoke();
                    if (i == 4) buySeconPositionDash.Invoke();

                }
            }
        }
        Check();
    }
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.E))
        {
            shopPanel.SetActive(!shopPanel.activeInHierarchy); 
            Check();

            if (shopPanel.activeInHierarchy) Time.timeScale = 0;
            else Time.timeScale = 1;

        }
    }

    void Check()
    {
        for (int i = 0; i < buyButtons.Length; i++)
        {
            if (Player.instance.currentMoney < prises[i])
            {
                buyButtons[i].interactable = false;
                boughtTexts[i].text = "���� �����";
            }
            else
            {
                buyButtons[i].interactable = true;
                boughtTexts[i].text = "������";
            }
            if (PlayerPrefs.GetInt("Position" + i) == 1)
            {
                buyButtons[i].interactable = false;
                boughtTexts[i].text = "��������";
            }

        }
    }
    public void Buy(int index)
    {
        
       
        buyButtons[index].interactable = false;
        boughtTexts[index].text = "��������";

        PlayerPrefs.SetInt("Position" + index, 1);

        if (index == 2) buySeconPosition.Invoke();
        if (index == 4) buySeconPositionDash.Invoke();
        Player.instance.AddMoney(- prises[index]);
        Check();
    }
    [ContextMenu("Delete Player Prefs")]
    void DeletePlayerPrefs() => PlayerPrefs.DeleteAll();
    
        
    
}
