using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class inventoryManager : MonoBehaviour
{
    public GameObject inventoryMenu;
    public GameObject equipmentMenu;

    public itemSlot[] itemSlot;
    public equipmentSlot[] equipmentSlot;
    public equippedSlot[] equippedSlot;

    public playerController playerController;
    public playerCombat playerCombat;

    public Health player;

    public itemSO[] itemSOs;
    void Start()
    {
        
    }

    void Update()
    {
        if (!player.isDead)
        {
            if (Input.GetButtonDown("inventoryMenu"))
                Inventory();
            if (Input.GetButtonDown("equipmentMenu"))
                Equipment();
        }
    }

    void Inventory()
    {
        if (inventoryMenu.activeSelf)
        {
            Time.timeScale = 1;
            inventoryMenu.SetActive(false);
            equipmentMenu.SetActive(false);
            GameObject.FindWithTag("Player").GetComponent<playerController>().enabled = true;
            GameObject.FindWithTag("Player").GetComponent<playerCombat>().enabled = true;
        }
        else if(inventoryMenu.activeSelf && player.isDead)
        {
            Time.timeScale = 0f;
            inventoryMenu.SetActive(true);
            equipmentMenu.SetActive(false);
            GameObject.FindWithTag("Player").GetComponent<playerController>().enabled = false;
            GameObject.FindWithTag("Player").GetComponent<playerCombat>().enabled = false;
        }
        else
        {
            Time.timeScale = 0f;
            inventoryMenu.SetActive(true);
            equipmentMenu.SetActive(false);
            GameObject.FindWithTag("Player").GetComponent<playerController>().enabled = false;
            GameObject.FindWithTag("Player").GetComponent<playerCombat>().enabled = false;
        }
    }
    void Equipment()
    {
        if (equipmentMenu.activeSelf)
        {
            Time.timeScale = 1;
            inventoryMenu.SetActive(false);
            equipmentMenu.SetActive(false);
            GameObject.FindWithTag("Player").GetComponent<playerController>().enabled = true;
            GameObject.FindWithTag("Player").GetComponent<playerCombat>().enabled = true;
        }

        else if (equipmentMenu.activeSelf && player.isDead)
        {
            Time.timeScale = 0f;
            inventoryMenu.SetActive(true);
            equipmentMenu.SetActive(false);
            GameObject.FindWithTag("Player").GetComponent<playerController>().enabled = false;
            GameObject.FindWithTag("Player").GetComponent<playerCombat>().enabled = false;
        }

        else
        {
            Time.timeScale = 0;
            inventoryMenu.SetActive(false);
            equipmentMenu.SetActive(true);
            GameObject.FindWithTag("Player").GetComponent<playerController>().enabled = false;
            GameObject.FindWithTag("Player").GetComponent<playerCombat>().enabled = false;
        }
    }

    public bool useItem(string itemName)
    {
        for (int i = 0; i < itemSOs.Length; i++)
        {
            if (itemSOs[i].itemName == itemName)
            {
                bool usable = itemSOs[i].useItem();
                return usable;
            }
        }
        return false;
    }

    public int addItem(string itemName, int quantity, Sprite itemSprite, string itemDescription, ItemType itemType)
    {
        if (itemType == ItemType.consumable || itemType == ItemType.crafting || itemType == ItemType.collectible)
        {
            for (int i = 0; i < itemSlot.Length; i++)
            {
                if (itemSlot[i].isFull == false && itemSlot[i].itemName == itemName || itemSlot[i].quantity == 0)
                {
                    int leftOverItems = itemSlot[i].addItem(itemName, quantity, itemSprite, itemDescription, itemType);
                    if (leftOverItems > 0)
                        leftOverItems = addItem(itemName, leftOverItems, itemSprite, itemDescription, itemType);
                    return leftOverItems;
                }
            } 
            return quantity;
        }
        else
        {
            for (int i = 0; i < equipmentSlot.Length; i++)
            {
                if (equipmentSlot[i].isFull == false && equipmentSlot[i].itemName == itemName || equipmentSlot[i].quantity == 0)
                {
                    int leftOverItems = equipmentSlot[i].addItem(itemName, quantity, itemSprite, itemDescription, itemType);
                    if (leftOverItems > 0)
                        leftOverItems = addItem(itemName, leftOverItems, itemSprite, itemDescription, itemType);
                    return leftOverItems;
                }
            }
            return quantity;
        }
    }


    public void deselectAllSlots()
    {
        for (int i = 0; i < itemSlot.Length; i++)
        {
            itemSlot[i].selectedShader.SetActive(false);
            itemSlot[i].thisItemSelected = false;
        }
        for (int i = 0; i < equipmentSlot.Length; i++)
        {
            equipmentSlot[i].selectedShader.SetActive(false);
            equipmentSlot[i].thisItemSelected = false;
        }
        for (int i = 0; i < equippedSlot.Length; i++)
        {
            equippedSlot[i].selectedShader.SetActive(false);
            equippedSlot[i].thisItemSelected = false;
        }
    }
}


public enum ItemType
{
    consumable,
    crafting,
    collectible,
    head,
    armor,
    pants,
    weapon,
    neck,
    ring,
    arm,
    boots,
    none
};