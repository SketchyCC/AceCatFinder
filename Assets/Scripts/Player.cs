using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvents;
using UnityEngine.UI;
using System;

public class Player : MonoBehaviour
{
    public InventoryObject inventory;
    public InventoryObject equipment; 
    public GameObject TrapArea;
    public GameObject PlayerCamera;
    public GameObject DrinkStatus;
    private Image FullDrink;

    public Item item;
    public bool netOut;

    public PlayerData playerData;  
    public float boostCounter = 0;// here is the variable for the save and load of energyDrink time. 
    public float timeElapsed=0;
    private bool timePaused = false;

    private void Awake()
    {
        SaveData.current.player = playerData;
        SaveData.current.inventoryData = inventory.container;
        SaveData.current.equipmentData = equipment.container;
        Debug.Log(inventory.container.Slots[0].item.Id);

        TrapArea.SetActive(false);

        for (int i = 0; i < equipment.GetSlots.Length; i++)
        {
            equipment.GetSlots[i].OnBeforeUpdate += OnRemoveItem;
            equipment.GetSlots[i].OnAfterUpdate += OnAddItem;
        }

        FullDrink = DrinkStatus.transform.Find("Full").GetComponent<Image>();
    }

    private void Start()
    {
        StartCoroutine(SpeedUpgradeOver(1f));
    }
         
    public void OnRemoveItem(InventorySlot _slot)
    {
        if (_slot.ItemObject == null) { return; }
        if (_slot.ItemObject.characterDisplay != null)
        {
            netOut = false;
            GameEventManager.Raise(new NetOutEvent(netOut));
            TrapArea.SetActive(false);
            UpdateInventoryData(_slot.item);
        }
    }
    public void OnAddItem(InventorySlot _slot)
    {
        if (_slot.ItemObject == null) { return; }
        if (_slot.ItemObject.characterDisplay != null)
        {            
            if (_slot.ItemObject.type == ItemType.Net)
            {
                netOut = true;
                GameEventManager.Raise(new NetOutEvent(netOut));
                TrapArea.SetActive(true);
                UpdateInventoryData(_slot.item);
            }
        }
    }

    void Update()
    {
        playerData.position = gameObject.transform.position;
        playerData.rotation = gameObject.transform.rotation;        

        if (Input.GetKeyDown(KeyCode.F))
        {
            PlaceItem(equipment.container.Slots[0]); //test
            UpdateInventoryData(equipment.container.Slots[0].item);
        }

    }

    private void OnApplicationQuit() //Resets the inventory each time it gets closed
    {
        inventory.container.Clear();
        equipment.container.Clear();
    }

    protected virtual void OnEnable()
    {
        GameEventManager.AddListener<CatCaughtEvent>(OnCatCaughtEvent);
        GameEventManager.AddListener<WrongCatEvent>(OnWrongCatEvent);
        GameEventManager.AddListener<RightCatEvent>(OnRightCatEvent);
        GameEventManager.AddListener<ItemBoughtEvent>(OnItemBought);
        GameEventManager.AddListener<LoadEvent>(OnLoadEvent);
        GameEventManager.AddListener<TimeStoppedEvent>(OnPausedEvent);
    }

    protected virtual void OnDisable()
    {
        GameEventManager.RemoveListener<CatCaughtEvent>(OnCatCaughtEvent);
        GameEventManager.RemoveListener<WrongCatEvent>(OnWrongCatEvent);
        GameEventManager.RemoveListener<RightCatEvent>(OnRightCatEvent);
        GameEventManager.RemoveListener<ItemBoughtEvent>(OnItemBought);
        GameEventManager.RemoveListener<LoadEvent>(OnLoadEvent);
        GameEventManager.RemoveListener<TimeStoppedEvent>(OnPausedEvent);
    }

    private void OnPausedEvent(TimeStoppedEvent e)
    {
        timePaused = e.timeStopped;
    }

    void OnLoadEvent(LoadEvent e)
    {
        playerData = SaveData.current.player;
        
        gameObject.transform.position = playerData.position;
        gameObject.transform.rotation = playerData.rotation;
        boostCounter = playerData.BoostCounter;
        Debug.Log("boost count in data is " + playerData.BoostCounter);
        if (boostCounter > 0)
        {
            SpeedUpgrade(boostCounter);
        }
    }

    public virtual void OnCatCaughtEvent(CatCaughtEvent e)
    {
        PlayerCamera.SetActive(false);
    }

    public void Delay()
    {
        PlayerCamera.SetActive(true);
    }


    public virtual void OnWrongCatEvent(WrongCatEvent e)
    {
        PlayerCamera.SetActive(false);
        Invoke("Delay", 0.5f);
    }


    public virtual void OnRightCatEvent(RightCatEvent e)
    {
        PlayerCamera.SetActive(false);
        Invoke("Delay", 0.5f);
    }

    public virtual void OnItemBought(ItemBoughtEvent e)
    {
        if (equipment.container.Slots[0].item.Id == e.item.data.Id)
        {

            equipment.AddItem(e.item.data, 1);
            UpdateInventoryData(e.item.data);
        }
        else
        {
            inventory.AddItem(e.item.data, 1);
            UpdateInventoryData(e.item.data);
        }
    }

    private void PlaceItem(InventorySlot _slot)
    {
        InventorySlot hold = equipment.container.Slots[0];
        if (hold.ItemObject != null) //nets cant be placed
        {
            if (hold.ItemObject.type != ItemType.Net)
            {
                if (hold.item.Id > -1 && hold.amount > 0)
                {
                    if (hold.ItemObject.type == ItemType.Food)
                    {
                        Vector3 Objectlocation = (gameObject.transform.forward * 3) + gameObject.transform.position;
                        TrapArea.SetActive(true);
                        GameObject placeditem = Instantiate(_slot.ItemObject.characterDisplay, TrapArea.transform.position + (Vector3.up * 0.2f), Quaternion.identity);
                        TrapArea.SetActive(false);
                    }

                    if (hold.ItemObject.type == ItemType.Consumable)
                    {
                        if (boostCounter <= 0f)
                        {
                            SpeedUpgrade(180f);
                        }
                        else
                        {
                            return;
                        }
                    }
                    
                    if (equipment.container.Slots[0].amount == 1)
                    {
                        equipment.container.Slots[0].RemoveItem();
                        return;
                    }
                    else
                    {
                        equipment.container.Slots[0].AddAmount(-1);
                    }
                }
            }
        }
    }

    public void SpeedUpgrade(float boostTime)
    {

        boostCounter = boostTime;
        DrinkStatus.SetActive(true);
        StartCoroutine(SpeedUpgradeOver(0));
    }

    IEnumerator SpeedUpgradeOver(float delay)
    {
        yield return new WaitForSeconds(delay);
        if(boostCounter <= 0) { yield break; }
        GameEventManager.Raise(new SpeedUpgradeBought(boostCounter));
        if (playerData.TimeElapsed > 0)
        {
            timeElapsed = playerData.TimeElapsed;
        }
        //float counter = boostCounter;
        float counter = 180f; 

        while (timeElapsed < counter)
        {
            float percentage = (timeElapsed/ (counter / 100)) / 100;
            FullDrink.fillAmount = Mathf.Lerp(1, 0, percentage);

            if (timePaused)
            {
                timeElapsed += 0;
            } else if(!timePaused)
            {
                timeElapsed += Time.deltaTime;
            }

            boostCounter = counter - timeElapsed;
            playerData.BoostCounter = boostCounter;
            playerData.TimeElapsed = timeElapsed;
            Debug.Log("Boost counter is " + boostCounter);
            yield return null;
        }

        boostCounter = 0;
        playerData.BoostCounter = boostCounter;
        playerData.TimeElapsed = timeElapsed;
        GameEventManager.Raise(new SpeedUpgradeBought(boostCounter));
        DrinkStatus.SetActive(false);
    }

    private void PickUpItem(InventorySlot _slot)
    {
        if (equipment.container.Slots[0].item.Id == _slot.item.Id) 
        {
            equipment.AddItem(_slot.item, 1);
        }
        else
        {
            inventory.AddItem(_slot.item, 1);
        }
    }

    private void UpdateInventoryData(Item item)
    {
        SaveData.current.inventoryData = inventory.container;
        SaveData.current.equipmentData = equipment.container;
    }

    private void OnDestroy()
    {
        for (int i = 0; i < equipment.GetSlots.Length; i++)
        {
            equipment.GetSlots[i].OnBeforeUpdate -= OnRemoveItem;
            equipment.GetSlots[i].OnAfterUpdate -= OnAddItem; 
        }
    }
}