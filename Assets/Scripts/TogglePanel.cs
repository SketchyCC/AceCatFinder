using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvents;
using System;
using UnityEngine.UI;

public class TogglePanel : MonoBehaviour
{
    public GameObject Panel;

    public InventoryObject inventory;
    public InventoryObject equipment;
    public Item item;
    public bool openedOnce = false;
    private bool isActive = true;
    private bool lookingAtBoard = false;

    private void Update()
    {
        if (!lookingAtBoard)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                OpenPanel();
            }
        }
    }

    public void OpenPanel()
    {        
        GameEventManager.Raise(new BagOpenEvent(isActive));
        if (Panel != null)
        {
            isActive = Panel.activeSelf;
            Panel.SetActive(!isActive);
            GetComponent<Image>().enabled = isActive;
            if (!openedOnce)
            {
                StartCoroutine(Delay());
            }

        }
    }

    protected virtual void OnEnable()
    {
        GameEventManager.AddListener<NetOutEvent>(OnNetUpdate);
        GameEventManager.AddListener<CommunityBoardLook>(OnLookUpdate);
        GameEventManager.AddListener<CommunityBoardLeave>(OnLeaveUpdate);
        GameEventManager.AddListener<UIOpened>(OnUIUpdate);

    }
    protected virtual void OnDisable()
    {
        GameEventManager.RemoveListener<NetOutEvent>(OnNetUpdate);
        GameEventManager.RemoveListener<CommunityBoardLook>(OnLookUpdate);
        GameEventManager.RemoveListener<CommunityBoardLeave>(OnLeaveUpdate);
        GameEventManager.RemoveListener<UIOpened>(OnUIUpdate);
    }

    private void OnNetUpdate(NetOutEvent e)
    {
        //OpenPanel(); //After adding the place item function for other objects, this line opened and closed the inventory each time I placed an item.
    }

    //without the coroutine the item adding would fire too early and the image would not appear 
    IEnumerator Delay()
    {
        if (!openedOnce)
        {
            yield return new WaitForSeconds(0.005f);
            openedOnce = true;
            inventory.AddItem(item, 1);
            int length = inventory.GetSlots.Length;
            for (int i = 0; i < length; i++)
            {
                inventory.GetSlots[i].UpdateSlot(SaveData.current.inventoryData.Slots[i].item, SaveData.current.inventoryData.Slots[i].amount);
            }
            if (equipment.GetSlots[0].item.Id >=0)
            {
                equipment.GetSlots[0].UpdateSlot(SaveData.current.equipmentData.Slots[0].item, SaveData.current.equipmentData.Slots[0].amount);
            }   
        }
    }

    private void OnLookUpdate(CommunityBoardLook e)
    {
        lookingAtBoard = true;
        if (!isActive)
        {
            OpenPanel();
        }
    }
    private void OnLeaveUpdate(CommunityBoardLeave e)
    {
        lookingAtBoard = false;
    }

    private void OnUIUpdate(UIOpened e)
    {
        lookingAtBoard = e.UIisopened;
        if (!isActive && e.UIisopened)
        {
            OpenPanel();
        }
    }
}

