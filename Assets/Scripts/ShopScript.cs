using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvents;
using UnityEngine.UI;

public class ShopScript : MonoBehaviour
{
    public bool[] purchasedCloths = new bool[7]; //list of all clothes ever bought
    public GameObject purchaseQuestion;
    public GameObject NoMoneyPopup;
    public Text notification;
    int clothingInQuestion;

    private void Awake()
    {
        if (GameManager.IsNewGame)
        {
            SaveData.current.ClothingUnlockData = purchasedCloths;
        }
    }

    public void BuyItem(ItemObject item)
    {
        Debug.Log("Buy button pressed");
        if (GameManager.MoneyInPurse >= item.price)
        {
            GameManager.MoneyInPurse -= item.price;
            GameEventManager.Raise(new MoneyUpdate());
            GameEventManager.Raise(new ItemBoughtEvent(item));
        }
        else
        {
            NotEnoughMoney();
        }
    }

    public void BuyClothes(int clothingPiece)
    {
        if (purchaseQuestion.activeSelf)
        {
            if (clothingPiece == 9) { clothingPiece = clothingInQuestion; }

            if (!purchasedCloths[clothingPiece])
            {
                
                switch (clothingPiece)
                {
                    case 0:
                        GameEventManager.Raise(new ClothingChangeEvent(clothingPiece));
                        break;
                    case 1:
                        if (GameManager.MoneyInPurse >= 60){ GameManager.MoneyInPurse -= 60; 
                        GameEventManager.Raise(new MoneyUpdate());
                        purchasedCloths[clothingPiece] = true;
                        SaveData.current.ClothingUnlockData[clothingPiece] = true;
                        } else NotEnoughMoney();
                        break;
                    case 2:
                        if (GameManager.MoneyInPurse >= 70) { GameManager.MoneyInPurse -= 70; 
                        GameEventManager.Raise(new MoneyUpdate());
                        purchasedCloths[clothingPiece] = true;
                            SaveData.current.ClothingUnlockData[clothingPiece] = true;
                        } else NotEnoughMoney();
                        break;
                    case 3:
                        if (GameManager.MoneyInPurse >= 70) { GameManager.MoneyInPurse -= 70; 
                        GameEventManager.Raise(new MoneyUpdate());
                        purchasedCloths[clothingPiece] = true;
                            SaveData.current.ClothingUnlockData[clothingPiece] = true;
                        } else NotEnoughMoney();
                        break;
                    case 4:
                        if (GameManager.MoneyInPurse >= 120) { GameManager.MoneyInPurse -= 120; 
                        GameEventManager.Raise(new MoneyUpdate());
                        purchasedCloths[clothingPiece] = true;
                            SaveData.current.ClothingUnlockData[clothingPiece] = true;
                        } else NotEnoughMoney();
                        break;
                    case 5:
                        if (GameManager.MoneyInPurse >= 200) { GameManager.MoneyInPurse -= 200; 
                        GameEventManager.Raise(new MoneyUpdate());
                        purchasedCloths[clothingPiece] = true;
                            SaveData.current.ClothingUnlockData[clothingPiece] = true;
                        } else NotEnoughMoney();
                        break;
                    case 6:
                        if (GameManager.MoneyInPurse >= 200) { GameManager.MoneyInPurse -= 200; 
                        GameEventManager.Raise(new MoneyUpdate());
                        purchasedCloths[clothingPiece] = true;
                            SaveData.current.ClothingUnlockData[clothingPiece] = true;
                        } else NotEnoughMoney();
                        break;
                    default:
                        GameEventManager.Raise(new ClothingChangeEvent(0)); //0 is the default outfit
                        break;
                }
                purchaseQuestion.SetActive(false);
            }
        }
        else if (!purchasedCloths[clothingPiece])
        {
            clothingInQuestion = clothingPiece; //save the clothing index number 
            GameEventManager.Raise(new ClothingChangeEvent(clothingPiece));
            purchaseQuestion.SetActive(true);
        }  
        else //happens when the item is already bought
        {
            GameEventManager.Raise(new ClothingChangeEvent(clothingPiece));
        }
    }

    public void NotEnoughMoney()
    {
        NoMoneyPopup.SetActive(!NoMoneyPopup.activeSelf);
        notification.text = "You don't have enough money!";
        GameEventManager.Raise(new ClothingChangeEvent(0)); //0 is the default outfit
    }
    public void NotifyPlayer(string message)
    {
        NoMoneyPopup.SetActive(!NoMoneyPopup.activeSelf);
        notification.text = message;
    }

    public void ChangePanel(GameObject obj)
    {
        obj.transform.SetSiblingIndex(1);
    }
    protected virtual void OnEnable()
    {
        GameEventManager.AddListener<LoadEvent>(OnLoad);
    }

    protected virtual void OnDisable()
    {
        GameEventManager.RemoveListener<LoadEvent>(OnLoad);
    }

    private void OnLoad(LoadEvent e)
    {
        purchasedCloths = SaveData.current.ClothingUnlockData;
    }
}

