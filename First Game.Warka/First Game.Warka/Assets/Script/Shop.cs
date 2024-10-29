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
    public event BuySeconPosition buySeconPosition;
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
                    boughtTexts[i].text = "Купленно";

                    if (i == 2) buySeconPosition.Invoke();                                                               
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
            if (PlayerPrefs.GetInt("Position" + i) == 1) break;
            if (Player.instance.currentMoney < prises[i])
            {
                buyButtons[i].interactable = false;
                boughtTexts[i].text = "Мало Монет";
            }
            else
            {
                buyButtons[i].interactable = true;
                boughtTexts[i].text = "Купить";
            }
        }
    }
    public void Buy(int index)
    {
        
       
        buyButtons[index].interactable = false;
        boughtTexts[index].text = "Купленно";

        PlayerPrefs.SetInt("Position" + index, 1);

        if (index == 2) buySeconPosition.Invoke();
     }
    [ContextMenu("Delete Player Prefs")]
    void DeletePlayerPrefs() => PlayerPrefs.DeleteAll();
    
        
    
}
